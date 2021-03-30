using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using TokyoChokoku.Bppg.StreamUtil;

namespace TokyoChokoku.Bppg.V1
{
    public class BPPGFileIOV1
    {
        CompressionLevel CL = CompressionLevel.NoCompression;

        void Valid(BPPGFile file)
        {
            file = file ?? throw new ArgumentNullException(nameof(file));
            if (file.FormatVersion != BPPGFile.FormatVersion1_0)
                throw new NotSupportedException($"Unsupported Version: {file.FormatVersion}");

        }

        /// <summary>
        /// BPPG バイナリ形式に変換します。
        /// </summary>
        /// <param name="file">ファイルオブジェクト</param>
        /// <returns>バイナリ化した結果</returns>
        public byte[] Binalize(BPPGFile file)
        {
            Valid(file);

            using (var memorystream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memorystream, ZipArchiveMode.Create, true))
                {
                    PutEntry(file, archive);
                }

                return memorystream.ToArray();
            }
        }

        /// <summary>
        /// バイナリを BPPG ファイルとして読み込みます。
        /// </summary>
        /// <param name="file">バイナリ</param>
        /// <returns>読み込み結果</returns>
        /// <exception cref="BPPGException">BPPG ファイル読み込み中にエラーが発生した場合 </exception>
        public BPPGFile Debinalize(byte[] file)
        {
            using(var memorystream = new MemoryStream(file, false))
            using (var archive = new ZipArchive(memorystream, ZipArchiveMode.Read, false))
            {
                // 読み込みに成功すれば Version1 のファイルとみなす
                var json = GetRootJson(archive);
                if (json.Version != "1.0")
                    throw new BPPGException(BPPGErrorCode.UnsupportedVersion, json.Version);

                var list = GetFieldList(archive);
                return new BPPGFileV1(list);
            }
        }


        void PutEntry(BPPGFile file, ZipArchive archive)
        {
            PutRootJson(archive);
            PutFieldList(archive, file.FieldList);
        }


        /// <summary>
        /// Json ファイル取得
        /// </summary>
        /// <param name="archive"></param>
        BPPGJson GetRootJson(ZipArchive archive)
        {
            try
            {
                var infojson = archive.GetEntry("BPPG.json") ?? throw new BPPGException(
                    BPPGErrorCode.Unrecognized,
                    "header not found"
                );

                // json ファイル書き込み
                using (Stream stream = infojson.Open())
                using (var sr = new StreamReader(stream))
                {
                    // FIXME: 失敗すると JsonException でクラッシュする問題の対応
                    return BPPGJson.FromJson(sr.ReadToEnd());
                }
            }
            catch (JsonException ex)
            {
                // JSON が読み込めない
                throw new BPPGException(
                    BPPGErrorCode.Unrecognized,
                    "header format error",
                     ex
                );
                throw ex;
            }
            catch (InvalidDataException ex)
            {
                // ZIP 壊れている場合
                throw new BPPGException(
                    BPPGErrorCode.Unrecognized,
                    "container format error",
                    ex
                );
                throw ex;
            }
        }

        /// <summary>
        /// Json ファイルを入れる
        /// </summary>
        /// <param name="archive"></param>
        void PutRootJson(ZipArchive archive)
        {
            var infojson = archive.CreateEntry("BPPG.json", CL);

            // json ファイル書き込み
            using (Stream stream = infojson.Open())
            using (var sw = new StreamWriter(stream))
            {
                sw.WriteLine(new BPPGJson("1.0"));
            }
        }

        /// <summary>
        /// フィールドリストを読み込む 
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        IList<BPPGFieldDataV1> GetFieldList(ZipArchive archive)
        {
            var lb = ImmutableList.CreateBuilder<BPPGFieldDataV1>();
            for (int i = 0; i < 51; ++i)
            {
                // ファイル作成
                var dat = archive.GetEntry($"data/bppg/FieldList/{i}.dat");

                // ない場合はスキップ
                if (ReferenceEquals(dat, null))
                {
                    System.Diagnostics.Debug.WriteLine("Ignored Field: Not Found");
                    continue;
                }

                // フィールドファイル読み取り
                using (Stream stream = dat.Open())
                {
                    //FIXME: 88 word ない場合はエラーを出してスキップする
                    //FIXME: 起きたエラーを記録する
                    var datagram = stream.ReadAllBytes(88 * 2);
                    if(datagram.IsEmpty)
                    {
                        System.Diagnostics.Debug.WriteLine("Ignored Field: File is Empty.");
                        continue;
                    }
                    if(datagram.ByteCount != 88*2)
                    {
                        System.Diagnostics.Debug.WriteLine("Ignored Field: File is Invalid File Size");
                        continue;
                    }
                    lb.Add(new BPPGFieldDataV1(datagram.Buffer));
                }
            }

            return lb.ToImmutableList();
        }

        /// <summary>
        /// バイナリファイルを入れる。
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="fields"></param>
        void PutFieldList(ZipArchive archive, IList<BPPGFieldData> fields)
        {
            var it = fields.GetEnumerator();

            for (int i = 0; i < 51; ++i)
            {
                if(!it.MoveNext())
                {
                    // フィールドがない場合は終了
                    break;
                }

                // フィールド取得
                var field = it.Current;

                // ファイル作成
                var dat = archive.CreateEntry($"data/bppg/FieldList/{i}.dat", CL);

                // フィールドファイル書き込み
                using (Stream stream = dat.Open())
                {
                    stream.Write(field.Data);
                }
            }
        }
    }
}
