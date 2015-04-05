using System.Collections.Generic;

namespace DataTypes
{
	public static class GraphExtensions
	{
		public static void MakeAdjacent(this Dictionary<int, Edge>[] nodes, int[] nodeIds, Edge edge)
		{
			for (int i = 0 ; i < nodeIds.Length; i++){
				for(int j = 0 ; j < nodeIds.Length; j++){
					if (i == j)
					{
						continue;
					}
					// is there already an edge between these two nodes?
					var sourceNodeId = nodeIds[i];
					var targetNodeId = nodeIds[j];
					if (nodes[sourceNodeId].ContainsKey(targetNodeId))
					{
						if (nodes[sourceNodeId][targetNodeId].Distance > edge.Distance)
						{
							// if the new edge has a shorter distance, replace the old one.
							nodes[sourceNodeId][targetNodeId] = edge;
						}
					}
					else
					{
						nodes[sourceNodeId].Add(targetNodeId, edge);
					}
				}
			}
		}

	}
}
