using System;
using System.Collections.Generic;

namespace DataTypes
{
	/// <summary>
	/// A priority queue, implemented as a min heap.
	/// </summary>
	public class PriorityQueue<T>
	{
		internal readonly List<T> _nodes = new List<T> ();
	    internal readonly List<int> _keys = new List<int>();

        // Using this as a way to look up a node's index.
	    private readonly Dictionary<T, int> _indices = new Dictionary<T, int>();

		private int _ubound = -1;

		public void Insert (T item, int key)
		{
		    int addedAt; 
			if (_ubound == _nodes.Count - 1) {
				_nodes.Add (item);
                _keys.Add(key);
			    addedAt = 0;
			} else {
				_nodes [_ubound] = item;
			    _keys[_ubound] = key;
			    addedAt = _ubound;
			}

            _indices.Add(item, addedAt);
			_BalanceUp (++_ubound);
		}

		public T ExtractMin()
		{
			var ret = _nodes[0];
			_nodes[0] = _nodes[_ubound];
		    _keys[0] = _keys[_ubound];
		    _ubound -= 1;
			_BalanceDown(0);
			return ret; 
		}

	    public void DecreaseKey(T current, int newKey)
	    {
	        var index = _indices[current];
	        var curKey = _keys[index];

	        if (curKey == newKey)
	        {
	            return;
	        }

	        if (newKey > curKey)
	        {
	            throw new ArgumentException("newKey must have a lower value.");
	        }

	        _keys[index] = newKey;
            _BalanceUp(index);
	    }

	    public bool Contains(T item)
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
					: _keys[leftChild] < _keys[rightChild] ? leftChild : rightChild;

			// compare the current node with the smaller of the two child nodes
			if (_keys[index] > _keys[indexToSwap])
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

			if (_keys [index] < _keys [parentIdx]) {
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

