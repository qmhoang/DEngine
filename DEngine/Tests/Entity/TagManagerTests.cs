using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngine.Tests.Entity {
	[TestFixture]
	public class TagManagerTests {
		private EntityManager manager;
		private TagManager tagManager;

		[SetUp]
		public void SetUp() {
			manager = new EntityManager();
			tagManager = new TagManager();
		}

		[Test]
		public void TestGettersAndIndexers() {
			var entity = manager.Create();
			tagManager.Register("player", entity);
			tagManager.Register("player1", entity);

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
			tagManager.Register("player", entity);
			tagManager.Register("player1", entity);			

			Assert.IsTrue(tagManager.IsRegistered("player"));
			Assert.IsTrue(tagManager.IsRegistered("player1"));

			//test failures
			Assert.IsFalse(tagManager.IsRegistered("fail"));

			var entity1 = manager.Create();
			Assert.AreNotSame(entity, entity1);
			Assert.AreNotEqual(entity, entity1);

			Assert.Throws<ArgumentException>(delegate { tagManager["player"] = entity; });
			Assert.Throws<ArgumentException>(delegate { tagManager.Register("player1", entity); });
		}

		[Test]
		public void TestUnregister() {
			var entity = manager.Create();
			tagManager.Register("player", entity);

			Assert.IsTrue(tagManager.IsRegistered("player"));

			tagManager.Unregister("player");

			Assert.IsFalse(tagManager.IsRegistered("player"));

			Assert.DoesNotThrow(delegate { tagManager.Unregister("fail"); });
		}
	}

}
