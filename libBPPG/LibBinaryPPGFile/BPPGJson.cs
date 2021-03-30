using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using TokyoChokoku.Bppg.Json;

namespace TokyoChokoku.Bppg
{
    [JsonObject]
    class BPPGJson
    {

        /// <summary>
        /// 引数 <c>json</c> を <c>BPPGJson</c> オブジェクトに デシリアライズします。
        /// </summary>
        /// <typeparam name="T">デシリアライズしたいオブジェクトの型</typeparam>
        /// <param name="json">json テキストデータ</param>
        /// <returns>デシリアライズされた オブジェクト</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合。</exception>
        /// <exception cref="JsonDataException">
        ///     Json読み取り中に <c>ArgumentException</c> が起きたか、<c>JsonDataException</c> が起きた、もしくは 引数の<c>json</c>文字列が空欄・空の場合。
        /// </exception>
        /// <exception cref="JsonException">
        ///     Json読み取り中に問題が起きた場合。
        /// </exception>
        public static BPPGJson FromJson(string json)
        {
            return NewtonsoftJsonIO.ObjectFromJson<BPPGJson>(json);
        }

        // ========


        [JsonProperty("version")]
        public string Version { get; }


        [JsonConstructor]
        public BPPGJson(
            [JsonProperty("version", Required = Required.Always)] string version
        )
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));

            // TODO: バージョン分岐に対応する  (今は 1.0 しかないのでこれでいい)
            if(version != "1.0")
            {
                throw new ArgumentException("version は 1.0 である必要があります");
            }
        }


        /// <summary>
        /// オブジェクトをJson文字列に変換します。
        /// </summary>
        /// <param name="isHumanReadable">true にすると、より人が読みやすい形式でシリアライズします。デフォルト値は true です。</param>
        /// <returns>Json文字列</returns>
        public string ToJson(bool isHumanReadable=true)
        {
            return NewtonsoftJsonIO.ObjectToJson(this, isHumanReadable);
        }


        /// <summary>
        /// <c>ToJson を呼びだします。</c>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToJson();
        }
    }
}