using System;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.Patmark.Extensions.Test
{
    [TestFixture()]
    public class MBDataExtTest
    {
        MBData CreateDM(string text)
        {
            var f = DataMatrix.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateQR(string text)
        {
            var f = QrCode.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateHT(string text)
        {
            var f = HorizontalText.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateXVT(string text)
        {
            var f = XVerticalText.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateYVT(string text)
        {
            var f = YVerticalText.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateIAT(string text)
        {
            var f = InnerArcText.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        MBData CreateOAT(string text)
        {
            var f = OuterArcText.Mutable.Create();
            f.Parameter.Text = text;
            return f.ToSerializable();
        }

        //

        IList<MBData> CreateZebraCrossFields(string text)
        {
            return new List<MBData>{
                CreateDM(text),
                CreateQR(text),
            };
        }

        IList<MBData> CreateTextFields(string text)
        {
            return new List<MBData>{
                CreateHT (text),
                CreateXVT(text),
                CreateYVT(text),
                CreateIAT(text),
                CreateOAT(text),
            };
        }

        IList<MBData> CreateFigureFields()
        {
            return new List<MBData>
            {
                Bypass      .Constant.Create().ToSerializable(),
                Circle      .Constant.Create().ToSerializable(),
                Ellipse     .Constant.Create().ToSerializable(),
                Line        .Constant.Create().ToSerializable(),
                Rectangle   .Constant.Create().ToSerializable(),
                Triangle    .Constant.Create().ToSerializable(),
            };

        }

        [Test()]
        public void IsZebraCrossFieldUT()
        {
            bool Call(MBData data) => data.IsZebraCrossField();

            var input = "SAMPLE";
            foreach (var f in CreateZebraCrossFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateTextFields(input))
            {
                Assert.IsFalse(Call(f));
            }
            foreach (var f in CreateFigureFields())
            {
                Assert.IsFalse(Call(f));
            }
        }

        [Test()]
        public void IsTextFieldUT()
        {
            bool Call(MBData data) => data.IsTextField();

            var input = "SAMPLE";
            foreach (var f in CreateZebraCrossFields(input))
            {
                Assert.IsFalse(Call(f));
            }
            foreach (var f in CreateTextFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateFigureFields())
            {
                Assert.IsFalse(Call(f));
            }
        }

        [Test()]
        public void IsTextContainerFieldUT()
        {
            bool Call(MBData data) => data.IsTextContainerField();

            var input = "SAMPLE";
            foreach (var f in CreateZebraCrossFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateTextFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateFigureFields())
            {
                Assert.IsFalse(Call(f));
            }
        }

        [Test()]
        public void IsHavableSerialUT()
        {
            bool Call(MBData data) => data.IsHavableSerial();

            var input = "SAMPLE";
            foreach (var f in CreateZebraCrossFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateTextFields(input))
            {
                Assert.IsTrue(Call(f));
            }
            foreach (var f in CreateFigureFields())
            {
                Assert.IsFalse(Call(f));
            }
        }

        [Test()]
        public void HasSerialUT()
        {
            bool Call(MBData data) => data.HasSerial();

            {
                var input = "abc@S[1234-1]def";
                foreach (var f in CreateZebraCrossFields(input))
                {
                    Assert.IsTrue(Call(f));
                }
                foreach (var f in CreateTextFields(input))
                {
                    Assert.IsTrue(Call(f));
                }
            }
            {
                var input = "abc1234def";
                foreach (var f in CreateZebraCrossFields(input))
                {
                    Assert.IsFalse(Call(f));
                }
                foreach (var f in CreateTextFields(input))
                {
                    Assert.IsFalse(Call(f));
                }
            }
            {
                var input = "@C[neko]@S[0000]";
                foreach (var f in CreateZebraCrossFields(input))
                {
                    Assert.IsFalse(Call(f));
                }
                foreach (var f in CreateTextFields(input))
                {
                    Assert.IsFalse(Call(f));
                }
            }
        }
        
    }
}
