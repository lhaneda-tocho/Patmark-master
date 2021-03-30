
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
    public class RenameFragment : DialogFragment
    {
        public class FileName {
            public string Text { get; }
            public FileName(string text)
            {
                Text = text;
            }
        }

        public RenameFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public RenameFragment()
        {
        }

        public string ErrorMessage { get; private set; } = "";
        public string PreviousText { get; private set; } = null;

        public string TargetFileName { get; }

        /// <summary>
        /// 保存ボタンが押され,保存のリクエストがきた場合.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action<FileName> OnRequestedRename;

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

            if(PreviousText == null) {
                textField.Text = TargetFileName;
            } else {
                textField.Text = PreviousText;
            }

            unchecked {
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    textField.Background.Mutate();
                    textField.Background.SetTint((int)0xFFFF0000);
                }
            }

            var alert = new AlertDialog.Builder(Activity)
                                       .SetTitle(GetString(Resource.String.label_rename))
                                       .SetMessage(String.Format("{0}{1}{2}", GetString(Resource.String.message_editName), System.Environment.NewLine, ErrorMessage))
                                      .SetView(view)
                                      .SetNegativeButton(Resource.String.cancel, OnClickCancelButton)
                                      .SetPositiveButton(Resource.String.save  , (sender,ev)=>OnClickRenameButton(sender,ev,textField) )
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

        public RenameFragment(string targetFileName)
        {
            TargetFileName = targetFileName;
        }

        public static RenameFragment Create(string targetFileName, string errorMessage = "", string previousName = null)
        {
            var ins = new RenameFragment(targetFileName);
            ins.ErrorMessage = errorMessage;
            ins.PreviousText = previousName;
            return ins;
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

        void OnClickRenameButton(object sender, DialogClickEventArgs ev, EditText editText)
        {
            Console.Write("OnClickRenameButton: ");
            Console.WriteLine(editText.Text);
            var text = editText.Text;
            OnRequestedRename?.Invoke(new FileName(text));
        }
        #endregion
    }
}
