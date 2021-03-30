using System.IO;
using Monad;

namespace TokyoChokoku.Patmark
{
    public struct PathName
    {
        public string Full { get; }

        public string Simple { get; }

        public LocalFilePath ToLocalFilePath() => LocalFilePath.Create(Full);

        PathName (string path, string name)
        {
            Full = path;
            Simple = name;
        }
        public override bool Equals (object obj)
        {
            if (!(obj is PathName))
                return false;
            var right = (PathName) obj;
            return Full.Equals (right.Full);
        }

        public override int GetHashCode ()
        {
            return Full.GetHashCode ();
        }

        public static PathName FromPath (string path)
        {
            return new PathName (
                path,
                Path.GetFileName (path)
            );
        }

        public static Option<PathName> FromPath (Option<string> maybePath) {
            return
                from path in maybePath
                select FromPath (path);
        }
    }
}

