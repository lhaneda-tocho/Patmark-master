using System;
using System.IO;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.Communication;


namespace TokyoChokoku.Patmark.Test
{
    [TestFixture()]
    public class RemoteFileInfoTest
    {
        [Test()]
        public void UT_Constructor()
        {
            var inputList = new List<(string, int)>()
            {
                ("xiuy289みゅう[]sertg", 12),
                ("e89ypzx;hd貴井adsi", 0),
                ("F138_neko1", 4),
                ("F139_neko2", 0)
            };

            foreach((string expectName, int expectFieldCount) in inputList)
            {
                var instance = new RemoteFileInfo(expectName, expectFieldCount);
                Assert.AreEqual(expectName      , instance.Name);
                Assert.AreEqual(expectFieldCount, instance.NumOfField);
            }
        }


        [Test()]
        public void UT_DisplayNameWithFileNo()
        {
            var inputList = new List<(int, string, int)>()
            {
                (15 , "xiuy289みゅう[]sertg", 12),
                (13 , "e89ypzx;hd貴井adsi", 0),
                (138, "F138_neko1", 4),
                (139, "F139_neko2", 0)
            };

            var expectList = new List<string>()
            {
                ("F015_xiuy289みゅう["),
                ("F013_"),
                ("F138_neko1"),
                ("F139_")
            };

            var resultList = inputList.Select(tuple =>
            {
                (int inputFieldNo, string inputName, int inputFieldCount) = tuple;
                var instance = new RemoteFileInfo(inputName, inputFieldCount);
                var result = instance.GetDisplayNameWithFileNo(inputFieldNo);
                return result;
            });

            foreach ((string expect, string result) in expectList.Zip(resultList))
            {
                Assert.AreEqual(expect, result);
            }
        }
    }
}
