using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Entity;
using NUnit.Framework;

namespace DEngine.Tests.Entity {
	[TestFixture]
	public class GroupManagerTests {
		private EntityManager manager;
		private GroupManager groupManager;

		[SetUp]
		public void SetUp() {
			manager = new EntityManager();
			groupManager = new GroupManager();
		}

		[Test]
		public void TestSets() {
			
		}
	}
}
