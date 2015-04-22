using NUnit.Framework;
using System.Collections.Generic;

namespace DataTypes.Tests
{
	[TestFixture]
	public class PriorityQueueTests
	{
		[Test]
		public void TestEnqueue ()
		{
			var h = new PriorityQueue<int> ();
			h.Enqueue (312);
			h.Enqueue (56);
			h.Enqueue (1);
			h.Enqueue (57);
			h.Enqueue (3);
			h.Enqueue (2);
			h.Enqueue (14);
			h.Enqueue (1);
			h.Enqueue (6);
			h.Enqueue (24);
			h.Enqueue (789);
			h.Enqueue (67);
			h.Enqueue (99);
			h.Enqueue (104);
			h.Enqueue (103);
			h.Enqueue (97);
			CollectionAssert.AreEqual (new List<int> {1, 1, 3, 2, 6, 56, 57, 14, 312, 24, 789, 67, 99, 104, 103, 97}, h._arr);
		}

		[Test]
		public void TestDequeue()
		{
			var h = new PriorityQueue<int> ();
			h.Enqueue (10);
			h.Enqueue (9);
			h.Enqueue (8);
			h.Enqueue (7);
			h.Enqueue (6);
			h.Enqueue (5);
			h.Enqueue (4);
			h.Enqueue (3);
			h.Enqueue (2);
			h.Enqueue (1);

			Assert.AreEqual(1, h.Dequeue());
			Assert.AreEqual(2, h.Dequeue());
			Assert.AreEqual(3, h.Dequeue());
			Assert.AreEqual(4, h.Dequeue());
			Assert.AreEqual(5, h.Dequeue());
			Assert.AreEqual(6, h.Dequeue());
			Assert.AreEqual(7, h.Dequeue());
			Assert.AreEqual(8, h.Dequeue());
			Assert.AreEqual(9, h.Dequeue());
			Assert.AreEqual(10, h.Dequeue());
		}
	}
}

