using System;
namespace TokyoChokoku.Communication.Binalizer
{

    public enum BytePack
    {
        Mono, Di, Tetra
    }

    public static class BytePackExt
    {
        public static int Size(this BytePack self) {
            switch (self)
            {
                case BytePack.Mono : return 1;
                case BytePack.Di   : return 2;
                case BytePack.Tetra: return 4;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
