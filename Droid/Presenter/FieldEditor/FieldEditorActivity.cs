
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TokyoChokoku.Patmark.Presenter.Embossment;

namespace TokyoChokoku.Patmark.Droid.Presenter.FieldEditor
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class FieldEditorActivity : Activity
    {
        const int TitleID = Resource.String.fieldEditorActivity_name;
        static Type NextActivityType = typeof(Embossment.EmbossmentActivity);


        EditText TextField { get; set; }
        EmbossmentFileRepository FileRepo { get; } = new EmbossmentFileRepository();
        FileContext              CurrentFile => FileRepo.CurrentFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FieldEditor);
            TextField = FindViewById<EditText>(Resource.Id.edittext_on_field_editor);
            // TODO: 他Localeへの対応について考える

            // TextLocale は API Level 17 から追加されました。
            // → https://developer.xamarin.com/api/property/Android.Widget.TextView.TextLocale/
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr1)
            {
                TextField.TextLocale = Java.Util.Locale.Japan;
            }
            // アクティビティのタイトルの設定
            Title = GetString(TitleID);

        }

        protected override void OnStart()
        {
            base.OnStart();

            var success = FileRepo.RestoreFile(def: FileContext.Empty());
            FileRepo.SaveQuickModeAsBlankIfNeeded();
            var file    = CurrentFile;
            if (!success)
            {
                // 何もしません。
            }
            else if (file.isLocalFile)
            {
                // クイックモードの場合は、テキストを編集してもらいます。
                TextField.Text = file.Owner.Fields[0].Text;
            }
            else
            {
                // アドバンスモードの場合は、即座に打刻画面へ遷移します。
                OnAcceptNextEvent();
            }
        }

        protected override void OnPause()
        {
            //OnAutoSave(FileRepo.CurrentFile);
            base.OnPause();
        }

        /// <summary>
        /// Start Next Activity.
        /// </summary>
        /// <param name="view">View.</param>
        [Java.Interop.Export]
        public void RequestNextEvent(View view)
        {
            OnAcceptNextEvent();
        }

        /// <summary>
        /// 次の画面へ遷移します
        /// </summary>
        public void OnAcceptNextEvent() {
            var file = CurrentFile;
            if (file.isLocalFile)
            {
                NextQuickMode(file);
            }
            else
            {
                NextAdvanceMode(file);
            }
        }

        public void NextQuickMode(FileContext file)
        {
            var context = this;
            var to = NextActivityType;
            // TODO: Text Field の編集を終了させる.
            // 自動保存イベント実行
            OnAutoSave(file);
            // 開始
            StartActivity(new Intent(context, to));
        }

        public void NextAdvanceMode(FileContext file) {
            var context = this;
            var to = NextActivityType;
            Toast.MakeText(
                this,
                Resources.GetString(Resource.String.toast_advance_mode_file_can_not_be_edited),
                ToastLength.Short
            ).Show();
            StartActivity(new Intent(context, to));
        }

        public void OnAutoSave(FileContext file) {
            // クイックモードの場合は、編集したテキストをセットします。
            FileRepo.SaveQuickModeText(TextField.Text);
        }
    }
}
