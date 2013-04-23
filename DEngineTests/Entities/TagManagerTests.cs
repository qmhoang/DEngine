using System;
using System.Collections.Generic;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests.Entities {
	[TestFixture]
	public class TagManagerTests {
		private EntityManager _manager;
		private TagManager<string> _tagManager;

		[SetUp]
		public void SetUp() {
			_manager = new EntityManager();
			_tagManager = new TagManager<string>();
		}

		[Test]
		public void TestGettersAndIndexers() {
			var entity = _manager.Create();
			_tagManager.Register("player", entity);
			_tagManager.Register("player1", entity);

			Assert.AreEqual(entity, _tagManager.GetEntity("player"));
			Assert.AreEqual(entity, _tagManager.GetEntity("player1"));

			Assert.AreEqual(entity, _tagManager["player"]);
			Assert.AreEqual(entity, _tagManager["player1"]);

			//test failures
			Assert.Throws<KeyNotFoundException>(delegate { var e = _tagManager["fail"]; });
			Assert.Throws<KeyNotFoundException>(delegate { var e = _tagManager.GetEntity("fail"); });
		}

		[Test]
		public void TestRegister() {
			var entity = _manager.Create();
			_tagManager.Register("player", entity);
			_tagManager.Register("player1", entity);			

			Assert.IsTrue(_tagManager.IsRegistered("player"));
			Assert.IsTrue(_tagManager.IsRegistered("player1"));

			//test failures
			Assert.IsFalse(_tagManager.IsRegistered("fail"));

			var entity1 = _manager.Create();
			Assert.AreNotSame(entity, entity1);
			Assert.AreNotEqual(entity, entity1);
		}

		[Test]
		public void TestReregister() {
			var e1 = _manager.Create();
			var e2 = _manager.Create();
			_tagManager.Register("player", e1);

			Assert.AreSame(_tagManager["player"], e1);

			_tagManager.Register("player", e2);

			Assert.AreSame(_tagManager["player"], e2);

		}

		[Test]
		public void TestHasTags() {
			var e1 = _manager.Create();
			var e2 = _manager.Create();

			_tagManager.Register("tagged", e1);

			Assert.IsTrue(_tagManager.IsRegistered("tagged"));
			Assert.IsTrue(_tagManager.HasTags(e1));
			Assert.IsFalse(_tagManager.HasTags(e2));
		}

		[Test]
		public void TestUnregister() {
			var entity = _manager.Create();
			_tagManager.Register("player", entity);

			Assert.IsTrue(_tagManager.IsRegistered("player"));

			_tagManager.Unregister("player");

			Assert.IsFalse(_tagManager.IsRegistered("player"));

			Assert.DoesNotThrow(delegate { _tagManager.Unregister("fail"); });
		}
	}

}
