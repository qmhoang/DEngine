using DEngine.Components;
using DEngine.Entities;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	public class EntityFactoryTests {
		private EntityFactory ef;
		private EntityManager em;

		[SetUp]
		public void SetUp() {
			ef = new EntityFactory();
			em = new EntityManager();
		}

		[Test]
		public void TestInheritance() {
			ef.Add("base", new Identifier("Item"), new Sprite("base", 1));
			ef.Inherits("inherited", "base", new Sprite("inherited", 2));
			ef.Compile();

			var anObject = ef.Create("base", em);
			Assert.NotNull(anObject);
			Assert.IsTrue(anObject.Has<ReferenceId>());
			Assert.AreEqual(anObject.Get<ReferenceId>().RefId, "base");
			Assert.AreEqual(anObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anObject.Get<Sprite>().Asset, "base");


			var anotherObject = ef.Create("inherited", em);
			Assert.NotNull(anotherObject);
			Assert.IsTrue(anotherObject.Has<ReferenceId>());
			Assert.AreEqual(anotherObject.Get<ReferenceId>().RefId, "inherited");
			Assert.AreEqual(anotherObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anotherObject.Get<Sprite>().Asset, "inherited");
		}

		[Test]
		public void TestIdentity() {
			var i1 = ef.Create("item", em);
			var i2 = ef.Create("item", em);

			Assert.AreNotSame(i1, i2);
		}
	}
}
