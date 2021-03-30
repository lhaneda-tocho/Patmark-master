using System;
using TokyoChokoku.Communication;

using TokyoChokoku.Text;

namespace TokyoChokoku.Communication.Text
{
    public static class TextExt
    {
        public static Programmer PutMonoByteText(this Programmer self, MonoByteText text, int index, int count, IndexType type)
        {
            return self.PutBytes(text, index, count, type);
        }

        public static Programmer PutWideText(this Programmer self, WideText text, int index, int count, IndexType type)
        {
            return self.PutWords(text, index, count, type);
        }


        public static Programmer PutMonoByteText(this Programmer self, string text, int index, int count, IndexType type)
        {
            var mono = MonoByteText.Encode(text);
            return self.PutMonoByteText(mono, index, count, type);
        }

        public static Programmer PutWideText(this Programmer self, string text, int index, int count, IndexType type)
        {
            var wide = WideText.Encode(text);
            return self.PutWideText(wide, index, count, type);
        }

        public static MonoByteText GetMonoByteText(this Programmer self, int index, int count, IndexType type)
        {
            var bytes = self.GetBytes(index, count, type);
            return new MonoByteText(bytes);
        }

        public static WideText GetWideText(this Programmer self, int index, int count, IndexType type)
        {
            var words = self.GetWords(index, count, type);
            return new WideText(words);
        }
    }
}
