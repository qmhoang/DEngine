using System;
using System.Collections.Generic;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests.Entity {
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

			Assert.Throws<ArgumentException>(delegate { tagManager["player"] = entity; });
			Assert.Throws<ArgumentException>(delegate { tagManager.Register(entity, "player1"); });
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
