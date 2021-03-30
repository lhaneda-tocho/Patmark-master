using System;
using System.Linq;
using Foundation;
using TokyoChokoku.FieldModel;
using TokyoChokoku.Patmark.UnitKit;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.RenderKit.Context;
using CoreGraphics;
using UIKit;
using CoreText;
using ObjCRuntime;
namespace TokyoChokoku.Patmark.iOS.RenderKitForIOS
{
    public class CanvasForiOS : Canvas
    {
        const bool EnableAspectScaleUp = false;

        public static readonly NSString CTLanguageAttribute;
        public static readonly CTFont   DefaultFont = new CTFont("Helvetica", 16);

        public CTFont CurrentFont { get; private set; } = DefaultFont;

        static CanvasForiOS()
		{
			var handle = Dlfcn.dlopen(Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
            try {
                CTLanguageAttribute = Dlfcn.GetStringConstant(handle, "kCTLanguageAttributeName");
			}
			finally
			{
				Dlfcn.dlclose(handle);
			}
        }

        #region context
        CGContext Ctx { get; }

        public CanvasForiOS(CGContext context)
        {
            Ctx = context;
		}

		public override void PushStateNative()
		{
			Ctx.SaveState();
		}

		public override void PopStateNative()
		{
			Ctx.RestoreState();
		}
        #endregion


        /// <summary>
        /// ロゴの表示メソッド
        /// </summary>
        public void ShowLogoHere(int id, Size2D size) {
            // ==================================
            // 先に矩形を表示する.
            Ctx.SaveState();
            Ctx.SetLineWidth(1);
            var path = Frame2D.Create(Pos2D.Zero(), size).ToStrip();
            StrokeLines(path);
            Ctx.RestoreState();

            // ==================================
            // 次に LOGO と ID を表示する．
            const double marginRatio = 0.1;

            var style = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
            style.Alignment = UITextAlignment.Center;


            NSAttributedString message = new NSAttributedString(
                "LOGO\n" + id,
                new UIStringAttributes
                {
                    Font = UIFont.SystemFontOfSize(1),
                    ParagraphStyle = style
                }
            );


            var stringSize = message.Size;

            var resolution = new CGSize(
                stringSize.Width * (1.0 + marginRatio),
                stringSize.Height * (1.0 + marginRatio));


            var drawingArea = new CGRect(
                stringSize.Width * (marginRatio / 2),
                stringSize.Height * (marginRatio / 2),
                stringSize.Width,
                stringSize.Height);


            Ctx.SaveState();

            Ctx.ConcatCTM(Transform.ToNative());
            Ctx.ConcatCTM(size.ToTransform().ToNative());
            Ctx.ScaleCTM(1 / resolution.Width, 1 / resolution.Height);

            message.DrawString(drawingArea);

            Ctx.RestoreState();
        }


		public override void ShowStringAt(Pos2D pos, string text, Size2D size)
		{
            UseNative(() =>
            {
                {
                    // テキストの位置に移動
                    var p = Transform * pos;
                    Affine2D SkRot;
                    Size2D s;
                    {
                        // 行列分解
                        var pSRS = Transform.FactorGlobalPos();
                        var SRS = pSRS.Item2;
                        var SRs = SRS.FactorLocalScale();
                        SkRot = SRs.Item1;
						s = SRs.Item2;
                    }
                    // 変換行列設定
                    Ctx.TranslateCTM((nfloat)p.X, (nfloat)p.Y);
                    Ctx.ConcatCTM(Transform.DeleteTranslation().ToNative());

					//UseNative(() =>
					//{
					//	Ctx.AddPath(Frame2D.Create(Pos2D.Zero(), size).ToStrip().ToNativePath());
					//	Ctx.StrokePath();
					//});

                    Ctx.TranslateCTM(0, (nfloat) size.H);
                    Ctx.TextMatrix = None.Run(() =>
                    {
                        Affine2D t;
						t = Affine2D.Scale((nfloat)size.AspectW, -1);
                        return t.ToNative();
                    });
                }
                // テキスト描画モード設定
                Ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);
                Ctx.SetFillColor(UIColor.Black.CGColor);
                Ctx.SetStrokeColor(UIColor.Black.CGColor);

                // 描画用のデータ取得
                var font = FontFromSize(size.H);
                var a = AttribFromFont(font);

				var at = new NSAttributedString(text, a);
				// 描画
				using (var textLine = new CTLine(at))
				{
					textLine.Draw(Ctx);
				}
			});
		}

