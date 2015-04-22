using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataTypes;

namespace MoviesPreprocessor
{
	class Program
	{
		private const string PRUNE_ON = "Kevin Bacon";
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

			Tuple<Graph, Dictionary<int, string>> graphData;

			if (!GenerateGraph(args[0], out graphData))
			{
				return;
			}

			var graph = graphData.Item1;
			var actorTable = graphData.Item2;

			using (var fileStream = File.Open("out.bin", FileMode.Create))
			{
				graphData.Item1.WriteToStream(graphData.Item2, fileStream);
			}

			var fileInfo = new FileInfo("out.bin");
			var longestActorName = actorTable.Max(c => c.Value.Length);
			var mostEdgesForANode = graph.Nodes.Max(n => n.Values.Count);
			Console.WriteLine();
			Console.WriteLine("Saved graph with {0} nodes (highest edges for a node is {1}), {2} edges and {3} actors (max actor name length is {4}) to out.bin ({5}MB).", graph.NodeCount, mostEdgesForANode, graph.EdgeCount, actorTable.Count, longestActorName, fileInfo.Length / 1000000f);

			Console.WriteLine("Creating readonly graph from file (smoke test)");
			using (var fileStream = File.OpenRead("out.bin"))
			{
				var readonlyGraph = ReadonlyGraph.NewFromStream(fileStream);
				GC.Collect();
				var memoryUsage = GetMemoryUsageInMegaBytes();
				Console.WriteLine("Readonly graph created with {0} nodes and {1} edges.  Memory used by process is {2}. Press any key to exit.", readonlyGraph.NodeCount, readonlyGraph.EdgeCount, memoryUsage);
			}
			Console.Read();
		}

		static bool GenerateGraph(string movieDump, out Tuple<Graph, Dictionary<int, string>> graphData)
		{
			try
			{
				using (var file = File.OpenRead(movieDump))
				using (var reader = new StreamReader(file))
				{
					var movies = new List<MovieEntry>();
					var actorsByName = new Dictionary<string, int>();
					var actorsById = new Dictionary<int, string>();
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
							if(!actorsByName.ContainsKey(member))
							{
								actorsByName.Add(member, -1);
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
					var arrActors = actorsByName.Keys.ToArray();
					for(int actorId = 0; actorId < arrActors.Length; ++actorId)
					{
						var name = arrActors[actorId];
						actorsByName[name] = actorId;
						actorsById.Add(actorId, name);
					}

					Console.WriteLine();
					Console.WriteLine("Generating graph with {0} nodes", arrActors.Length);

					var graph = new Graph(arrActors.Length);
					for (int i = 0; i < movies.Count; i++)
					{
						var entry = movies[i];

						// Stuff all actor id's into an array
						var actorIds = new int[entry.Cast.Length];
						for (int j = 0; j < entry.Cast.Length; ++j)
						{
							actorIds[j] = actorsByName[entry.Cast[j]];
						}
						var edge = new Edge(entry.Id, (byte) (255 - (2015 - entry.Year)));
						graph.AddEdge(edge, actorIds);
						if (i%10000 == 0)
						{
							Console.Write(".");
						}
					}


				    // get the current process

					GC.Collect();
					// get the physical mem usage
					var memoryUsed = GetMemoryUsageInMegaBytes();
					Console.WriteLine();
					Console.WriteLine("{0:N}MB in use before pruning.  Pruning on {1}", memoryUsed, PRUNE_ON);

					var pruned = Graph.Prune(graph, actorsByName[PRUNE_ON]).ToArray();
					Console.WriteLine("{0} items left after pruning.", pruned.Length);

					// original Id is key, new id is value
					var actorTranslationTableByOrigId = new Dictionary<int, int>(pruned.Length);
					var actorTranslationTableByNewId = new Dictionary<int, int>(pruned.Length);
					for(int i = 0; i < pruned.Length; ++i)
					{
						actorTranslationTableByOrigId.Add(pruned[i].Item1, i);
						actorTranslationTableByNewId.Add(i, pruned[i].Item1);
					}

					Console.WriteLine("Creating new pruned graph");
					var prunedGraph = new Graph(pruned.Length);
					var newActors = new Dictionary<int, string>();

					for (int i = 0; i < pruned.Length; ++i)
					{
						newActors.Add(i, actorsById[actorTranslationTableByNewId[i]]);
						foreach (var kvp in pruned[i].Item2)
						{
							var edge = new Edge(kvp.Value.MovieId, kvp.Value.Distance);
							prunedGraph.AddEdge(edge, i, actorTranslationTableByOrigId[kvp.Key]);
						}

						if (i%10000 == 0)
						{
							Console.Write(".");
						}
					}

					graphData = Tuple.Create(prunedGraph, newActors);
					return true;
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("{0} is an invalid file location.", movieDump);
				graphData = null;
				return false;
			}
		}

		static float GetMemoryUsageInMegaBytes() 
		{
			var currentProcess = Process.GetCurrentProcess();
			// get the physical mem usage
			long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
			return totalBytesOfMemoryUsed/1000000f;
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
