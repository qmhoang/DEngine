using DEngine.Actor;
using NUnit.Framework;
using System;

namespace DEngineTests.Core
{
	[TestFixture]
	public class APTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestNegativeAP()
		{
			var ap = new AP(-1);
		}
	}
}