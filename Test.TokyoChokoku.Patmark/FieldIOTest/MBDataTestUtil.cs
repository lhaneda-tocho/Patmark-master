using System;
using System.IO;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using NUnit.Framework;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.Communication;


using DataMatrixField = TokyoChokoku.MarkinBox.Sketchbook.Fields.DataMatrix;
using MutableDataMatrixField = TokyoChokoku.MarkinBox.Sketchbook.Fields.DataMatrix.Mutable;

using IDataMatrixParameter = TokyoChokoku.MarkinBox.Sketchbook.Parameters.IBaseDataMatrixParameter;
using DataMatrixParameter = TokyoChokoku.MarkinBox.Sketchbook.Parameters.DataMatrixParameter;
using MutableDataMatrixParameter = TokyoChokoku.MarkinBox.Sketchbook.Parameters.MutableDataMatrixParameter;
using TokyoChokoku.MarkinBox.Sketchbook;

using MBDataBinarizer = TokyoChokoku.Structure.Binary.MBDataBinarizer;
using Programmer = TokyoChokoku.Communication.Programmer;


namespace TokyoChokoku.PatmarkTest.FieldIOTest
{
    /// <summary>
    /// テスト用のユティリティメソッド 
    /// </summary>
    static class MBDataTestUtil
    {
        /// <summary>
        /// エンディアンフォーマッタです。
        /// </summary>
        static readonly EndianFormatter MyEndianFormatter = new PatmarkEndianFormatter();

        /// <summary>
        /// バイナリに変換します。
        /// </summary>
        public static byte[] Binarize(MBData data)
        {
            System.Diagnostics.Debug.WriteLine($"EndianFormatter: {MyEndianFormatter.GetType().Name}");
            var binarizer = new MBDataBinarizer(MyEndianFormatter, data);
            return binarizer.ToCommandFormat();
        }

        /// <summary>
        /// バイナリから読み取ります。
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static MBData Debinarize(byte[] bin)
        {
            System.Diagnostics.Debug.WriteLine($"EndianFormatter: {MyEndianFormatter.GetType().Name}");
            var binarizer = MBDataBinarizer.Read(MyEndianFormatter, bin);
            return binarizer.ToMBData();
        }

        /// <summary>
        /// PPG ファイルに変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Textize(MBData data)
        {
            using (var w = new StringWriter())
            {
                MBDataTextizer.Save(w, new MBData[] { data });
                return w.ToString();
            }
        }

        /// <summary>
        /// テキストファイルから読み取ります。
        /// </summary>
        /// <param name="text">データ文字列</param>
        /// <param name="expectListSize">ファイルに保存されているべき要素数を指定します。この数と一致しない場合は Assertion例外が発生します。</param>
        /// <returns></returns>
        public static List<MBData> Detextize(string text, int expectListSize)
        {
            using (var r = new StringReader(text))
            {
                var list = MBDataTextizer.Of(r).ToMBData();
                var count = list.Count;
                Assert.AreEqual(expectListSize, count, $"Expect List Size: {expectListSize}, Actual List Size: {count}");
                return list;
            }
        }
    }
}
