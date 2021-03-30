using System;
using TokyoChokoku.Patmark.Common;
namespace TokyoChokoku.Patmark.Settings
{
    public class CommunicationResult : IUnstableDescription
    {
        public string Message { get; }
        public Exception InnerException { get; }
        public bool IsSuccess { get; }
        /// <summary>
        /// コントローラのデータを書き換えたかどうかを表します。
        /// 書き換えた場合は true, そうでなければ false です。
        /// </summary>
        public bool IsChanged { get; }


        public static CommunicationResult SuccessWithoutChanged(string message = "SUCCESS")
        {
            return new CommunicationResult(message, true, false, null);
        }

        public static CommunicationResult SuccessChanged(string message = "SUCCESS")
        {
            return new CommunicationResult(message, true, true, null);
        }

        public static CommunicationResult Failure(string message = "FAIL", Exception innerException = null)
        {
            return new CommunicationResult(message, false, false, innerException);
        }

        public static CommunicationResult<T> Success<T>(T value, string message = "SUCCESS")
        {
            return new CommunicationResult<T>(value, message, true, null);
        }

        public static CommunicationResult<T> Failure<T>(string message = "FAIL", Exception innerException = null)
        {
            return new CommunicationResult<T>(default(T), message, false, innerException);
        }


        public CommunicationResult()
        {
            InnerException = null;
            IsSuccess = false;
            Message = "Filtered";
            IsChanged = false;
        }

        private CommunicationResult(string message, bool isSuccess, bool isChanged, Exception innerException)
        {
            InnerException = innerException;
            IsSuccess = isSuccess;
            IsChanged = isChanged;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override string ToString()
        {
            return Message;
        }
    }


    public class CommunicationResult<T> : IUnstableDescription
    {
        public string Message { get; }
        public Exception InnerException { get; }
        public bool IsSuccess { get; }
        public T Value { get; }


        public CommunicationResult()
        {
            InnerException = null;
            IsSuccess = false;
            Message = "Filtered";
            Value = default(T);
        }

        internal CommunicationResult(T value, string message, bool isSuccess, Exception innerException)
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
