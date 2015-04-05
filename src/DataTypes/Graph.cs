using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataTypes
{
	public class Graph : IGraph
	{
		// An array of Dictionaries, where the index is the node id (actor id)
		// and the dictionary is keyed by the adjacent node id's with the value being
		// the edge data.
		public Dictionary<int, Edge>[] Nodes { get; }
		
		// Keeps track of distinct edge references so we don't keep any duplicates.
		private readonly Dictionary<Edge, HashSet<int>> _edges = new Dictionary<Edge, HashSet<int>>();

		public Graph(int nodeCount)
		{
			NodeCount = nodeCount;
			Nodes = new Dictionary<int, Edge>[nodeCount];
			for (int i = 0; i < Nodes.Length; ++i) 
			{
				Nodes [i] = new Dictionary<int, Edge> ();
			}
		}

		public void AddEdge(int movieId, byte weight, params int[] nodeIds)
		{

			var edge = new Edge(movieId, weight);
			if (!_edges.ContainsKey(edge))
			{
				_edges.Add(edge, new HashSet<int>(nodeIds));
			}
			else
			{
				var set = _edges[edge];
				foreach (var nodeId in nodeIds)
				{
					set.Add(nodeId);
				}
			}

			for (int i = 0 ; i < nodeIds.Length; i++){
				for(int j = 0 ; j < nodeIds.Length; j++){
					if (i == j)
					{
						continue;
					}
					// is there already an edge between these two nodes?
					var sourceNodeId = nodeIds[i];
					var targetNodeId = nodeIds[j];
					if (Nodes[sourceNodeId].ContainsKey(targetNodeId))
					{
						if (Nodes[sourceNodeId][targetNodeId].Distance > edge.Distance)
						{
							// if the new edge has a shorter distance, replace the old one.
							Nodes[sourceNodeId][targetNodeId] = edge;
						}
					}
					else
					{
						Nodes[sourceNodeId].Add(targetNodeId, edge);
					}
				}
			}
		}

		public static IEnumerable<Tuple<int, Dictionary<int, Edge>>> Prune(Graph graph, int sourceNodeId)
		{
			var visited = new HashSet<int>();
			var queue = new Queue<int>();
			visited.Add(sourceNodeId);
			int curNode = sourceNodeId;
			do
			{
				var edges = graph.Nodes[curNode];
				foreach (var key in edges.Keys)
				{
					if (visited.Contains(key))
					{
						continue;
					}

					visited.Add(key);
					queue.Enqueue(key);
				}

				curNode = queue.Count == 0 ? -1 : queue.Dequeue();
			} while (curNode > -1);

			return visited.Select(v => Tuple.Create(v, graph.Nodes[v]));
		}

		public void WriteToStream(Dictionary<int, string> actorTable, Stream outStream)
		{

			using (var writer = new BinaryWriter(outStream))
			{
				// First byte is a signed 32 bit integer representing the number of actors in the actor table.
				writer.Write(BitConverter.GetBytes(actorTable.Count));

				foreach (var kvp in actorTable)
				{
					var nameBytes = Encoding.Unicode.GetBytes(kvp.Value);
					// Each row in the actor table starts with a signed 32-bit integer representing the character length 
					// of the actors name, followed by unicode encoded characters of the aforementioned length.
					writer.Write(BitConverter.GetBytes(kvp.Value.Length));
					writer.Write(nameBytes);
				}

				var edges = _edges.Keys.ToArray();

				// The edge table starts with a 32-bit signed integer representing the number of edges in the edge table
				writer.Write(BitConverter.GetBytes(edges.Length));
				for (int i = 0; i < edges.Length; ++i)
				{
					var edge = edges[i];
					// Determine the nodes connected by the edge
					var connectedNodes = _edges[edge].ToArray();

					// The next 4 bytes are a 32-bit signed integer representing the movie id
					writer.Write(BitConverter.GetBytes(edge.MovieId));

					// the following byte represents the distance (or weight) of the edge
					writer.Write(edge.Distance);

					// Each edge row starts with a 32-bit signed integer representing the number of nodes connected
					// by the edge.
					writer.Write(BitConverter.GetBytes(connectedNodes.Length));
					foreach (var connectedNode in connectedNodes)
					{
						// write each connected node id as a 32-bit signed integer
						writer.Write(BitConverter.GetBytes(connectedNode));
					}
				}
			}
		}

		public int NodeCount { get; }

		public int EdgeCount => _edges.Count;
	}
}