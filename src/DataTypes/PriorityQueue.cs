using System;
using System.Collections.Generic;

namespace DataTypes
{
	/// <summary>
	/// A priority queue, implemented as a min heap.
	/// </summary>
	public class PriorityQueue<TNodeType, TMetadataType> 
	{
		internal readonly List<TNodeType> _nodes = new List<TNodeType> ();
	    internal readonly List<Tuple<int, TMetadataType>> _keys = new List<Tuple<int, TMetadataType>>();

        // Using this as a way to look up a node's index.
	    private readonly Dictionary<TNodeType, int> _indices = new Dictionary<TNodeType, int>();

		private int _ubound = -1;

		public void Insert (TNodeType item, int key, TMetadataType metadata = default(TMetadataType))
		{
		    int addedAt; 
			if (_ubound == _nodes.Count - 1) {
				_nodes.Add (item);
                _keys.Add(Tuple.Create(key, metadata));
			    addedAt = 0;
			} else {
				_nodes [_ubound] = item;
			    _keys[_ubound] = Tuple.Create(key, metadata);
			    addedAt = _ubound;
			}

            _indices.Add(item, addedAt);
			_BalanceUp (++_ubound);
		}

	    public TNodeType PeekMin()
	    {
	        return _nodes[0];
	    }

	    public int GetKey(TNodeType node)
	    {
	        return _keys[_indices[node]].Item1;
	    }

	    public TMetadataType GetNodeMetadata(TNodeType node)
	    {
	        return _keys[_indices[node]].Item2;
	    }

	    public bool TryGetKey(TNodeType node, out int key)
	    {
	        int tmpKey;
	        if (!_indices.TryGetValue(node, out tmpKey))
	        {
	            key = -1;
	            return false;
	        }

	        key = tmpKey;
	        return true;
	    }

		public bool TryExtractMin(out TNodeType node)
		{
		    if (_ubound == -1)
		    {
		        node = default(TNodeType);
		        return false;
		    }

			var ret = _nodes[0];
			_nodes[0] = _nodes[_ubound];
		    _keys[0] = _keys[_ubound];
		    _ubound -= 1;
			_BalanceDown(0);
			node = ret;
		    return true;
		}

	    public void DecreaseKey(TNodeType node, int newKey, TMetadataType metadata = default(TMetadataType))
	    {
	        var index = _indices[node];
	        var curKey = _keys[index];

	        if (curKey.Item1 == newKey)
	        {
	            return;
	        }

	        if (newKey > curKey.Item1)
	        {
	            return;
	        }

	        _keys[index] = Tuple.Create(newKey, metadata);
            _BalanceUp(index);
	    }

	    public bool ContainsNode(TNodeType item)
	    {
	        return _indices.ContainsKey(item);
	    }

		private void _BalanceDown(int index)
		{
			int leftChild = index*2 + 1;
			int rightChild = index*2 + 2;

			if (leftChild > _ubound)
			{
				return;
			}

			// determine which child node is smaller
			int indexToSwap = rightChild > _ubound 
					? leftChild 
					: _keys[leftChild].Item1 < _keys[rightChild].Item1 ? leftChild : rightChild;

			// compare the current node with the smaller of the two child nodes
			if (_keys[index].Item1 > _keys[indexToSwap].Item1)
			{
				// if the current node is bigger than the smaller of the two children,
				// then continue to push that node down by swapping the two nodes				
                _SwapNodes(index, indexToSwap);
				_BalanceDown(indexToSwap);
			}
		}

		private void _BalanceUp (int index)
		{
			if (index == 0) {
				return;
			}
			int parentIdx = index / 2;

			if (_keys [index].Item1 < _keys [parentIdx].Item1) {
                _SwapNodes(index, parentIdx);
				_BalanceUp (parentIdx);
			}
		}

	    private void _SwapNodes(int idx1, int idx2)
	    {
            var tmpNode = _nodes [idx2];
            _nodes [idx2] = _nodes [idx1];
            _nodes [idx1] = tmpNode;

	        var tmpKey = _keys[idx2];
	        _keys[idx2] = _keys[idx1];
	        _keys[idx1] = tmpKey;

            _indices[_nodes[idx1]] = idx1;
            _indices[_nodes[idx2]] = idx2;
	    }
	}
}