        public override void ShowStringMonoSpaceAt(Pos2D pos, string text, Size2D capsize, double stride)
        {
            UseNative(() =>
			{
				Size2D s;
                {
                    // テキストの位置に移動
                    var p = Transform * pos;
                    Affine2D SkRot;
                    {
                        // 行列分解
                        var pSRS = Transform.FactorGlobalPos();
                        var SRS = pSRS.Item2;
                        var SRs = SRS.FactorLocalScale();
                        SkRot = SRs.Item1;
                        s = SRs.Item2;
                    }
                    // 変換行列設定
                    Ctx.TranslateCTM((nfloat)p.X, (nfloat)p.Y);
                    Ctx.ConcatCTM(Transform.DeleteTranslation().ToNative());


					Ctx.TranslateCTM(0, (nfloat)capsize.H);
                    Ctx.TextMatrix = CGAffineTransform.MakeIdentity();
					//Ctx.TextMatrix = None.Run(() =>
					//{
					//	Affine2D t;
					//	t = Affine2D.Scale((nfloat)capsize.AspectW, -1);
					//	return t.ToNative();
					//});
                }
                // テキスト描画モード設定
                Ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);
                Ctx.SetFillColor(UIColor.Black.CGColor);
                Ctx.SetStrokeColor(UIColor.Black.CGColor);

                // 描画用のデータ取得
                var font = FontFromCapSize(capsize.H);
                var uifont = UIFont.FromName(font.FullName, font.Size);
                var a = AttribFromFont(font);
				var tm = None.Run(() =>
				{
					Affine2D t;
					t = Affine2D.Scale((nfloat)capsize.AspectW, -1);
					return t.ToNative();
				});


                for (int i = 0; i < text.Length; i++)
                {
					var single = text[i].ToString();
					var astring = new NSAttributedString(single, a);
                    var stringsize = astring.Size;

                    var aspect = stringsize.Width / capsize.H;
                    var r = (nfloat)(capsize.AspectW / aspect);

					//UseNative(() =>
					//{
     //                   Ctx.AddPath(Frame2D.Create(Pos2D.InitY(-capsize.H), capsize).ToStrip().ToNativePath());
					//    Ctx.StrokePath();
					//});

					// 描画
					using (var textLine = new CTLine(astring))
					{
						Console.WriteLine("rate" + r);
						Console.WriteLine("aspect" + aspect);

                        if (EnableAspectScaleUp || r < 1.0f)
                            Ctx.TextMatrix = CGAffineTransform.MakeScale(r, -1);//CGAffineTransform.Scale(tm, r, 1);
                        else
                        {
                            Ctx.TextMatrix = CGAffineTransform.MakeScale(1, -1);
                        }
						textLine.Draw(Ctx);
					}
                    Ctx.TranslateCTM((nfloat)stride, 0);
                }
			});
        }

