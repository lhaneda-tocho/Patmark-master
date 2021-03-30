
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
    public class SaveOverDialogFragment : DialogFragment
    {
        public string FileName { get; }

        /// <summary>
        /// 上書き保存がリクエストされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public Action OnRequestedSaveOver;

        /// <summary>
        /// キャンセルされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRequestedCancel;

        public SaveOverDialogFragment(string fileName)
        {
            if (fileName == null)
                throw new NullReferenceException();
            FileName = fileName;
        }

        public SaveOverDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SaveOverDialogFragment()
        {
        }


        public static SaveOverDialogFragment Create(string fileName) {
            return new SaveOverDialogFragment(fileName);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;
            var message = new StringBuilder();
            message.AppendLine(FileName);
            message.Append(GetString(Resource.String.message_saveOverCheck));

            return new AlertDialog.Builder(Activity)
                                  .SetTitle(Resource.String.label_saveOverCheck)
                                  //.SetMessage(FileName)
                                  //.SetMessage(Resource.String.message_saveOverCheck)
                                  .SetMessage(message.ToString())
                                  .SetNegativeButton(Resource.String.cancel      , OnClickNoButton)
                                  .SetPositiveButton(Resource.String.save, OnClickSaveOverButton)
                                  .Create();
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


        #region callbacks
        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            Console.WriteLine("OnCancel");
            OnRequestedCancel?.Invoke();
        }
        void OnClickNoButton(object sender, DialogClickEventArgs ev)
        {
            Console.WriteLine("OnClickNoButton");
            OnRequestedCancel?.Invoke();
        }
        void OnClickSaveOverButton(object sender, DialogClickEventArgs ev)
        {
            Console.Write("OnClickSaveOverButton: ");
            Console.WriteLine(FileName);
            OnRequestedSaveOver?.Invoke();
        }
        #endregion

    }
}
