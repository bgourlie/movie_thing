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

		private ReadonlyGraph(Dictionary<int, Edge>[] nodes, Dictionary<int, string> actorTable, int edgeCount)
		{
			_nodes = nodes;
			_actorTable = actorTable;
			NodeCount = nodes.Length;
			EdgeCount = edgeCount;
		}

		public static ReadonlyGraph NewFromStream(Stream stream)
		{
			using (var reader = new BinaryReader(stream, Encoding.Unicode))
			{
				var actorTable = new Dictionary<int, string>();
				var edgeNodes = new Dictionary<Edge, HashSet<int>>();
				var edges = new Dictionary<Edge, Edge>();

				// first four bytes is the number records in actor table as a signed int32
				var numRecords = reader.ReadInt32();

				for (int i = 0; i < numRecords; ++i)
				{
					// the first byte for each actor record is the length in characters of the name as a signed int32
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

		public int NodeCount { get; }

		public int EdgeCount { get; }
	}
}
