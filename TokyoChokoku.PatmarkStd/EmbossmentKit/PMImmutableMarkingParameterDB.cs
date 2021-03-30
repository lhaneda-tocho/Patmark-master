using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// IPMMarkingParameterDB の不変クラスです。
    /// このクラスで定義される全てのメソッドは、スレッドセーフです。
    /// </summary>
    public class PMImmutableMarkingParameterDB : IPMMarkingParameterDB
    {
        public string DisplayName => nameof(PMImmutableMarkingParameterDB);
        public bool IsMutable => false;

        private IDictionary<TextSizeLevel, PMTextSize> TableOfTextSize { get; }
        private IDictionary<ForceLevel, PMForce> TableOfForce { get; }
        private IDictionary<QualityLevel, PMQuality> TableOfQuality { get; }


        public PMImmutableMarkingParameterDB(IPMMarkingParameterDB src)
        {
            TableOfTextSize = TextSizeLevels.AllItems
                .Select(it => (Key: it, Value: src.GetTextSize(it)))
                .ToDictionary(it => it.Key, it => it.Value);

            TableOfForce = ForceLevels.AllItems
                .Select(it => (Key: it, Value: src.GetForce(it)))
                .ToDictionary(it => it.Key, it => it.Value);

            TableOfQuality = QualityLevels.AllItems
                .Select(it => (Key: it, Value: src.GetQuality(it)))
                .ToDictionary(it => it.Key, it => it.Value);
        }

        /// <summary>
        /// この インスタンスをそのまま返します。
        /// </summary>
        /// <returns>このインスタンス</returns>
        public IPMMarkingParameterDB Baked()
        {
            return this;
        }

        public PMForce GetForce(ForceLevel key)
        {
            return TableOfForce[key];
        }

        public PMQuality GetQuality(QualityLevel key)
        {
            return TableOfQuality[key];
        }

        public PMTextSize GetTextSize(TextSizeLevel key)
        {
            return TableOfTextSize[key];
        }
    }
}
