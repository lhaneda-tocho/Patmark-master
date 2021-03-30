using NUnit.Framework;
using System;
namespace TokyoChokoku.FieldTextStreamer.Test
{
    [TestFixture()]
    public class SerialPartTest
    {
        [Test()]
        public void ConstructorUT_異常系境界チェック()
        {
            void AssertExcept<T>(Action action)
            {
                try
                {
                    action();
                    Assert.Fail();
                } catch (Exception ex)
                {
                    Assert.AreEqual(typeof(T), ex.GetType());
                }
            }


            AssertExcept<ArgumentOutOfRangeException>(() => {
                var v = new SerialPart(0, 10000);
            });

            AssertExcept<ArgumentOutOfRangeException>(() => {
                var v = new SerialPart(0, -1);
            });

            AssertExcept<ArgumentOutOfRangeException>(() => {
                var v = new SerialPart(-1, 0);
            });

            AssertExcept<ArgumentOutOfRangeException>(() => {
                var v = new SerialPart(10, 0);
            });
        }

        [Test()]
        public void ConstructorUT_正常系境界チェック()
        {
            var i1 = new SerialPart(0, 9999);
            var i2 = new SerialPart(0,    0);
            var i3 = new SerialPart(9,    0);
            var i4 = new SerialPart(0,    0);

            Assert.AreEqual(0, i1.ID);
            Assert.AreEqual(9999, i1.Value);

            Assert.AreEqual(0, i2.ID);
            Assert.AreEqual(0, i2.Value);

            Assert.AreEqual(9, i3.ID);
            Assert.AreEqual(0, i3.Value);

            Assert.AreEqual(0, i4.ID);
            Assert.AreEqual(0, i4.Value);
        }

        [Test()]
        public void ToStringUT()
        {
            var instance = new SerialPart(4, 1234);
            var expect = $"@S[1234-4]";
            var result = instance.ToString();

            Assert.AreEqual(expect, result);
        }
    }
}
