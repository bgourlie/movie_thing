using NUnit.Framework;
using System.Collections.Generic;

namespace DataTypes.Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test]
		public void TestCase1 ()
		{
			var h = new Heap<int> ();
			h.Add (3);
			h.Add (2);
			h.Add (1);
			CollectionAssert.AreEqual (new List<int> {1, 2, 3}, h._arr);
		}

		[Test]
		public void TestCase2 ()
		{
			var h = new Heap<int> ();
			h.Add (312);
			h.Add (56);
			h.Add (1);
			h.Add (57);
			h.Add (3);
			h.Add (2);
			h.Add (14);
			h.Add (1);
			h.Add (6);
			h.Add (24);
			h.Add (789);
			h.Add (67);
			h.Add (99);
			h.Add (104);
			h.Add (103);
			h.Add (97);
			CollectionAssert.AreEqual (new List<int> {1, 1, 3, 2, 6, 56, 57, 14, 312, 24, 789, 67, 99, 104, 103, 97}, h._arr);
		}
	}
}

