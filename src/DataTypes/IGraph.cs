using System.Collections.Generic;

namespace DataTypes
{
	public interface IGraph
	{
		Dictionary<int, Edge>[] Nodes { get; }
		int NodeCount { get; }
		int EdgeCount { get; }
	}
}
