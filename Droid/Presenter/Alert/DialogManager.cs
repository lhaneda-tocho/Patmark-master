
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
    /// <summary>
    /// ボタン連打でダイアログが複数出現してしまう問題の対策用クラス
    /// </summary>
    public class DialogManager
    {
        //object TheLock = new object();

        /// <summary>
        /// 最後に表示されたダイアログ
        /// </summary>
        /// <value>The current dialog.</value>
        public DialogFragment CurrentDialog { get; private set; }

        /// <summary>
        /// ダイアログの表示を行い，登録を行います.
        /// </summary>
        /// <returns><c>true</c> の時 表示に成功した. <c>false</c> の場合は他のダイアログが表示されていて表示ができない場合.</returns>
        public bool StartShowing(Func<DialogFragment> dialogSupplier) {
            if (CurrentDialog != null) {
                var dialog = CurrentDialog.Dialog;
                if (dialog != null && dialog.IsShowing)
                    return false;
            }
            CurrentDialog = dialogSupplier();
            return true;
        }

        /// <summary>
        /// ダイアログの登録を解除し, 他のダイアログが表示できるようにします.
        /// </summary>
        public void EndShowing() {
            CurrentDialog = null;
        }
    }
}
