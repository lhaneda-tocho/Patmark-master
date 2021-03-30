using System;
using System.Runtime.Serialization;

namespace TokyoChokoku.Patmark
{
    /// <summary>
    /// LocalFileIO にてパスに指定された場所にアクセス・操作できない場合にスローされます。
    /// エラーの原因は message に渡されます。
    /// </summary>
    public class LocalFileIOCouldNotAccessException : Exception
    {
        public LocalFileIOCouldNotAccessException(string message) : base(message)
        {
        }

        public LocalFileIOCouldNotAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocalFileIOCouldNotAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
