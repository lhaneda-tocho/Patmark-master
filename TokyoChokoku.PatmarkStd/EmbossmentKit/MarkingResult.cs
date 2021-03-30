using System;
using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public class MarkingResult : IUnstableDescription
    {
        public const string DefaultErrorMessage = "FAIL";

        public string Message { get; }
        public Exception InnerException { get; }
        public bool IsSuccess { get; }


        public static MarkingResult Success(string message = "SUCCESS")
        {
            return new MarkingResult(message, true, null);
        }

        public static MarkingResult Failure(string message = DefaultErrorMessage, Exception innerException = null)
        {
            return new MarkingResult(message, false, innerException);
        }

        public static MarkingResult<T> Success<T>(T value, string message = "SUCCESS")
        {
            return new MarkingResult<T>(value, message, true, null);
        }

        public static MarkingResult<T> Failure<T>(string message = DefaultErrorMessage, Exception innerException = null)
        {
            return new MarkingResult<T>(default(T), message, false, innerException);
        }


        public MarkingResult()
        {
            InnerException = null;
            IsSuccess = false;
            Message = "Filtered";
        }

        private MarkingResult(string message, bool isSuccess, Exception innerException)
        {
            InnerException = innerException;
            IsSuccess = isSuccess;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override string ToString()
        {
            return Message;
        }
    }


    public class MarkingResult<T> : IUnstableDescription
    {
        public string Message { get; }
        public Exception InnerException { get; }
        public bool IsSuccess { get; }
        public T Value { get; }


        public MarkingResult()
        {
            InnerException = null;
            IsSuccess = false;
            Message = "Filtered";
            Value = default(T);
        }

        internal MarkingResult(T value, string message, bool isSuccess, Exception innerException)
        {
            InnerException = innerException;
            IsSuccess = isSuccess;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Value = value;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
