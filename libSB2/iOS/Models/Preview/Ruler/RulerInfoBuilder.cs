using System;
using System.Linq;
using System.Collections.Generic;
using static System.Math;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class RulerInfoBuilder
    {
        /// <summary>
        /// ルーラの開始値
        /// </summary>
        /// <value>The start.</value>
        public double RulerStart { get; set; }

        /// <summary>
        /// ルーラの終了値
        /// </summary>
        /// <value>The end.</value>
        public double RulerEnd { get; set; }

        int maxGridCount;
        /// <summary>
        /// ルーラ中のメモリを最大いくつ表示するべきか指定します．
        /// </summary>
        public int MaxGridCount {
            get {
                return maxGridCount;
            }
            set {
                if (value < 0)
                    maxGridCount = 0;
                else
                    maxGridCount = value;
            }
        }

        ISet<int> needleCountSet = new HashSet<int> () { 1, 2, 5, 10 };
        /// <summary>
        /// 0以上 10未満 の範囲に いくつメモリを配置するべきか指定します．
        /// 複数指定可能です．
        /// </summary>
        public ISet<int> BaseCountSet {
            get {
                return needleCountSet;
            }
            set {
                if (value == null)
                    needleCountSet = new HashSet<int> () { 1, 2, 5, 10 };
                else
                    needleCountSet = value;
            }
        }


        int MaxDigit ()
        {
            return (int) Ceiling (Log10 (RulerEnd - RulerStart));
        }

        GridScaleInfo CreateGridScaleInfo (int needleCount, int digit)
        {
            var scale = 1.0 / needleCount * Pow (10.0, digit);
            var count = (int) Floor ((RulerEnd - RulerStart) / scale);
            return new GridScaleInfo {
                Scale = scale,
                Count = count
            };
        }

        GridScaleInfo MinGridScaleWithDigit (IList<int> needleCountList, int digit, GridScaleInfo minScale)
        {
            foreach (var needleCount in BaseCountSet) {
                var checkee = CreateGridScaleInfo (needleCount, digit);
                if (maxGridCount < checkee.Count)
                    // グリッド数が最大値を超えた場合は更新しない
                    continue;
                if (minScale.Scale > checkee.Scale)
                    // 最小値の更新
                    minScale = checkee;
            }
            return minScale;
        }

        double RecommendedGridScale ()
        {
            var maxDigit = MaxDigit ();
            var baseCountList = new List <int> (BaseCountSet);
            var minScale = CreateGridScaleInfo (baseCountList.Min (), maxDigit);

            for (int digit = maxDigit;; digit--) {
                var maybeNewScale = MinGridScaleWithDigit (baseCountList, digit, minScale);
                if (maybeNewScale.Count == minScale.Count)
                    // 最小値が変化していない時は終了
                    break;
                // 最小値の更新
                minScale = maybeNewScale;
            }

            return minScale.Scale;
        }

        public RulerInfo BuildInfo ()
        {
            return new RulerInfo (RulerStart, RulerEnd, RecommendedGridScale ());
        }

        struct GridScaleInfo
        {
            public double Scale { get; set; }
            public int    Count { get; set; }
        }
    }
}

