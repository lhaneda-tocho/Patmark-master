using System.IO;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class LocalFilePathExt
    {
        static LocalFilePathExt()
        {
        }

        public static string DirectoryPath {
            get {
                return PpgPath.LocalPpgDirectory;
            }
        }

        /// <summary>
        /// 必要であればディレクトリの作成を行います．
        /// </summary>
        public static void ReadyDirectory ()
        {
            string path = DirectoryPath;
            if (!Directory.Exists (path))
                Directory.CreateDirectory (path);
        }

        /// <summary>
        /// string の内容がファイル名として有効である時，それを 端末内のppgを表すパスに変換します．
        /// string の内容がファイル名として扱えない場合は 空 を返します．
        /// </summary>
        /// <returns>The local ppg path.</returns>
        /// <param name="name">Name.</param>
        public static Maybe<string> ToLocalPpgPath (this string name)
        {
            if (!PpgPath.IsValidName (name))
                return Maybe<string>.Nothing;

            var endDotppg = new System.Text.RegularExpressions.Regex (@"\.ppg$");
            string fileName;

            if (endDotppg.IsMatch (name)) {
                fileName = name;
            } else {
                fileName = name + ".ppg";
            }

            var path = Path.Combine (DirectoryPath, fileName);

            return path.ToMaybe ();
        }
    }
}

