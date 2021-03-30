using System;
using System.IO;

namespace TokyoChokoku.Patmark.StorageUtil
{
    public abstract class FieldStorage
    {
        public static FieldStorage FromProvider
        {
            get => StorageProvider.FieldStorage;
        }


        public abstract string Dir { get; }


        public const string AutoSaveDirName = "AutoSave";


        Lazy<string> AutoSaveDirHolder;
        public string AutoSaveDir => AutoSaveDirHolder.Value;

        public string DefaultText
        {
            get
            {
                using(var reader = OpenOrCreateReaderIn(AutoSaveDir, "DefaultText.txt")) {
                    return reader.ReadToEnd();
                }
            }
            set
            {
                using(var writer = CreateWriter(AutoSaveDir, "DefaultText.txt")) {
                    if(value != null)
                        writer.Write(value);
                }
            }
        }


        protected FieldStorage()
        {
            AutoSaveDirHolder = new Lazy<string>(() =>
            {
                var dir =  Path.Combine(Dir, AutoSaveDirName);
                Console.WriteLine("resolve a path to AutoSave = " + dir);
                return dir;
            });
        }

        void CreateDirectory(string dir)
        {
            Directory.CreateDirectory(dir);
        }

        #region Utils
        protected FileStream CreateStreamIn(string dir, string name)
		{
			Console.WriteLine("Stream in " + dir);
            CreateDirectory(dir);
            var file = Path.Combine(dir, name);
            return new FileStream(file, FileMode.Create);
        }

        protected FileStream OpenStreamIn(string dir, string name)
		{
			Console.WriteLine("Stream in " + dir);
			CreateDirectory(dir);
            var file = Path.Combine(dir, name);
            return new FileStream(file, FileMode.Open);
        }

        protected FileStream OpenOrCreateStreamIn(string dir, string name)
		{
			Console.WriteLine("Stream in " + dir);
			CreateDirectory(dir);
            var file = Path.Combine(dir, name);
            return new FileStream(file, FileMode.OpenOrCreate);
        }


		protected StreamWriter CreateWriter(string dir, string name)
		{
			return new StreamWriter(CreateStreamIn(dir, name));
		}

        protected StreamReader OpenReaderIn(string dir, string name)
        {
            return new StreamReader(OpenStreamIn(dir, name));
        }

        protected StreamReader OpenOrCreateReaderIn(string dir, string name)
        {
            return new StreamReader(OpenOrCreateStreamIn(dir, name));
        }
        #endregion
    }
}
