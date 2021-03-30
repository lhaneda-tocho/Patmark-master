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
    public class LoadingMaskView : UIView
    {
        // control declarations
        UIActivityIndicatorView activitySpinner;
        //UILabel loadingLabel;

        public LoadingMaskView(CGRect frame) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.5f;
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
                centerY - (activitySpinner.Frame.Height / 2),
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            //// create and configure the "Loading Data" label
            //loadingLabel = new UILabel(new CGRect(
            //    centerX - (labelWidth / 2),
            //    centerY + 20,
            //    labelWidth,
            //    labelHeight
            //    ));
            //loadingLabel.BackgroundColor = UIColor.Clear;
            //loadingLabel.TextColor = UIColor.White;
            //loadingLabel.Text = "Now in process.".Localize();
            //loadingLabel.TextAlignment = UITextAlignment.Center;
            //loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            //AddSubview(loadingLabel);

        }


        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Close()
        {
            UIView.Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
            //RemoveFromSuperview();
        }


        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// UIスレッドで呼び出してください.
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        public static async Task ShowWithTask(UIView target, Func<CancellationToken, Task> func)
        {
            if (target == null)
                throw new NullReferenceException("Not allowed null here.");

            var cancelation = new CancellationTokenSource();
            var app = UIApplication.SharedApplication;
            // バックグラウンドタスクを行うことを通知する
            var token = app.BeginBackgroundTask(() => {
                // システムから時間切れを言い渡された場合、キャンセル通知
                cancelation.Cancel();
            });

            var loadingMask = new LoadingMaskView(target.Bounds);
            target.AddSubview(loadingMask);

            try
            {
                // スレッドプール上で実行
                await Task.Run(async () =>
                {
                    await func(cancelation.Token);
                });
            }
            finally
            {
                try
                {
                    loadingMask.Close();
                }
                finally
                {
                    app.EndBackgroundTask(token);
                    token = UIApplication.BackgroundTaskInvalid;
                }
            }
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        public static async Task ShowWithTask(UIView target, Func<Task> func)
        {
            await ShowWithTask(target, async (CancellationToken token) =>
            {
                await func();
            });
        }
    }


}
