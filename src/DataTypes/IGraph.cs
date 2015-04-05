using System.Collections.Generic;

namespace DataTypes
{
	public interface IGraph
	{
		int NodeCount { get; }
		int EdgeCount { get; }
	}
}
