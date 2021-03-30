using NUnit.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.MarkinBox;

namespace TokyoChokoku.SerialModule.Counter.Test
{
    /// <summary>
    /// SCSetting UT
    /// </summary>
    [TestFixture()]
    public class SCSettingTest
    {
        private static SCSetting CreateA()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range         = SCCountRange.Init(0, 9999);
            i.Format        = SCFormat.FillZero;
            i.ResetRule     = SCResetRule.ReachingMaximum;
            i.SkipNumber    = 1;
            i.RepeatCount   = 1;
            i.ResetTime = SCTime.Init(0, 0);
            return i.ToImmutable();
        }
        private static SCSetting CreateB()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range         = SCCountRange.Init(0, 999);
            i.Format        = SCFormat.LeftJustify;
            i.ResetRule     = SCResetRule.IntoSpecMonth;
            i.SkipNumber    = 2;
            i.RepeatCount   = 4;
            i.ResetTime     = SCTime.Init(12, 17);
            return i.ToImmutable();
        }
        private static SCSetting CreateC()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range         = SCCountRange.Init(0, 99);
            i.Format        = SCFormat.RightJustify;
            i.ResetRule     = SCResetRule.IntoSpecYear;
            i.SkipNumber    = 7;
            i.RepeatCount   = 3;
            i.ResetTime     = SCTime.Init(12, 17);
            return i.ToImmutable();
        }
        private static SCSetting CreateD()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range         = SCCountRange.Init(0, 9);
            i.Format        = SCFormat.FillZero;
            i.ResetRule     = SCResetRule.OntoSpecDay;
            i.SkipNumber    = 2;
            i.RepeatCount   = 6;
            i.ResetTime     = SCTime.Init(12, 17);
            return i.ToImmutable();
        }
        private static IList<SCSetting> CreateTestData()
        {
            return new List<SCSetting>()
            {
                CreateA(),
                CreateB(),
                CreateC(),
                CreateD(),
            };
        }

        private static void AssertEqualsElement(SCSetting expect, SCSetting actual)
        {
            Assert.AreEqual(expect.Format       , actual.Format);
            Assert.AreEqual(expect.ResetRule    , actual.ResetRule);
            Assert.AreEqual(expect.Range        , actual.Range);
            Assert.AreEqual(expect.SkipNumber   , actual.SkipNumber);
            Assert.AreEqual(expect.RepeatCount  , actual.RepeatCount);
            Assert.AreEqual(expect.ResetTime    , actual.ResetTime);
        }

        private void AssertEqualsList(SCSettingList expect, SCSettingList actual)
        {
            Assert.AreEqual(expect.Count, actual.Count);
            foreach (var (e, a) in expect.Zip(actual, (x, y) => (x, y)))
            {
                AssertEqualsElement(e, a);
            }
        }

        [Test()]
        public void ToMBFormUT()
        {
            var list = new SCSettingList.Immutable(CreateTestData());

            var encoded = list.ToMBForm();
            Assert.AreEqual(MBSerial.NumOfSerial, encoded.Count);

            var endecodedRaw = SCSettingList.CreateFrom(encoded);
            Assert.AreEqual(list.Count, endecodedRaw.Count);

            var endecoded = new SCSettingList.Immutable(endecodedRaw);
            AssertEqualsList(list, endecoded);
        }
    }
}
