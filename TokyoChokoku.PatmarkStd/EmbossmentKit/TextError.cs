using System;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public struct TextError
    {
        public bool HasError;
        public char ErrorChar;
        public int  Index;

        public static TextError Error(char errorChar, int index)
        {
            TextError st;
            st.HasError  = true;
            st.ErrorChar = errorChar;
            st.Index     = index;
            return st;
        }

        public static TextError Success()
        {
            TextError st;
            st.HasError  = false;
            st.ErrorChar = '\0';
            st.Index     = 0;
            return st;
        }
    }
}
