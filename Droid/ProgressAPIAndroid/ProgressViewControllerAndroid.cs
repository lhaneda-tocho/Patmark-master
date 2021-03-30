using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.Patmark.Droid.Custom;
using Unicast.ProgressKit;

namespace TokyoChokoku.Patmark.Droid
{
    public class ProgressViewControllerAndroid : ProgressViewController
    {
        bool                Latched { get; set; } = true;
        Action              LatchedAction { get; set; } = null;
        LoadingOverlayView  TheOverlayFrame { get; }

        ProgressEventListenerToken ListenerToken { get; set; }


        public ProgressViewControllerAndroid(LoadingOverlayView overlay)
        {
            TheOverlayFrame = overlay ?? throw new ArgumentNullException(nameof(overlay));
        }

        // ========

        TaskCompletionSource<object> Tcs { get; set; } = null;

        void StartShowing()
        {
            var tcs = Tcs;
            if (ReferenceEquals(tcs, null))
            {
                tcs = new TaskCompletionSource<object>();
                Tcs = tcs;
                _ = TheOverlayFrame.ShowWhileProcessing(() => tcs.Task);
            }
        }

        void EndShowing()
        {
            var tcs = Tcs;
            if (ReferenceEquals(tcs, null))
            {
                return;
            }
            Tcs = null;
            tcs.SetResult(null);
        }

        // ========

        public void OnCreate()
        {
            Log.Debug("ProgressViewControllerAndroid.OnCreate()");
            ListenerToken = ProgressAPIAndroid.Register(this);
        }

        public void OnResume()
        {
            Log.Debug("ProgressViewControllerAndroid.OnResume()");
            Latched = false;
            LatchedAction?.Invoke();
            LatchedAction = null;
        }

        public void OnPause()
        {
            Log.Debug("ProgressViewControllerAndroid.OnPause()");
            Latched = true;
        }

        public void OnDestroy()
        {
            Log.Debug("ProgressViewControllerAndroid.OnDestroy()");
            ListenerToken.Dispose();
            ListenerToken = null;
        }

        // ========

        void StartOrLatchAction(Action action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            if (Latched)
                LatchedAction = action;
            else
                action.Invoke();
        }

        // ========

        public void OnBusy(IList<IProgressTask> tasks)
        {
            Log.Debug("OnBusy()");
            StartOrLatchAction(StartShowing);
        }

        public void OnIdle()
        {
            Log.Debug("OnIdle()");
            StartOrLatchAction(EndShowing);
        }

        public void DidSubmitTask(IProgressTask task)
        {
            Log.Debug("DidSubmitTask()");
            StartOrLatchAction(StartShowing);
        }

        public void WillRemoveTask(IProgressTask task)
        {
            Log.Debug("WillRemoveTask()");
            // Ignore
        }

        public void ViewControllerWillUnregisterd()
        {
            Log.Debug("ViewControllerWillUnregisterd()");
            Latched = true;
            LatchedAction = null;
        }
    }
}
