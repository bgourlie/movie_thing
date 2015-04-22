using System;
using System.Text;
using System.Collections.Generic;

namespace DataTypes
{
	public class Heap<T> where T : struct, IComparable<T>
	{
		internal readonly List<T> _arr = new List<T> ();
		private int _ubound = -1;

		public void Add (T item)
		{
			if (_ubound == _arr.Count - 1) {
				_arr.Add (item);
			} else {
				_arr [_ubound] = item;
			}

			_Balance (++_ubound);
		}

		private void _Balance (int index)
		{
			if (index == 0) {
				return;
			}
			int parentIdx = index / 2;

			if (_arr [index].CompareTo (_arr [parentIdx]) < 0) {
				var tmp = _arr [parentIdx];
				_arr [parentIdx] = _arr [index];
				_arr [index] = tmp;
				_Balance (parentIdx);
			}
		}
	}
}

