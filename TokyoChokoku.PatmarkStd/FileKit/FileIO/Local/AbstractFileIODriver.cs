using System;
using System.IO;
using Monad;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// 抽象化ドライバです。
    /// </summary>
    public abstract class AbstractFileIODriver : ILocalMBFileIODriver
    {
        /// TODO: LocalFileIOCouldNotAccess 以外の 例外発生時の処理を考える。
        public bool SaveEmpty(LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            if (Directory.Exists(p))
                throw new LocalFileIOCouldNotAccessException("Directory Exists");
            if (File.Exists(p))
                return false;

            WriteEmpty(path);

            return true;
        }

        /// TODO: LocalFileIOCouldNotAccess 以外の 例外発生時の処理を考える。
        public bool SaveAs(FileOwner owner, LocalFilePath path)
        {
            owner = owner ?? throw new ArgumentNullException();

            var p = path.ToStringNormalized();

            if (Directory.Exists(p))
                throw new LocalFileIOCouldNotAccessException("Directory Exists");
            if (File.Exists(p))
                return false;

            Write(owner, path);

            return true;
        }

        /// TODO: LocalFileIOCouldNotAccess 以外の 例外発生時の処理を考える。
        public void SaveOver(FileOwner owner, LocalFilePath path)
        {
            owner = owner ?? throw new ArgumentNullException();

            var p = path.ToStringNormalized();

            if (Directory.Exists(p))
                throw new LocalFileIOCouldNotAccessException("Directory Exists");

            Write(owner, path);
        }

        public Option<FileOwner> Load(LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            if (Directory.Exists(p))
                throw new LocalFileIOCouldNotAccessException("Directory Exists");

            if (!File.Exists(p))
                return Option.Nothing<FileOwner>();

            var ans = Read(path);
            return Option.Return(() => ans);

        }

        public Option<string> LoadToString(LocalFilePath path)
        {
            var p = path.ToStringNormalized();

            if (Directory.Exists(p))
                throw new LocalFileIOCouldNotAccessException("Directory Exists");

            if (!File.Exists(p))
                return Option.Nothing<string>();

            var ans = ReadToString(path);
            return Option.Return(() => ans);
        }

        protected abstract void WriteEmpty(LocalFilePath path);

        protected abstract void Write(FileOwner owner, LocalFilePath path);

        protected abstract FileOwner Read(LocalFilePath path);

        protected abstract string ReadToString(LocalFilePath path);
    }

}

