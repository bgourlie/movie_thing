using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataTypes
{
	public class ReadonlyGraph : IGraph
	{
		private readonly Dictionary<int, Edge>[] _nodes;
		private readonly Dictionary<int, string> _actorTable;
		private readonly int _edgeCount;

		private ReadonlyGraph(Dictionary<int, Edge>[] nodes, Dictionary<int, string> actorTable, int edgeCount)
		{
			_nodes = nodes;
			_actorTable = actorTable;
			_edgeCount = edgeCount;
		}

	    public List<Edge> GetShortestPath(int fromNode, int toNode)
	    {
            var edges = new List<Edge>();

            // Assign to every node a tentative distance value: set it to zero
            // for our initial node and to infinity for all other nodes.
	        var distances = new PriorityQueue<int>();
            distances.Insert(fromNode, 0);

            // Keep a set of visited nodes.  This set starts with just the 
            // initial node.
	        var visited = new HashSet<int> {fromNode};
	        var curNode = fromNode;

	        while (visited.Count < _nodes.Length)
	        {
                // For the current node, consider all of its unvisited neighbors and
                // calculate (distance to the current node) + (distance from the 
                // current node to neighbor).  If this is less than their current 
                // tentative distance, replace it with this new value.
                var curDistance = distances.GetKey(curNode);
                foreach (var kvp in _nodes[curNode])
                {
                    if (visited.Contains(kvp.Key))
                    {
                        continue;
                    }

                    int currentBestDistance;
                    if(!distances.TryGetKey(kvp.Key, out currentBestDistance))
                    {
                        currentBestDistance = int.MaxValue;
                    }

                    var testDistance = curDistance + kvp.Value.Distance;

                    if (testDistance < currentBestDistance)
                    {
                        if (!distances.ContainsNode(kvp.Key))
                        {
                            distances.Insert(kvp.Key, testDistance);
                        }
                        else
                        {
                            distances.DecreaseKey(kvp.Key, testDistance);
                        }
                    }
                }

                // When we are done considering all of the neighbors of the current 
                // node, mark the current node as visited and remove it from the 
                // unvisited set.
                visited.Add(curNode);

                // If the destination node has been marked visited, the algorithm
                // has finished.
                if (visited.Contains(toNode))
                {
                    // TODO: add the nodes involved in the shortest path to the 
                    // edges list
                    return edges;
                }

                // Set the unvisited node marked with the smallest tentative 
                // distance as the next "current node" and go back to step 3.
                curNode = distances.PeekMin();
	        }

            throw new Exception($"No path from node {fromNode} to {toNode}");
	    }

		public static ReadonlyGraph NewFromStream(Stream stream)
		{
			using (var reader = new BinaryReader(stream, Encoding.Unicode))
			{
				var actorTable = new Dictionary<int, string>();
				var edgeNodes = new Dictionary<Edge, HashSet<int>>();
				var edges = new Dictionary<Edge, Edge>();

                // first four bytes is the number records in actor table as a 
                // signed int32
				var numRecords = reader.ReadInt32();

				for (int i = 0; i < numRecords; ++i)
				{
                    // the first byte for each actor record is the length in 
                    // characters of the name as a signed int32
					var strLength = reader.ReadInt32();

					// subsequent bytes are length * number of unicode chars
					var name = reader.ReadChars(strLength);
					actorTable.Add(i, new string(name));
				}

				var nodes = new Dictionary<int, Edge>[numRecords];

				for (int i = 0; i < nodes.Length; ++i)
				{
					nodes[i] = new Dictionary<int, Edge>();
				}

				var edgeCount = reader.ReadInt32();
				for (int i = 0; i < edgeCount; ++i)
				{
					var movieId = reader.ReadInt32();
					var distance = reader.ReadByte();
					var edge = new Edge(movieId, distance);
					if (!edgeNodes.ContainsKey(edge))
					{
						edges.Add(edge, edge);
						var numConnectedNodes = reader.ReadInt32();
						var connectedNodes = new HashSet<int>();
						for (int j = 0; j < numConnectedNodes; ++j)
						{
							connectedNodes.Add(reader.ReadInt32());
						}

						nodes.MakeAdjacent(connectedNodes.ToArray(), edge);
						edgeNodes.Add(edge, connectedNodes);
					}
				}

				return new ReadonlyGraph(nodes, actorTable, edges.Count);
			}
		}

		public int NodeCount => _nodes.Length;

	    public int EdgeCount => _edgeCount;
	}
}
