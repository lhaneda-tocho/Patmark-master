using System;

using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.Settings
{
    /// <summary>
    /// フォールバック時の打刻設定です。アプリに設定が保存されていない場合に, この設定が使用されます。
    /// この設定は不変です。
    /// </summary>
    public class FallbackMarkingParameterDB : IPMMarkingParameterDB
    {
        public bool IsMutable => false;

        public string DisplayName => nameof(FallbackMarkingParameterDB);

        public PMForce GetForce(ForceLevel key)
        {
            return key.DefaultValueFixed();
        }

        public PMQuality GetQuality(QualityLevel key)
        {
            return key.DefaultValueFixed();
        }

        public PMTextSize GetTextSize(TextSizeLevel key)
        {
            return key.DefaultValueFixed();
        }

        public IPMMarkingParameterDB Baked()
        {
            return this;
        }
    }
}
