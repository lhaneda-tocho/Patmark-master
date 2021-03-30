using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TokyoChokoku.Communication;

namespace nunit.TokyoChokoku.Communication
{
    [TestFixture()]
    public class CommunicationClientLifecycleTest
    {



        // ConnectionState, ExclusionError 2種のどちらか. 状態遷移の流れを表現するために利用します.
        // IsRightがtrueの時は Right の値が,
        // IsLeftがtrueの時は Left の値が入っていることを表す.
        // IsRightの時に Left, IsLeftの時にRight を取得してはいけません. (未定義の動作)
        class Moment
        {
            bool IsRight { get; }
            bool IsLeft => !IsRight;
            ConnectionState Right;
            ExclusionError Left;

            public static Moment Create(ConnectionState right)
            {
                return new Moment(right, ExclusionError.BrokenPipe, true);
            }

            public static Moment Create(ExclusionError left)
            {
                return new Moment(ConnectionState.NotConnected, left, false);
            }

            private Moment(ConnectionState right, ExclusionError left, bool isRight)
            {
                Right = right;
                Left = left;
                IsRight = isRight;
            }

            public bool ContentEquals(Moment b)
            {
                if (IsRight && b.IsLeft)
                    return false;
                if (IsRight)
                    return Right == b.Right;
                else
                    return Left == b.Left;
            }
        }

        class EventSequencer : ConnectionStateListener
        {
            ConcurrentQueue<Moment> Sequence = new ConcurrentQueue<Moment>();

            public List<Moment> Poll()
            {
                var c = Sequence.Count;
                var list = new List<Moment>(c);
                Moment n;
                for (int i = 0; i < c; i++)
                {
                    if (Sequence.TryDequeue(out n))
                        list.Add(n);
                    else
                        break;
                }
                return list;
            }

            public void DidSumbit(ConnectionState current)
            {
                Sequence.Enqueue(Moment.Create(current));
            }

            public void OnFailOpening(ExclusionError error)
            {
                Sequence.Enqueue(Moment.Create(error));
            }

            public void OnFailReleasing(ExclusionError error)
            {
                Sequence.Enqueue(Moment.Create(error));
            }

            public void OnStateChanged(ConnectionState next, ConnectionState prev)
            {
                Sequence.Enqueue(Moment.Create(next));
            }
        }


        private class MySynchronizationContext : SynchronizationContext
        {
            readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
            readonly Timer timer;

            public MySynchronizationContext()
            {
                timer = new Timer((obj) => Dispatch());
            }

            public void SetEnable(bool enable)
            {
                if (enable)
                    timer.Change(0, 16);
                else
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            void Dispatch()
            {
                foreach (var m in PollingMessages())
                {
                    m();
                }
            }

            List<Action> PollingMessages()
            {
                var c = queue.Count;
                var messages = new List<Action>(c);
                for (int i = 0; i < c; i++)
                {
                    Action message;
                    if (queue.TryDequeue(out message))
                    {
                        messages.Add(message);
                    }
                    else
                        break;
                }
                return messages;
            }

            Task Accept(Action func)
            {
                var task = new TaskCompletionSource<object>();
                queue.Enqueue(() =>
                {
                    try
                    {
                        func();
                        task.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        task.SetException(e);
                    }
                });
                return task.Task;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                Accept(() => d(state));
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                Accept(() => d(state)).Wait();
            }

            public void RunBlocking(Func<Task> d)
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                Send(async (arg) => {
                    try
                    {
                        await d();
                        tcs.SetResult(null);
                    } catch (Exception ex) {
                        tcs.SetException(ex);
                    }
                },  null);
                var task = tcs.Task;
                if (task.IsFaulted)
                    throw task.Exception;
            }

