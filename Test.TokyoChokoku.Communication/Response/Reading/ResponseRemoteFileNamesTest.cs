using System;
using System.Text;
using System.IO;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.Communication;

namespace TokyoChokoku.Communication.Test
{
    [TestFixture()]
    public class ResponseRemoteFileNamesTest
    {
        static ResponseRemoteFileNamesTest()
        {
            // テキストエンコーディング対応
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }


        [Test()]
        public void UT_DecodeSuccessful()
        {
            var inputText = "abcdefghijklmnopqrstuvwxyz-_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var input = Encoding.ASCII.GetBytes(inputText);
            var resultText = ResponseRemoteFileNames.Decode(input);
            Assert.AreEqual(inputText, resultText);
        }



        [Test()]
        public void UT_DecodeSuccessfulWithNullChar()
        {
            var inputText  = "abcdefghijklmnop\0qrstuvwxyz-_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var expectText = "abcdefghijklmnop";
            var input = Encoding.ASCII.GetBytes(inputText);
            var resultText = ResponseRemoteFileNames.Decode(input);
            Assert.AreEqual(expectText, resultText);
        }
    }
}
