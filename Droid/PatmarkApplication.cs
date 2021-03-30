using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

using Android.Runtime;
using Android.App;
using Android.OS;
using Android.Views;


using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.Droid.Util;


namespace TokyoChokoku.Patmark.Droid
{
    [Application]
    public class PatmarkApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static FragmentAccessHandler         FragmentAccessHandler = new FragmentAccessHandler();
        public static BackgroundDetector            BackgroundDetector    = new BackgroundDetector();
        public static ConnectionStateObserver       ConnectionStateObserver { get; private set; }
        public static CommunicationClientController CommunicationClientController { get; private set; }

        static Stack<Activity> TheActivityStack = new Stack<Activity>();
        //public CommunicationNotifier TheCommunicationNotifier { get; private set; }

        public static Activity CurrentActivity => TheActivityStack.Peek();


        // このコンストラクタを明示的に override する必要がある
        public PatmarkApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
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
                return ProgressAPIAndroid.LaunchComputeTaskAsync(async (_) => {
                    await block();
                });
                //LoadingOverlay.ShowWithTask(terminator, it => block());
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
                return ProgressAPIAndroid.LaunchComputeTaskAsync(async (_) => {
                    return await block();
                });
                //return LoadingOverlay.ShowWithTask<T>(it => block());
            }
            else
            {
                return onOffline();
            }
        }


        // MainThread上でAsyncTaskを実行します.
        public static Task RunOnMain(bool terminator, Func<Task> task)
        {
            var tcs = new TaskCompletionSource<object>();
            Application.SynchronizationContext.Post(async (_) =>
            {
                try
                {
                    await task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    if(terminator)
                        tcs.SetResult(null);
                    else
                        tcs.SetException(ex);
                }
            }, null);
            return tcs.Task;
        }

        public static Task RunOnMain(Func<Task> task)
        {
            return RunOnMain(false, task);
        }


        public static Task RunOnMain(Action task)
        {
            var tcs = new TaskCompletionSource<object>();
            Application.SynchronizationContext.Post((_) =>
            {
                try
                {
                    task();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    tcs.SetException(ex);
                }
            }, null);
            return tcs.Task;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Injecter.InjectIfNeeded(Context);
            RegisterActivityLifecycleCallbacks(this);

            ProgressAPIAndroid.InitOnCurrentThread();

            //// 監視
            {
                var sender = CommunicationClient.Instance.ConnectionStateEventSender;
                sender.TheSynchronizationContext = Application.SynchronizationContext;
                //
                ConnectionStateObserver = new ConnectionStateObserver(sender);
                CommunicationClientController = new CommunicationClientController();
                ConnectionStateObserver.AddListener(CommunicationClientController);

                CommunicationClient.Instance.StartObserveLineState();
            }

            BackgroundDetector.OnBackground += () => {
                CommunicationClient.Instance.StopObserveLineState();
            };

            BackgroundDetector.OnForeground += () =>
            {
                CommunicationClient.Instance.StartObserveLineState();
            };

            EmbossmentKit.EmbossmentToolKit.InitGlobalIfNeeded();
        }


        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            TheActivityStack.Push(activity);
            // スリープ無効
            activity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            TheActivityStack.Push(activity);
            Console.WriteLine("Stack Pushed.");
            BackgroundDetector.Foreground();
            FragmentAccessHandler.OnResume();
        }

        public void OnActivityPaused(Activity activity)
        {
            FragmentAccessHandler.OnPause();
            BackgroundDetector.StartMeasure();
            TheActivityStack.Pop();
            Console.WriteLine("Stack Poped.");
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }


        bool firstStarted = true;
        public void OnActivityStarted(Activity activity)
        {
            firstStarted = false;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }


    public class BackgroundDetector : IDisposable
    {
        const long MaxActivityTransitionTimeMS = 2500;

        public enum State
        {
            Foreground, Measuring, Background
        }

        volatile int identifier = 0;
        volatile int state = (int)State.Foreground;
        System.Timers.Timer timer;


        public event Action OnForeground = null; 
        public event Action OnBackground = null;

        public int Increment()
        {
            return Interlocked.Increment(ref this.identifier);
        }

        public BackgroundDetector()
        {
            timer = new System.Timers.Timer(MaxActivityTransitionTimeMS);
            timer.AutoReset = false;
        }


        /// <summary>
        /// Schedule this instance.
        /// </summary>
        void Schedule() {
            var myID = Increment();
            timer.Elapsed += (sender, e) => {
                PatmarkApplication.RunOnMain(() => {
                    if (identifier == myID)
                    {
                        Background();
                    }
                });
            };
        }


        /// <summary>
        /// バックグラウンドであるか計測を始める
        /// </summary>
        /// <returns>計測を開始できた場合 <c>true</c>.  それ以外は<c>false</c>. (Foregroundメソッドを呼び出し忘れていると起こる場合があります.)</returns>
        public bool StartMeasure()
        {
            if (!Walk(State.Measuring, State.Foreground))
                return false;
            Schedule();
            timer.Start();
            return true;
        }


        public void Foreground()
        {
            timer.Stop();
            State before = Walk(State.Foreground);
            if(before == State.Background) {
                // Foregroundのコールバック
                OnForeground?.Invoke();
            }
        }


        void Background()
        {
            bool success = Walk(State.Background, State.Measuring);
            if (success) {
                // Backgroudのコールバック
                OnBackground?.Invoke();
            }
            timer.Stop();
        }


        State GetAndWalk(State to, State from)
        {
            return (State)Interlocked.CompareExchange(ref state, (int)to, (int)from);
        }


        bool Walk(State to, State from)
        {
            return from == GetAndWalk(to, from);
        }


        State Walk(State to)
        {
            return (State)Interlocked.Exchange(ref state, (int)to);
        }


        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }

}


