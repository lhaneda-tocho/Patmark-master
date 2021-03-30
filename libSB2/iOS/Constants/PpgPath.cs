using System;
using System.IO;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class PpgPath
    {
        public static string LocalPpgDirectory {
            get {
                string documentPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
                string ppgDirectory = Path.Combine (documentPath, @"save/text");

                return ppgDirectory;
            }
        }

        public static string AutoSaveDirectory {
            get {
                string documentPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
                string ppgDirectory = Path.Combine (documentPath, @"auto_save/text");

                return ppgDirectory;
            }
        }

        public static bool IsValidName (string name)
        {
            //ファイル名に使用できない文字を取得
            char [] invalidChars = Path.GetInvalidFileNameChars ();

            if (string.IsNullOrEmpty (name)) {
                return false;
            }

            if (name.IndexOfAny (invalidChars) >= 0) {
                return false;
            }

            var dotStart = new System.Text.RegularExpressions.Regex (@"^\.");

            if (dotStart.IsMatch (name)) {
                return false;
            }

            return true;
        }
    }
}

