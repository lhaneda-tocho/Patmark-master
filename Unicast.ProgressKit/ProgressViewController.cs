using System.Collections.Generic;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// Progress Bar を表示する View のためのコントローラです。
    /// </summary>
    public interface ProgressViewController
    {
        /// <summary>
        /// タスクが追加された後に呼び出されます。
        /// </summary>
        /// <param name="task">起動されたタスク</param>
        void DidSubmitTask(IProgressTask task);

        /// <summary>
        /// タスクが消去される直前に呼び出されます。
        /// </summary>
        /// <param name="task">消去されるタスク</param>
        void WillRemoveTask(IProgressTask task);

        /// <summary>
        /// アイドル状態の時に定期的に呼び出されます。
        /// ProgressViewController が ProgressAPI に登録された時にも呼び出されます。
        /// </summary>
        void OnIdle();

        /// <summary>
        /// ビジー状態の時に定期的に呼び出されます。
        /// ProgressViewController が ProgressAPI に登録された時にも呼び出されます。
        /// </summary>
        /// <param name="tasks">起動中のタスク</param>
        void OnBusy(IList<IProgressTask> tasks);

        /// <summary>
        /// このコントローラが ProgressAPI に登録された際に呼び出されます。
        /// OnIdle, OnBusy よりも先に呼び出されます。
        /// デフォルト実装では何も行いません。
        /// </summary>
        void ViewControllerDidRegisterd() { }

        /// <summary>
        /// このコントローラが ProgressAPI から登録解除される直前に呼び出されます。
        /// デフォルト実装では何も行いません。
        ///
        /// このメソッドは、 <c>ProgressEventListenerToken.Dispose()</c> された時に呼び出されます。
        /// ProgressEventListenerToken への参照が期限切れになり、
        /// ProgressAPI から登録解除された場合は,  このメソッドは呼び出されない点に注意してください。
        /// </summary>
        void ViewControllerWillUnregisterd() { }
    }
}
