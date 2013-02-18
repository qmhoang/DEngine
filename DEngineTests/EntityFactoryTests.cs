﻿using System;
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

			ef.Add("base", new Identifier("Item"), new Sprite("base", 1));
			ef.Add("item", new Identifier("Item"), new Sprite("item", 1));
			ef.Inherits("inherited", "base", new Sprite("inherited", 2));
			ef.Compile();
		}

		[Test]
		public void Add() {
			var anObject = ef.Create("base", em);
			Assert.NotNull(anObject);
			Assert.IsTrue(anObject.Has<ReferenceId>());
			Assert.AreEqual(anObject.Get<ReferenceId>().RefId, "base");
			Assert.AreEqual(anObject.Get<Identifier>().Name, "Item");
			Assert.AreEqual(anObject.Get<Sprite>().Asset, "base");
		}

		[Test]
		public void Inheritance() {
			// should not modify base
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
		public void Identity() {
			var i1 = ef.Create("item", em);
			var i2 = ef.Create("item", em);

			Assert.AreNotSame(i1, i2);
		}

		[Test]
		[ExpectedException(typeof(IllegalInheritanceException))]
		public void IllegalInherits() {
			EntityFactory entityFactory = new EntityFactory();

			entityFactory.Inherits("1", "2", new Identifier("blah"));

			entityFactory.Compile();
		}
	}
}
