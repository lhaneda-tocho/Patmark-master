using System;
namespace Unicast.ProgressKit
{
    /// <summary>
    /// ProgressViewController のイベントの講読を行っていることを表すオブジェクトです。
    /// イベントの講読を直ちに停止したい場合は、<c>Dispose</c> メソッドを呼び出します。
    ///
    /// Dispose が呼ばれるか、 このトークンが参照する <c>ProgressViewController</c>が GC されるか
    /// のいずれかで　イベントの購読が終了します。
    ///
    /// Dispose メソッドを呼び出した場合に限り、 ProgressViewController.WillUnregisterd コールバックが呼び出されます。
    /// 確実に・安全に処理ができるよう、Dispose メソッドを使用して講読を終了することをお勧めいたします。
    /// </summary>
    public class ProgressEventListenerToken: IDisposable
    {

        /// <summary>
        /// トークンを作成します。
        ///
        /// 引数に指定したオブジェクトへの参照は、 「<c>Dispose</c> されるまで」作成したトークンによって参照されます。
        /// </summary>
        /// <typeparam name="T">リスナの型</typeparam>
        /// <param name="disposeOn">UI スレッドのワーカを指定します。このスレッド以外からの <c>dispose</c>呼び出しを禁止するために使用します。</param>
        /// <param name="reference">リスナへの強参照</param>
        /// <param name="unregister">イベントの講読終了メソッド. 例外をスローしてはなりません。</param>
        /// <returns>作成したトークンです。</returns>
        public static ProgressEventListenerToken Create<T>(ProgressTaskWorker disposeOn, T reference, Action<T> unregister) where T: class
        {
            return new ProgressEventListenerToken(
                disposeOn,
                reference,
                () => unregister(reference)
            );
        }

        // ================

        private readonly object theLock = new object();

        /// <summary>
        /// 保持中のオブジェクトです。Dispose 後は、 null となります。
        /// </summary>
        public object ReferenceOrNull { get; private set; }

        Action Deleter { get; set; }
        ProgressTaskWorker DisposeOn { get; set; }


        ProgressEventListenerToken(ProgressTaskWorker disposeOn, object reference, Action deleter)
        {
            ReferenceOrNull = reference ?? throw new ArgumentNullException(nameof(reference));
            Deleter         = deleter   ?? throw new ArgumentNullException(nameof(deleter));
            DisposeOn       = disposeOn ?? throw new ArgumentNullException(nameof(disposeOn));
        }

        /// <summary>
        /// イベントの講読を終了し、<c>reference</c> への強参照を切ります。
        /// </summary>
        public void Dispose()
        {
            Action deleter = null;
            object reference = null;
            lock(theLock)
            {
                var disposeOn = DisposeOn;
                if (ReferenceEquals(disposeOn, null))
                    return;
#if ENABLE_THREAD_CHECK
                System.Diagnostics.Debug.WriteLine($"Before disposeOn.CurrentThreadUsing()");
                if (!disposeOn.CurrentThreadUsing())
                {
                    System.Diagnostics.Debug.WriteLine("Invocation on non ui thread.");
                    throw new InvalidOperationException("Invocation on non ui thread.");
                }
                System.Diagnostics.Debug.WriteLine($"After disposeOn.CurrentThreadUsing()");
#endif
                deleter = Deleter;
                reference = ReferenceOrNull;
                Deleter         = null;
                DisposeOn       = null;
                ReferenceOrNull = null;
            }
            deleter();
        }
    }
}
