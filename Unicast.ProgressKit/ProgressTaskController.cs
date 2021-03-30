using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// タスクの起動とイベントの配線を担当します。
    ///
    /// MEMO: ここで起きた大半のエラーが例外としてスローできないため、そのエラーをハンドルするためのロガーを用意した方がいいかもしれない。
    /// </summary>
    public class ProgressTaskController
    {
        /// <summary>
        /// タスク実行中の状態監視周期
        /// </summary>
        const int BusyLoopIntervalMs = 250;

        /// <summary>
        /// タスク停止中の状態監視周期
        /// </summary>
        const int IdleLoopIntervalMs = 7000;

        /// <summary>
        /// 実行中のタスクの集合
        /// </summary>
        HashSet<IProgressTask>                       TaskSet { get; } = new HashSet<IProgressTask>();

        /// <summary>
        /// ProgressViewController への弱参照
        /// </summary>
        HashSet<ProgressViewControllerWeakReference> ViewSet { get; } = new HashSet<ProgressViewControllerWeakReference>();

        /// <summary>
        /// 進捗を表示するためのProgressViewController を登録します。
        ///
        /// このメソッドは UI スレッドで呼び出す必要があります。
        /// </summary>
        /// <param name="worker">UIスレッドで動くワーカです。ProgressEventListenerToken の解放処理に使用します。</param>
        /// <param name="controller">登録するコントローラ</param>
        /// <returns>ProgressEventListenerToken. これを Dispose することで、 イベントの購読が解除されます。</returns>
        internal ProgressEventListenerToken AddViewController(ProgressTaskWorker worker, ProgressViewController controller)
        {
            // 弱参照自体は自動削除されない. 仕方ないのでコントローラを追加された時にクリーンアップするようにする。 
            ViewSet.RemoveWhere(it => it.IsExpired);
            // 弱参照を作成
            var weak = ProgressViewControllerWeakReference.Init(controller);
            // トークンを作成
            var token = ProgressEventListenerToken.Create(worker, controller, unregister: it =>
            {
                // コールバック呼び出し
                WillUnregistered(it);
                // 破棄
                ViewSet.Remove(weak);
            });
            // 弱参照をこのコントローラに追加
            ViewSet.Add(ProgressViewControllerWeakReference.Init(controller));
            // コールバック呼び出し
            DidRegistered(controller);
            // トークンを返す
            return token;
        }

        /// <summary>
        /// <c>ProgressTaskLauncher</c> を指定して、非同期タスクの登録を行います。
        /// 登録されたタスクは、このコントローラ上で実行され、その処理が終了するまでこのコントローラ上で管理されます。
        ///
        /// このメソッドは UI スレッドで呼び出す必要があります。
        /// </summary>
        /// <typeparam name="R">返り値の型</typeparam>
        /// <param name="launcher">タスクを起動するオブジェクト</param>
        /// <returns>起動したタスク</returns>
        internal Task<R> SubmitTask<R>(ProgressTaskLauncher<R> launcher)
        {
            var task = new ProgressTask<R>((handler) => HandleTask(handler, launcher));
            return task.StartIfNeeded();
        }

        /// <summary>
        /// コントローラの状態監視処理を開始します。
        /// OnBusy, OnIdle を定期呼び出しします。
        /// 
        /// このメソッドは UI スレッドで呼び出す必要があります。
        ///
        /// このメソッドは例外をスローしません。
        /// </summary>
        /// <param name="ct">ProgressAPI が終了する際に, キャンセル要求状態になる token</param>
        /// <returns></returns>
        /// <remarks>
        /// タスクのキャンセルはアプリを終了時に呼び出されることを想定しております。
        /// </remarks>
        internal async Task StartObserve(CancellationToken ct)
        {
            void RemoveGarbage()
            {
                try
                {
                    // 万が一、ゴミが残っていたら消す
                    TaskSet.RemoveWhere(it => it.IsCompleted);
                }
                catch (Exception ex)
                {
                    // 上記の処理で例外スローされても処理に影響はない。
                    // エラーを通知するだけに留める。(ここで例外がスローされても、誰も処理できない.)
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {ex}");
                }
            }

            int epochInterval = BusyLoopIntervalMs;
            int epochTime = 0;
            int idleIntervalTick = IdleLoopIntervalMs / epochInterval;

            try
            {
                var wasBusy = true;
                while (true)
                {
                    if (epochTime == 0)
                    {
                        RemoveGarbage();
                    }
                    if (TaskSet.Count > 0)
                    {
                        wasBusy = true;
                        OnBusy();
                    }
                    else if (epochTime == 0 || wasBusy)
                    {
                        wasBusy = false;
                        OnIdle();
                    }

                    await Task.Delay(epochInterval, ct);
                    epochTime = (epochTime+1) % idleIntervalTick;
                }
            } catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cancelled {ex}");
            }

            OnTerminate();
        }


        /// <summary>
        /// ProgressTask から Start 時に呼び出されるメソッドです。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Safe Throwable \n
        /// このメソッドでは、安全に例外をスローします。
        /// </summary>
        /// <exception cref="Exception">ProgressTaskLauncher もしくは、 ProgressTaskHandler で例外が生じた場合</exception>
        async Task<R> HandleTask<R>(ProgressTaskHandler handler, ProgressTaskLauncher<R> launcher)
        {
            // 例外を出さずにタスクを除去します。
            void SafeTaskRemove()
            {
                try
                {
                    TaskSet.Remove(handler.ProgressTask);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
                }
            }

            TaskSet.Add(handler.ProgressTask);
            DidSubmitTask(handler.ProgressTask);
            try
            {
                return await launcher.Launch(handler);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception In Handle Task: {ex}");
                throw ex;
            }
            finally
            {
                WillRemoveTask(handler.ProgressTask);
                SafeTaskRemove();
                OnIdleIfEffective();
            }
        }


        /// <summary>
        /// タスク起動時に UIスレッド上で呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void DidSubmitTask(IProgressTask sender)
        {
            foreach(var view in ViewSet)
            {
                try
                {
                    view.GetOrNull()?.DidSubmitTask(sender);
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
                }
            }
        }


        /// <summary>
        /// タスク消去前に呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void WillRemoveTask(IProgressTask sender)
        {
            foreach (var view in ViewSet)
            {
                try
                {
                    view.GetOrNull()?.WillRemoveTask(sender);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
                }
            }
        }

        /// <summary>
        /// OnIdle を呼びだした方が良い状態であれば、OnIdle を実行する。
        ///
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        ///
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void OnIdleIfEffective()
        {
            if(TaskSet.Count == 0)
            {
                OnIdle();
            }
        }

        /// <summary>
        /// アイドル状態の時に定期的に呼び出されます。
        /// ProgressViewController が ProgressAPI に登録してもらった時にも呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void OnIdle()
        {
            foreach (var view in ViewSet)
            {
                try
                {
                    view.GetOrNull()?.OnIdle();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
                }
            }
        }

        /// <summary>
        /// ビジー状態の時に定期的に呼び出されます。
        /// ProgressViewController が ProgressAPI に登録してもらった時にも呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void OnBusy()
        {
            var tasks = TaskSet.ToList().ToImmutableList();
            foreach (var view in ViewSet)
            {
                try
                {
                    view.GetOrNull()?.OnBusy(tasks);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
                }
            }
        }

        /// <summary>
        /// ProgressViewController が登録された時に呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        /// <param name="view">登録したい ProgressViewController インスタンス</param>
        void DidRegistered(ProgressViewController view)
        {
            try
            {
                view.ViewControllerDidRegisterd();
                if(TaskSet.Count > 0)
                {
                    view.OnBusy(TaskSet.ToList());
                } else
                {
                    view.OnIdle();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
            }
        }

        /// <summary>
        /// ProgressViewController の登録が抹消される時に呼び出されます。
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        /// <param name="view">登録を抹消したい ProgressViewController インスタンス</param>
        void WillUnregistered(ProgressViewController view)
        {
            try
            {
                view.ViewControllerWillUnregisterd();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled Exception {ex}");
            }
        }

        /// <summary>
        /// このコントローラが破棄される時に呼び出されます。
        /// (監視タスク側から自動的に実行されます)
        ///
        /// 
        /// Thread Safety: Anti Thread \n
        /// このメソッドは、 UI スレッド上で呼び出してください。
        /// 
        /// Exception Safety: Not Throwable \n
        /// このメソッドは、例外をスローすることができません。
        /// 内部で生じた例外は、ログに出力されます。
        /// </summary>
        void OnTerminate()
        {
            foreach(var tasks in TaskSet)
            {
                tasks.Cancel();
            }
        }
    }
}
