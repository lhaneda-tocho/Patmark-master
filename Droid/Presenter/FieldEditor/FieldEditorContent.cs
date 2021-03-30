using System;
using TokyoChokoku.Patmark.EmbossmentKit;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace TokyoChokoku.Patmark.Droid.Presenter.FieldEditor
{
    public class FieldEditorContent
    {
        public const string DataKey = "EmbossmentData";

        public EmbossmentData Data { get; set; }
        public string DataJson {
            get => Data.ToJson();
            set => Data = EmbossmentData.FromJson(value);
        }

        public Bundle ToBundle()
		{
			var bundle = new Bundle();

            bundle.PutString(DataKey, DataJson);
            return bundle;
        }


		public static FieldEditorContent Of(EmbossmentData data)
		{
			var content = new FieldEditorContent();
            content.Data = data;
			return content;
		}

        public static FieldEditorContent FromBundle(Bundle bundle)
		{
			var content = new FieldEditorContent();
            content.DataJson = bundle.GetString(DataKey);
            return content;
		}

        public static FieldEditorContent CreateSingle(string text)
        {
            var data = EmbossmentData.Create(text);
            var fd = new FieldEditorContent();
            fd.Data = data;
            return fd;
        }
	}
}
