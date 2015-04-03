using System;
using System.Collections.Generic;
using System.Linq;

namespace DataTypes
{
	public class Graph
	{
		private readonly Dictionary<int, Edge>[] _nodes;
		public readonly int NodeCount;

		public Graph(int nodeCount)
		{
			NodeCount = nodeCount;
			_nodes = new Dictionary<int, Edge>[nodeCount];
			for (int i = 0; i < _nodes.Length; ++i) 
			{
				_nodes [i] = new Dictionary<int, Edge> ();
			}
		}

		public void AddEdge(int movieId, byte weight, params int[] nodeIds)
		{
			var edge = new Edge(movieId, weight);
			for(int i = 0 ; i < nodeIds.Length; i++){
				for(int j = 0 ; j < nodeIds.Length; j++){
					if (i == j)
					{
						continue;
					}
					// is there already an edge between these two nodes?
					var sourceNodeId = nodeIds[i];
					var targetNodeId = nodeIds[j];
					if (_nodes[sourceNodeId].ContainsKey(targetNodeId))
					{
						if (_nodes[sourceNodeId][targetNodeId].Distance > edge.Distance)
						{
							// if the new edge has a shorter distance, replace the old one.
							_nodes[sourceNodeId][targetNodeId] = edge;
						}
					}
					else
					{
						_nodes[sourceNodeId].Add(targetNodeId, edge);
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
				var edges = graph._nodes[curNode];
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

			return visited.Select((v) => Tuple.Create(v, graph._nodes[v]));
		}
	}
}