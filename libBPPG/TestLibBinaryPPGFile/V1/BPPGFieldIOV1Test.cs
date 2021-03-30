using System;
using System.Collections.Generic;
using TokyoChokoku.Bppg;
using TokyoChokoku.Bppg.V1;

 using NUnit.Framework;


namespace TestLibBinaryPPGFile.V1
{
    public class BPPGFieldIOV1Test
    {
        byte[] Field1 = new byte[88*2];



        [SetUp]
        public void Setup()
        {
            for (int i = 0; i < 88 * 2; ++i)
            {
                Field1[i] = (byte)i;
            }
        }


        [Test]
        public void UT_Binalize_Recurse()
        {
            var io = new BPPGFileIOV1();
            var expectFieldList = new List<BPPGFieldDataV1>() {
                new BPPGFieldDataV1(Field1),
                new BPPGFieldDataV1(Field1),
                new BPPGFieldDataV1(Field1),
            };

            // バイナライズ
            var bin = io.Binalize(new BPPGFileV1(expectFieldList));
            System.IO.File.WriteAllBytes("Test.zip", bin);

            // オブジェクトに戻す
            var result = io.Debinalize(bin);

            // Check
            Assert.AreEqual(BPPGFile.FormatVersion1_0, result.FormatVersion);
            Assert.AreEqual(expectFieldList.Count, result.FieldList.Count);

            Assert.That(expectFieldList[0].Data, Is.EqualTo(result.FieldList[0].Data));
            Assert.That(expectFieldList[1].Data, Is.EqualTo(result.FieldList[1].Data));
            Assert.That(expectFieldList[2].Data, Is.EqualTo(result.FieldList[2].Data));

        }
    }
}
