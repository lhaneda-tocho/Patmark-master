using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// 非同期タスクを起動するインタフェースです。
    /// </summary>
    public interface ProgressTaskWorker
    {
        /// <summary>
        /// Taskを返すべき関数から null が返された場合にスローします。
        /// </summary>
        public class NullTaskException : Exception
        {
            /// <inheritdoc/>
            public NullTaskException()
            {
            }

            /// <inheritdoc/>
            public NullTaskException(string message) : base(message)
            {
            }

            /// <inheritdoc/>
            public NullTaskException(string message, Exception innerException) : base(message, innerException)
            {
            }

            /// <inheritdoc/>
            protected NullTaskException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        /// <summary>
        /// タスクを開始するユーティリティメソッドです。
        ///
        /// 呼び出し元の SynchronizationContext で実行されます。
        /// <c>block</c> が <c>null</c> を返す場合は、 <c>NullTaskException</c> をスローします。
        /// </summary>
        /// <typeparam name="T">メッセージ型</typeparam>
        /// <typeparam name="R">返り値型</typeparam>
        /// <param name="message">メッセージ</param>
        /// <param name="block">非同期タスク処理</param>
        /// <returns>タスクの返り値</returns>
        protected static async Task<R> StartTaskAsync<T, R>(T message, Func<T, Task<R>> block)
        {
            var task = block(message);
            if (ReferenceEquals(task, null))
            {
                throw new NullTaskException();
            }
            return await task;
        }

        /// <summary>
        /// タスクを開始するユーティリティメソッドです。
        ///
        /// 呼び出し元の SynchronizationContext で実行されます。
        /// <c>block</c> が <c>null</c> を返す場合は、 <c>NullTaskException</c> をスローします。
        /// </summary>
        /// <typeparam name="T">メッセージ型</typeparam>
        /// <param name="message">メッセージ</param>
        /// <param name="block">非同期タスク処理</param>
        /// <returns>タスクの返り値</returns>
        protected static async Task StartTaskAsync<T>(T message, Func<T, Task> block)
        {
            var task = block(message);
            if (ReferenceEquals(task, null))
            {
                throw new NullTaskException();
            }
            await task;
        }

        // ========

        /// <summary>
        /// 現在のスレッドを使ってタスクの起動を行う(もしくは行える)場合は true を返します。
        /// そうでなければ false です。
        ///
        /// タスクが起動されるスレッドが不明な場合は常に false を返します。
        /// </summary>
        public bool CurrentThreadUsing();


        /// <summary>
        /// このワーカ上でタスクを起動します。
        /// </summary>
        /// <typeparam name="T">task の引数に渡すオブジェクトの型</typeparam>
        /// <typeparam name="R">返り値型</typeparam>
        /// <param name="message">task の引数に渡すオブジェクト</param>
        /// <param name="block">実行して欲しい処理</param>
        /// <returns>起動したタスク</returns>
        public Task<R> Launch<T, R>(T message, Func<T, Task<R>> block);


        /// <summary>
        /// このワーカ上でタスクを起動します。
        /// </summary>
        /// <typeparam name="T">task の引数に渡すオブジェクトの型</typeparam>
        /// <param name="message">task の引数に渡すオブジェクト</param>
        /// <param name="block">実行して欲しい処理</param>
        /// <returns>起動したタスク</returns>
        public Task Launch<T>(T message, Func<T, Task> block);
    }
}
