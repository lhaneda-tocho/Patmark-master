using System;
namespace TokyoChokoku.Patmark.TextData
{
    /// <summary>
    /// 文字の大きさを表す共通クラスです
    /// </summary>
    public interface ITextSize
    {
        /// <summary>
        /// 文字の高さを mm で返します
        /// </summary>
        /// <value>The height.</value>
        double Heightmm { get; }

        /// <summary>
        /// 文字の高さを pt で返します
        /// 72.27 [pt] ≒ 1[inch] と定義します．
        /// </summary>
        /// <value>The point.</value>
        double StdPoint { get; }


        /// <summary>
        /// 文字の高さを adobeの pt で返します
        /// 72.00 [pt] ≒ 1[inch] と定義します．
        /// </summary>
        /// <value>The point.</value>
        double AdobePoint { get; }
    }

    public static class TextSize
    {
        public static ITextSize OfHeightmm(double heightmm)
        {
            return new TextSizeOfHeight(heightmm);
        }

		public static ITextSize OfAdobePoint(double point)
		{
			return new TextSizeOfAdobePoint(point);
		}
    }

    #region TextSize Concretion

    class TextSizeOfHeight : ITextSize
    {
        public double Heightmm { get; }
        public double StdPoint
        {
            get
            {
                var inch = Heightmm / 25.4;
                var pt = inch * 72.27;
                return pt;
            }
        }
        public double AdobePoint
        {
            get
            {
                var inch = Heightmm / 25.4;
                var pt = inch * 72.00;
                return pt;
            }
        }

        internal TextSizeOfHeight(double height)
        {
            Heightmm = height;
        }
    }

    class TextSizeOfAdobePoint : ITextSize
    {
        public double Heightmm
        {
            get
            {
                var inch = AdobePoint / 72.0;
                var mm = inch * 25.4;
                return mm;
            }
        }

        public double StdPoint
        {
            get
            {
                var inch = AdobePoint / 72.0;
                return inch * 72.27;
            }
        }

        public double AdobePoint { get; }


        internal TextSizeOfAdobePoint(double adobePoint)
        {
            AdobePoint = adobePoint;
        }
    }

    #endregion
}
