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
			var entity = manager.Create();
			Assert.IsFalse(groupManager.IsGrouped(entity));

			groupManager.Set("group", entity);
			Assert.IsTrue(groupManager.IsGrouped(entity));

			Assert.Throws<ArgumentException>(() => groupManager.Set("", entity));
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
			groupManager.Set("group", entity);

			Assert.AreEqual(groupManager.GetGroupOf(entity), "group");
			Assert.AreNotEqual(groupManager.GetGroupOf(entity), "group2");

			groupManager.Set("group2", entity);

			Assert.AreNotEqual(groupManager.GetGroupOf(entity), "group");
			Assert.AreEqual(groupManager.GetGroupOf(entity), "group2");

			Assert.Throws<ArgumentNullException>(() => groupManager.GetGroupOf(null));
		}

		[Test]
		public void TestGetEntities() {
			var list = new List<DEngine.Entity.Entity>();

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
