using System.IO;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public struct PathName
    {
        public string Full { get; }

        public string Simple { get; }

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

        public static Maybe<PathName> FromPath (Maybe<string> maybePath) {
            return
                from path in maybePath
                select FromPath (path);
        }
    }
}

