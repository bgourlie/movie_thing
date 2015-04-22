using System;
using System.Collections.Generic;

namespace DataTypes
{
	/// <summary>
	/// A priority queue, implemented as a min heap.
	/// </summary>
	public class PriorityQueue<T> where T : IComparable<T>
	{
		internal readonly List<T> _arr = new List<T> ();
		private int _ubound = -1;

		public void Enqueue (T item)
		{
			if (_ubound == _arr.Count - 1) {
				_arr.Add (item);
			} else {
				_arr [_ubound] = item;
			}

			_BalanceUp (++_ubound);
		}

		public T Dequeue()
		{
			var ret = _arr[0];
			_arr[0] = _arr[_ubound--];
			_BalanceDown(0);
			return ret; 
		}

		public void _BalanceDown(int index)
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
					: _arr[leftChild].CompareTo(_arr[rightChild]) == -1 ? leftChild : rightChild;

			// compare the current node with the smaller of the two child nodes
			if (_arr[index].CompareTo(_arr[indexToSwap]) > 0)
			{
				// if the current node is bigger than the smaller of the two children,
				// then continue to push that node down by swapping the two nodes				
				var tmp = _arr[index];
				_arr[index] = _arr[indexToSwap];
				_arr[indexToSwap] = tmp;
				_BalanceDown(indexToSwap);
			}
		}

		private void _BalanceUp (int index)
		{
			if (index == 0) {
				return;
			}
			int parentIdx = index / 2;

			if (_arr [index].CompareTo (_arr [parentIdx]) < 0) {
				var tmp = _arr [parentIdx];
				_arr [parentIdx] = _arr [index];
				_arr [index] = tmp;
				_BalanceUp (parentIdx);
			}
		}
	}
}

