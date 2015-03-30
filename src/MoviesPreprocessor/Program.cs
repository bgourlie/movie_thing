using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MoviesPreprocessor
{
	class Program
	{
		private const int IDX_ID = 0;
		private const int IDX_TITLE = 2;
		private const int IDX_YEAR = 3;
		private const int IDX_CAST = 10;

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("You must supply the file name.");
				return;
			}

			try
			{
				using (var file = File.OpenRead(args[0]))
				using (var reader = new StreamReader(file))
				{
					var movies = new List<MovieEntry>();
					var actors = new Dictionary<string, int>();
					int processed = 0;
					int skippedMovies = 0;
					string line;
					Console.Write("Processing Movie Dump");
					while((line = reader.ReadLine()) != null)
					{
						var parts = line.Split('\t');
						int id;
						ushort year;
						string[] cast;

						if (!ushort.TryParse(parts[IDX_YEAR], out year))
						{
							skippedMovies++;
							continue;
						}

						if (!int.TryParse(parts[IDX_ID], out id))
						{
							skippedMovies++;
							continue;
						}

						var title = parts[IDX_TITLE];
						if(!TryGetCast(parts[IDX_CAST], out cast))
						{
							skippedMovies++;
							continue;
						}

						movies.Add(new MovieEntry(id, title, year, cast));

						foreach(var member in cast)
						{
							if(!actors.ContainsKey(member))
							{
								actors.Add(member, -1);
							}	
						}

						if(++processed % 10000 == 0)
						{
							Console.Write(".");
						}
					}

					Console.WriteLine();
					Console.Write("Processed {0} movies.  Skipped {1}.", movies.Count, skippedMovies);

					// Create a lookup table so we can determine an actor's id by their name.
					var arrActors = actors.Keys.ToArray();
					for(int actorId = 0; actorId < arrActors.Length; ++actorId)
					{
						actors[arrActors[actorId]] = actorId;

						if(actorId % 10000 == 0)
						{
							Console.Write(".");
						}
					}

					Console.WriteLine();
					Console.Write("Generating graph with {0} nodes", arrActors.Length);

					var graph = new Graph(arrActors.Length);
					for (int i = 0; i < movies.Count; i++)
					{
						var entry = movies[i];

						// Stuff all actor id's into an array
						var actorIds = new int[entry.Cast.Length];
						for (int j = 0; j < entry.Cast.Length; ++j)
						{
							actorIds[j] = actors[entry.Cast[j]];
						}

						graph.AddEdge(entry.Id, (byte) (255 - (2015 - entry.Year)), actorIds);
						if (i%10000 == 0)
						{
							Console.Write(".");
						}
					}


				    // get the current process
					var currentProcess = Process.GetCurrentProcess();

					// get the physical mem usage
					long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;

					GC.Collect();
					Console.WriteLine();
					Console.WriteLine("{0:N}MB in use.  Press any key to exit.", totalBytesOfMemoryUsed / 1000000f);
					Console.Read();
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("{0} is an invalid file location.", args[0]);
			}
		}

		static bool TryGetCast(string unparsed, out string[] cast)
		{
			cast = unparsed.Split(new [] { ", "}, StringSplitOptions.RemoveEmptyEntries);
			// In order to link two actors to eachother, we need at least two actors cast in a movie!
			if (cast.Length < 2)
			{
				return false;
			}

			if (cast[0] == "N/A")
			{
				return false;
			}

			foreach (var member in cast)
			{
				if (!member.Contains(" "))
				{
					return false;
				}
			}

			return true;
		}
	}
}
