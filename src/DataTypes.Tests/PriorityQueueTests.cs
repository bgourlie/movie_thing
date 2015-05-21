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
			var h = new PriorityQueue<string> ();
			h.Insert ("a", 312);
			h.Insert ("b", 56);
			h.Insert ("c", 1);
			h.Insert ("d", 57);
			h.Insert ("e", 3);
			h.Insert ("f", 2);
			h.Insert ("g", 14);
			h.Insert ("h", 1);
			h.Insert ("i", 6);
			h.Insert ("j", 24);
			h.Insert ("k", 789);
			h.Insert ("l", 67);
			h.Insert ("m", 99);
			h.Insert ("n", 104);
			h.Insert ("o", 103);
			h.Insert ("p", 97);
			CollectionAssert.AreEqual (new List<int> {1, 1, 3, 2, 6, 56, 57, 14, 312, 24, 789, 67, 99, 104, 103, 97}, h._keys);
		}

		[Test]
		public void TestExtractMin()
		{
			var h = new PriorityQueue<string> ();
			h.Insert ("j", 10);
			h.Insert ("i", 9);
			h.Insert ("h", 8);
			h.Insert ("g", 7);
			h.Insert ("f", 6);
			h.Insert ("e", 5);
			h.Insert ("d", 4);
			h.Insert ("c", 3);
			h.Insert ("b", 2);
			h.Insert ("a", 1);

			Assert.AreEqual("a", h.ExtractMin());
			Assert.AreEqual("b", h.ExtractMin());
			Assert.AreEqual("c", h.ExtractMin());
			Assert.AreEqual("d", h.ExtractMin());
			Assert.AreEqual("e", h.ExtractMin());
			Assert.AreEqual("f", h.ExtractMin());
			Assert.AreEqual("g", h.ExtractMin());
			Assert.AreEqual("h", h.ExtractMin());
			Assert.AreEqual("i", h.ExtractMin());
			Assert.AreEqual("j", h.ExtractMin());
		}

		[Test]
		public void TestDecreaseKey()
		{
			var h = new PriorityQueue<string> ();
			h.Insert ("j", 10);
			h.Insert ("i", 9);
			h.Insert ("h", 8);
			h.Insert ("g", 7);
			h.Insert ("f", 6);
			h.Insert ("e", 5);
			h.Insert ("d", 4);
			h.Insert ("c", 3);
			h.Insert ("b", 2);
			h.Insert ("a", 1);
            h.DecreaseKey("i", 0);

			Assert.AreEqual("i", h.ExtractMin());
			Assert.AreEqual("a", h.ExtractMin());
		}
	}
}

