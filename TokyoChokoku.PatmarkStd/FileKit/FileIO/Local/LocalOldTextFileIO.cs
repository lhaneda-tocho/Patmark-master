using System;
using System.IO;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;
using Monad;


namespace TokyoChokoku.Patmark.FileKit.FileIO
{
    // 旧形式
    internal static class LocalOldTextFileIO
    {
        public static bool SaveEmpty(string path)
        {
            if (File.Exists(path))
                return false;

            using (TextWriter writer = File.CreateText(path))
            {
                MBDataTextizer.Save(writer, new List<MBData> { });
            }

            return true;
        }

        public static bool SaveAs(FileOwner owner, string path)
        {
            if (File.Exists(path))
                return false;

            using (TextWriter writer = File.CreateText(path))
            {
                MBDataTextizer.Save(writer, owner.Serializable);
            }

            return true;
        }

        public static bool SaveOver(FileOwner owner, string path)
        {
            using (TextWriter writer = File.CreateText(path))
            {
                MBDataTextizer.Save(writer, owner.Serializable);
            }

            return true;
        }

        public static Option<FileOwner> LoadPpgAsList(string path)
        {
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                return Option.Return(() => {
                    return new FileOwner(MBDataTextizer.Of(new StringReader(text)).ToMBData());
                });
            }
            else
            {
                return Option.Nothing<FileOwner>();
            }
        }

        public static void LoadToConsole(string path)
        {
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                Console.WriteLine(text);
            }
        }

        public static List<string> GetLocalPpgPathList()
        {
            var ppgDirectory = LocalFilePathGeneratorPublisher.Instance.DirectorySave();

            if (!Directory.Exists(ppgDirectory))
                return new List<string>();

            string[] files = Directory.GetFiles(ppgDirectory, "*.ppg");
            return new List<string>(files);
        }
    }
}
