using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using TokyoChokoku.Bppg;
using TokyoChokoku.Bppg.V1;

using TokyoChokoku.Patmark.EmbossmentKit;
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

namespace TokyoChokoku.Patmark
{
    public class BinaryPPGFileIODriver : AbstractFileIODriver
    {
        /// <summary>
        /// BigEndian 形式のバイナリで出力する設定にします。
        /// </summary>
        static EndianFormatter MyEndianFormatter { get; } = new MarkinBoxEndianFormatter();

        /// <summ ary>
        /// フィールドをバイナリに変換します。
        /// </summary>
        static byte[] BinalizeField(MBData data)
        {
            Log.Debug($"EndianFormatter: {MyEndianFormatter.GetType().Name}");
            var binarizer = new MBDataBinarizer(MyEndianFormatter, data);
            return binarizer.ToCommandFormat();
        }

        /// <summary>
        /// バイナリからフィールドを読み取ります。
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        static MBData DebinalizeField(byte[] bin)
        {
            Log.Debug($"EndianFormatter: {MyEndianFormatter.GetType().Name}");
            var binarizer = MBDataBinarizer.Read(MyEndianFormatter, bin);
            return binarizer.ToMBData();
        }

        /// <summary>
        /// バイナリからファイルを読み取ります。
        /// ファイルの内容を 16進数化したデータを 1フィールド1リスト に対応させて返します。
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        static IList<string> DebinalizeFieldToString(byte[] bin)
        {
            return bin.Select(it =>
            {
                var hex = it.ToString("X");
                if (hex.Length == 1)
                    hex = $"0{hex}";
                return hex;
            }).ToList();
        }

        /// <summary>
        /// ファイルをバイナリに変換します。
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        static byte[] BinalizeFile(IList<MBData> fields)
        {
            var file = new BPPGFileV1(
                fields
                    .Select(it => new BPPGFieldDataV1(BinalizeField(it)))
                    .ToList()
            );
            return new BPPGFileIOV1().Binalize(file);
        }

        /// <summary>
        /// バイナリからファイルを読み取ります。
        /// </summary>
        static IList<MBData> DebinalizeFile(byte[] binFile)
        {
            var file = new BPPGFileIOV1().Debinalize(binFile);
            return file.FieldList
                .Select(it => DebinalizeField(it.Data))
                .ToList();
        }

        /// <summary>
        /// バイナリからファイルを読み取ります。
        /// ファイルの内容を 16進数化したデータを 1フィールド1リスト に対応させて返します。
        /// </summary>
        static IList<IList<string>> DebinalizeFileToString(byte[] binFile)
        {
            var file = new BPPGFileIOV1().Debinalize(binFile);
            return file.FieldList
                .Select(it => DebinalizeFieldToString(it.Data))
                .ToList();
        }

        static void WriteAllBytes(LocalFilePath path, byte[] binFile)
        {
            var p = path.ToStringNormalized();

            using (var stream = File.Create(p))
            {
                stream.Write(binFile);
            }
        }

        static byte[] ReadAllBytes(LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            using (var stream = File.OpenRead(p))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer);
                return buffer;
            }
        }

        // ======



        protected override FileOwner Read(LocalFilePath path)
        {
            var bytes = ReadAllBytes(path);
            var file  = DebinalizeFile(bytes);
            return new FileOwner(file);
        }

        protected override string ReadToString(LocalFilePath path)
        {
            var eol    = Environment.NewLine;
            var bytes  = ReadAllBytes(path);
            var file   = DebinalizeFileToString(bytes);
            var i      = 0;
            var fields = file.SelectMany(field =>
            {
                return new List<string>()
                {
                    $"==== Field {i++} ===="   , eol,
                    string.Join(" | ", field), eol
                };
            });
            return string.Join(eol, fields);
        }

        protected override void Write(FileOwner owner, LocalFilePath path)
        {
            var bytes = BinalizeFile(owner.Fields);
            WriteAllBytes(path, bytes);
        }

        protected override void WriteEmpty(LocalFilePath path)
        {
            var file = new FileOwner(new List<MBData>());
            var bytes = BinalizeFile(file.Fields);
            WriteAllBytes(path, bytes);
        }
    }
}
