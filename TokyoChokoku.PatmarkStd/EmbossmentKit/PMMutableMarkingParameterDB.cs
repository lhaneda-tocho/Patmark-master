using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// IPMMarkingParameterDB の可変クラスです。
    /// このクラスはスレッドセーフではありません。
    /// </summary>
    public class PMMutableMarkingParameterDB : IPMMutableMarkingParameterDB
    {
        public string DisplayName => nameof(PMMutableMarkingParameterDB);
        public bool IsMutable => true;

        private IDictionary<TextSizeLevel, PMTextSize> TableOfTextSize { get; }
        private IDictionary<ForceLevel, PMForce> TableOfForce { get; }
        private IDictionary<QualityLevel, PMQuality> TableOfQuality { get; }

        public PMMutableMarkingParameterDB() : this(new FallbackMarkingParameterDB())
        { }

        public PMMutableMarkingParameterDB(IPMMarkingParameterDB src)
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

        public IPMMarkingParameterDB Baked()
        {
            return this.StandardImmutableCopy();
        }

        public PMForce GetForce(ForceLevel key)
        {
            return TableOfForce[key];
        }

        public void SetForce(ForceLevel key, PMForce value)
        {
            TableOfForce[key] = value;
        }

        public PMQuality GetQuality(QualityLevel key)
        {
            return TableOfQuality[key];
        }

        public void SetQuality(QualityLevel key, PMQuality value)
        {
            TableOfQuality[key] = value;
        }

        public PMTextSize GetTextSize(TextSizeLevel key)
        {
            return TableOfTextSize[key];
        }

        public void SetTextSize(TextSizeLevel key, PMTextSize value)
        {
            TableOfTextSize[key] = value;
        }

        public void Drain(IPMMarkingParameterDB src)
        {
            this.StandardDrain(src);
        }

        public void Commit()
        {
            // 何もしない
        }
    }
}
