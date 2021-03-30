
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Custom
{
    public class ProgressDialogFragment : DialogFragment
    {
        const string BundleKey = "ProgressDialogFragmentBundleKey";
        const string PromptDefault = "Wait...";

        public string Prompt { get; set; } = PromptDefault;
        public int?   PromptResourceID { get; set; } = Resource.String.overlay_now_processing;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            if (savedInstanceState != null)
                UnpackMyState(savedInstanceState.GetBundle(BundleKey));
            else if(PromptResourceID != null)
                Prompt = GetString((int)PromptResourceID);
            
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.ProgressDialog, null);
            var textView = view.FindViewById(Resource.Id.ProgressDialogPrompt) as TextView;
            if(textView != null) {
                textView.Text = Prompt;
            }
            return
                new AlertDialog.Builder(Activity)
                               .SetView(view)
                               .Create();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBundle(BundleKey, PackMyState());
            base.OnSaveInstanceState(outState);
        }

        Bundle PackMyState() {
            Bundle b = new Bundle();
            b.PutString("Prompt", Prompt);
            if(PromptResourceID!=null)
                b.PutInt("PromptResourceID", (int)PromptResourceID);
            return b;
        }

        void UnpackMyState(Bundle b) {
            if(b.ContainsKey("PromptResourceID")) {
                // id 利用
                int resID= b.GetInt("PromptResourceID");
                Prompt = Context.GetString(resID);
                PromptResourceID = resID;
            } else {
                // id がない場合
                Prompt = b.GetString(PromptDefault) ?? PromptDefault;
            }
        }

    }

    public static class ProgressDialogManager {
        static object theLock = new object();


        public static Task ShowAndProcessOnBackground(string fragmentTag, string prompt, Func<Task> task)
        {
            bool finishTask = false;
            // 表示の予約
            Task<ProgressDialogFragment> showTask = PatmarkApplication.FragmentAccessHandler.Post(() =>  // フラグメント操作開始
            {
                if (finishTask)
                    return null;
                var activity = PatmarkApplication.CurrentActivity;
                var f = new ProgressDialogFragment();
                f.Cancelable = false;
                if(prompt != null)
                {
                    f.Prompt = prompt;
                    f.PromptResourceID = null;
                }
                f.Show(activity.FragmentManager, fragmentTag);
                return f;
            });

            Func<Task> SafeFinish = async () =>
            {
                finishTask = true;         // 終わったことを通知する.
                var frag = await showTask; // 実行されるまで待つ.
                if (frag == null)
                {
                    return;                // フラグメントが生成されていない場合は無視
                }
                else
                {
                    await PatmarkApplication.FragmentAccessHandler.Post(() => {  // フラグメント操作開始
                        frag.Dismiss();    // フラグメントが生成されていた場合は消す.
                    });
                    return;
                }
            };

            return ProgressAPIAndroid.LaunchUITaskAsync(async (taskHandler) =>
            {
                try
                {
                    // Thread pool で長いタスクを実行
                    await Task.Run(task);
                }
                finally
                {
                    // 終わったら終了通知
                    await SafeFinish();
                }
            });
        }

        public static Task ShowAndProcessOnBackground(string fragmentTag, Func<Task> task)
        {
            return ShowAndProcessOnBackground(fragmentTag, null, task);
        }
    }
}
