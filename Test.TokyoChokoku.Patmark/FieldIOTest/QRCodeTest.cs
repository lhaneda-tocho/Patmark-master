//using System;
//using System.IO;
//using System.Linq;
//using TokyoChokoku.Patmark.EmbossmentKit;
//using NUnit.Framework;

//using TokyoChokoku.MarkinBox.Sketchbook;
//using IQrCodeParameter              = TokyoChokoku.MarkinBox.Sketchbook.Parameters.IBaseQrCodeParameter;
//using QrCodeParameter               = TokyoChokoku.MarkinBox.Sketchbook.Parameters.QrCodeParameter;
//using MutableQrCodeParameter        = TokyoChokoku.MarkinBox.Sketchbook.Parameters.MutableQrCodeParameter;


//namespace TokyoChokoku.PatmarkTest.FieldIOTest
//{
//    using static MBDataTestUtil;

//    [TestFixture()]
//    public class QRCodeTest
//    {
//        private void AssertEquals(IQrCodeParameter a, IQrCodeParameter b)
//        {
//        Assert.AreEqual(a.Text, b.Text);
//            Assert.AreEqual(a.X, b.X);
//            Assert.AreEqual(a.Y, b.Y);
//            Assert.AreEqual(a.Height, b.Height);
//            Assert.AreEqual(a.Angle, b.Angle);
//            Assert.AreEqual(a.Power, b.Power);
//            Assert.AreEqual(a.Speed, b.Speed);
//            //Assert.AreEqual(a.Jogging   , b.Jogging   );
//            Assert.AreEqual(a.Pause, b.Pause);
//            Assert.AreEqual(a.Reverse, b.Reverse);
//            Assert.AreEqual(a.Mirrored, b.Mirrored);
//            Assert.AreEqual(a.BasePoint, b.BasePoint);
//        }


//        [Test()]
//        public void UT_Constructor()
//        {
//            var text = "Hey Hey";
//            var x = 10.0m;
//            var y = 5.0m;
//            var height = 10.0m;
//            var angle = 1.0m;
//            short power = 4;
//            short speed = 4;
//            var jogging = true;
//            var pause = true;
//            var reverse = true;
//            var mirrored = true;
//            byte basePoint = Consts.FieldBasePointCM;

//            // パラメタ作成
//            var p = MutableDataMatrixParameter.Create();
//            p.Text = text;
//            p.X = x;
//            p.Y = y;
//            p.Height = height;
//            p.Angle = angle;
//            p.Power = power;
//            p.Speed = speed;
//            p.Jogging = jogging;
//            p.Pause = pause;
//            p.Reverse = reverse;
//            p.Mirrored = mirrored;
//            p.BasePoint = basePoint;


//            var c = DataMatrixParameter.CopyOf(p);
//            Assert.AreEqual(c.Text, text);
//            Assert.AreEqual(c.X, x);
//            Assert.AreEqual(c.Y, y);
//            Assert.AreEqual(c.Height, height);
//            Assert.AreEqual(c.Angle, angle);
//            Assert.AreEqual(c.Power, power);
//            Assert.AreEqual(c.Speed, speed);
//            Assert.AreEqual(c.Jogging, jogging);
//            Assert.AreEqual(c.Pause, pause);
//            Assert.AreEqual(c.Reverse, reverse);
//            Assert.AreEqual(c.Mirrored, mirrored);
//            Assert.AreEqual(c.BasePoint, basePoint);


//            AssertEquals(p, c);
//        }


//        [Test()]
//        public void UT_ToMBData_RecursionTest()
//        {
//            var text = "Hey Hey";
//            var x = 10.0m;
//            var y = 5.0m;
//            var height = 10.0m;
//            var angle = 1.0m;
//            short power = 4;
//            short speed = 4;
//            var jogging = true;
//            var pause = true;
//            var reverse = true;
//            var mirrored = true;
//            byte basePoint = Consts.FieldBasePointCM;

//            // パラメタ作成
//            var p = MutableDataMatrixParameter.Create();
//            p.Text = text;
//            p.X = x;
//            p.Y = y;
//            p.Height = height;
//            p.Angle = angle;
//            p.Power = power;
//            p.Speed = speed;
//            p.Jogging = jogging;
//            p.Pause = pause;
//            p.Reverse = reverse;
//            p.Mirrored = mirrored;
//            p.BasePoint = basePoint;

//            var c = DataMatrixParameter.Create(p.ToSerializable());
//            AssertEquals(p, c);
//        }

//        [Test()]
//        public void UT_Textize_RecursionTest()
//        {
//            var text = "Hey Hey";
//            var x = 10.0m;
//            var y = 5.0m;
//            var height = 10.0m;
//            var angle = 1.0m;
//            short power = 4;
//            short speed = 4;
//            var jogging = true;
//            var pause = true;
//            var reverse = true;
//            var mirrored = true;
//            byte basePoint = Consts.FieldBasePointCM;

//            // パラメタ作成
//            var p = MutableDataMatrixParameter.Create();
//            p.Text = text;
//            p.X = x;
//            p.Y = y;
//            p.Height = height;
//            p.Angle = angle;
//            p.Power = power;
//            p.Speed = speed;
//            p.Jogging = jogging;
//            p.Pause = pause;
//            p.Reverse = reverse;
//            p.Mirrored = mirrored;
//            p.BasePoint = basePoint;

//            // テキスト変換
//            var bin = Detextize(Textize(p.ToSerializable()), expectListSize: 1)[0];

//            var c = DataMatrixParameter.Create(bin);
//            AssertEquals(p, c);
//        }




//        [Test()]
//        public void UT_Binarize_RecursionTest()
//        {
//            var text = "Hey Hey";
//            var x = 10.0m;
//            var y = 5.0m;
//            var height = 10.0m;
//            var angle = 1.0m;
//            short power = 4;
//            short speed = 4;
//            var jogging = true;
//            var pause = true;
//            var reverse = true;
//            var mirrored = true;
//            byte basePoint = Consts.FieldBasePointCM;

//            // パラメタ作成
//            var p = MutableDataMatrixParameter.Create();
//            p.Text = text;
//            p.X = x;
//            p.Y = y;
//            p.Height = height;
//            p.Angle = angle;
//            p.Power = power;
//            p.Speed = speed;
//            p.Jogging = jogging;
//            p.Pause = pause;
//            p.Reverse = reverse;
//            p.Mirrored = mirrored;
//            p.BasePoint = basePoint;

//            // テキスト変換
//            var bin = Debinarize(Binarize(p.ToSerializable()));

//            var c = DataMatrixParameter.Create(bin);
//            AssertEquals(p, c);
//        }
//    }
//}
