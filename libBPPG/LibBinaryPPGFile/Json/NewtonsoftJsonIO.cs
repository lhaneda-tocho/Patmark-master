using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace TokyoChokoku.Bppg.Json
{
    /// <summary>
    /// Newtonsoft.Json に基づくクラスのJsonテキストIOを担当するクラスです。
    /// </summary>
    public static class NewtonsoftJsonIO
    {
        /// <summary>
        /// オブジェクトをJson文字列に変換します。
        /// </summary>
        /// <param name="isHumanReadable">true にすると、より人が読みやすい形式でシリアライズします。デフォルト値は false です。</param>
        /// <returns>Json文字列</returns>
        public static string ObjectToJson(object obj, bool isHumanReadable = false)
        {
            var f = (isHumanReadable) ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(obj, f);
        }

        /// <summary>
        /// 引数 <c>json</c> を型引数 <c>T</c>型のオブジェクトにデシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズしたいオブジェクトの型</typeparam>
        /// <param name="json">json テキストデータ</param>
        /// <returns>デシリアライズされた オブジェクト</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合。</exception>
        /// <exception cref="JsonDataException">
        ///     Json読み取り中に <c>ArgumentException</c> が起きたか、<c>JsonDataException</c> が起きた、もしくは 引数の<c>json</c>文字列が空欄・空の場合。
        /// </exception>
        /// <exception cref="JsonException">Json読み取り中に問題が起きた場合。</exception>
        public static T ObjectFromJson<T>(string json)
        {
            if (json == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(json))
                throw new JsonDataException("blank text");
            try
            {
                var ins = JsonConvert.DeserializeObject<T>(json);
                return ins;
            }
            catch (ArgumentException ex)
            {
                throw new JsonDataException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Jsonファイルからオブジェクトをデシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズしたいオブジェクトの型</typeparam>
        /// <param name="path">ファイルのパス。</param>
        /// <returns>デシリアライズされた オブジェクト</returns>
        /// <exception cref="ArgumentNullException">引数がnullの場合。</exception>
        /// <exception cref="ArgumentException"    >引数が空文字列である場合。</exception>
        /// <exception cref="DirectoryNotFoundException">マップされていないドライブにあるなど、指定されたパスが無効です。</exception>
        /// <exception cref="FileNotFoundException">ファイルが見つかりませんでした。</exception>
        /// <exception cref="IOException"><c>パス</c>の構文エラーが発生したとき。</exception>
        /// <exception cref="JsonDataException">Json読み取り結果に問題がある場合。</exception>
        /// <exception cref="JsonException">Json読み取り中に問題が起きた場合。</exception>
        public static T ObjectFromJsonFile<T>(string path)
        {
            string text;
            using (var r = new StreamReader(path))
            {
                text = r.ReadToEnd();
            }
            return ObjectFromJson<T>(text);
        }





        /// <summary>
        /// Jsonファイルからオブジェクトをシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズしたいオブジェクトの型</typeparam>
        /// <param name="obj">書き込みたいオブジェクト。</param>
        /// <param name="path">ファイルのパス。</param>
        /// <param name="isHumanReadable">人が読みやすい形式にする場合は true を指定します。</param>
        /// <param name="encoding">エンコーディングを設定します。</param>
        /// <returns>デシリアライズされた オブジェクト</returns>
        /// <exception cref="ArgumentNullException">引数がnullの場合。</exception>
        /// <exception cref="ArgumentException"    >引数が空文字列である場合。</exception>
        /// <exception cref="DirectoryNotFoundException">マップされていないドライブにあるなど、指定されたパスが無効です。</exception>
        /// <exception cref="FileNotFoundException">ファイルが見つかりませんでした。</exception>
        /// <exception cref="IOException"><c>パス</c>の構文エラーが発生したとき。</exception>
        /// <exception cref="System.Security.SecurityException">セキュリティマネージャによってアクセスを拒否された場合。。</exception>
        /// <exception cref="JsonDataException">Json読み取り結果に問題がある場合。</exception>
        /// <exception cref="JsonException">Json読み取り中に問題が起きた場合。</exception>
        public static void ObjectToJsonFile(object obj, string path, bool isHumanReadable = false, Encoding encoding = null)
        {
            string text = ObjectToJson(obj, isHumanReadable);
            StreamWriter writer;
            if (encoding == null)
                writer = new StreamWriter(path, false);
            else
                writer = new StreamWriter(path, false, encoding);
            using (writer)
            {
                writer.Write(text);
                writer.Flush();
                var s = writer.BaseStream;
                s.SetLength(s.Position);
            }
        }
    }

}
