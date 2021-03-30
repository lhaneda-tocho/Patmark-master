using System;

using Foundation;
using UIKit;
using ObjCRuntime;
using CoreGraphics;
using CoreFoundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class GestureFactory
    {
        static GestureFactory ()
        {
        }

        public static UILongPressGestureRecognizer CreateStandardRepeater (Action action) {
            var recognizer = CreateLongPressRepeater (interval: 0.2, action: action);
            recognizer.MinimumPressDuration = 1.0;
            return recognizer;
        }


        public static UILongPressGestureRecognizer CreateLongPressRepeater(double interval, Action action) {

            bool repeat = false;
            Action<NSTimer> repeatAction = (timer) => { 
                if (repeat) {
                    action();
                } else {
                    timer.Invalidate ();
                    timer.Dispose ();
                }
            };

            return new UILongPressGestureRecognizer( (sender) => {
                switch (sender.State)
                {
                case UIGestureRecognizerState.Began:
                    {
                        repeat = true;
                        var myTimer = NSTimer.CreateRepeatingTimer (interval, repeatAction);
                        NSRunLoop.Current.AddTimer(myTimer, NSRunLoopMode.Default);
                        break;
                    }

                case UIGestureRecognizerState.Changed:
                    break;

                default:
                    repeat = false;
                    break;

                }
            });
        }
    }
}

