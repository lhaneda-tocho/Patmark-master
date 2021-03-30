using System;
using CoreGraphics;
using Foundation;
using UIKit;

// 実装
using TokyoChokoku.SerialModule.Ast;
using TokyoChokoku.CalendarModule.Ast;

using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

using TokyoChokoku.FieldTextKit;
using TokyoChokoku.Patmark.RenderKit.Transform;
using TokyoChokoku.Patmark.FieldCanvas;

using TokyoChokoku.Patmark.iOS.RenderKitForIOS;

// ユーティリティ
using TokyoChokoku.Patmark.UnitKit;

namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    public abstract class FieldCanvas
    {
        // FIXME : ノードプロセッサとFieldText表示周りを 分離する.
        // FIXME : DI Injection に変更する
        CalendarNodeProcessor cp => CRGlobalSetting.CreateNodeProcessorWithCurrentTime();
        SerialNodeProcessor   sp => SerialGlobalSetting.CreateNodeProcessor();

        // FIXME : フォントの変更
        public const String FontDefault = "Arial";

        /// <summary>
        /// 描画コンテキストです。
        /// </summary>
        public CGContext Context { get; }


        public CGAffineTransform CanvasViewMatrix              { get; }
        public CGAffineTransform CanvasViewTranslation         { get; }
        public CGAffineTransform CanvasViewRotation            { get; }
        public CGAffineTransform CanvasViewScaling             { get; }
        public CGAffineTransform CanvasViewTranslationRotation { get; }

        private static CGAffineTransform ToCGAffine(Affine2D matrix) {
            return matrix.ToNative();
        }


        /// <summary>
        /// 初期設定を行います。
        /// </summary>
        public FieldCanvas(CGContext context, CanvasInfo info)
        {
            Context = context;

            CanvasViewMatrix      = ToCGAffine(info.CanvasViewMatrix);
            CanvasViewTranslation = ToCGAffine(info.CanvasViewTranslationMatrix);
            CanvasViewRotation    = ToCGAffine(info.CanvasViewRotationMatrix);
            CanvasViewScaling     = ToCGAffine(info.CanvasViewScalingMatrix);

            CanvasViewTranslationRotation = ToCGAffine(info.CanvasViewTranslationMatrix * info.CanvasViewRotationMatrix);
        }




        public void DrawPoint(CGAffineTransform m, CGPoint point, double size)
        {
            m = FieldCanvas.Translate(m, point.X, point.Y);


            var cgrect = new CGRect(
                -size / 2,
                -size / 2,
                size,
                size);

            cgrect = m.TransformRect(cgrect);

            Context.BeginPath();
            Context.FillEllipseInRect(cgrect);
            Context.AddEllipseInRect(cgrect);
            Context.StrokePath();
        }



        public void DrawBasePoint(CGAffineTransform m, double baseX, double baseY)
        {
            var size = BasePointSize.GetCurrent();


            m = FieldCanvas.Translate(m, baseX, baseY);


            var cgrect = new CGRect(
                m.x0 - size / 2,
                m.y0 - size / 2,
                size,
                size);


            Context.BeginPath();
            Context.FillEllipseInRect(cgrect);
            Context.AddEllipseInRect(cgrect);
            Context.StrokePath();
        }


        public static CGAffineTransform MakeIdentity()
        {
            return CGAffineTransform.MakeIdentity();
        }

        public static CGAffineTransform Rotate(CGAffineTransform transform, double degrees)
        {
            return CGAffineTransform.Rotate(transform, (nfloat)Unit.deg.ToRad(degrees));
        }


        public static CGAffineTransform Scale(CGAffineTransform transform, double sx, double sy)
        {
            return CGAffineTransform.Scale(transform, (nfloat)sx, (nfloat)sy);
        }


        public static CGAffineTransform Translate(CGAffineTransform Transform, double tx, double ty)
        {
            return CGAffineTransform.Translate(Transform, (nfloat)tx, (nfloat)ty);
        }


        public static CGAffineTransform Concat(CGAffineTransform globalTransform, CGAffineTransform localTransform)
        {
            return CGAffineTransform.Multiply(localTransform, globalTransform);
        }


        public static CGAffineTransform PhaseAxisMirror(CGAffineTransform transform, double centerPhase)
        {
            var result = Rotate(transform, centerPhase);
            result = Scale(result, -1, 1); // x軸を鏡面写し．
            result = Rotate(result, -centerPhase);

            return result;
        }


        public static CGAffineTransform XAxisMirror(CGAffineTransform transform, double centerX)
        {
            transform = FieldCanvas.Translate(transform, centerX, 0);
            transform = FieldCanvas.Scale(transform, -1, 1); // x方向に 鏡面写し
            transform = FieldCanvas.Translate(transform, -centerX, 0);

            return transform;
        }


        public static CGAffineTransform YAxisMirror(CGAffineTransform transform, double centerY)
        {
            transform = FieldCanvas.Translate(transform, 0, centerY);
            transform = FieldCanvas.Scale(transform, 1, -1); // y方向に 鏡面写し
            transform = FieldCanvas.Translate(transform, 0, -centerY);

            return transform;
        }





        public static void Rotate(CGContext context, double degrees)
        {
            context.RotateCTM((nfloat)Unit.deg.ToRad(degrees));
        }


        public static void Scale(CGContext context, double sx, double sy)
        {
            context.ScaleCTM((nfloat)sx, (nfloat)sy);
        }


        public static void Translate(CGContext context, double tx, double ty)
        {
            context.TranslateCTM((nfloat)tx, (nfloat)ty);
        }


        public static void Concat(CGContext context, CGAffineTransform localTransform)
        {
            context.ConcatCTM(localTransform);
        }


        public static void PhaseAxisMirror(CGContext context, double centerPhase)
        {
            Rotate(context, centerPhase);
            Scale(context, -1, 1); // x軸を鏡面写し．
            Rotate(context, -centerPhase);
        }


        public static void XAxisMirror(CGContext context, double centerX)
        {
            FieldCanvas.Translate(context, centerX, 0);
            FieldCanvas.Scale(context, -1, 1); // x方向に 鏡面写し
            FieldCanvas.Translate(context, -centerX, 0);
        }

        public static void YAxisMirror(CGContext context, double centerY)
        {
            FieldCanvas.Translate(context, 0, centerY);
            FieldCanvas.Scale(context, 1, -1); // y方向に 鏡面写し
            FieldCanvas.Translate(context, 0, -centerY);
        }


        /// <summary>
        /// FieldTextをパースした結果を表示します．
        /// </summary>
        /// <returns>The field text node.</returns>
        /// <param name="root">Root Node.</param>
        /// <param name="matrixSupplier">Matrix supplier.</param>
        public void DrawFieldTextNode(FTRootFieldTextNode root, Func<int, CGAffineTransform> matrixSupplier)
        {
            int index = 0;
            foreach (var node in root.ElementEnumerable)
            {
                switch (node.FieldTextType)
                {
                    case FieldTextType.Text:
                        {
                            var concreted = (FTTextNode)node;
                            index = DrawTextNode(index, concreted, matrixSupplier);
                            break;
                        }
                    case FieldTextType.Logo:
                        {
                            var concreted = (FTLogoNode)node;
                            index = DrawLogoNode(index, concreted, matrixSupplier);
                            break;
                        }
                    case FieldTextType.Serial:
                        {
                            var concreted = (FTSerialNode)node;
                            index = DrawSerialNode(index, concreted, matrixSupplier);
                            break;
                        }
                    case FieldTextType.Calendar:
                        {
                            var concreted = (FTCalendarNode)node;
                            index = DrawCalendarNode(index, concreted, matrixSupplier);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// TextNodeを表示します．
        /// </summary>
        /// <returns>次の描画開始地点を表すインデックス</returns>
        /// <param name="startIndex">開始インデックス</param>
        /// <param name="node">表示したいTextNodeを指定します</param>
        /// <param name="matrixSupplier">整数インデックスをアフィン変換行列に写像する関数</param>
        private int DrawTextNode(int startIndex, FTTextNode node, Func<int, CGAffineTransform> matrixSupplier)
        {
            // オーバーフローの検出
            if (int.MaxValue - startIndex < node.Text.Length)
                throw new OverflowException();
            String text = node.Text;
            for (int textI = 0; textI < text.Length; textI++)
            {
                int matrixI = textI + startIndex;
                DrawCharHere(matrixSupplier(matrixI), text[textI]);
            }
            return startIndex + text.Length;
        }

        private int DrawLogoNode(int startIndex, FTLogoNode node, Func<int, CGAffineTransform> matrixSupplier)
        {
            // オーバーフローの検出
            if (int.MaxValue - startIndex < 1)
                throw new OverflowException();
            Context.SaveState();
            DrawLogoIdHere(matrixSupplier(startIndex), node.LogoIdentifier);
            Context.RestoreState();
            return startIndex + 1;
        }

        private int DrawSerialNode(int startIndex, FTSerialNode node, Func<int, CGAffineTransform> matrixSupplier)
        {
            var serialNode = node;
            var text = sp.Convert(serialNode);
            // オーバーフローの検出
            if (int.MaxValue - startIndex < text.Length)
                throw new OverflowException();
            for (int textI = 0; textI < text.Length; textI++)
            {
                int matrixI = textI + startIndex;
                DrawCharHere(matrixSupplier(matrixI), text[textI]);
            }
            return startIndex + text.Length;
        }

        private int DrawCalendarNode(int startIndex, FTCalendarNode node, Func<int, CGAffineTransform> matrixSupplier)
        {
            var textNode = new FTTextNode(cp.Convert(node)); 
            return DrawTextNode(startIndex, textNode, matrixSupplier);
        }

        private void DrawCharHere(CGAffineTransform m, char oneCharacter)
        {
            Context.SaveState();
            // 与えられた変換行列を設定します．
            Context.ConcatCTM(m);
            // 描画ステータスをセットします。
            Context.TextMatrix = new CGAffineTransform(
                1.0f, 0.0f,
                0.0f, -1.0f,
                0.0f, 0.0f);
            Context.SelectFont(FieldCanvas.FontDefault, 1, CGTextEncoding.MacRoman);
            // 描画します。            
            Context.ShowTextAtPoint(0, 1, oneCharacter.ToString());
            Context.RestoreState();
        }

        public void DrawStringHere(String text)
        {
            Context.SaveState();


            // 描画ステータスをセットします。
            Context.TextMatrix = new CGAffineTransform(
                1.0f, 0.0f,
                0.0f, -1.0f,
                0.0f, 0.0f);

            Context.SelectFont(FieldCanvas.FontDefault, 1, CGTextEncoding.MacRoman);

            // 描画します。
            Context.ShowTextAtPoint(0, 1, text);

            Context.RestoreState();
        }



        public void DrawLogoIdHere(CGAffineTransform transform, int id)
        {

            // 先に矩形を表示する．
            CGPoint[] rect = {
                transform.TransformPoint(new CGPoint (0, 0)),
                transform.TransformPoint(new CGPoint (1, 0)),
                transform.TransformPoint(new CGPoint (1, 1)),
                transform.TransformPoint(new CGPoint (0, 1))
            };
            Context.BeginPath();
            Context.AddLines(rect);
            Context.ClosePath();
            Context.StrokePath();

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


            Context.SaveState();

            Context.ConcatCTM(transform);
            Context.ScaleCTM(1 / resolution.Width, 1 / resolution.Height);
            message.DrawString(drawingArea);

            Context.RestoreState();
        }



        /// <summary>
        /// 線を描画します。
        /// </summary>
        public void DrawLine(CGAffineTransform m, float startX, float startY, float endX, float endY)
        {

            Context.BeginPath();

            Context.AddLines(new CGPoint[]{
                m.TransformPoint ( new CGPoint (startX, startY) ),
                m.TransformPoint ( new CGPoint (endX,   endY  ) )
            });

            Context.StrokePath();
        }

        /// <summary>
        /// 線を描画します。
        /// </summary>
        public void DrawLine(float startX, float startY, float endX, float endY)
        {
            Context.SaveState();

            Context.BeginPath();

            Context.AddLines(new CGPoint[]{
                new CGPoint(startX, startY),
                new CGPoint(  endX,   endY)
            });


            Context.StrokePath();

            Context.RestoreState();
        }




        public void DrawLoopedLines(CGAffineTransform transform, CGPoint[] vertices)
        {
            var points = new CGPoint[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                var p = vertices[i];
                points[i] = transform.TransformPoint(new CGPoint(p.X, p.Y));
            }


            Context.BeginPath();
            Context.AddLines(points);
            Context.ClosePath();
            Context.StrokePath();
        }




        public void DrawArcBeam(
            CGAffineTransform m, double startPhase, double endPhase, double innerRadius, double thickness)
        {
            // 基準分割角度
            const double BaseDegreesPerStep = 15;
            var xEntity = new CGPoint(1, 0);

            if (endPhase < startPhase)
            {
                var tmp = startPhase;
                startPhase = endPhase;
                endPhase = tmp;
            }

            var openAngle = endPhase - startPhase;

            // 基準分割角度に近い大きさになるよう ステップ数を求める
            int needsStep = 1 + (int)(openAngle / BaseDegreesPerStep);

            // 1ステップごとに 回転する量 [degrees]
            double dps = openAngle / needsStep;

            if (openAngle < 360)
            {
                // 基準分割数に近い大きさになるよう ステップ数を求める
                needsStep = 1 + (int)(openAngle / BaseDegreesPerStep);

                // 1ステップごとに 回転する量 [degrees]
                dps = openAngle / needsStep;
            }
            else
            {
                dps = BaseDegreesPerStep;
                needsStep = (int)(360d / BaseDegreesPerStep);
                openAngle = 360d;
            }



            // これから求める 頂点配列
            CGPoint[] vertices = new CGPoint[(needsStep + 1) * 2];


            // 内円弧を 直線に分割．
            CGPoint xInner = ScalePoint(xEntity, (nfloat)innerRadius);
            for (int step = 0; step < needsStep + 1; step++)
            {
                double phase = step * dps + startPhase;
                vertices[step] = RotatePoint(xInner, (nfloat)phase);
            }


            // 外円弧を 直線に分割．
            CGPoint xOuter = ScalePoint(xEntity, (nfloat)(innerRadius + thickness));
            for (int step = 0; step < needsStep + 1; step++)
            {
                double phase = openAngle - (step * dps) + startPhase;
                vertices[needsStep + 1 + step] = RotatePoint(xOuter, (nfloat)phase);
            }

            DrawLoopedLines(m, vertices);
        }


        public CGPoint ScalePoint(CGPoint p, nfloat s)
        {
            return
                ScalePoint(p, s, s);
        }

        public CGPoint ScalePoint(CGPoint p, nfloat sx, nfloat sy)
        {
            return
                new CGPoint(p.X * sx, p.Y * sy);
        }

        public CGPoint RotatePoint(CGPoint p, nfloat deg)
        {
            var transform = CGAffineTransform.MakeRotation((float)Unit.deg.ToRad(deg));
            return transform.TransformPoint(p);
        }

        public CGPoint TranslatePoint(CGPoint p, nfloat x, nfloat y)
        {
            return new CGPoint(p.X + x, p.Y + y);
        }
    }
}
