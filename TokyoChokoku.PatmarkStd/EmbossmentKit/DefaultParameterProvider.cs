using System;
namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// TextSizeLevel, ForceLevel, QualityLevel に対応する実際の値を返すクラスです。
    /// 
    /// ※注意※ アプリ開始時に必ずセットしてください。
    /// </summary>
    public class DefaultParameterProvider
    {
        public static DefaultParameterProvider Instance = null;

        public Func<TextSizeLevel, PMTextSize> GetTextSize{
            get; private set;
        }

        public Func<ForceLevel, PMForce> GetForce{
            get; private set;
        }

        public Func<QualityLevel, PMQuality> GetQuality{
            get; private set;
        }

        public static void Init(
            Func<TextSizeLevel, PMTextSize> GetTextSize,
            Func<ForceLevel   , PMForce> GetForce,
            Func<QualityLevel , PMQuality> GetQuality
        ){
            Instance = new DefaultParameterProvider();
            Instance.GetTextSize = GetTextSize;
            Instance.GetForce = GetForce;
            Instance.GetQuality = GetQuality;
        }
    }
}
