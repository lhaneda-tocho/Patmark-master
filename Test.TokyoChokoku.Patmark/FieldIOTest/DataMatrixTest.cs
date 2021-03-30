using System;
using System.IO;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;

using TokyoChokoku.MarkinBox.Sketchbook;
using IDataMatrixParameter       = TokyoChokoku.MarkinBox.Sketchbook.Parameters.IBaseDataMatrixParameter;
using DataMatrixParameter        = TokyoChokoku.MarkinBox.Sketchbook.Parameters.DataMatrixParameter;
using MutableDataMatrixParameter = TokyoChokoku.MarkinBox.Sketchbook.Parameters.MutableDataMatrixParameter;
using System.Text;

namespace TokyoChokoku.PatmarkTest.FieldIOTest
{
    using static MBDataTestUtil;

    [TestFixture()]
    public class DataMatrixTest
    {
        static DataMatrixTest()
        {
            // テキストエンコーディング対応
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void AssertEquals(IDataMatrixParameter a, IDataMatrixParameter b)
        {
            var eps = 0.0001m;
            Assert.AreEqual(a.Text      , b.Text      );
            Assert.AreEqual(a.X         , b.X         );
            Assert.AreEqual(a.Y         , b.Y         );
            //Assert.AreEqual(a.Height    , b.Height    );
            Assert.AreEqual(a.Angle     , b.Angle     );
            Assert.AreEqual(a.Power     , b.Power     );
            Assert.AreEqual(a.Speed     , b.Speed     );
            //Assert.AreEqual(a.Jogging   , b.Jogging   );
            Assert.AreEqual(a.Pause     , b.Pause     );
            Assert.AreEqual(a.Reverse   , b.Reverse   );
            Assert.AreEqual(a.Mirrored  , b.Mirrored  );
            Assert.AreEqual(a.BasePoint , b.BasePoint );
            Assert.AreEqual(a.DotCount  , b.DotCount  );
        }

        private void AssertEquals(MBData a, MBData b)
        {
            Assert.AreEqual(a.Text, b.Text);
            Assert.AreEqual(a.X, b.X);
            Assert.AreEqual(a.Y, b.Y);

            Assert.IsTrue ((a.Mode & 0x0800) != 0, $"Unexpected {nameof(a.Mode)}");
            Assert.AreEqual(a.Mode, b.Mode);

            Assert.AreEqual(a.PrmFl, 50, $"Unexpected {nameof(a.PrmFl)}"); // 50 固定
            Assert.AreEqual(a.PrmFl, b.PrmFl);

            Assert.AreEqual(a.Height, b.Height);

            var aDM = new DataMatrixWrapper(a.ToMutable());
            Assert.AreEqual(aDM.BarcodeTypeByte, 0, $"BarcodeType should be 0 : Actual={aDM.BarcodeTypeByte}");

            //Assert.AreEqual(a.Pitch ,  b.Pitch);
            Assert.AreEqual(a.Angle ,  b.Angle);
            //Assert.AreEqual(a.Jogging   , b.Jogging   );
            Assert.AreEqual(a.Speed  , b.Speed);
            Assert.AreEqual(a.Power  , b.Power);
            Assert.AreEqual(a.OptionSw, b.OptionSw);

            Assert.AreEqual(a.Type, 6, $"Unexpected Type");
            Assert.AreEqual(a.Type, b.Type);

            Assert.AreEqual(a.BasePoint, b.BasePoint);
            Assert.AreEqual(a.Spares[0], b.Spares[0]);
        }

        private DataMatrixParameter CreateTestData()
        {
            var   text       = "Hey Hey";
            var   x          = 10.0m;
            var   y          =  5.0m;
            var   height     = 10.0m;
            var   angle      =  1.0m;
            short power      =     4;
            short speed      =     4;
            var   jogging    =  true;
            var   pause      =  true;
            var   reverse    =  true;
            var   mirrored   =  true;
            var   dotcount   = new DotCount2D(16, 48);
            byte  basePoint  = Consts.FieldBasePointCM;
            
            
            // パラメタ作成
            var p = MutableDataMatrixParameter.Create();
            p.Text      =      text;
            p.X         =         x;
            p.Y         =         y;
            p.Height    =    height;
            p.Angle     =     angle;
            p.Power     =     power;
            p.Speed     =     speed;
            p.Jogging   =   jogging;
            p.Pause     =     pause;
            p.Reverse   =   reverse;
            p.Mirrored  =  mirrored;
            p.BasePoint = basePoint;
            p.DotCount  =  dotcount;


            var c = DataMatrixParameter.CopyOf(p);
            Assert.AreEqual(c.Text      ,       text);
            Assert.AreEqual(c.X         ,          x);
            Assert.AreEqual(c.Y         ,          y);
            Assert.AreEqual(c.Height    ,     height);
            Assert.AreEqual(c.Angle     ,      angle);
            Assert.AreEqual(c.Power     ,      power);
            Assert.AreEqual(c.Speed     ,      speed);
            Assert.AreEqual(c.Jogging   ,    jogging);
            Assert.AreEqual(c.Pause     ,      pause);
            Assert.AreEqual(c.Reverse   ,    reverse);
            Assert.AreEqual(c.Mirrored  ,   mirrored);
            Assert.AreEqual(c.BasePoint ,  basePoint);
            Assert.AreEqual(c.DotCount  ,   dotcount);

            AssertEquals(p, c);

            return c;
        }


        [Test()]
        public void UT_Constructor()
        {
            CreateTestData();
        }


        [Test()]
        public void UT_ToMBData_RecursionTest()
        {
            var a   = CreateTestData();
            var b   = DataMatrixParameter.Create(a.ToSerializable());
            AssertEquals(a, b);
        }

        [Test()]
        public void UT_Textize_RecursionTest()
        {
            var a      = CreateTestData();
            var aDat   = a.ToSerializable();
            var txt    = Textize(aDat);
            var bDat   = Detextize(txt, expectListSize: 1).First();
            var b      = DataMatrixParameter.Create(bDat);

            // パラメタ読み取り結果が一致することを確認
            AssertEquals(a, b);

            // MBData のデータ一致チェック
            AssertEquals(aDat, bDat);
        }




        [Test()]
        public void UT_Binarize_RecursionTest()
        {
            var a      = CreateTestData();
            var aDat   = a.ToSerializable();
            var bin    = Binarize(aDat);
            var bDat   = Debinarize(bin);
            var b      = DataMatrixParameter.Create(bDat);

            // パラメタ読み取り結果が一致することを確認
            AssertEquals(a, b);

            // MBData のデータ一致チェック
            AssertEquals(aDat, bDat);
        }
    }
}
