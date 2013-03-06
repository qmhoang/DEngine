﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core;
using NUnit.Framework;

namespace DEngineTests {
	[TestFixture]
	internal class PointTests {
		[Test]
		public static void TestCreationAndAccess() {
			Point p = new Point(1, 2);
			Assert.AreEqual(p.X, 1);
			Assert.AreEqual(p.Y, 2);
		}

		[Test]
		public static void TestDirectionsAndComparison() {
			Assert.IsTrue(Point.Zero == new Point(0, 0));
			Assert.IsTrue(Point.One == new Point(1, 1));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public static void TestDivideByZero() {
			var p = new Point(5, 2);

			var result = p / 0;
		}

		[Test]
		public void TestMultiplyMaxInt32() {
			var p = new Point(5, -2);
			var result = p * int.MaxValue;

			Assert.AreEqual(result.X, 2147483643);	// overflow on 32 bit integer
			Assert.AreEqual(result.Y, 2);
		}

		[Test]
		public void TestStruct() {
			var i = new Point(0, 0);
			var j = i;

			j.X++;

			Assert.AreNotEqual(i, j);
		}

		[Test]
		public static void TestMinorMathCases() {
			Point p1 = new Point(111, 222);
			Point p2 = new Point(333, 444);

			Point r1 = p1 + p2;
			Assert.IsTrue(r1.X == 444 && r1.Y == 666);

			Point r2 = p1 - p2;
			Assert.IsTrue(r2.X == -222 && r2.Y == -222);

			Assert.AreEqual(p1 * 3, new Point(333, 666));
			Assert.AreEqual(p2 * -1, new Point(-333, -444));
		}

		public static IEnumerable AddCases {
			get {
				yield return new TestCaseData(new Point(111, 222), new Point(333, 444)).Returns(new Point(444, 666));
				yield return new TestCaseData(new Point(300, 400), new Point(100, 200)).Returns(new Point(400, 600));
				yield return new TestCaseData(new Point(-111, 222), new Point(333, -444)).Returns(new Point(222, -222));
				yield return new TestCaseData(new Point(111, 222), Point.Zero).Returns(new Point(111, 222));
			}
		}

		[Test, TestCaseSource("AddCases")]
		public static Point TestAddition(Point p1, Point p2) {
			return p1 + p2;
		}


		public static IEnumerable SubCases {
			get {
				yield return new TestCaseData(new Point(111, 222), new Point(333, 444)).Returns(new Point(-222, -222));
				yield return new TestCaseData(new Point(300, 400), new Point(100, 200)).Returns(new Point(200, 200));
				yield return new TestCaseData(new Point(-111, 222), new Point(333, -444)).Returns(new Point(-444, 666));
				yield return new TestCaseData(new Point(111, 222), Point.Zero).Returns(new Point(111, 222));
			}
		}


		[Test, TestCaseSource("SubCases")]
		public static Point TestSubstraction(Point p1, Point p2) {
			return p1 - p2;
		}

		public static IEnumerable MultCases {
			get {
				yield return new TestCaseData(new Point(111, 222), 1).Returns(new Point(111, 222));
				yield return new TestCaseData(new Point(300, 400), -1).Returns(new Point(-300, -400));
				yield return new TestCaseData(new Point(-111, 222), 0).Returns(new Point(-0, 0));
				yield return new TestCaseData(new Point(111, 222), 154).Returns(new Point(17094, 34188));
			}
		}

		[Test, TestCaseSource("MultCases")]
		public static Point TestScalarMult(Point p, int scalar) {
			return p * scalar;
		}

		public static IEnumerable DivCases {
			get {
				yield return new TestCaseData(new Point(111, 222), 1).Returns(new Point(111, 222));
				yield return new TestCaseData(new Point(300, 400), -1).Returns(new Point(-300, -400));
				yield return new TestCaseData(new Point(-111, 222), 0).Throws(typeof(DivideByZeroException)).SetName("DivideByZero");
				yield return new TestCaseData(new Point(111, 222), 111).Returns(new Point(1, 2));
			}
		}

		[Test, TestCaseSource("DivCases")]
		public static Point TestScalarDiv(Point p, int scalar) {
			return p / scalar;
		}

		public static IEnumerable LengthCases {
			get {
				yield return new TestCaseData(new Point(3, 4)).Returns(5);
				yield return new TestCaseData(new Point(3, 6)).Returns(6.7082039324993690892275210061938);
				yield return new TestCaseData(new Point(5, 10)).Returns(11.180339887498948482045868343656);
				yield return new TestCaseData(new Point(1, 10)).Returns(10.04987562112089027021926491276);
			}
		}
	
		[TestCaseSource("LengthCases")]
		public static double TestLength(Point point) {
			return point.Length;
		}

		public static IEnumerable LengthSquaredCases {
			get {
				yield return new TestCaseData(new Point(3, 4)).Returns(25);
				yield return new TestCaseData(new Point(3, 6)).Returns(45);
				yield return new TestCaseData(new Point(5, 10)).Returns(125);
				yield return new TestCaseData(new Point(1, 10)).Returns(101);
			}
		}

		[TestCaseSource("LengthSquaredCases")]
		public static double TestLengthSquared(Point point) {
			return point.LengthSquared;
		}

		public static IEnumerable DistanceToCases {
			get {
				yield return new TestCaseData(new Point(3, 4), new Point(0, 0)).Returns(5);
				yield return new TestCaseData(new Point(3, 6), new Point(7, 3)).Returns(5);
			}
		}

		[TestCaseSource( "DistanceToCases")]
		public static double TestDistanceTo(Point p1, Point p2) {
			return p1.DistanceTo(p2);
		}

		public static IEnumerable CircleCases {
			get {
				yield return new TestCaseData(new Point(1, 2), new Point(0, 0), 3).Returns(true);
				yield return new TestCaseData(new Point(3, 2), new Point(0, 0), 3).Returns(false);
				yield return new TestCaseData(new Point(111, 222), new Point(333, 444), 300).Returns(false);
				yield return new TestCaseData(new Point(111, 222), new Point(333, 444), 543).Returns(true);
				yield return new TestCaseData(new Point(0, 0), new Point(0, 0), 0).Returns(true);
			}
		}

		[TestCaseSource("CircleCases")]
		public static bool TestInCircle(Point p, Point origin, int radius) {
			return p.IsInCircle(origin, radius);
		}

		public static IEnumerable ShiftCases {
			get {
				yield return new TestCaseData(new Point(111, 222), 333, 444).Returns(new Point(444, 666));
				yield return new TestCaseData(new Point(300, 400), 100, 200).Returns(new Point(400, 600));
				yield return new TestCaseData(new Point(-111, 222), 333, -444).Returns(new Point(222, -222));
				yield return new TestCaseData(new Point(111, 222), 0, 0).Returns(new Point(111, 222));
			}
		}

		[TestCaseSource("ShiftCases")]
		public static Point TestShift(Point p, int x, int y) {
			return p.Shift(x, y);
		}
	}
}