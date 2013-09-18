using System;
using System.Collections.Generic;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests.Entities {
	[TestFixture]
	public class GroupManagerTests {
		private EntityManager _manager;
		private GroupManager<string> _groupManager;

		[SetUp]
		public void SetUp() {
			_manager = new EntityManager();
			_groupManager = new GroupManager<string>();
		}

		[Test]
		public void TestSets() {
			var entity = _manager.Create();
			Assert.IsFalse(_groupManager.IsGrouped(entity));

			_groupManager.Set("group", entity);
			Assert.IsTrue(_groupManager.IsGrouped(entity));
			CollectionAssert.Contains(_groupManager.GetEntities("group"), entity);

			_groupManager.Set("", entity);
			Assert.IsTrue(_groupManager.IsGrouped(entity));
			CollectionAssert.Contains(_groupManager.GetEntities(""), entity);
			
			Assert.Throws<ArgumentNullException>(() => _groupManager.Set("1", null));
		}

		[Test]
		public void TestRemove() {
			var entity = _manager.Create();
			_groupManager.Set("group", entity);
			Assert.IsTrue(_groupManager.IsGrouped(entity));

			_groupManager.Remove(entity);
			Assert.IsFalse(_groupManager.IsGrouped(entity));

			Assert.Throws<ArgumentNullException>(() => _groupManager.Remove(null));
		}

		[Test]
		public void TestGetters() {
			var entity = _manager.Create();

			Assert.IsFalse(_groupManager.IsValidGroup("group"));
			_groupManager.Set("group", entity);
			Assert.IsTrue(_groupManager.IsValidGroup("group"));

			Assert.AreEqual(_groupManager.GetGroupOf(entity), "group");
			Assert.AreNotEqual(_groupManager.GetGroupOf(entity), "group2");

			Assert.IsTrue(_groupManager.IsValidGroup("group"));			
			_groupManager.Set("group2", entity);
			Assert.IsFalse(_groupManager.IsValidGroup("group"));

			Assert.AreNotEqual(_groupManager.GetGroupOf(entity), "group");
			Assert.AreEqual(_groupManager.GetGroupOf(entity), "group2");

			Assert.Throws<ArgumentNullException>(() => _groupManager.GetGroupOf(null));
			
			//test failures
			_groupManager.Remove(entity);
			Assert.Throws<KeyNotFoundException>(delegate { var g = _groupManager.GetGroupOf(entity); });			
		}

		[Test]
		public void TestGetEntities() {
			var list = new List<Entity>();

			list.Add(_manager.Create());
			list.Add(_manager.Create());
			list.Add(_manager.Create());
			list.Add(_manager.Create());


			foreach (var entity in list) {
				_groupManager.Set("group", entity);
			}

			CollectionAssert.AreEqual(_groupManager.GetEntities("group"), list);
			CollectionAssert.IsEmpty(_groupManager.GetEntities("nogroup"));

			CollectionAssert.DoesNotContain(_groupManager.GetEntities("group"), _manager.Create());
		}
	}
}
