using System;
using System.Collections.Generic;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests.Entities {
	[TestFixture]
	public class TagManagerTests {
		private EntityManager manager;
		private TagManager<string> tagManager;

		[SetUp]
		public void SetUp() {
			manager = new EntityManager();
			tagManager = new TagManager<string>();
		}

		[Test]
		public void TestGettersAndIndexers() {
			var entity = manager.Create();
			tagManager.Register(entity, "player");
			tagManager.Register(entity, "player1");

			Assert.AreEqual(entity, tagManager.GetEntity("player"));
			Assert.AreEqual(entity, tagManager.GetEntity("player1"));

			Assert.AreEqual(entity, tagManager["player"]);
			Assert.AreEqual(entity, tagManager["player1"]);

			//test failures
			Assert.Throws<KeyNotFoundException>(delegate { var e = tagManager["fail"]; });
			Assert.Throws<KeyNotFoundException>(delegate { var e = tagManager.GetEntity("fail"); });
		}

		[Test]
		public void TestRegister() {
			var entity = manager.Create();
			tagManager.Register(entity, "player");
			tagManager.Register(entity, "player1");			

			Assert.IsTrue(tagManager.IsRegistered("player"));
			Assert.IsTrue(tagManager.IsRegistered("player1"));

			//test failures
			Assert.IsFalse(tagManager.IsRegistered("fail"));

			var entity1 = manager.Create();
			Assert.AreNotSame(entity, entity1);
			Assert.AreNotEqual(entity, entity1);
		}

		[Test]
		public void TestReregister() {
			var e1 = manager.Create();
			var e2 = manager.Create();
			tagManager.Register(e1, "player");

			Assert.AreSame(tagManager["player"], e1);

			tagManager.Register(e2, "player");

			Assert.AreSame(tagManager["player"], e2);

		}

		[Test]
		public void TestHasTags() {
			var e1 = manager.Create();
			var e2 = manager.Create();

			tagManager.Register(e1, "tagged");

			Assert.IsTrue(tagManager.IsRegistered("tagged"));
			Assert.IsTrue(tagManager.HasTags(e1));
			Assert.IsFalse(tagManager.HasTags(e2));
		}

		[Test]
		public void TestUnregister() {
			var entity = manager.Create();
			tagManager.Register(entity, "player");

			Assert.IsTrue(tagManager.IsRegistered("player"));

			tagManager.Unregister("player");

			Assert.IsFalse(tagManager.IsRegistered("player"));

			Assert.DoesNotThrow(delegate { tagManager.Unregister("fail"); });
		}
	}

}
