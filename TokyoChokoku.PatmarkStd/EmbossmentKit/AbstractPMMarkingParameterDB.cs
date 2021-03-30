using System;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// 可変 or 読み取り専用 のいずれかになる MarkingParameterDB の抽象化実装です。
    /// 
    /// </summary>
    public abstract class AbstractPMMarkingParameterDB : IPMMutableMarkingParameterDB
    {
        public abstract string DisplayName { get; }

        /// <summary>
        /// true なら変更可能、 false なら読み取り専用です。
        /// </summary>
        public abstract bool IsMutable { get; }


        /// <summary>
        /// フォールバック用 DB
        /// </summary>
        protected IPMMarkingParameterDB FallbackDB = new FallbackMarkingParameterDB();


        public virtual IPMMarkingParameterDB Baked()
        {
            return this.StandardImmutableCopy();
        }

        public virtual void Commit()
        {
        }

        public virtual void Drain(IPMMarkingParameterDB src)
        {
            this.StandardDrain(src);
        }

        public abstract PMForce GetForce(ForceLevel key);

        public abstract PMQuality GetQuality(QualityLevel key);

        public abstract PMTextSize GetTextSize(TextSizeLevel key);

        public abstract void SetForce(ForceLevel key, PMForce value);

        public abstract void SetQuality(QualityLevel key, PMQuality value);

        public abstract void SetTextSize(TextSizeLevel key, PMTextSize value);
    }
}
