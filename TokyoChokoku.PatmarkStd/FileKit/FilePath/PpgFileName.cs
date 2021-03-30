using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using Monad;

namespace TokyoChokoku.Patmark
{
    public static class PpgFileName
    {
        private static ISet<string> SupportExtensions { get; } = ImmutableHashSet.Create<string>(
            "bppg", "ppg"
        );

        /// <summary>
        /// ファイルパスを取得します。
        /// </summary>
        /// <param name="baseName">基本となる名前です。</param>
        /// <param name="extension">ファイルタイプです。 "ppg", "bppg" が入ります。 "*" を指定した場合は サポートする任意の拡張子となります。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">引数が null の場合。</exception>
        public static Option<string> From(string baseName, string extension)
        {
            baseName  = baseName  ?? throw new ArgumentNullException(nameof(baseName));
            extension = extension ?? throw new ArgumentNullException(nameof(extension));

            if (!IsValidName(baseName))
                return Option.Nothing<string>();
            if(!SupportExtensions.Contains(extension) && extension != "*")
                return Option.Nothing<string>();

            var path = LocalFilePath.Create(baseName);

            if(extension != "*")
            {
                path = path.RemoveExtensions(SupportExtensions.ToArray());
                path = path.SetExtension(extension);
            }

            string fileName = path.ToString();
            return Option.Return(() => fileName);
        }

        public static bool IsValidName(string name)
        {
            //ファイル名に使用できない文字を取得
            char[] invalidChars = Path.GetInvalidFileNameChars();

            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (name.IndexOfAny(invalidChars) >= 0)
            {
                return false;
            }

            var dotStart = new Regex(@"^\.");

            if (dotStart.IsMatch(name))
            {
                return false;
            }

            return true;
        }
    }
}
