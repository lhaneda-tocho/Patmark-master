using System;
using System.Threading.Tasks;
using System.Threading;
using CoreGraphics;
using System.Drawing;
using Foundation;
using UIKit;
using TokyoChokoku.Patmark.iOS.Presenter.Utility;

using static UIKit.UIViewAutoresizing;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.iOS.Presenter.FieldPreview;
using TokyoChokoku.Patmark.iOS.Presenter.FieldEditor;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using System.Linq;



namespace TokyoChokoku.Patmark.iOS.Presenter.Loading
{
    public class LoadingOverlay : UIView
    {
        // control declarations
        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        public LoadingOverlay(CGRect frame) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            loadingLabel.Text = "Now in process.".Localize();
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(loadingLabel);

        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            UIView.Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// UIスレッドで呼び出してください.
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        /// <param name="terminator">true を指定すると、 <c>func</c> で発生した例外をキャッチしてログに出力します。</param>
        public static Task ShowWithTask(Func<CancellationToken, Task> block)
        {
            return ShowWithTask(false, block);
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// UIスレッドで呼び出してください.
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        /// <param name="terminator">true を指定すると、 <c>func</c> で発生した例外をキャッチしてログに出力します。</param>
        public static Task ShowWithTask(bool terminator, Func<CancellationToken, Task> block)
        {
            return ShowWithTask(
                onExcept: (ex) =>
                {
                    if(terminator)
                    {
                        Log.Error($"Unhandled Exception: {ex}");
                        return Task.FromResult<Nil>(null);
                    }
                    return Task.FromException<Nil>(ex);
                },
                block: async (it) =>
                {
                    await block(it);
                    return null;
                }
            );
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// UIスレッドで呼び出してください.
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        /// <param name="terminator">true を指定すると、 <c>func</c> で発生した例外をキャッチしてログに出力します。</param>
        public static Task<T> ShowWithTask<T>(Func<CancellationToken, Task<T>> block)
        {
            return ShowWithTask<T>(
                onExcept: (ex) =>
                {
                    return Task.FromException<T>(ex);
                },
                block: block
            );
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// UIスレッドで呼び出してください.
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        /// <param name="terminator">true を指定すると、 <c>func</c> で発生した例外をキャッチしてログに出力します。</param>
        public static async Task<T> ShowWithTask<T>(Func<Exception, Task<T>> onExcept, Func<CancellationToken, Task<T>> block)
        {
            var cancelation = new CancellationTokenSource();
            var app = UIApplication.SharedApplication;
            // バックグラウンドタスクを行うことを通知する
            var token = app.BeginBackgroundTask(() => {
                // システムから時間切れを言い渡された場合、キャンセル通知
                cancelation.Cancel();
            });


            async Task<T> RunAsync()
            {
                Exception cause = null;
                try
                {
                    // スレッドプール上で実行
                    return await Task.Run(async () =>
                    {
                        return await block(cancelation.Token);
                    });
                }
                catch (Exception ex)
                {
                    try
                    {
                        return await onExcept(ex);
                    }
                    catch (Exception ex2)
                    {
                        cause = ex2;
                        throw;
                    }
                }
            }


            try
            {
                var ctrl = ControllerUtils.FindTopViewController();
                return await ShowOn(ctrl, RunAsync, cancelation.Token);
            }
            finally
            {
                // オーバーレイを解除する。
                cancelation.Cancel();
                // バックグラウンドタスクの終了を通知する。
                app.EndBackgroundTask(token);
                token = UIApplication.BackgroundTaskInvalid;
            }
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        public static async Task ShowWithTask(Func<Task> block, bool terminator=false)
        {
            await ShowWithTask(terminator: true, block: async (CancellationToken token) =>
            {
                await block();
            });
        }



        static async Task<T> ShowOn<T>(UIViewController ctrl, Func<Task<T>> func, CancellationToken ct)
        {
            var bounds = ctrl?.View?.Bounds ?? UIScreen.MainScreen.Bounds;
            var loadingOverlay = new LoadingOverlay(bounds);

            // Overlay
            if (ctrl.View != null)
            {
                ctrl.View.Add(loadingOverlay);
            }

            // オーバーレイ表示のオブザーバ. トップのコントローラが変わったら、そこにもオーバーレイ表示を行う。 
            async Task OverlayObserver()
            {
                try
                {
                    // 新しいコントローラが来るまでスピン待機 
                    var newCtrl = ControllerUtils.FindTopViewController();
                    while (ReferenceEquals(newCtrl, ctrl))
                    {
                        await Task.Delay(100, ct);
                        newCtrl = ControllerUtils.FindTopViewController();
                    }
                    // 新しいコントローラが来た場合はそのコントローラ上に表示を行う
                    await ShowOn<object>(
                        newCtrl,
                        async () => {
                            while (true)
                            {
                                await Task.Delay(10000, ct);
                            }
                        },
                        ct
                    );
                }
                catch (TaskCanceledException)
                {
                    // 表示が終わったらここに来る 
                    Log.Debug("Overlay Observer Cancelled.");
                }
                catch (Exception ex)
                {
                    Log.Error($"Unhandled Exception {ex}");
                }
            }
            try
            {
                // Observe
                _ = OverlayObserver();
                // Invoke
                return await func.Invoke();
            }
            finally
            {
                // Dismiss
                loadingOverlay.Hide();
            }
        }
    }
}

