using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TokyoChokoku.FieldModel;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    public class EmbossmentData
    {
        /// <summary>
        /// 文字列が 空の場合に true を返します。
        /// </summary>
        public bool IsEmpty { 
            get {
                return string.IsNullOrWhiteSpace(Text.Text);
            }
        }

        public EmbossmentMode        Mode   { get; }
        public EmbossmentAttrib      Attrib { get; }
        public EmbossmentText        Text   { get; }

        public IPMMarkingParameterDB SpecifiedMarkingParameterDB { get; private set; } = null;

        public IPMMarkingParameterDB MarkingParameterDB => SpecifiedMarkingParameterDB ?? AppSettingMarkingParameterDB.CreateDefault();


        public MaterializableTextSizeLevel TextSizeLevel => MarkingParameterDB.MaterializableOf(Mode.TextSize);
        public EmbossmentTextSize EmbossmentTextSize => MarkingParameterDB.GetTextSize(Mode.TextSize).ToEmbossmentTextSize();

        [JsonObject]
        public class JsonFormat
		{
			[JsonProperty("mode")]
            public MutableEmbossmentMode Mode { get; set; }
			[JsonProperty("single")]
            public string Text { get; set; }

            public JsonFormat() {}
            public JsonFormat(EmbossmentMode mode, EmbossmentText text)
            {
                Mode = mode.MutableCopy();
                Text = text.Text;
            }

            public EmbossmentData Bake() {
                var mode = Mode ?? throw new NullReferenceException("required class mode");
                var text = Text ?? throw new NullReferenceException("required class single");
                // FIXME: EmbossmentAttrib のシリアライズに対応する
                return new EmbossmentData(Mode.Bake(), new EmbossmentText(text), EmbossmentAttrib.CreateFromGlobal());
            }

            public static JsonFormat From(EmbossmentData data)
            {
                return new JsonFormat(data.Mode, data.Text);
            }

			public static JsonFormat FromJson(string text) {
				return JsonConvert.DeserializeObject<JsonFormat>(text);
            }

            public string ToJson() {
                return JsonConvert.SerializeObject(this);
            }
		}

		//public IList<FieldText> CurrentPages
		//{
		//	get
		//	{
		//		return Mode.TextSize.Match(
		//			small : (it) => { return Text.PagesForSmall; },
		//			medium: (it) => { return Text.PagesForMedium; },
		//			large : (it) => { return Text.PagesForLarge; }
		//		);
		//	}
		//}
        
        public IList<FieldText> CreatePages(EmbossArea area)
        {
			return Mode.TextSize.Match(
                small : (it) => { return Text.CreatePagesForSmall (area); },
				medium: (it) => { return Text.CreatePagesForMedium(area); },
				large : (it) => { return Text.CreatePagesForLarge (area); }
			);
        }

		#region Init
        public static EmbossmentData Empty { get; } = new EmbossmentData(new EmbossmentMode(), new EmbossmentText(""), new EmbossmentAttrib());

        public static EmbossmentData Create(EmbossmentMode mode, string text, IPMMarkingParameterDB markingParameterDB)
        {
            return new EmbossmentData(mode, new EmbossmentText(text), EmbossmentAttrib.CreateFromGlobal(), markingParameterDB);
        }

        public static EmbossmentData Create(EmbossmentMode mode, string text)
		{
            return new EmbossmentData(mode, new EmbossmentText(text), EmbossmentAttrib.CreateFromGlobal());
		}

		public static EmbossmentData Create(string text)
		{
            return new EmbossmentData(new EmbossmentMode(), new EmbossmentText(text), EmbossmentAttrib.CreateFromGlobal());
		}

        EmbossmentData(EmbossmentMode mode, EmbossmentText text, EmbossmentAttrib attrib, IPMMarkingParameterDB markingParameterDB = null)
		{
			Mode   = mode;
            Text   = text.ChangeParent(this);
            Attrib = attrib;
            SpecifiedMarkingParameterDB = markingParameterDB;
		}
		#endregion



		public EmbossmentData ReplaceMode(EmbossmentMode mode) {
            if (mode == null) throw new NullReferenceException();
            return new EmbossmentData(mode, Text, Attrib);
        }

        public EmbossmentData ReplaceText(EmbossmentText text) {
            if (text == null) throw new NullReferenceException();
            return new EmbossmentData(Mode, text, Attrib);
        }

        public EmbossmentData ReplaceAttrib(EmbossmentAttrib attrib) {
            if (attrib == null) throw new NullReferenceException();
            return new EmbossmentData(Mode, Text, attrib);
        }

        public EmbossmentData ReplaceAttribWithGlobal() {
            var attrib = EmbossmentAttrib.CreateFromGlobal();
            return ReplaceAttrib(attrib);
        }

        public string ToJson()
        {
            return JsonFormat.From(this).ToJson();
        }

        public static EmbossmentData FromJson(string text)
        {
            return JsonFormat.FromJson(text).Bake();
        }

    }
}
