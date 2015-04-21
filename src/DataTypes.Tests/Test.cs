using NUnit.Framework;
using System;

namespace DataTypes.Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
			var h = new Heap<int> ();
			h.Add (3);
			h.Add (2);
			h.Add (1);
			var blah = h.ToString ();
			Console.WriteLine (blah);
		}
	}
}

