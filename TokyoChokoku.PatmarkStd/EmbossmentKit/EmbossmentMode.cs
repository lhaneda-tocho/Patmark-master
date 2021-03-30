using System;
using Newtonsoft.Json;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public class EmbossmentMode
    {
        /// <summary>
        /// 文字サイズ
        /// </summary>
        /// <value>The size of the text.</value>
        public TextSizeLevel TextSize { get; }

        /// <summary>
        /// 打刻力
        /// </summary>
        /// <value>The force.</value>
        public ForceLevel    Force    { get; }

        /// <summary>
        /// 品質
        /// </summary>
        /// <value>The quality.</value>
        public QualityLevel  Quality  { get; }


		public void CopyTo(MutableEmbossmentMode mutable)
		{
            mutable.TextSize = TextSize;
            mutable.Force    = Force;
            mutable.Quality  = Quality;
		}

        public MutableEmbossmentMode MutableCopy()
        {
            var m = new MutableEmbossmentMode();
            CopyTo(m);
            return m;
		}

		public string ToJson()
		{
            return MutableCopy().ToJson();
		}

        public static EmbossmentMode FromJson(string text)
        {
            return MutableEmbossmentMode.FromJson(text).Bake();
        }

        public static EmbossmentMode FromMBData(MBData data)
        {
            return new EmbossmentMode(
                AppMarkingParameterRanges.GetTextSizeLevel(PMTextSize.CreateOrDefaultWithFloat(data.Height)),
                AppMarkingParameterRanges.GetForceLevel(PMForce.CreateFromBinaryOrDefault(data.Power)),
                AppMarkingParameterRanges.GetQualityLevel(PMQuality.CreateFromBinaryOrDefault(data.Speed))
            );
        }


        public EmbossmentMode() : this(TextSizeLevel.Medium, ForceLevel.Medium, QualityLevel.Medium)
		{
		}

		public EmbossmentMode(TextSizeLevel textSize, ForceLevel force, QualityLevel quality)
		{
			TextSize = textSize;
			Force = force;
			Quality = quality;
		}
    }

    public class MutableEmbossmentMode
	{
		/// <summary>
		/// 文字サイズ
		/// </summary>
		/// <value>The size of the text.</value>
		[JsonProperty("text_size")]
		public TextSizeLevel TextSize { get; set; } = TextSizeLevel.Medium;

		/// <summary>
		/// 打刻力
		/// </summary>
		/// <value>The force.</value>
		[JsonProperty("force")]
        public ForceLevel    Force    { get; set; } = ForceLevel.Medium;

		/// <summary>
		/// 品質
		/// </summary>
		/// <value>The quality.</value>
		[JsonProperty("quality")] 
        public QualityLevel  Quality  { get; set; } = QualityLevel.Medium;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static MutableEmbossmentMode FromJson(string text)
		{
			return JsonConvert.DeserializeObject<MutableEmbossmentMode>(text);
        }

        public EmbossmentMode Bake()
        {
            return new EmbossmentMode(TextSize, Force, Quality);
        }
    }
}
