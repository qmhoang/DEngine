//using DEngine.Entities;
//using NUnit.Framework;
//
//namespace DEngineTests.Entities {
//	[TestFixture]
//	public class EntityTests {
//		private Entity entity;
//		private EntityManager manager;
//
//		private class A : Component {
//			public virtual string Get { get { return "A"; } }
//			public override Component Copy() {
//				throw new System.NotImplementedException();
//			}
//		}
//
//		private class B : A {
//			public override string Get { get { return "B"; } }
//		}
//
//		private class C : A {
//			public override string Get { get { return "C"; } }
//		}
//
//		[SetUp]
//		public void SetUp() {
//			manager = new EntityManager();
//			entity = manager.Create();			
//		}
//
//		[Test]
//		public void TestGetBase() {
//			entity.Add(new B());
//
//			Assert.IsTrue(entity.Has<B>());
//			Assert.IsTrue(entity.Has<A>());
//			Assert.IsFalse(entity.Has<C>());
//		}
//	}
//}