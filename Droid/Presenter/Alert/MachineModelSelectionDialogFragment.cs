using System;
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
using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.Droid.Presenter.Alert
{
    public class MachineModelSelectionDialogFragment : DialogFragment
    {
        // ArrayAdapterに挿入するオブジェクト
        class MyListModel
        {
            // 保持するインスタンス
            public PatmarkMachineModel Spec { get; }

            // コンストラクタ
            public MyListModel(PatmarkMachineModel spec)
            {
                Spec = spec;
            }

            // リストの要素の名前となる
            public override string ToString()
            {
                return Spec.LocalizedName(Application.Context);
            }
        }

        public string Title { get; }
        public string Message { get; }

        List<MyListModel> Items => (
            from spec in PatmarkMachineModel.SpecList
           select new MyListModel(spec)
        ).ToList();

        /// <summary>
        /// 行がタップされ，モデル番号が確定したとき.
        /// </summary>
        public event Action<PatmarkMachineModel> OnClickRow;

        /// <summary>
        /// キャンセルされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRequestedCancel;


        /// <summary>
        /// プログレスオーバーレイを表示を制御するコントローラです。
        /// </summary>
        ProgressViewControllerAndroid ProgressViewController { get; set; }

        public MachineModelSelectionDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MachineModelSelectionDialogFragment(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public MachineModelSelectionDialogFragment(Context context)
        {
            Title = context.Resources.GetString(Resource.String.label_machine_model_no_title);
            Message = context.Resources.GetString(Resource.String.label_machine_model_no_message);;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;

            // ====

            var view = new ListView(Activity);
            view.LayoutParameters = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MatchParent,
                FrameLayout.LayoutParams.WrapContent
            );
            var adapter = new ArrayAdapter<MyListModel>(Activity, Android.Resource.Layout.SimpleListItem1, Items);
            view.Adapter = adapter;
            view.ItemClick += (sender, e) => {
                var item = adapter.GetItem(e.Position);
                Dismiss();
                OnClickRow?.Invoke(item.Spec);
            };

            // ====

            var overlay = new LoadingOverlayView(Activity);
            overlay.LayoutParameters = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MatchParent,
                FrameLayout.LayoutParams.MatchParent
            );
            overlay.Visibility = ViewStates.Gone;

            // ====

            var layout = new FrameLayout(Activity);
            layout.LayoutParameters = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MatchParent,
                FrameLayout.LayoutParams.WrapContent
            );
            layout.AddView(view);
            layout.AddView(overlay);

            // ====

            var dialog = new AlertDialog.Builder(Activity)
                                  .SetTitle(Title)
                                  .SetMessage(Message)
                                  .SetView(layout)
                                  .Create();
            
            dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // この時点でサブコントローラを初期化する。
            ProgressViewController = new ProgressViewControllerAndroid(overlay);
            ProgressViewController.OnCreate();

            return dialog;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnResume()
        {
            base.OnResume();
            ProgressViewController.OnResume();
        }

        public override void OnPause()
        {
            ProgressViewController.OnPause();
            base.OnPause();
        }

        public override void OnDestroy()
        {
            ProgressViewController.OnDestroy();
            base.OnDestroy();
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            Console.WriteLine("OnCancel");
            OnRequestedCancel?.Invoke();

        }
        public void OnClickCancel(object sender, DialogClickEventArgs e)
        {
            Console.WriteLine("OnClickCancel");
            OnRequestedCancel?.Invoke();
        }

        public void ShowOn(Activity activity, string fragmentTag)
        {
            var mng = activity.FragmentManager;
            Show(mng, fragmentTag);
        }

        public static FileActionDialogFragment Create(string title)
        {
            return new FileActionDialogFragment(title);
        }
    }
}