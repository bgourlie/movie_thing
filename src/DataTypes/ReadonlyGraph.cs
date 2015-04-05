using System;
using System.Collections.Generic;
using System.IO;

namespace DataTypes
{
	public class ReadonlyGraph : IGraph
	{
		public ReadonlyGraph(IGraph graph)
		{
			Nodes = graph.Nodes;
			NodeCount = graph.NodeCount;
			EdgeCount = graph.EdgeCount;
		}

		public Dictionary<int, Edge>[] Nodes { get; }

		public int NodeCount { get; }

		public int EdgeCount { get; }
	}
}
