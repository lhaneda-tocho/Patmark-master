using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.RenderKit.Context;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AG = Android.Graphics;
using TokyoChokoku.Patmark.Droid.Custom;

namespace TokyoChokoku.Patmark.Droid.RenderKitForDroid
{
    public class ContextStack
    {
        readonly Stack<PaintContext> TheStack = new Stack<PaintContext>();
        public PaintContext Current { get => TheStack.Peek(); }

        public ContextStack(AG.Paint initState)
        {
            TheStack.Push(new PaintContext(initState));
        }

        public void PushState()
        {
            var c = Current;
            TheStack.Push(c.Copy());
        }

        public void PopState()
        {
            TheStack.Pop();
        }
    }
}
