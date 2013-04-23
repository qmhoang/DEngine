using System;
using System.Collections.Generic;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests.Entities {
	[TestFixture]
	public class GroupManagerTests {
		private EntityManager manager;
		private GroupManager<string> groupManager;

		[SetUp]
		public void SetUp() {
			manager = new EntityManager();
			groupManager = new GroupManager<string>();
		}

		[Test]
		public void TestSets() {
			var entity = manager.Create();
			Assert.IsFalse(groupManager.IsGrouped(entity));

			groupManager.Set("group", entity);
			Assert.IsTrue(groupManager.IsGrouped(entity));
			CollectionAssert.Contains(groupManager.GetEntities("group"), entity);

			groupManager.Set("", entity);
			Assert.IsTrue(groupManager.IsGrouped(entity));
			CollectionAssert.Contains(groupManager.GetEntities(""), entity);
			
			Assert.Throws<ArgumentNullException>(() => groupManager.Set("1", null));
		}

		[Test]
		public void TestRemove() {
			var entity = manager.Create();
			groupManager.Set("group", entity);
			Assert.IsTrue(groupManager.IsGrouped(entity));

			groupManager.Remove(entity);
			Assert.IsFalse(groupManager.IsGrouped(entity));

			Assert.Throws<ArgumentNullException>(() => groupManager.Remove(null));
		}

		[Test]
		public void TestGetters() {
			var entity = manager.Create();

			Assert.IsFalse(groupManager.IsValidGroup("group"));
			groupManager.Set("group", entity);
			Assert.IsTrue(groupManager.IsValidGroup("group"));

			Assert.AreEqual(groupManager.GetGroupOf(entity), "group");
			Assert.AreNotEqual(groupManager.GetGroupOf(entity), "group2");

			Assert.IsTrue(groupManager.IsValidGroup("group"));			
			groupManager.Set("group2", entity);
			Assert.IsFalse(groupManager.IsValidGroup("group"));

			Assert.AreNotEqual(groupManager.GetGroupOf(entity), "group");
			Assert.AreEqual(groupManager.GetGroupOf(entity), "group2");

			Assert.Throws<ArgumentNullException>(() => groupManager.GetGroupOf(null));
			
			//test failures
			groupManager.Remove(entity);
			Assert.Throws<KeyNotFoundException>(delegate { var g = groupManager.GetGroupOf(entity); });			
		}

		[Test]
		public void TestGetEntities() {
			var list = new List<DEngine.Entities.Entity>();

			list.Add(manager.Create());
			list.Add(manager.Create());
			list.Add(manager.Create());
			list.Add(manager.Create());


			foreach (var entity in list) {
				groupManager.Set("group", entity);
			}

			CollectionAssert.AreEqual(groupManager.GetEntities("group"), list);
			CollectionAssert.IsEmpty(groupManager.GetEntities("nogroup"));

			CollectionAssert.DoesNotContain(groupManager.GetEntities("group"), manager.Create());
		}
	}
}
