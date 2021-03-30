using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin;

using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.iOS.Presenter.FieldPreview;
using TokyoChokoku.Patmark.iOS.Presenter.FieldEditor;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using System.Linq;

namespace TokyoChokoku.Patmark.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public static ConnectionStateObserver       ConnectionStateObserver { get; private set; }
        public static SynchronizationContext        UI { get; private set; }
        public static CommunicationClientController CommunicationClientController { get; private set; }

        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        /// <summary>
        /// 通信可能な時に別スレッドで　<c>block</c> を呼び出します。
        /// <c>block</c> の実行中はUIのロックも行います。
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task CommunicateOnReady(Func<Task> onOffline, Func<Task> block)
        {
            return CommunicateOnReady(false, onOffline, block);
        }

        /// <summary>
        /// 通信可能な時に別スレッドで　<c>block</c> を呼び出します。
        /// <c>block</c> の実行中はUIのロックも行います。
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task CommunicateOnReady(bool terminator, Func<Task> onOffline, Func<Task> block)
        {
            if (CommunicationClientController.CurrentRound == CommunicationRound.Ready)
            {
                return LoadingOverlay.ShowWithTask(terminator, it => block());
            }
            else
            {
                return onOffline();
            }
        }

        /// <summary>
        /// 通信可能な時に別スレッドで　<c>block</c> を呼び出します。
        /// <c>block</c> の実行中はUIのロックも行います。
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Task<T> CommunicateOnReady<T>(Func<Task<T>> onOffline, Func<Task<T>> block)
        {
            if (CommunicationClientController.CurrentRound == CommunicationRound.Ready)
            {
                return LoadingOverlay.ShowWithTask<T>(it => block());
            } else
            {
                return onOffline();
            }
        }

        public static Task RunOnMain(Func<Task> task)
        {
            return RunOnMain(false, task);
        }

        public static Task RunOnMain(bool terminator, Func<Task> task)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            UI.Post(async (_) =>
            {
                try
                {
                    await task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    if(terminator)
                    {
                        Log.Error($"Unhandled Excepion {ex}");
                        tcs.SetResult(null);
                    }
                    else
                    {
                        tcs.SetException(ex);
                    }
                }
            }, null);
            return tcs.Task;
        }

        public static Task RunOnMain(Action task)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            UI.Post((_) =>
            {
                try
                {
                    task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);
            return tcs.Task;
        }

        /// <summary>
        /// アプリ起動時に呼ばれるコールバックです
        /// </summary>
        /// <returns><c>true</c>, if launching was finisheded, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="launchOptions">Launch options.</param>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            {
                // Synchronization Context 設定
                UI = SynchronizationContext.Current;
                var sender = CommunicationClient.Instance.ConnectionStateEventSender;
                sender.TheSynchronizationContext = UI;

                ConnectionStateObserver = new ConnectionStateObserver(sender);

                // Listener 作成
                CommunicationClientController = new CommunicationClientController();
                ConnectionStateObserver.AddListener(CommunicationClientController);

                CommunicationClient.Instance.StartObserveLineState();

                EmbossmentToolKit.InitGlobalIfNeeded();
            }
            IQKeyboardManager.SharedManager.Enable = true;

            // スリープ無効化
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            try
            {
                NSNotification n = NSNotification.FromName(@"OnResignActivation", this);
                NSNotificationCenter.DefaultCenter.PostNotification(n);
            } catch(Exception ex) {
                Console.Error.WriteLine("Error on OnResignActivation");
                Console.Error.WriteLine(ex);
            }
            CommunicationClient.Instance.StopObserveLineState();
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void OnActivated(UIApplication application)
        {
            try
            {
                NSNotification n = NSNotification.FromName(@"OnActivated", this);
                NSNotificationCenter.DefaultCenter.PostNotification(n);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error on OnActivated");
                Console.Error.WriteLine(ex);
            }
            CommunicationClient.Instance.StartObserveLineState();
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
            ConnectionStateObserver.Purge();
            ConnectionStateObserver.Dispose();
            CommunicationClient.Instance.StopObserveLineState();
        }
    }
}

