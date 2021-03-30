using System;
using System.Runtime.Serialization;

namespace TokyoChokoku.Bppg.Json
{
    /// <summary>
    /// Jsonの解析ができても、内容に問題がある場合にスローされる例外です。
    /// </summary>
    public class JsonDataException : Newtonsoft.Json.JsonException
    {
        public JsonDataException()
        {
        }

        public JsonDataException(string message) : base(message)
        {
        }

        public JsonDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public JsonDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
