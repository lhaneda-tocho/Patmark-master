using System;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public struct EmbossmentTextSize
    {
        public double Heightmm;
        public double Widthmm;
        public double Stridemm;

        public double Aspect {
            get {
                return Widthmm / Heightmm;
            }
        }

        public static EmbossmentTextSize InitWithHeightmm(double heightmm)
        {
            EmbossmentTextSize s;
            s.Heightmm = heightmm;
            s.Widthmm = heightmm * 0.6;
            s.Stridemm = heightmm * 0.8;
            return s;
        }
    }
}
