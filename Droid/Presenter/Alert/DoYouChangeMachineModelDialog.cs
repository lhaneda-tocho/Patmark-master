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
    public class DoYouChangeMachineModelDialog : DialogFragment
    {
        /// <summary>
        /// Fitボタンを押した時に呼ばれるコールバック
        /// </summary>
        public event Action OnClickFitButton;

        public DoYouChangeMachineModelDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DoYouChangeMachineModelDialog()
        {
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;
            var alert = new AlertDialog
                .Builder(Activity)      
                .SetTitle(GetString(Resource.String.label_changeOfMachineModel)) 
                .SetMessage(GetString(Resource.String.label_doYouChangeMachineModel)) 
                .SetNegativeButton(Resource.String.cancel, OnClickCancelButton)
                .SetPositiveButton(Resource.String.apply, (sender, ev) => OnClickFitButton?.Invoke())
                .Create();
            this.Cancelable = true;
            return alert;
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public void ShowOn(Activity activity)
        {
            var mng = activity.FragmentManager;
            Show(mng, "DoYouChangeMachineModelDialog");
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
        }

        void OnClickCancelButton(object sender, DialogClickEventArgs ev)
        {
            Console.WriteLine("OnClickCancelButton");
        }
        #endregion
    }
}
