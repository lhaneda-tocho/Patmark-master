
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

namespace TokyoChokoku.Patmark.Droid.Presenter.Alert
{
    public class SaveAsFragment : DialogFragment
    {
        public class FileName {
            public string Text { get; }
            public FileName(string text)
            {
                Text = text;
            }
        }

        public SaveAsFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SaveAsFragment()
        {
        }

        /// <summary>
        /// 保存ボタンが押され,保存のリクエストがきた場合.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action<FileName> OnRequestedSave;

        /// <summary>
        /// キャンセルされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action           OnRequestedCancel;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.SaveAsDialog, null);
            var textField = view.FindViewById<EditText>(Resource.Id.saveAsDialog_textField);

            var alert = new AlertDialog.Builder(Activity)
                                  .SetTitle(GetString(Resource.String.label_saveas))
                                  .SetMessage(GetString(Resource.String.message_saveas))
                                  .SetView(view)
                                  .SetNegativeButton(Resource.String.cancel, OnClickCancelButton)
                                  .SetPositiveButton(Resource.String.save  , (sender,ev)=>OnClickSaveButton(sender,ev,textField) )
                                  .Create();
            this.Cancelable = false;
            return alert;
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public void ShowOn(Activity activity)
        {
            var mng = activity.FragmentManager;
            Show(mng, "TAG");
        }

        public static SaveAsFragment Create()
        {
            return new SaveAsFragment();
        }

        #region callbacks
        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            Console.WriteLine("OnCancel");
            OnRequestedCancel?.Invoke();
        }

        void OnClickCancelButton(object sender, DialogClickEventArgs ev)
        {
            Console.WriteLine("OnClickCancelButton");
            OnRequestedCancel?.Invoke();
        }

        void OnClickSaveButton(object sender, DialogClickEventArgs ev, EditText editText)
        {
            Console.Write("OnClickSaveButton: ");
            Console.WriteLine(editText.Text);
            var text = editText.Text;
            OnRequestedSave?.Invoke(new FileName(text));
        }
        #endregion
    }
}
