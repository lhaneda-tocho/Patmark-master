using System;
using TokyoChokoku.Patmark.Common;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public class EmbossArea
	{
        #region Properties
        public double Widthmm  { get; }
        public double Heightmm { get; }

		public double WidthPt
		{
			get
			{
				var inch = Widthmm / 25.4;
				return inch * 72;
			}
		}
		public double HeightPt
		{
			get
			{
                var inch = Heightmm / 25.4;
				return inch * 72;
			}
		}

        public double WidthMainScreenDot {
            get
            {
                var screen = ScreenUtil.Instance;
                var dpmm = screen.getDPMM().value;
                return dpmm * Widthmm; // mm -> dot
            }
        }

		public double HeightMainScreenDot
		{
			get
			{
				var screen = ScreenUtil.Instance;
				var dpmm = screen.getDPMM().value;
                return dpmm * Heightmm; // mm -> dot
			}
		}
        #endregion

        public EmbossArea(double wmm, double hmm)
        {
            Widthmm  = wmm;
            Heightmm = hmm;
        }

        public override string ToString()
        {
            return string.Format("[EmbossArea: {0}, {1}]", Widthmm, Heightmm);
        }
	}
}
