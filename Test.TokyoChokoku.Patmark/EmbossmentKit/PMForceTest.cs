using System;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;
using System.Collections.Generic;

namespace TokyoChokoku.PatmarkTest.EmbossmentKit
{
    [TestFixture()]
    public class PMForceTest
    {
        private PMForce Default = PMForce.Default;

        private PMForce Create(int value) => PMForce.Create(value);
        private PMForce CreateFromBinary(int value) => PMForce.CreateFromBinary(value);

        private PMForce CreateOrDefault(int value) => PMForce.CreateOrDefault(value);
        private PMForce CreateFromBinaryOrDefault(int value) => PMForce.CreateFromBinaryOrDefault(value);

        [Test()]
        public void UT_CreateTest()
        {
            Assert.Throws<ArgumentException>(() => Create(-1));
            Assert.AreEqual(0, Create(0).Value);
            Assert.AreEqual(1, Create(1).Value);

            Assert.AreEqual(8, Create(8).Value);
            Assert.AreEqual(9, Create(9).Value);
            Assert.Throws<ArgumentException>(() => Create(10));
        }

        [Test()]
        public void UT_CreateFromBinaryTest()
        {
            Assert.Throws<ArgumentException>(() => CreateFromBinary( 99));
            Assert.AreEqual(Create(0), CreateFromBinary(100));
            Assert.Throws<ArgumentException>(() => CreateFromBinary(101));

            Assert.Throws<ArgumentException>(() => CreateFromBinary(-1));
            Assert.AreEqual(Create(1), CreateFromBinary(0));
            Assert.AreEqual(Create(2), CreateFromBinary(1));


            Assert.AreEqual(Create(9), CreateFromBinary(8));
            Assert.Throws<ArgumentException>(() => CreateFromBinary(9));
        }

        [Test()]
        public void UT_CreateOrDefaultTest()
        {
            var d = Default.Value;

            Assert.AreEqual(d, CreateOrDefault(-1).Value);
            Assert.AreEqual(0, CreateOrDefault( 0).Value);
            Assert.AreEqual(1, CreateOrDefault( 1).Value);


            Assert.AreEqual(8, CreateOrDefault( 8).Value);
            Assert.AreEqual(9, CreateOrDefault( 9).Value);
            Assert.AreEqual(d, CreateOrDefault(10).Value);
        }

        [Test()]
        public void UT_CreateFromBinaryOrDefaultTest()
        {
            var d = Default.Value;

            Assert.AreEqual(d, CreateFromBinaryOrDefault( 99).Value);
            Assert.AreEqual(0, CreateFromBinaryOrDefault(100).Value);
            Assert.AreEqual(d, CreateFromBinaryOrDefault(101).Value);

            Assert.AreEqual(d, CreateFromBinaryOrDefault( -1).Value);
            Assert.AreEqual(1, CreateFromBinaryOrDefault(  0).Value);
            Assert.AreEqual(2, CreateFromBinaryOrDefault(  1).Value);

            Assert.AreEqual(8, CreateFromBinaryOrDefault(  7).Value);
            Assert.AreEqual(9, CreateFromBinaryOrDefault(  8).Value);
            Assert.AreEqual(d, CreateFromBinaryOrDefault(  9).Value);
        }

        [Test()]
        public void UT_MappingTest()
        {
            var list = new List<(int, int)> {
                (0, 100),
                (1, 0),
                (2, 1),
                (3, 2),
                (4, 3),
                (5, 4),
                (6, 5),
                (7, 6),
                (8, 7),
                (9, 8),
            };

            foreach((int value, int bin) in list)
            {
                Assert.AreEqual(bin, Create         (value).ToBinary());
                Assert.AreEqual(bin, CreateOrDefault(value).ToBinary());

                Assert.AreEqual(value, CreateFromBinary         (bin).ToInt());
                Assert.AreEqual(value, CreateFromBinaryOrDefault(bin).ToInt());


                Assert.AreEqual(value, Create(value).ToInt());
                Assert.AreEqual(value, CreateOrDefault(value).ToInt());

                Assert.AreEqual(bin, CreateFromBinary(bin).ToBinary());
                Assert.AreEqual(bin, CreateFromBinaryOrDefault(bin).ToBinary());
            }
        }

        [Test()]
        public void UT_CompareTest_A()
        {
            var a = Create(3);
            var b = Create(4);

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, a);

            Assert.IsTrue (a <  b);
            Assert.IsTrue (a <= b);
            Assert.IsFalse(a >  b);
            Assert.IsFalse(a >= b);

            Assert.IsFalse(b <  a);
            Assert.IsFalse(b <= a);
            Assert.IsTrue (b >  a);
            Assert.IsTrue (b >= a);
        }

        [Test()]
        public void UT_CompareTest_B()
        {
            var a = Create(7);
            var b = Create(7);

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);

            Assert.IsFalse (a <  b);
            Assert.IsTrue  (a <= b);
            Assert.IsFalse (a >  b);
            Assert.IsTrue  (a >= b);
            
            Assert.IsFalse(b <  a);
            Assert.IsTrue (b <= a);
            Assert.IsFalse(b >  a);
            Assert.IsTrue (b >= a);
        }


        [Test()]
        public void UT_MinTest()
        {
            var list = new List<PMForce> {
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
            var list = new List<PMForce> {
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
