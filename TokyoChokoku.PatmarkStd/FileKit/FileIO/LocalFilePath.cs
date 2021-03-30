using System;
using System.IO;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;
using Monad;

namespace TokyoChokoku.Patmark
{
    using Common;

    public class LocalFilePath : AutoComparableValueType<LocalFilePath>
    {
        private static void ShouldNotNullAndBlank(string path, string argname)
        {
            path = path ?? throw new ArgumentNullException($"{argname} should not null");
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{argname} should not blank");
        }

        private static void ShouldFileName(string path, string argname)
        {
            if (IsFileNameWith(path))
                return;
            throw new ArgumentException($"{argname} should be file name.");
        }

        /// <summary>
        /// パス末端が / で終わるかどうかを確認する。
        /// </summary>
        private static bool IsDirectoryPathWith(string validPath)
        {
            var sp1 = Path.DirectorySeparatorChar.ToString();
            var sp2 = Path.AltDirectorySeparatorChar.ToString();

            return validPath.EndsWith(sp1) || validPath.EndsWith(sp2);
        }

        private static bool IsFileNameWith(string validPath)
        {
            var sp1 = Path.DirectorySeparatorChar.ToString();
            var sp2 = Path.AltDirectorySeparatorChar.ToString();

            return !validPath.Contains(sp1) && !validPath.Contains(sp2);
        }

        /// <summary>
        /// パス末端の / を削除したもの。 ただし、 パスの長さが1文字の場合は / が来ても削除せずにそのまま返します。
        /// </summary>
        private static string NormalizeWith(string validPath)
        {
            if (validPath.Length > 1 && IsDirectoryPathWith(validPath))
            {
                return validPath.Substring(0, validPath.Length - 1);
            }
            else
            {
                return validPath;
            }
        }

        /// <param name="path">
        /// 確認したい文字列
        /// </param>
        /// <exception cref="ArgumentException">
        ///     パスが不正の場合
        /// </exception>
        private static void ValidPath(string path)
        {
            // バリデーション(強引な方法だが)
            try
            {
                Path.GetFullPath(path);
            }
            catch (Exception ex) when (ex is System.Security.SecurityException || ex is UnauthorizedAccessException)
            {
                // パスが正しいことを表しているので無視
            }
            catch (Exception ex) when (ex is PathTooLongException || ex is NotSupportedException)
            {
                throw new ArgumentException($" {ex.Message}　({ex})", ex);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        public static LocalFilePath Create(string path)
        {
            return new LocalFilePath(path);
        }

        // ==== ==== ==== ==== ==== ==== ==== ====

        /// <summary>
        /// 挿入された文字列
        /// </summary>
        private string Text { get; }

        private string Normalized { get; }

        public int Count => Text.Length;

        /// <summary>
        /// ファイル名、もしくはディレクトリ名です。 最後が / で終わる場合は その1つ前の名前を返します。
        /// </summary>
        private string LastName
        {
            get
            {
                return Path.GetFileName(Normalized);
            }
        }

        /// <summary>
        /// ディレクトリパスです。 ファイル名のみの文字列である場合は Empty です。
        /// 名前末尾に ディレクトリセパレータは入りません。
        /// </summary>
        private string DirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(Text);
            }
        }

        public bool IsPathRooted
        {
            get
            {
                return Path.IsPathRooted(Text);
            }
        }

        public bool IsDirectoryPath
        {
            get
            {
                return IsDirectoryPathWith(Text);
            }
        }

        /// <summary>
        /// パスを指定して初期化します。
        /// </summary>
        /// <param name="path">パス文字列</param>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, パスとして不正な場合</exception>
        protected LocalFilePath(string path)
        {
            ShouldNotNullAndBlank(path: path, argname: nameof(path));
            ValidPath(path);
            Text = path;
            Normalized = NormalizeWith(path);
        }

