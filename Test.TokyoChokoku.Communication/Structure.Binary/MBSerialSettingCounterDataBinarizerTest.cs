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
    public class MBSerialSettingCounterDataBinarizerTest
    {
        private static void AssertEqualsElement(MBSerialCounterData expect, MBSerialCounterData actual)
        {
            Assert.AreEqual(expect.SerialNo         , actual.SerialNo       );
            Assert.AreEqual(expect.CurrentValue     , actual.CurrentValue   );
            Assert.AreEqual(expect.RepeatingCount   , actual.RepeatingCount );
            Assert.AreEqual(expect.LastUpdateDate   , actual.LastUpdateDate );
            Assert.AreEqual(expect.LastUpdateTime   , actual.LastUpdateTime );
            Assert.AreEqual(expect.FileNo           , actual.FileNo         );
        }

        private void AssertEqualsList(IList<MBSerialCounterData> expect, IList<MBSerialCounterData> actual)
        {
            Assert.AreEqual(4, expect.Count);
            Assert.AreEqual(expect.Count, actual.Count);
            foreach (var (e, a) in expect.Zip(actual, (x, y) => (x, y)))
            {
                AssertEqualsElement(e, a);
            }
        }

        private EndianFormatter Formatter { get; } = new PatmarkEndianFormatter();


        private MBSerialCounterDataBinarizer CreateBinalizer()
        {
            var bytesize = MBSerial.NumOfSerial * 8 * 2;
            var dataStore = new byte[bytesize];
            var prog = new Programmer(Formatter, dataStore);
            return new MBSerialCounterDataBinarizer(prog);
        }

        private MBSerialCounterDataBinarizer CreateBinalizer(byte[] datagram)
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
            return new MBSerialCounterDataBinarizer(prog);
        }

        private MBSerialCounterDataBinarizer CreateBinalizer(SCCountStateList sdata)
        {
            return new MBSerialCounterDataBinarizer(Formatter, sdata);
        }

        private SCCountStateList.Mutable CreateSerialData()
        {
            var list = new SCCountStateList.Mutable(SCCountState.DefaultList.Select((SCCountState e, int i) => {
                var data = e.ToMutable();
                data.CurrentValue = (ushort)(i * 7);
                data.RepeatingCount = (ushort)(i * 13);
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                data.LastUpdateDate = SCDate.InitWithInt32((UInt32)(i * 31));
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                data.LastUpdateTime = SCTime.Init((byte)(i * 11), (byte)(i * 5));
                data.FileNo = (ushort)(i * 3);
                return data.ToImmutable();
            }));
            return list;
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

            foreach(var (index, it) in Enumerable.Range(1, expect.Count).Zip(expect))
            {
                Console.WriteLine($@"
= {nameof(expect)}[{index}]
SerialNo        = {it.SerialNo}
CurrentValue    = {it.CurrentValue}
RepeatingCount  = {it.RepeatingCount}
LastUpdateDate  = {it.LastUpdateDate}
LastUpdateTime  = {it.LastUpdateTime}
FileNo          = {it.FileNo}
".Trim());
            }


            foreach (var (index, it) in Enumerable.Range(1, expect.Count).Zip(result))
            {
                Console.WriteLine($@"
= {nameof(result)}[{index}]
SerialNo        = {it.SerialNo}
CurrentValue    = {it.CurrentValue}
RepeatingCount  = {it.RepeatingCount}
LastUpdateDate  = {it.LastUpdateDate}
LastUpdateTime  = {it.LastUpdateTime}
FileNo          = {it.FileNo}
".Trim());
            }
        }
    }
}
