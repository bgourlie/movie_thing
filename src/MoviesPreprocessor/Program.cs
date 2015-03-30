using System;
using System.Collections.Generic;
using System.IO;

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
				goto exit;
			}

			try
			{
				using (var file = File.OpenRead(args[0]))
				using (var reader = new StreamReader(file))
				{
					var movies = new List<MovieEntry>();
					int skippedMovies = 0;
					string line;
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
					}
				
					Console.WriteLine("Processed {0} movies.  Skipped {1}.", movies.Count, skippedMovies);
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("{0} is an invalid file location.", args[0]);
				goto exit;
			}

			exit:
			Console.WriteLine("Press any key to exit.");
			Console.Read();
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
}