        public override void ShowFieldTextMonoSpaceAt(Pos2D pos, FieldText text, Size2D capsize, double stride)
        {
            UseNative(() =>
            {

                // 描画用のデータ取得
                var font = FontFromCapSize(capsize.H);
                var uifont = UIFont.FromName(font.FullName, font.Size);
                var a = AttribFromFont(font);
                var tm = None.Run(() =>
                {
                    Affine2D t;
                    t = Affine2D.Scale((nfloat)capsize.AspectW, -1);
                    return t.ToNative();
                });

                // テキストの位置に移動する行列
                var origin = Transform * pos.ToTransform();

                // テキスト描画用の原点
                var textOriginNative = (
                    origin * Pos2D.InitY(capsize.H).ToTransform()
                ).ToNative();

                Action<int> ReadyText = (index) =>
                {
                    // テキスト描画モード設定
                    Ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);
                    Ctx.SetFillColor(UIColor.Black.CGColor);
                    Ctx.SetStrokeColor(UIColor.Black.CGColor);

                    // テキストの位置に移動
                    Ctx.ConcatCTM(textOriginNative);
                    Ctx.TranslateCTM((nfloat)(index * stride), 0);

                    // テキストの描画原点あわせ
                    Ctx.TextMatrix = CGAffineTransform.MakeIdentity();
                };

                Action<int> ReadyLogo = (index) =>
                {
                    // 原点の書き換え
                    SetTransform(origin);

                    // 位置合わせ
                    Translate(index * stride, 0);
                };

                // 描画
                for (int i = 0; i < text.Count; i++)
                {
                    var elem = text[i];
                    PushState();
                    elem.Do(
                        isChar: (charelem) => {
                            var single = charelem.Char.ToString();
                            var astring = new NSAttributedString(single, a);
                            var stringsize = astring.Size;

                            var aspect = stringsize.Width / capsize.H;
                            var r = (nfloat)(capsize.AspectW / aspect);


                            if (!Text.TextEncodingsExt.CompatByte2(charelem.Char))
                            {
                                ReadyLogo(i);
                                PushStateNative();

                                Ctx.SetFillColor(UIColor.Red.CGColor);
                                Ctx.SetStrokeColor(UIColor.Red.CGColor);
                                Ctx.SetLineWidth(1);
                                var path = Frame2D.Create(Pos2D.Zero(), capsize).ToStrip();
                                StrokeLines(path);
                                PopStateNative();
                            }

                            // 変換行列準備
                            ReadyText(i);

                            if (!Text.TextEncodingsExt.CompatByte2(charelem.Char))
                            {
                                Ctx.SetFillColor(UIColor.Red.CGColor);
                                Ctx.SetStrokeColor(UIColor.Red.CGColor);
                            }

                            // 描画
                            using (var textLine = new CTLine(astring))
                            {
                                //Console.WriteLine("rate" + r);
                                //Console.WriteLine("aspect" + aspect);
                                if (EnableAspectScaleUp || r < 1.0f)
                                    Ctx.TextMatrix = CGAffineTransform.MakeScale(r, -1);//CGAffineTransform.Scale(tm, r, 1);
                                else
                                {
                                    Ctx.TextMatrix = CGAffineTransform.MakeScale(1, -1);
                                }
                                textLine.Draw(Ctx);
                            }
                        },
                        isLogo: (single) => {
                            // 変換行列設定
                            ReadyLogo(i);
                            ShowLogoHere(single.id, capsize);
                        }
                    );
                    PopState();
                }
            });
        }

        public override void StrokeLines(IStrip path)
        {
            var newPath = Transform * path;
            var n = newPath.ToNativePath();
            UseNative(()=>
            {
				Ctx.AddPath(n);
				Ctx.StrokePath(); 
            });
        }

        public override void FillLines(IStrip path)
		{
			var newPath = Transform * path;
			var n = newPath.ToNativePath();
            UseNative(() =>
			{
				Ctx.AddPath(n);
				Ctx.FillPath();
            });

        }

        public override void StrokeLine(Line line)
        {
            var td = Transform * line;
            UseNative(()=>
            {
                Ctx.StrokeLineSegments(td.ToNativePoints());
            });
        }

        public override void SetStrokeColor(CommonColor color)
        {
            var nc = color.ToNativeColor();
            Ctx.SetStrokeColor(nc);
        }

        public override void SetFillColor  (CommonColor color)
        {
			var nc = color.ToNativeColor();
			Ctx.SetFillColor(nc);
		}

        CTFont FontFromSize(double heightpt)
        {
            // サイズ決定
            var font = new CTFont(CurrentFont.FullName, (nfloat)(heightpt));
            return font;
		}

		CTFont FontFromCapSize(double heightpt)
		{
			// 倍率計算
			var b = new CTFont(CurrentFont.FullName, 10);
			var rate = b.CapHeightMetric / 10.0;

			// サイズ決定
			var font = new CTFont(CurrentFont.FullName, (nfloat)(heightpt / rate));

			b.Dispose();

			return font;
		}

		NSDictionary AttribFromFont(CTFont font)
		{
			var mstyle = NSParagraphStyle.Default.MutableCopy() as NSMutableParagraphStyle;
			mstyle.HeadIndent = 0;
            mstyle.TailIndent = 0;
            mstyle.FirstLineHeadIndent = 0;
            mstyle.AllowsDefaultTighteningForTruncation = false;

            mstyle.LineBreakMode = UILineBreakMode.WordWrap;


			var a = new NSMutableDictionary();
			a.Add(CTLanguageAttribute, new NSString("ja"));
			a.Add(CTStringAttributeKey.ForegroundColorFromContext, NSObject.FromObject(true));
			//
			a.Add(CTStringAttributeKey.Font, NSObject.FromObject(font));
            a.Add(CTStringAttributeKey.ParagraphStyle, mstyle);
            return a;
        }
    }
}
