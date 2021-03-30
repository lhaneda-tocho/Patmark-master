using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;
using TokyoChokoku.FieldModel;
using TokyoChokoku.SerialModule.Ast;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.SerialModule.Setting;
using TokyoChokoku.CalendarModule.Setting;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    using Settings;

    public class EmbossmentText
    {
        private readonly MutableEmbossmentText data;

        /// <summary>
        /// 打刻文字列．ブランクのテキスト(もしくは null)でこのオブジェクトを初期化した場合，
        /// このプロパティは 0文字の文字列を返します．
        /// </summary>
        /// <value>The text.</value>
        public string           Text   { get => data.Text; }


        //[Obsolete("CreatePagesForSmall(EmbossArea) に置き換えられました.")]
        //public IList<FieldText> PagesForSmall  => data.PagesForSmall;
        //[Obsolete("CreatePagesForMedium(EmbossArea) に置き換えられました.")]
        //public IList<FieldText> PagesForMedium => data.PagesForMedium;
        //[Obsolete("CreatePagesForLarge(EmbossArea) に置き換えられました.")]
        //public IList<FieldText> PagesForLarge  => data.PagesForLarge;

        public IList<FieldText> CreatePagesForSmall  (EmbossArea area) => data.CreatePagesForSmall (area);
        public IList<FieldText> CreatePagesForMedium (EmbossArea area) => data.CreatePagesForMedium(area);
        public IList<FieldText> CreatePagesForLarge  (EmbossArea area) => data.CreatePagesForLarge(area);

        public bool IsEmpty => data.IsEmpty;

        public EmbossmentText(string text, EmbossmentData allData = null)
        {
            data = new MutableEmbossmentText(text, allData);
        }

        public EmbossmentText ChangeParent(EmbossmentData allData) {
            return new EmbossmentText(Text, allData);
        }

		public string ToJson()
		{
            return data.ToJson();
		}

		public static EmbossmentText FromJson(string text)
		{
            return MutableEmbossmentText.FromJson(text).Bake();
		}

        public MutableEmbossmentText MutableCopy() {
            return new MutableEmbossmentText(Text, data.AllData);
        }
    }


    public class MutableEmbossmentText
    {
        [JsonIgnore]
        private volatile string _Text = "";

        /// <summary>
        /// 打刻文字列．ブランクのテキスト(もしくは null)でこのオブジェクトを初期化した場合，
        /// このプロパティは 0文字の文字列を返します．
        /// </summary>
        /// <value>The text.</value>
        [JsonProperty("text")]
        public string Text {
            get => _Text;
            set => _Text = value ?? "";
        }

        [JsonIgnore]
        public EmbossmentData AllData { get; }

        [JsonIgnore]
        public FieldText FieldText {
            get
            {
                var attrib = AllData.Attrib;
                return attrib.FieldTextFrom(Text);
            }
        }


        [JsonIgnore]
        public MaterializableTextSizeLevel SmallLevel
            => AllData.MarkingParameterDB.MaterializableOf(TextSizeLevel.Small);

        [JsonIgnore]
        public MaterializableTextSizeLevel MediumLevel
            => AllData.MarkingParameterDB.MaterializableOf(TextSizeLevel.Medium);

        [JsonIgnore]
        public MaterializableTextSizeLevel LargeLevel
            => AllData.MarkingParameterDB.MaterializableOf(TextSizeLevel.Large);


        public IList<FieldText> CreatePagesForSmall(EmbossArea area)
        {
            var embossWidth = area.Widthmm;
            var es = SmallLevel.ToEmbossmentTextSize();
            var stride = es.Stridemm;
            var width = es.Widthmm;
            var countStride = (int)(embossWidth / stride);
            var tip = embossWidth - countStride * stride;
            var countLast = (tip >= width) ? 1 : 0;

            return FieldText.SplitPer(countStride + countLast);
        }


        public IList<FieldText> CreatePagesForMedium(EmbossArea area)
        {
            var embossWidth = area.Widthmm;
            var es = MediumLevel.ToEmbossmentTextSize();
            var stride = es.Stridemm;
            var width = es.Widthmm;
            var countStride = (int)(embossWidth / stride);
            var tip = embossWidth - countStride * stride;
            var countLast = (tip >= width) ? 1 : 0;

            return FieldText.SplitPer(countStride + countLast);
        }


        public IList<FieldText> CreatePagesForLarge(EmbossArea area)
        {
            var embossWidth = area.Widthmm;
            var es = LargeLevel.ToEmbossmentTextSize();
            var stride = es.Stridemm;
            var width = es.Widthmm;
            var countStride = (int)(embossWidth / stride);
            var tip = embossWidth - countStride * stride;
            var countLast = (tip >= width) ? 1 : 0;

            return FieldText.SplitPer(countStride + countLast);
        }

		[JsonIgnore]
		public bool IsEmpty
		{
			get
			{
				return string.Equals(Text, "");
			}
		}

		public MutableEmbossmentText(string text, EmbossmentData data)
		{
			if (string.IsNullOrWhiteSpace(text))
				text = "";
			Text = text;
            AllData = data;
		}

  //      IList<FieldText> ConstantDivide(int count)
		//{
		//	if (IsEmpty)
		//		return new List<string>().ToImmutableList();
		//	// TODO: 文字サイズを取得して適切に分割できるようにする．
		//	var c = (Text.Length / count) + ((Text.Length % count == 0) ? 0 : 1);
		//	var ml = new List<string>(c);
		//	for (int i = 0; i < c; i++)
		//	{
		//		var s = i * count;
		//		var len = Math.Min(Text.Length - s, count);
		//		var sub = Text.Substring(s, len);
		//		ml.Add(sub);
		//	}
		//	return ml.ToImmutableList();
		//}

        public string ToJson()
        {
			return JsonConvert.SerializeObject(this);
        }

        public static MutableEmbossmentText FromJson(string text)
        {
            return JsonConvert.DeserializeObject<MutableEmbossmentText>(text);
        }

        public EmbossmentText Bake() {
            return new EmbossmentText(Text, null);
        }
    }
}