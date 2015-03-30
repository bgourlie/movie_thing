using System;
using System.Collections.Generic;
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
					Console.Write("Processed {0} movies.  Skipped {1}.  Generating actor table", movies.Count, skippedMovies);

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
					Console.WriteLine("Generated actor table, {0} entries.", arrActors.Length);


					foreach (var entry in movies) 
					{
						var edge = new Edge (entry.Id, 255 - (2015 - entry.Year));
						var nodeIds = new int[entry.Cast.length];


					}

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
			if (cast.Length == 0)
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


	class MovieEntry
	{
		public readonly int Id;
		public readonly string Title;
		public readonly string[] Cast;
		public readonly ushort Year;

		public MovieEntry(int id, string title, ushort year, string[] cast)
		{
			Id = id;
			Title = title;
			Year = year;
			Cast = cast;
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}] starring {2}", Title, Year, string.Join(", ", Cast));
		}
	}

	class Graph
	{
		private readonly HashSet<Edge>[] _nodes;

		public Graph(int graphSize)
		{
			_nodes = new HashSet<Edge>[graphSize];
			for (int i = 0; i < _nodes.Length; ++i) 
			{
				_nodes [i] = new HashSet<Edge> ();
			}
		}

		bool AddEdge(Edge edge, params int[] nodes)
		{
			for (int i = 0; i < nodes.Length; ++i)
			{
				_nodes [nodes [i]].Add (edge);
			}
		}
	}

	class Edge
	{
		public readonly int MovieId;
		public readonly byte Weight;

		public Edge(int movieId, byte weight)
		{
			MovieId = movieId;
			Weight = weight;
		}
	}
}
