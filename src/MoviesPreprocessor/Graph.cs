using System.Collections.Generic;

namespace MoviesPreprocessor
{
	class Graph
	{
		private readonly Dictionary<int, Edge>[] _nodes;

		public Graph(int graphSize)
		{
			_nodes = new Dictionary<int, Edge>[graphSize];
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
	}
}