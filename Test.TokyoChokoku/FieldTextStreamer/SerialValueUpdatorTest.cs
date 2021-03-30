using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace TokyoChokoku.FieldTextStreamer.Test
{
    using SerialModule.Counter;

    [TestFixture()]
    public class SerialValueUpdatorTest
    {
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
        ///
        /// 定義域外のシリアルを全て調べるのは大変であるため、境界値チェックで対応します。
        /// </summary>
        [Test()]
        public void ReplaceUT()
        {
            var slist = SettingListZeroFill3;
            var clist = CountList;
            var ins = new SerialValueUpdater(slist, clist);
            var input =   @"@S[1234-0] tango-@S[9999-1]-chan jiru-@S[9999-2]-chan nao-@S[12-3]-chan :@S[99-4] @S[5678-5] @S[5678-6] @S[5678-7] @S[5678-8] @S[5678-9]";
            var expect = $@"@S[1234-0] tango-@S[012-1]-chan jiru-@S[016-2]-chan nao-@S[020-3]-chan :@S[024-4] @S[5678-5] @S[5678-6] @S[5678-7] @S[5678-8] @S[5678-9]";
            var result = ins.Replace(input);
            Assert.AreEqual(expect, result);
            PrintResult(nameof(ReplaceUT), result);
        }
    }
}
