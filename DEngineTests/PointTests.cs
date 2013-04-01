using System;
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
		public void TestZero() {
			Point v = Point.Zero;

			Assert.AreEqual(0, v.X);
			Assert.AreEqual(0, v.Y);
		}

		[Test]
		public void TestOne() {
			Point v = Point.One;

			Assert.AreEqual(1, v.X);
			Assert.AreEqual(1, v.Y);
		}

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

		#region Operators
		public static IEnumerable AddCases {
			get {
				yield return new TestCaseData(new Point(111, 222), new Point(333, 444)).Returns(new Point(444, 666));
				yield return new TestCaseData(new Point(300, 400), new Point(100, 200)).Returns(new Point(400, 600));
				yield return new TestCaseData(new Point(-111, 222), new Point(333, -444)).Returns(new Point(222, -222));
				yield return new TestCaseData(new Point(111, 222), Point.Zero).Returns(new Point(111, 222));

				yield return new TestCaseData(new Point(1, 2), new Point(2, 2)).Returns(new Point(3, 4));
				yield return new TestCaseData(new Point(1, 2), new Point(-1, -2)).Returns(new Point(0, 0));
				yield return new TestCaseData(new Point(4, 0), new Point(0, 5)).Returns(new Point(4, 5));
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

				yield return new TestCaseData(new Point(1, 2), new Point(2, 2)).Returns(new Point(-1, 0));
				yield return new TestCaseData(new Point(1, 2), new Point(-1, -2)).Returns(new Point(2, 4));
				yield return new TestCaseData(new Point(4, 0), new Point(0, 5)).Returns(new Point(4, -5));
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

				yield return new TestCaseData(new Point(1, 2), 2).Returns(new Point(2, 4));
				yield return new TestCaseData(new Point(1, 2), -1).Returns(new Point(-1, -2));
				yield return new TestCaseData(new Point(4, 5), 0).Returns(new Point(0, 0));
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

				yield return new TestCaseData(new Point(1, 2), 2).Returns(new Point(0, 1));
				yield return new TestCaseData(new Point(6, -9), -3).Returns(new Point(-2, 3));
				yield return new TestCaseData(new Point(4, -5), 1).Returns(new Point(4, -5));
			}
		}
		
		[Test, TestCaseSource("DivCases")]
		public static Point TestScalarDiv(Point p, int scalar) {
			return p / scalar;
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void TestOperatorPointDividedByZeroThrows() {
			Point dummy = new Point(1, 3) / 0;
		}
		
		public static IEnumerable LengthCases {
			get {
				yield return new TestCaseData(new Point(3, 4)).Returns(5);
				yield return new TestCaseData(new Point(3, 6)).Returns(6.7082039324993690892275210061938);
				yield return new TestCaseData(new Point(5, 10)).Returns(11.180339887498948482045868343656);
				yield return new TestCaseData(new Point(1, 10)).Returns(10.04987562112089027021926491276);

				yield return new TestCaseData(new Point(0, 0)).Returns(0);
				yield return new TestCaseData(new Point(1, 0)).Returns(1);
				yield return new TestCaseData(new Point(0, -1)).Returns(1);
				yield return new TestCaseData(new Point(1, 1)).Returns(Math.Sqrt(2));
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

				yield return new TestCaseData(new Point(0, 0)).Returns(0);
				yield return new TestCaseData(new Point(1, 0)).Returns(1);
				yield return new TestCaseData(new Point(0, -1)).Returns(1);
				yield return new TestCaseData(new Point(1, 1)).Returns(2);				
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
		#endregion

		#region Equals
		[Test]
		public void TestOperatorEquals() {
			Assert.AreEqual(true, new Point(1, 2) == new Point(1, 2));
			Assert.AreEqual(true, new Point(1, 1) == Point.One);
			Assert.AreEqual(false, new Point(1, 1) == Point.Zero);
			Assert.AreEqual(false, new Point(3, 4) == new Point(4, 5));

			Assert.AreEqual(false, new Point(3, 4) == null);
			Assert.AreEqual(false, null == new Point(3, 4));
		}

		[Test]
		public void TestOperatorNotEquals() {
			Assert.AreEqual(false, new Point(1, 2) != new Point(1, 2));
			Assert.AreEqual(false, new Point(1, 1) != Point.One);
			Assert.AreEqual(true, new Point(1, 1) != Point.Zero);
			Assert.AreEqual(true, new Point(3, 4) != new Point(4, 5));

			Assert.AreEqual(true, new Point(3, 4) != null);
			Assert.AreEqual(true, null != new Point(3, 4));
		}

		[Test]
		public void TestEquals() {
			Point v1 = new Point(3, 5);
			Point v2 = new Point(3, 5);
			Point v3 = new Point(4, 5);
			Point v4 = new Point(0, 0);
			Point v5 = Point.Zero;

			object obj2 = v2;

			// typed Equals
			Assert.IsTrue(v1.Equals(v1));
			Assert.IsTrue(v1.Equals(v2));
			Assert.IsFalse(v1.Equals(v3));
			Assert.IsFalse(v3.Equals(v4));
			Assert.IsTrue(v4.Equals(v5));

			// object Equals
			Assert.IsTrue(v1.Equals(obj2));
			Assert.IsTrue(obj2.Equals(v1));
			Assert.IsFalse(v3.Equals(obj2));

			// null
			Assert.IsFalse(v1.Equals(null));
		}
		
		#endregion

		[TestCaseSource( "DistanceToCases")]
		public static double TestDistanceTo(Point p1, Point p2) {
			return p1.DistanceTo(p2);
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

		[Test]
		public void TestX() {
			Assert.AreEqual(3, new Point(3, 2).X);
			Assert.AreEqual(-4, new Point(-4, 0).X);
		}

		[Test]
		public void TestY() {
			Assert.AreEqual(3, new Point(2, 3).Y);
			Assert.AreEqual(-4, new Point(0, -4).Y);
		}

		[Test]
		public void TestToString() {
			Assert.AreEqual("(X=0, Y=1)", new Point(0, 1).ToString());
			Assert.AreEqual("(X=17, Y=5)", new Point(17, 5).ToString());
			Assert.AreEqual("(X=38274, Y=-4273)", new Point(38274, -4273).ToString());
		}

		[Test]
		public void TestGetHashCode() {
			// make sure the hash code is consistent
			Assert.AreEqual(new Point(0, 0).GetHashCode(), new Point(0, 0).GetHashCode());
			Assert.AreEqual(new Point(1, 3).GetHashCode(), new Point(1, 3).GetHashCode());
			Assert.AreEqual(new Point(15, -123).GetHashCode(), new Point(15, -123).GetHashCode());

			// just check for a few obvious collisions
			Assert.AreNotEqual(new Point(0, 0).GetHashCode(), new Point(0, 1).GetHashCode());
			Assert.AreNotEqual(new Point(0, 1).GetHashCode(), new Point(1, 0).GetHashCode());
			Assert.AreNotEqual(new Point(100, -1).GetHashCode(), new Point(100, 1).GetHashCode());
		}

		[Test]
		public void TestIsAdjacentTo() {
			Assert.IsTrue(new Point(2, 5).IsAdjacentTo(new Point(2, 6)));
			Assert.IsTrue(new Point(2, 5).IsAdjacentTo(new Point(1, 4)));
			Assert.IsTrue(new Point(2, 5).IsAdjacentTo(new Point(1, 5)));

			Assert.IsFalse(new Point(2, 5).IsAdjacentTo(new Point(2, 3)));
			Assert.IsFalse(new Point(2, 5).IsAdjacentTo(new Point(1, 3)));
			Assert.IsFalse(new Point(2, 5).IsAdjacentTo(new Point(2, 5)));
			Assert.IsFalse(new Point(2, 5).IsAdjacentTo(new Point(-1, 0)));
		}

		[Test]
		public void TestShift() {
			Assert.AreEqual(new Point(2, 5), new Point(1, 3).Shift(1, 2));
			Assert.AreEqual(new Point(0, 0), new Point(1, 3).Shift(-1, -3));
			Assert.AreEqual(new Point(3, 4), new Point(3, 4).Shift(0, 0));

			// make sure the original is not changed
			Point v = new Point(2, 3);
			Point offset = v.Shift(1, 5);

			Assert.AreEqual(2, v.X);
			Assert.AreEqual(3, v.Y);
		}

		[Test]
		public void TestShiftX() {
			Assert.AreEqual(new Point(2, 3), new Point(1, 3).ShiftX(1));
			Assert.AreEqual(new Point(0, 3), new Point(1, 3).ShiftX(-1));
			Assert.AreEqual(new Point(3, 4), new Point(3, 4).ShiftX(0));

			// make sure the original is not changed
			Point v = new Point(2, 3);
			Point offset = v.ShiftX(1);

			Assert.AreEqual(2, v.X);
			Assert.AreEqual(3, v.Y);
		}

		[Test]
		public void TestShiftY() {
			Assert.AreEqual(new Point(1, 4), new Point(1, 3).ShiftY(1));
			Assert.AreEqual(new Point(1, 2), new Point(1, 3).ShiftY(-1));
			Assert.AreEqual(new Point(3, 4), new Point(3, 4).ShiftY(0));

			// make sure the original is not changed
			Point v = new Point(2, 3);
			Point offset = v.ShiftY(1);

			Assert.AreEqual(2, v.X);
			Assert.AreEqual(3, v.Y);
		}
	}
}