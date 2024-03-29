﻿using NUnit.Framework;
using System.Collections.Generic;

namespace DataTypes.Tests
{
    using System.Linq;

    [TestFixture]
	public class PriorityQueueTests
	{
		[Test]
		public void TestEnqueue ()
		{
			var h = new PriorityQueue<string, int> ();
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
			CollectionAssert.AreEqual (new List<int> {1, 1, 3, 2, 6, 56, 57, 14, 312, 24, 789, 67, 99, 104, 103, 97}, h._keys.Select(k => k.Item1));
		}

		[Test]
		public void TestExtractMin()
		{
			var h = new PriorityQueue<string, int> ();
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

		    string val;
		    h.TryExtractMin(out val);
			Assert.AreEqual("a", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("b", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("c", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("d", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("e", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("f", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("g", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("h", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("i", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("j", val);
		}

		[Test]
		public void TestDecreaseKey()
		{
			var h = new PriorityQueue<string, int> ();
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
            h.DecreaseKey("i", 0, 0);

		    string val;
		    h.TryExtractMin(out val);
			Assert.AreEqual("i", val);
		    h.TryExtractMin(out val);
			Assert.AreEqual("a", val);
		}
	}
}

