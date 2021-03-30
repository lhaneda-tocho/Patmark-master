using System;
using System.Linq;
using TokyoChokoku.Communication.Text;

namespace TokyoChokoku.Communication.OnControllerTextValidation
{
    public static class TextValidationMemory
    {
        const int RequiredByteSize = 11 * 2;
        private static readonly WordMapper   TextMapper;
        private static readonly UInt16Mapper SizeMapper;
        private static readonly UInt16Mapper StartMapper;

        static TextValidationMemory() {
            var chain = new MapperChain();
            TextMapper  = chain.Alloc(9, IndexType.Word).AsWord();
            SizeMapper  = chain.AllocUInt16();
            StartMapper = chain.AllocUInt16();

            // check
            var totalCount = chain.TotalByteCount();
            if (totalCount != RequiredByteSize) 
                throw new InvalidProgramException("TextValidationMemory size validation error: " + chain.TotalByteCount());
        }

        private static WideText Convert(string text)
        {
            //return WideText.Encode(String.Join("", TokyoChokoku.Text.TextUtil.ToFullForKana(text).Take(9)));
            return WideText.Encode(String.Join("", text.Take(9)));
        }

        /// <summary>
        /// 文字列からTextValidationメモリに書き込める形式に変換します.
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="formatter">Formatter.</param>
        /// <param name="text">Text.</param>
        public static Programmer From(EndianFormatter formatter, string text) {
            var data = new Programmer(formatter, RequiredByteSize);
            var w = Convert(text);
            TextMapper .PutTo(data, w, 0, w.Count);
            SizeMapper .PutTo(data, (UInt16)w.Count);
            StartMapper.PutTo(data, 1);
            return data;
        }
    }
}