            public T RunBlocking<T>(Func<Task<T>> d)
            {
                TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
                Send(async (arg) => {
                    try
                    {
                        var v = await d();
                        tcs.SetResult(v);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }, null);
                var task = tcs.Task;
                if (task.IsFaulted)
                    throw task.Exception;
                else
                    return task.Result;
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }

        MySynchronizationContext Ctx = new MySynchronizationContext();

        MockedLineObserver         LineObserver;
        MockedPipeStateManager     PipeStateManager;
        ConnectionStateEventSender EventSender;
        ConnectionStateObserver    Observer;

        public CommunicationClientLifecycleTest() {
        }

        [SetUp]
        public void BeforeTest()
        {
            LineObserver     = new MockedLineObserver();
            PipeStateManager = new MockedPipeStateManager(LineObserver);
            EventSender      = PipeStateManager.TheConnectionStateEventSender;
            Observer         = new ConnectionStateObserver(EventSender);
            // SynchronizationContextの設定 (単一スレッド上で動作しているように振る舞う(実際の処理はスレッドプールで行う))
            SynchronizationContext.SetSynchronizationContext(Ctx);
            Ctx.SetEnable(true);
        }

        [TearDown]
        public void AfterTest()
        {
            SynchronizationContext.SetSynchronizationContext(null);
            Ctx.SetEnable(false);
        }


        /// <summary>
        /// Online, Offlineの通知でLineObserverの状態が適切に変化していることを確認します.
        /// </summary>
        [Test()]
        public void SwitchOnlineOffline()
        {
            // 初期状態は offline
            Assert.True(LineObserver.IsOffline());
            // online通知
            LineObserver.NotifyOnline();
            // 確認
            Assert.True(LineObserver.IsOnline());
            // 状態遷移を待つ.
            LineObserver.WaitFree();
            // offline通知
            LineObserver.NotifyOffline();
            // 確認
            Assert.True(LineObserver.IsOffline());
        }



        /// <summary>
        /// Wifiの有効化の通知で, LineObserver, PipeStateManagerが適切な状態に遷移しているか確認します.
        /// </summary>
        [Test()]
        public void NotifyWifiEnabled()
        {
            // 初期状態は offline
            Assert.True(LineObserver.IsOffline());
            // online通知
            LineObserver.NotifyOnline();
            // 確認
            Assert.True(LineObserver.IsOnline());
            LineObserver.WaitFree();
            Assert.True(PipeStateManager.Enable);
            Assert.True(PipeStateManager.CurrentState == PipeState.Release);
        }



        /// <summary>
        /// Wifiの有効化の通知で, LineObserver, PipeStateManagerが適切な状態に遷移しているか確認します.
        /// </summary>
        [Test()]
        public void NotifyWifiEnabledAndExcluding()
        {
            // 初期状態は offline
            Assert.True(LineObserver.IsOffline());
            // online通知
            LineObserver.NotifyOnline();
            // 状態遷移を待機し, 排他開始する.
            LineObserver.WaitFree();
            var success = Ctx.RunBlocking(async ()=>{
                return await PipeStateManager.OnStartCommunication();
            });
            if (success)
                Assert.True(PipeStateManager.CurrentState == PipeState.Open);
            else
                Assert.Fail();
        }
        /// <summary>
        /// Listens the routine
        /// </summary>
        [Test()]
        public void ListenRoutine_Offline_Online_Connect_Disconnect_Offline()
        {
            var seq = new EventSequencer();
            // 初期状態は offline
            Assert.True(LineObserver.IsOffline());
            // リスナの設定
            Observer.AddListener(seq);
            // online and wait
            LineObserver.NotifyOnline();
            LineObserver.WaitFree();
            // excluding
            Ctx.RunBlocking(async () => {
                var success = await PipeStateManager.OnStartCommunication();
                if (!success)
                    Assert.Fail();
            });
            // unexcluding
            Ctx.RunBlocking(async () => {
                var success = await PipeStateManager.OnTerminateCommunication();
                if (!success)
                    Assert.Fail();
            });
            // offline and wait
            LineObserver.NotifyOffline();
            LineObserver.WaitFree();
            // 状態遷移の確認
            var result = seq.Poll();
            var expected = (new List<Moment>{
                Moment.Create(ConnectionState.NotConnected),
                Moment.Create(ConnectionState.NotExcluding),
                Moment.Create(ConnectionState.Ready),
                Moment.Create(ConnectionState.NotExcluding),
                Moment.Create(ConnectionState.NotConnected),
            });
            Assert.True(result.Count == expected.Count);
            result.Zip(expected, (r, e)=>{
                Assert.True(r.ContentEquals(e));
                return (object)null;
            });
        }
    }
}
