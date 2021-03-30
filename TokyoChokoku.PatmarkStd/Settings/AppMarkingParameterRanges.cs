using System;
using System.Linq;
using TokyoChokoku.Patmark.EmbossmentKit;
using System.Collections.Generic;

namespace TokyoChokoku.Patmark.Settings
{
    public class AppMarkingParameterRanges
    {
        public static Dictionary<string, PMTextSize> TextSizeSmall = Common.FloatArrayMaker.CreateFloatsWithDecimalRange(
            0.5m,
            4.5m,
            step: 0.5m
        )
        .Select(it => PMTextSize.CreateWithFloat(it))
        .ToDictionary(
            it => it.ToString(),
            it => it
        );


        public static Dictionary<string, PMTextSize> TextSizeMedium = Common.FloatArrayMaker.CreateFloatsWithDecimalRange(
            5.0m,
            9.5m,
            step: 0.5m
        )
        .Select(it => PMTextSize.CreateWithFloat(it))
        .ToDictionary(
            it => it.ToString(),
            it => it
        );

        public static Dictionary<string, PMTextSize> TextSizeLarge = Common.FloatArrayMaker.CreateFloatsWithDecimalRange(
            10.0m,
            20.0m,
            step: 0.5m
        )
        .Select(it => PMTextSize.CreateWithFloat(it))
        .ToDictionary(
            it => it.ToString(),
            it => it
        );

        public static Dictionary<string, PMForce> ForceWeak = new List<int> {
            0,
            1,
            2,
            3
        }.ToDictionary(
            it => it.ToString(),
            it => PMForce.Create(it)
        );

        public static Dictionary<string, PMForce> ForceMedium = new List<int> {
            4,
            5,
            6
        }.ToDictionary(
            it => it.ToString(),
            it => PMForce.Create(it)
        );

        public static Dictionary<string, PMForce> ForceStrong = new List<int> {
            7,
            8,
            9
        }.ToDictionary(
            it => it.ToString(),
            it => PMForce.Create(it)
        );


        public static Dictionary<string, PMQuality> QualityDot = new List<int> {
            1,
            2,
            3
        }.ToDictionary(
            it => it.ToString(),
            it => PMQuality.Create(it)
        );

        public static Dictionary<string, PMQuality> QualityMedium = new List<int> {
            4,
            5,
            6
        }.ToDictionary(
            it => it.ToString(),
            it => PMQuality.Create(it)
        );

        public static Dictionary<string, PMQuality> QualityLine = new List<int> {
            7,
            8,
            9
        }.ToDictionary(
            it => it.ToString(),
            it => PMQuality.Create(it)
        );


        public static Dictionary<string, PMTextSize> GetValueList(TextSizeLevel level)
        {
            return level.Match(
                small   : _ => TextSizeSmall,
                medium  : _ => TextSizeMedium,
                large   : _ => TextSizeLarge
            );
        }

        public static Dictionary<string, PMForce> GetValueList(ForceLevel level)
        {
            return level.Match(
                weak    : _ => ForceWeak,
                medium  : _ => ForceMedium,
                strong  : _ => ForceStrong
            );
        }

        public static Dictionary<string, PMQuality> GetValueList(QualityLevel level)
        {
            return level.Match(
                dot     : _ => QualityDot,
                medium  : _ => QualityMedium,
                line    : _ => QualityLine
            );
        }

        public static TextSizeLevel GetTextSizeLevel(PMTextSize val)
        {
            if (TextSizeSmall.Values.Min() <= val && val <= TextSizeSmall.Values.Max())
            {
                return TextSizeLevel.Small;
            }
            else if (TextSizeMedium.Values.Min() <= val && val <= TextSizeMedium.Values.Max())
            {
                return TextSizeLevel.Medium;
            }
            else if (TextSizeLarge.Values.Min() <= val && val <= TextSizeLarge.Values.Max())
            {
                return TextSizeLevel.Large;
            }
            else
            {
                return TextSizeLevel.Medium;
            }
        }

        public static ForceLevel GetForceLevel(PMForce val)
        {
            if (ForceWeak.Values.Min() <= val && val <= ForceWeak.Values.Max())
            {
                return ForceLevel.Weak;
            }
            else if (ForceMedium.Values.Min() <= val && val <= ForceMedium.Values.Max())
            {
                return ForceLevel.Medium;
            }
            else if (ForceStrong.Values.Min() <= val && val <= ForceStrong.Values.Max())
            {
                return ForceLevel.Strong;
            }
            else
            {
                return ForceLevel.Medium;
            }
        }

        public static QualityLevel GetQualityLevel(PMQuality val)
        {

            if (QualityDot.Values.Min() <= val && val <= QualityDot.Values.Max())
            {
                return QualityLevel.Dot;
            }
            else if (QualityMedium.Values.Min() <= val && val <= QualityMedium.Values.Max())
            {
                return QualityLevel.Medium;
            }
            else if (QualityLine.Values.Min() <= val && val <= QualityLine.Values.Max())
            {
                return QualityLevel.Line;
            }
            else
            {
                return QualityLevel.Medium;
            }
        }
    }
}
