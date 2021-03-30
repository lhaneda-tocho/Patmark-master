using System;
using BitConverter;
namespace TokyoChokoku.Communication
{
    public static class EndianBitConverterExt
    {
        public static Word ToWord(this EndianBitConverter self, DiByte pack)
        {
            var v = self.ToUInt16(pack.ToBytes(), 0);
            return Word.Init(v);
        }

        public static DWord ToDWord(this EndianBitConverter self, TetraByte pack)
        {
            var v = self.ToUInt32(pack.ToBytes(), 0);
            return DWord.Init(v);
        }

        public static Byte[] GetBytes(this EndianBitConverter self, Word word)
        {
            return self.GetBytes(word.UInt);
        }

        public static Byte[] GetBytes(this EndianBitConverter self, DWord dword)
        {
            return self.GetBytes(dword.UInt);
        }
    }
}
