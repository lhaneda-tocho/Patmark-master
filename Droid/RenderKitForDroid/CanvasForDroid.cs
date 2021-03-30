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
using AG=Android.Graphics;
using AT=Android.Text;
using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.FieldModel;

namespace TokyoChokoku.Patmark.Droid.RenderKitForDroid
{
    public class CanvasForDroid : Canvas
    {
        readonly ContextStack TheStack;
		readonly AG.Canvas    TheCanvas;
        PaintContext Ctx { get => TheStack.Current; }

        public CanvasForDroid(AG.Paint initState, AG.Canvas canvas) {
            TheStack  = new ContextStack(initState);
            TheCanvas = canvas ?? throw new NullReferenceException("argument of canvas is not allowed null.");

            InitPaint();
		}

        public void InitPaint() 
        {
            var paint = Ctx.Paint;

            // setElegantTextHeight は API Level 21 から追加されました。
            // → https://developer.android.com/reference/android/widget/TextView.html#setElegantTextHeight(boolean)
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                paint.ElegantTextHeight = false;
            }
            paint.TextScaleX = 1;
            paint.TextAlign = AG.Paint.Align.Left;
            paint.AntiAlias = true;
        }

		public override void PopStateNative()
		{
			TheStack.PopState();
            TheCanvas.Restore();
		}

		public override void PushStateNative()
		{
			TheStack.PushState();
			TheCanvas.Save();
		}

		public override void SetFillColor(CommonColor color)
		{
            Ctx.FillColor = color.ToNative();
		}

		public override void SetStrokeColor(CommonColor color)
		{
            Ctx.StrokeColor = color.ToNative();
		}

