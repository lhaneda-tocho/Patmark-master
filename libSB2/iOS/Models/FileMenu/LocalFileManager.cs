using System;
using System.IO;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class LocalFileManager
    {
        public static bool CreateNew (string path)
        {
            var context = iOSFieldContext.Create ();
            iOSFieldContext.LoadDemo (context);
            return SaveAs (context, path);
        }

        public static bool SaveAs (List<iOSOwner> fieldList, string path)
        {
            if (File.Exists (path))
                return false;

            using (TextWriter writer = File.CreateText (path)) {
                MBDataTextizer.Save (writer, fieldList.ToArray ());
            }

            return true;
        }

        public static bool SaveAs (iOSFieldContext context, string path)
        {
            if (File.Exists (path))
                return false;

            using (TextWriter writer = File.CreateText (path)) {
                MBDataTextizer.Save (writer, context.Serializable);
            }

            return true;
        }

        public static bool SaveOver (List<iOSOwner> fieldList, string path)
        {
            using (TextWriter writer = File.CreateText (path)) {
                MBDataTextizer.Save (writer, fieldList.ToArray ());
            }

            return true;
        }

        public static bool SaveOver (iOSFieldContext context, string path)
        {
            using (TextWriter writer = File.CreateText (path)) {
                MBDataTextizer.Save (writer, context.Serializable);
            }

            return true;
        }

        public static List<iOSOwner> LoadPpgAsList (string path)
        {
            var text = File.ReadAllText (path);
            return iOSOwner.From( FieldFactory.ParsePpg (text) );
        }

        public static void LoadPpgAsContext (string path, iOSFieldContext dest)
        {
            dest.TrySubmitAll (LoadPpgAsList (path));
        }

        public static void LoadToConsole (string path)
        {
            if (File.Exists (path)) {
                string text = File.ReadAllText (path);
                Console.WriteLine (text);
            }
        }

        public static List<string> GetLocalPpgPathList ()
        {
            var ppgDirectory = LocalFilePathExt.DirectoryPath;

            if (!Directory.Exists (ppgDirectory))
                return new List<string> ();

            string [] files = Directory.GetFiles (ppgDirectory, "*.ppg");
            return new List<string> (files);
        }
    }
}

