using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace TokyoChokoku.iOS.Custom
{
    public static class StatusBarSizeObservation
    {
        static Observation RegisterNSNotificationAction(Action<NSNotification> listen)
        {
			var c = NSNotificationCenter.DefaultCenter;
            var handler = c.AddObserver(UIApplication.DidChangeStatusBarFrameNotification, (notification) => listen(notification));
            return new Observation(c, handler);
        }

        public static Observation Register(Action listen)
        {
            return RegisterNSNotificationAction((notification) =>
            {
				//var info = (NSValue) notification.UserInfo[UIApplication.StatusBarFrameUserInfoKey];
				//var rect = info.CGRectValue;
                listen();
            });
        }
    }

    public class Observation: IDisposable
    {
        NSNotificationCenter center;
        NSObject handler;

        public Observation(NSNotificationCenter center, NSObject handler)
        {
            this.center = center;
            this.handler = handler;
        }

        public void Dispose()
        {
            Unregister();
            handler.Dispose();
        }

        public void Unregister()
        {
            if (handler != null)
            {
                center.RemoveObserver(handler);
                handler = null;
                center = null;
            }
        }


    }
}