        public void ShowMultiLineCenter(Frame2D frame, params string[] lines) {
            var p = new AT.TextPaint(Ctx.Paint);
            p.TextAlign = AG.Paint.Align.Center;
            // グリフ形状情報を取得
            var fm = p.GetFontMetrics();
            // 文字最大高さ計算
            float fh = fm.Descent - fm.Ascent;
            // 描画サイズの計算
            var mlSize = MultiLineBounds(p, lines);
            // 描画内容がない場合は確実に 0 となる事を利用して条件分岐
            // さらに float 型は 0を正確に表現できるので この警告は無視できる.
            // これを取らないと，後の処理で Infinity や NaN が発生する原因となる.
            if(mlSize.W == 0d || mlSize.H == 0d) {
                // 無視
                return;
            }
            // 修正描画倍率の計算
            // 表示したい大きさに合わせてサイズ修正するため
            var rate = Size2D.Init(
                frame.W / mlSize.W, frame.H / mlSize.H
            );
            // 
            PushState();
            // 原点合わせ
            Translate(frame.Pos);
            // サイズ修正
            Scale(rate);
            TheCanvas.Concat(Transform.ToNative());
            // 描画開始
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var tx   = (float) (mlSize.W / 2);
                var ty   = fh*(i+1) - fm.Descent;
                // ベースラインまで移動し，描画開始
                TheCanvas.DrawText(line, tx, ty, p);
            }
            PopState();
        }

        static Size2D MultiLineBounds(AT.TextPaint p, params string[] lines) {
            if(lines.Length == 0) {
                return Size2D.Init(0d, 0d);
            }
            // グリフ形状情報を取得
            var fm = p.GetFontMetrics();
            // 文字最大高さ計算
            float fh = fm.Descent - fm.Ascent;
            // 高さ計算
            float mlH = fh * lines.Length;
            // 文字最大幅計算
            float mlW = lines.Max((str) =>
            {
                return p.MeasureText(str, 0, str.Length);
            });
            return Size2D.Init(mlW, mlH);
        }


        /// <summary>
        /// ロゴの表示メソッド
        /// </summary>
        public void ShowLogoHere(int id, Size2D size)
        {
            // ==================================
            // 先に矩形を表示する.
            PushStateNative();
            Ctx.StrokeWidth = 1;
            var path = Frame2D.Create(Pos2D.Zero(), size).ToStrip();
            StrokeLines(path);
            PopStateNative();

            // ==================================
            // 次に LOGO と ID を表示する．
            const double marginRatio = 0.1;
            // 描画内容の生成
            const string title = "LOGO";
            string idstr = id.ToString();
            // 描画範囲の決定(0~1の範囲で計算)
            var drawingArea = Frame2D.Create(
                size.W * marginRatio / 2,
                size.H * marginRatio / 2,
                size.W *(1 - marginRatio),
                size.H *(1 - marginRatio)
            );

            PushStateNative();
            Ctx.Paint.Color = AG.Color.Black;
            ShowMultiLineCenter(drawingArea, title, idstr);
            PopStateNative();
        }

        // Deprecated
		public override void ShowStringAt(Pos2D pos, string text, Size2D size)
        {
            Use(() =>
			{
                var h  = (float) size.H;
                var aw = (float) size.AspectW;
                var rect = new AG.Rect();

                Ctx.Paint.TextSize = h;
                Ctx.Paint.TextScaleX = aw;
                Ctx.Paint.GetTextBounds(text, 0, text.Length, rect);
                var tw = rect.Width();

                // 変換行列設定
                Translate((float)pos.X, (float)(pos.Y + h));
				TheCanvas.Concat(Transform.ToNative());

                // デバッグ: 枠出力
                //Ctx.StrokeMode();
                //TheCanvas.DrawRect(new AG.RectF((float)pos.X, (float)pos.Y, tw, h), Ctx.Paint);


				// 描画
				Ctx.FillMode();
                TheCanvas.DrawText(text, 0, text.Length, 0, 0, Ctx.Paint);
				Ctx.StrokeMode();
				TheCanvas.DrawText(text, 0, text.Length, 0, 0, Ctx.Paint);
            });
			//throw new NotImplementedException();
		}

		public override void ShowStringMonoSpaceAt(Pos2D pos, string text, Size2D capsize, double stride)
		{
			UseNative(() =>
			{
                // テキストの高さ
                var h = capsize.H;
                // 要求されるアスペクト比 
                var requiredAspectW = capsize.AspectW;


				// 変換行列設定
				TheCanvas.Concat(Transform.ToNative());
                // pos まで移動
                TheCanvas.Translate((float)pos.X, (float)(pos.Y + h));

				// サイズの設定
				var paint = Ctx.Paint;
                SetCapSizeHeight(capsize.H, Ctx.Paint);

				for (int i = 0; i < text.Length; i++)
				{
					var w = paint.MeasureText(text, i, i + 1);
					var aspectW = w / h;
                    var wfactor = requiredAspectW / aspectW;

                    Console.WriteLine(wfactor);

					// 行列情報の保存
					TheCanvas.Save(); 
                    //
                    // 描画位置まで移動
                    TheCanvas.Translate((float) (i * stride), 0);
					// アスペクト比を合わせる
					TheCanvas.Scale((float)wfactor, 1);
					// 描画
					Ctx.FillMode();
					TheCanvas.DrawText(text, i, i + 1, 0, 0, paint);
					Ctx.StrokeMode();
					TheCanvas.DrawText(text, i, i + 1, 0, 0, paint);
                    //TheCanvas.DrawRect(new AG.RectF(0, -(float)h, (float)(capsize.W / wfactor), 0), paint);
                    //
					// 行列情報のリストア
					TheCanvas.Restore();
				}

			});
		}

        public override void FillLines(IStrip path)
        {
			var npath = (Transform * path).ToNativePath();
			UseNative(() =>
			{
                Ctx.FillMode();
				TheCanvas.DrawPath(npath, Ctx.Paint);
			});
        }

        public override void StrokeLine(Line line)
		{
			var td = Transform * line;
            UseNative(()=>
            {
                Ctx.StrokeMode();
				var s = td.Start;
				var e = td.End;
				TheCanvas.DrawLine((float)s.X, (float)s.Y, (float)e.X, (float)e.Y, Ctx.Paint);
            });

        }

        public override void StrokeLines(IStrip path)
		{
            var npath = (Transform * path).ToNativePath();
			UseNative(() =>
			{
				Ctx.StrokeMode();
                TheCanvas.DrawPath(npath, Ctx.Paint);
			});
        }

        void SetCapSizeHeight(double capSize, AG.Paint paint)
        {
            const string ji = "W";
            paint.TextSize = (float)capSize;

            var rect = new AG.Rect();
            paint.GetTextBounds(ji, 0, 1, rect);

            var h = rect.Height();
            var ans = (float)(capSize * (capSize / h));
            Console.Error.WriteLine(String.Format("TextSize={0}", ans));
            paint.TextSize = ans;
        }

        float CapSizeToPix(double capSize, AG.Paint paint)
        {
            const string ji = "W";
            paint = new AG.Paint(paint);
            paint.TextSize = (float)capSize;

            var rect = new AG.Rect();
            paint.GetTextBounds(ji, 0, 1, rect);

            var h = rect.Height();
            var ans = (float)(capSize * (capSize / h));
            return ans;
        }

        void DrawFieldStringHere(string text, int offset, int length)
        {
            // 描画
            var path = new AG.Path();
            var tp = Ctx.CreateTextPaint();
            tp.AntiAlias = true;
            tp.GetTextPath(text, offset, length, 0, 0, path);
            path.Close();
            TheCanvas.DrawPath(path, tp);
        }

        /// <summary>
        /// 高さ方向のスケール変換成分をテキストサイズに設定します.
        /// </summary>
        void ScaleHToTextSize()
        {
            var scale = TheCanvas.Matrix.ToAffine2D().FactorLocalScale().Item2;
            var s = (float)scale.H;
            Ctx.Paint.TextSize = Ctx.Paint.TextSize * s;
            TheCanvas.Scale(1f / s, 1f / s);
        }

        // TODO：Logo用の描画処理を書いてください。
        /// <summary>
        /// Shows the field text mono space at pos, text, capsize and stride.
        /// </summary>
        /// <param name="pos">Position.</param>
        /// <param name="text">Text.</param>
        /// <param name="capsize">Capsize.</param>
        /// <param name="stride">Stride.</param>
        public override void ShowFieldTextMonoSpaceAt(Pos2D pos, FieldText text, Size2D capsize, double stride)
        {
            UseNative(() =>
            {
                // テキストの高さ
                var h = capsize.H;
                // 要求されるアスペクト比 
                var requiredAspectW = capsize.AspectW;

                // origin
                var origin           = Transform * pos.ToTransform();
                var textOriginNative = (Transform * (pos + Pos2D.InitY(h)).ToTransform()).ToNative();

                // サイズの設定
                //var paint = Ctx.Paint;
                SetCapSizeHeight(capsize.H, Ctx.Paint);

                Action<int> ReadyText = (index) =>
                {
                    // テキスト描画モード設定
                    Ctx.FillColor   = AG.Color.Black;
                    Ctx.StrokeColor = AG.Color.Black;

                    // テキストの位置に移動
                    TheCanvas.Concat(textOriginNative);
                    TheCanvas.Translate((float)(index * stride), 0);

                    // テキストの描画原点あわせ
                    //Ctx.TextMatrix = CGAffineTransform.MakeIdentity();
                };

                Action<int> ReadyLogo = (index) =>
                {
                    // 原点の書き換え
                    SetTransform(origin);

                    // 位置合わせ
                    Translate(index * stride, 0);
                };

                for (int i = 0; i < text.Count; i++)
                {
                    var elem = text[i];
                    PushState();
                    try
                    {
                        elem.Do(
                            isChar: (charelem) =>
                            {
                                var single = charelem.Char.ToString();
                                var w = Ctx.Paint.MeasureText(single);
                                var aspectW = w / h;
                                var r = (float)(capsize.AspectW / aspectW);
                                var wfactor = requiredAspectW / aspectW;


                                if (!Text.TextEncodingsExt.CompatByte2(charelem.Char))
                                {
                                    ReadyLogo(i);
                                    PushState();

                                    Ctx.FillColor = AG.Color.Red;
                                    Ctx.StrokeColor = AG.Color.Red;
                                    var path = Frame2D.Create(Pos2D.Zero(), capsize).ToStrip();
                                    StrokeLines(path);

                                    PopState();
                                }
                                //Console.WriteLine(wfactor);
                                ReadyText(i);
                                if (!Text.TextEncodingsExt.CompatByte2(charelem.Char))
                                {
                                    Ctx.FillColor = AG.Color.Red;
                                    Ctx.StrokeColor = AG.Color.Red;
                                }
                                // アスペクト比を合わせる
                                if (r < 1.0f)
                                    TheCanvas.Scale((float)wfactor, 1);
                                Ctx.StrokeMode();
                                // Debug: 枠描画
                                //TheCanvas.DrawRect(new AG.RectF(0, -(float)h, (float)(capsize.W / wfactor), 0), Ctx.Paint);

                                ScaleHToTextSize();
                                
                                // 描画
                                Ctx.FillMode();
                                DrawFieldStringHere(single, 0, 1);
                                Ctx.StrokeMode();
                                DrawFieldStringHere(single, 0, 1);
                            },
                            isLogo: (single) =>
                            {
                                // 変換行列設定
                                ReadyLogo(i);
                                ShowLogoHere(single.id, capsize);
                            }
                        );
                    }
                    finally
                    {
                        PopState();
                    }
                }

            });
        }
    }
}
