using System;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;
using System.Collections.Generic;

namespace TokyoChokoku.PatmarkTest.EmbossmentKit
{
    [TestFixture()]
    public class PMQualityTest
    {

        private PMQuality Default = PMQuality.Default;

        private PMQuality Create(int value) => PMQuality.Create(value);
        private PMQuality CreateFromBinary(int value) => PMQuality.CreateFromBinary(value);

        private PMQuality CreateOrDefault(int value) => PMQuality.CreateOrDefault(value);
        private PMQuality CreateFromBinaryOrDefault(int value) => PMQuality.CreateFromBinaryOrDefault(value);

        [Test()]
        public void UT_CreateTest()
        {
            Assert.Throws<ArgumentException>(() => Create(0));
            Assert.AreEqual(1, Create(1).Value);
            Assert.AreEqual(9, Create(9).Value);
            Assert.Throws<ArgumentException>(() => Create(10));
        }

        [Test()]
        public void UT_CreateFromBinaryTest()
        {
            Assert.Throws<ArgumentException>(() => CreateFromBinary(-1));
            Assert.AreEqual(Create(1), CreateFromBinary(0));
            Assert.AreEqual(Create(9), CreateFromBinary(8));
            Assert.Throws<ArgumentException>(() => CreateFromBinary(9));
        }

        [Test()]
        public void UT_CreateOrDefaultTest()
        {
            var d = Default.Value;
            Assert.AreEqual(d, CreateOrDefault(0).Value);
            Assert.AreEqual(1, CreateOrDefault(1).Value);
            Assert.AreEqual(9, CreateOrDefault(9).Value);
            Assert.AreEqual(d, CreateOrDefault(10).Value);
        }

        [Test()]
        public void UT_CreateFromBinaryOrDefaultTest()
        {
            var d = Default.Value;
            Assert.AreEqual(d, CreateFromBinaryOrDefault(-1).Value);
            Assert.AreEqual(1, CreateFromBinaryOrDefault(0).Value);
            Assert.AreEqual(9, CreateFromBinaryOrDefault(8).Value);
            Assert.AreEqual(d, CreateFromBinaryOrDefault(9).Value);
        }

        [Test()]
        public void UT_CompareTest_A()
        {
            var a = Create(3);
            var b = Create(4);

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, a);

            Assert.IsTrue(a < b);
            Assert.IsTrue(a <= b);
            Assert.IsFalse(a > b);
            Assert.IsFalse(a >= b);

            Assert.IsFalse(b < a);
            Assert.IsFalse(b <= a);
            Assert.IsTrue(b > a);
            Assert.IsTrue(b >= a);
        }

        [Test()]
        public void UT_CompareTest_B()
        {
            var a = Create(7);
            var b = Create(7);

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);

            Assert.IsFalse(a < b);
            Assert.IsTrue(a <= b);
            Assert.IsFalse(a > b);
            Assert.IsTrue(a >= b);

            Assert.IsFalse(b < a);
            Assert.IsTrue(b <= a);
            Assert.IsFalse(b > a);
            Assert.IsTrue(b >= a);
        }


        [Test()]
        public void UT_MinTest()
        {
            var list = new List<PMQuality> {
                Create(3),
                Create(1),
                Create(5),
                Create(2)
            };

            var result = list.Min();

            Assert.AreEqual(1, result?.ToInt());
            Assert.AreEqual(Create(1), result);
        }


        [Test()]
        public void UT_MaxTest()
        {
            var list = new List<PMQuality> {
                Create(3),
                Create(1),
                Create(5),
                Create(2)
            };

            var result = list.Max();

            Assert.AreEqual(5, result?.ToInt());
            Assert.AreEqual(Create(5), result);
        }
    }
}
