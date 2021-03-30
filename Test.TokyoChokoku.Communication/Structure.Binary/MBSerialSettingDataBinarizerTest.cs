using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.Structure.Binary.Test
{
    [TestFixture()]
    public class MBSerialSettingDataBinarizerTest
    {
        private static void AssertEqualsElement(MBSerialData expect, MBSerialData actual)
        {
            Assert.AreEqual(expect.Format       , actual.Format);
            Assert.AreEqual(expect.ResetRule    , actual.ResetRule);
            Assert.AreEqual(expect.MaxValue     , actual.MaxValue);
            Assert.AreEqual(expect.MinValue     , actual.MinValue);
            Assert.AreEqual(expect.SkipNumber   , actual.SkipNumber);
            Assert.AreEqual(expect.RepeatCount  , actual.RepeatCount);
            Assert.AreEqual(expect.ResetTime    , actual.ResetTime);
        }

        private void AssertEqualsList(IList<MBSerialData> expect, IList<MBSerialData> actual)
        {
            Assert.AreEqual(4, expect.Count);
            Assert.AreEqual(expect.Count, actual.Count);
            foreach (var (e, a) in expect.Zip(actual, (x, y) => (x, y)))
            {
                AssertEqualsElement(e, a);
            }
        }
        private EndianFormatter Formatter { get; } = new PatmarkEndianFormatter();


        private MBSerialSettingDataBinarizer CreateBinalizer()
        {
            var bytesize = MBSerial.NumOfSerial * 8 * 2;
            var dataStore = new byte[bytesize];
            var prog = new Programmer(Formatter, dataStore);
            return new MBSerialSettingDataBinarizer(prog);
        }

        private MBSerialSettingDataBinarizer CreateBinalizer(byte[] datagram)
        {
            var bytesize = MBSerial.NumOfSerial * 8 * 2;
            if (datagram.Length != bytesize)
            {
                Assert.Fail($@"
データグラム長が一致しません. テストを継続できません。
想定: {bytesize}
結果: {datagram.Length}
".Trim());
            }
            var prog = new Programmer(Formatter, datagram);
            return new MBSerialSettingDataBinarizer(prog);
        }

        private MBSerialSettingDataBinarizer CreateBinalizer(SCSettingList sdata)
        {
            return new MBSerialSettingDataBinarizer(Formatter, sdata);
        }

        private static SCSetting CreateA()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range = SCCountRange.Init(0, 9999);
            i.Format = SCFormat.FillZero;
            i.ResetRule = SCResetRule.ReachingMaximum;
            i.SkipNumber = 1;
            i.RepeatCount = 1;
            i.ResetTime = SCTime.Init(0, 0);
            return i.ToImmutable();
        }
        private static SCSetting CreateB()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range = SCCountRange.Init(0, 999);
            i.Format = SCFormat.LeftJustify;
            i.ResetRule = SCResetRule.IntoSpecMonth;
            i.SkipNumber = 2;
            i.RepeatCount = 4;
            i.ResetTime = SCTime.Init(12, 17);
            return i.ToImmutable();
        }
        private static SCSetting CreateC()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range = SCCountRange.Init(0, 99);
            i.Format = SCFormat.RightJustify;
            i.ResetRule = SCResetRule.IntoSpecYear;
            i.SkipNumber = 7;
            i.RepeatCount = 3;
            i.ResetTime = SCTime.Init(12, 17);
            return i.ToImmutable();
        }
        private static SCSetting CreateD()
        {
            var i = SCSetting.Default.ToMutable();
            i.Range = SCCountRange.Init(0, 9);
            i.Format = SCFormat.FillZero;
            i.ResetRule = SCResetRule.OntoSpecDay;
            i.SkipNumber = 2;
            i.RepeatCount = 6;
            i.ResetTime = SCTime.Init(12, 17);
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
        private SCSettingList.Mutable CreateSerialData()
        {
            var list = new List<SCSetting>()
            {
                CreateA(),
                CreateB(),
                CreateC(),
                CreateD(),
            };
            return new SCSettingList.Mutable(list);
        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// = 概要
        /// シリアル設定を エンコード・デコード するテストです。
        /// 以下の式が成立することを確認します。
        ///
        ///   A == Decode(Encode(A))
        ///
        /// = 評価内容
        /// 1. シリアル設定を作成して A を得る
        /// 2. シリアル設定 A をバイト配列に変換して B を得る
        ///    変換には Patmark Endian Rule を使用する。
        /// 3. バイト配列 B をまたシリアル設定に変換して C を得る。
        /// 4. A == C が成り立つことを確認する。
        /// 
        /// </summary>
        [Test]
        public void EndecodingUT()
        {
            var input = CreateSerialData();
            var encoded = CreateBinalizer(input).Data.GetAllBytes();

            var expect = input.ToMBForm();
            var result = CreateBinalizer(encoded).ConstructObject();

            AssertEqualsList(expect, result);

//            foreach (var (index, it) in Enumerable.Range(1, expect.Count).Zip(expect))
//            {
//                Console.WriteLine($@"
//= {nameof(expect)}[{index}]
//SerialNo        = {it.SerialNo}
//CurrentValue    = {it.CurrentValue}
//RepeatingCount  = {it.RepeatingCount}
//LastUpdateDate  = {it.LastUpdateDate}
//LastUpdateTime  = {it.LastUpdateTime}
//FileNo          = {it.FileNo}
//".Trim());
//            }


//            foreach (var (index, it) in Enumerable.Range(1, expect.Count).Zip(result))
//            {
//                Console.WriteLine($@"
//= {nameof(result)}[{index}]
//SerialNo        = {it.SerialNo}
//CurrentValue    = {it.CurrentValue}
//RepeatingCount  = {it.RepeatingCount}
//LastUpdateDate  = {it.LastUpdateDate}
//LastUpdateTime  = {it.LastUpdateTime}
//FileNo          = {it.FileNo}
//".Trim());
//            }
        }
    }
}
