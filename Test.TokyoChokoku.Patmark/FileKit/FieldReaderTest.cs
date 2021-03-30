using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TokyoChokoku.Patmark.FileKit.Test
{
    using SerialModule.Counter;
    using SerialModule.Setting;
    using FieldTextStreamer;
    using MarkinBox.Sketchbook;
    using MarkinBox.Sketchbook.Fields;

    [TestFixture()]
    public class FieldReaderTest
    {
        public FieldReaderTest()
        {
        }
        SCCountStateList CountList
        {
            get
            {
                var reqsize = SCCountStateList.RequiredSize;

                var list = new List<SCCountState>();
                for (int i = 0; i < reqsize; i++)
                {
                    var data = SCCountState.Data.Init();
                    data.CurrentValue = (uint)(12 + i * 4);
                    var state = new SCCountState(ref data);
                    list.Add(state);
                }
                var slist = new SCCountStateList.Immutable(list);
                return slist;
            }
        }

        SCSettingList SettingListZeroFill3
        {
            get
            {
                var reqsize = SCSettingList.RequiredSize;

                var list = new List<SCSetting>();
                for (int i = 0; i < reqsize; i++)
                {
                    var data = SCSetting.Data.Init();
                    data.Range = SCCountRange.Init(0, 100);
                    data.Format = SCFormat.FillZero;
                    var setting = new SCSetting(ref data);
                    list.Add(setting);
                }
                var slist = new SCSettingList.Immutable(list);
                return slist;
            }
        }

        void PrintResult(string title, string value)
        {
            var ans = string.Format("RESULT {0,20} | {1}", title, value);
            Console.WriteLine(ans);
        }

        /// <summary>
        /// 定義域内のシリアルが全て置換可能であること、そして、定義域外のシリアルが置換されないことを確かめます。
        /// SerialValueUpdater の結果と比較して対応します。
        /// </summary>
        [Test()]
        public void TextPropertyUT()
        {
            var slist = SettingListZeroFill3;
            var clist = CountList;

            var inputA =  @"@S[1234-0] tango-@S[9999-1]-chan jiru-@S[9999-2]-chan nao-@S[12-3]-chan :@S[99-4] @S[5678-5] @S[5678-6] @S[5678-7] @S[5678-8] @S[5678-9]";
            var inputB = HorizontalText.Mutable.Create().Also(it => {
                it.Parameter.Text = inputA;
            }).ToSerializable();
            var expect = $@"@S[1234-0] tango-@S[012-1]-chan jiru-@S[016-2]-chan nao-@S[020-3]-chan :@S[024-4] @S[5678-5] @S[5678-6] @S[5678-7] @S[5678-8] @S[5678-9]";


            var insA = new SerialValueUpdater(slist, clist);
            var insB = FieldReader.Create(inputB, serialSetting: new SerialSetting.Immutable(slist, clist));

            var resultA = insA.Replace(inputA);
            var resultB = insB.Text;
            Assert.AreEqual(expect, resultA);
            Assert.AreEqual(expect, resultB);

            PrintResult(nameof(TextPropertyUT), resultA);
            PrintResult(nameof(TextPropertyUT), resultB);
        }
    }
}
