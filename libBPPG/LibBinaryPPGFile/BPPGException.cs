using System;
using System.IO;
using System.Runtime.Serialization;

namespace TokyoChokoku.Bppg
{
    public enum BPPGErrorCode: int
    {
        /**
         * BPPG ファイルとして認識されなかった場合にエラーが発生します。
         * このエラーは、BPPG.json が読めなかった場合に発生します。
         */
        Unrecognized,

        /**
         * サポートされていないバージョンを検出した際にエラーが発生します。
         */
        UnsupportedVersion,

        /**
         * BPPG ファイルと認識された上で、ファイルフォーマットに問題がある場合に発生します。
         */
        InvalidFormat
    }

    public static class EnumBPPGErrorCodeExt
    {
        public static  string ToLocalizationID(this BPPGErrorCode code)
        {
            return $"BPPGException{code}";
        }

        public static string ToDescription(this BPPGErrorCode code, string additionalMessage = null)
        {
            var id = code.ToLocalizationID();
            var rm = Resources.ExceptionMessages.ResourceManager;
            var desc = rm.GetString(id);
            if (additionalMessage == null)
                return desc;
            return $"{desc}: {additionalMessage}";
        }
    }

    public class BPPGException : IOException
    {
        /// <summary>
        /// エラーコードです。
        /// </summary>
        public BPPGErrorCode ErrorCode { get; }

        /// <summary>
        /// エラータイトルです。
        /// </summary>
        public string ErrorTitle => ErrorCode.ToString();

        public BPPGException(BPPGErrorCode code) : base(code.ToDescription())
        {
            ErrorCode = code;
        }

        public BPPGException(BPPGErrorCode code, Exception innerException) : base(code.ToDescription(), innerException)
        {
            ErrorCode = code;
        }

        public BPPGException(BPPGErrorCode code, string message) : base(code.ToDescription(message))
        {
            ErrorCode = code;
        }

        public BPPGException(BPPGErrorCode code, string message, Exception innerException) : base(code.ToDescription(message), innerException)
        {
            ErrorCode = code;
        }

        protected BPPGException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
