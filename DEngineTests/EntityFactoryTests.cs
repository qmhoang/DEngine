using System;
using DEngine.Components;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class EntityFactoryTests {
		private EntityFactory _ef;
		private EntityManager _em;

		[SetUp]
		public void SetUp() {
			_ef = new EntityFactory();
			_em = new EntityManager();

			_ef.Add("base", new Identifier("Item"), new Sprite("base", 1));
			_ef.Add("item", new Identifier("Item"), new Sprite("item", 1));
			_ef.Inherits("inherited", "base", new Sprite("inherited", 2));
			_ef.Compile();
		}

		[Test]
		public void TestAdd() {
			var anObject = _ef.Create("base", _em);
			Assert.NotNull(anObject);
			Assert.IsTrue(anObject.Has<ReferenceId>());
			Assert.AreEqual(anObject.Get<ReferenceId>().RefId, "base");
			Assert.AreEqual(anObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anObject.Get<Sprite>().Asset, "base");
		}

		[Test]
		public void TestInheritance() {
			// should not modify base
			var anObject = _ef.Create("base", _em);
			Assert.NotNull(anObject);
			Assert.IsTrue(anObject.Has<ReferenceId>());
			Assert.AreEqual(anObject.Get<ReferenceId>().RefId, "base");
			Assert.AreEqual(anObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anObject.Get<Sprite>().Asset, "base");


			var anotherObject = _ef.Create("inherited", _em);
			Assert.NotNull(anotherObject);
			Assert.IsTrue(anotherObject.Has<ReferenceId>());
			Assert.AreEqual(anotherObject.Get<ReferenceId>().RefId, "inherited");
			Assert.AreEqual(anotherObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anotherObject.Get<Sprite>().Asset, "inherited");
		}

		[Test]
		public void TestIdentity() {
			var i1 = _ef.Create("item", _em);
			var i2 = _ef.Create("item", _em);

			Assert.AreNotSame(i1, i2);
		}

		[Test]
		[ExpectedException(typeof(IllegalInheritanceException))]
		public void TestIllegalInherits() {
			EntityFactory entityFactory = new EntityFactory();

			entityFactory.Inherits("1", "2", new Identifier("blah"));

			entityFactory.Compile();
		}
	}
}
