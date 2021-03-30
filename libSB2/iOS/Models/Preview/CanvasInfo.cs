namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// Canvas の表示位置，拡大率，[ピクセル/mm] を記憶するクラス．
    /// </summary>
    public sealed class CanvasInfo {
        /// <summary>
        /// 拡大率 * PixelPerMilli
        /// </summary>
        /// <value>The canvas view scale.</value>
        float CanvasViewScale { 
            get { 
                return PixelPerMilli * Scale;
            }
        }

        /// <summary>
        /// X座標
        /// </summary>
        /// <value>The x.</value>
        public float X     { get; }
        /// <summary>
        /// Y座標
        /// </summary>
        /// <value>The y.</value>
        public float Y     { get; }
        /// <summary>
        /// 拡大率
        /// </summary>
        /// <value>The scale.</value>
        public float Scale { get; }
        /// <summary>
        /// 回転量
        /// </summary>
        /// <value>The angle.</value>
        public float Angle { get; }
        /// <summary>
        /// 拡大率1の時，キャンバス内での1mmメートルに対するピクセル数を返します．
        /// </summary>
        /// <value>The pixel per milli.</value>
        public float PixelPerMilli { get; }

        public AffineMatrix2D CanvasViewMatrix
        {
            get {
                return CanvasViewTranslationMatrix
                    * CanvasViewRotationMatrix
                    * CanvasViewScalingMatrix;
            }
        }


        public AffineMatrix2D CanvasViewTranslationMatrix {
            get {
                return AffineMatrix2D.NewTranslation (X, Y);
            }
        }


        public AffineMatrix2D CanvasViewScalingMatrix {
            get {
                var scale = CanvasViewScale;
                return AffineMatrix2D.NewScaling (scale, scale);
            }
        }


        public AffineMatrix2D CanvasViewRotationMatrix {
            get {
                return AffineMatrix2D.NewRotation (Angle);
            }
        }


        public AffineMatrix2D ViewCanvasMatrix
        {
            get {
                return CanvasViewMatrix.Inverse ();
            }
        }

        public AffineMatrix2D ViewCanvasScRotMatrix {
            get {
                return (
                          CanvasViewRotationMatrix
                    * CanvasViewScalingMatrix).Inverse ();
            }
        }






        public CanvasInfo () {
            X = 0f;
            Y = 0f;
            Scale = 1f;
            Angle = 0f;
            PixelPerMilli = Consts.PixelPerMilli;
        }

        public CanvasInfo (float x, float y, float scale, float angle, float pixelPerMilli)
        {
            X = x;
            Y = y;
            Scale = scale;
            Angle = angle;
            PixelPerMilli = pixelPerMilli;
        }

        /// <summary>
        /// 拡大率をリセットします。
        /// </summary>
        public CanvasInfo CraeteDefaultScale()
        {
            return new CanvasInfo (
                x: X / Scale,
                y: Y / Scale,
                scale: 1.0f,
                pixelPerMilli: PixelPerMilli,
                angle: Angle
            );
        }


        /// <summary>
        /// 拡大率を上げます。
        /// </summary>
        public CanvasInfo CreateIncresedScale()
        {
            return new CanvasInfo (
                x: X,
                y: Y,
                scale: Scale + 0.1f,
                pixelPerMilli: PixelPerMilli, 
                angle: Angle);
        }

        /// <summary>
        /// 拡大率を下げます。
        /// </summary>
        public CanvasInfo CreateDecresedScale()
        {
            return new CanvasInfo (
                x: X,
                y: Y,
                scale: Scale - 0.1f,
                pixelPerMilli: PixelPerMilli, 
                angle: Angle);
        }

        /// <summary>
        /// プレビュー領域をリセットします。
        /// </summary>
        public CanvasInfo CreateDefaultPosition()
        {
            return new CanvasInfo (
                x: 0f,
                y: 0f,
                scale: Scale,
                pixelPerMilli: PixelPerMilli, 
                angle: Angle);
        }

        /// <summary>
        /// View座標系での キャンバスの位置です．
        /// </summary>
        /// <returns>The position.</returns>
        public Position2D GetPosition ()
        {
            return new Position2D (X, Y);
        }

        /// <summary>
        /// 指定された キャンバス上での座標点を 中心に捉えるように 画面を移動します
        /// </summary>
        /// <returns>新しい CanvasInfo</returns>
        /// <param name="viewport">点を表示する領域</param>
        /// <param name="inView">ビュー上の1点</param>
        public CanvasInfo LookAtViewPoint (Position2D inView, RectangleArea viewport)
        {
            var viewCenter = viewport.GetCenter ();
            return Move (viewCenter - inView);
        }

        public CanvasInfo Move ( Position2D pos ) {

            var x = X + pos.X;
            var y = Y + pos.Y;

            return new CanvasInfo ((float)x, (float)y, Scale, Angle, PixelPerMilli);
        }


        public CanvasInfo SetPosition ( Position2D pos ) {

            var x = pos.X;
            var y = pos.Y;

            return new CanvasInfo ((float)x, (float)y, Scale, Angle, PixelPerMilli);
        }


        public CanvasInfo SetScale ( float scale ) {

            return new CanvasInfo (X, Y, scale, Angle, PixelPerMilli);
        }

        public Position2D CanvasToView (Position2D inCanvas)
        {
            var rotsc = CanvasViewMatrix;// CanvasViewRotationMatrix * CanvasViewScalingMatrix;
            return rotsc * inCanvas;
        }

        public CanvasInfo SetPixelPerMilli (float ppm)
        {
            return new CanvasInfo (
                X, Y, Scale, Angle, ppm
            );
        }

        public CanvasInfo SetPixelPerMilli (float pixels, float millis)
        {
            return SetPixelPerMilli (pixels / millis);
        }
    }
}