        /// <summary>
        /// パスの最後の名前が指定された拡張子を持つか調べます。
        /// 先頭に . を付けた文字列を渡した場合は、ファイル名末端がその文字列に一致することを確認します。
        /// つけなくとも 自動的に . を追加してファイル名末端と一致するか確認します。
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>一致した場合は true, そうでなければ false.</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, 拡張子名として不正な場合</exception>
        public bool HasExtension(string extension)
        {
            ShouldNotNullAndBlank(path: extension, argname: nameof(extension));
            ValidPath(extension);
            ShouldFileName(path: extension, argname: nameof(extension));

            if (!extension.StartsWith("."))
                extension = $".{extension}";

            return LastName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// パスの最後の名前が指定された拡張子を持つか調べ、一致したものを返します。
        /// 先頭に . を付けた文字列を渡した場合は、ファイル名末端がその文字列に一致することを確認します。
        /// つけなくとも 自動的に . を追加してファイル名末端と一致するか確認します。
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>一致した拡張子名(引数に指定したものと同じ)。存在しない場合は 空.</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, 拡張子名として不正な場合</exception>
        public Option<string> GetMatchedExtension(params string[] extensions)
        {
            extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
            int i = 0;
            foreach(var item in extensions)
            {
                var extension = item;
                ShouldNotNullAndBlank(path: extension, argname: $"{nameof(extension)}[{i}]");
                ValidPath(extension);
                ShouldFileName(path: extension, argname: $"{nameof(extension)}[{i}]");

                if (extension.StartsWith("."))
                    extension = $".{extension}";

                if (LastName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase))
                    return Option.Return(()=>extension);
            }
            return Option.Nothing<string>();
        }

        /// <summary>
        /// パス末尾が ディレクトリセパレータの場合はそれを削除したパスを返します。
        /// </summary>
        /// <returns></returns>
        public LocalFilePath Normalize()
        {
            return new LocalFilePath(Normalized);
        }

        /// <summary>
        /// 名前を追加します。
        /// </summary>
        /// <param name="name">ファイル・ディレクトリ名。(Directory Separator を含まない)</param>
        /// <returns>新しいファイルパス</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, ファイル名として不正な場合</exception>
        public LocalFilePath AppendName(string name)
        {
            ShouldNotNullAndBlank(path: name, argname: nameof(name));
            ValidPath(name);
            ShouldFileName(path: name, argname: nameof(name));

            return new LocalFilePath(Path.Combine(Normalized, name));
        }

        /// <summary>
        /// 引数に指定した拡張子を持たない場合、指定した拡張子を追加した 新しい LocalFilePath を取得します。
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>新しいファイルパス</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合,  拡張子名として不正な場合</exception>
        public LocalFilePath SetExtension(string extension)
        {
            ShouldNotNullAndBlank(path: extension, argname: nameof(extension));
            ValidPath(extension);
            ShouldFileName(path: extension, argname: nameof(extension));

            if (!extension.StartsWith("."))
                extension = $".{extension}";

            if (HasExtension(extension))
                return this;

            return new LocalFilePath(Normalized + extension);
        }

        /// <summary>
        /// 引数に指定した拡張子を持つ場合、その拡張子を削除します。持たない場合は何もしません。
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>新しいファイルパス</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, 拡張子名として不正な場合</exception>
        public LocalFilePath RemoveExtension(string extension)
        {
            ShouldNotNullAndBlank(path: extension, argname: nameof(extension));
            ValidPath(extension);
            ShouldFileName(path: extension, argname: nameof(extension));

            if (!extension.StartsWith("."))
                extension = $".{extension}";

            if (!HasExtension(extension))
                return this;

            return new LocalFilePath(Normalized.Substring(0, Normalized.Length - extension.Length));
        }

        /// <summary>
        /// 引数に指定した拡張子を持つ場合、その拡張子を削除します。持たない場合は何もしません。
        /// </summary>
        /// <param name="extension">拡張子</param>
        /// <returns>新しいファイルパス</returns>
        /// <exception cref="ArgumentNullException">引数が null の場合</exception>
        /// <exception cref="ArgumentException">引数が空の場合, 拡張子名として不正な場合</exception>
        public LocalFilePath RemoveExtensions(params string[] extensions)
        {
            foreach(var extension in extensions)
            {
                var p = RemoveExtension(extension);
                if (Count == p.Count)
                    continue;
                return p;
            }
            return this;
        }

        public override string ToString()
        {
            return Text;
        }

        public string ToStringNormalized()
        {
            return Normalized;
        }

        protected override ListValueType<object> GetValueList()
        {
            return ListValueType<object>.CreateBuilder()
                .Add(Text)
                .Build();
        }

        public override int CompareTo(LocalFilePath other)
        {
            return Text.CompareTo(other.Text);
        }
    }

}
