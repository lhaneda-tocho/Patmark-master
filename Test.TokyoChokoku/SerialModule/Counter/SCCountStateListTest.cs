using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox;

namespace TokyoChokoku.SerialModule.Counter.Test
{
    [TestFixture()]
    public class SCCountStateListTest
    {
        private static void AssertEqualsElement(SCCountState expect, SCCountState actual)
        {
            Assert.AreEqual(expect.CurrentValue  , actual.CurrentValue);
            Assert.AreEqual(expect.RepeatingCount, actual.RepeatingCount);
            Assert.AreEqual(expect.LastUpdateDate, actual.LastUpdateDate);
            Assert.AreEqual(expect.LastUpdateTime, actual.LastUpdateTime);
            Assert.AreEqual(expect.FileNo        , actual.FileNo);
        }

        private void AssertEqualsList(SCCountStateList expect, SCCountStateList actual)
        {
            Assert.AreEqual(expect.Count, actual.Count);
            foreach(var (e, a) in expect.Zip(actual, (x, y) => (x, y)))
            {
                AssertEqualsElement(e, a);
            }
        }

        [Test()]
        public void ToMBFormUT()
        {
            var list = new SCCountStateList.Mutable(SCCountState.DefaultList.Select((SCCountState e, int i) => {
                var data = e.ToMutable();
                data.CurrentValue   = (ushort)(i *  7);
                data.RepeatingCount = (ushort)(i * 13);
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                data.LastUpdateDate = SCDate.InitWithInt32((UInt32)(i*31));
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                data.LastUpdateTime = SCTime.Init((byte)(i*11), (byte)(i*5));
                data.FileNo         = (ushort)(i *  3);
                return data.ToImmutable();
            }));

            var encoded = list.ToMBForm();
            Assert.AreEqual(MBSerial.NumOfSerial, encoded.Count);


            foreach(var (i, e) in Enumerable.Range(1, MBSerial.NumOfSerial).Zip(encoded))
            {
                Assert.AreEqual(i, e.SerialNo);
            }

            var endecodedRaw = SCCountStateList.CreateFrom(encoded);
            Assert.AreEqual(list.Count, endecodedRaw.Count);

            var endecoded = new SCCountStateList.Immutable(endecodedRaw);
            AssertEqualsList(list, endecoded);
        }
    }
}