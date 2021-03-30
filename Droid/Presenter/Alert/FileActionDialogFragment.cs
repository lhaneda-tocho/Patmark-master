
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
    public class FileActionDialogFragment : DialogFragment
    {
        public string Title { get; }
        /// <summary>
        /// Retrieveボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRetrieve;
        /// <summary>
        /// Replaceボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnReplace;
        /// <summary>
        /// Renameボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRename;
        /// <summary>
        /// Clearボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnClear;

        /// <summary>
        /// キャンセルされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRequestedCancel;


        string[] Items => new string[] { 
            GetString(Resource.String.label_retrieve),
            GetString(Resource.String.label_replace),
            GetString(Resource.String.label_rename),
            GetString(Resource.String.label_clear)
        };

        public FileActionDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FileActionDialogFragment()
        {
        }

        public FileActionDialogFragment(string title)
        {
            Title = title;
        }

        private Dialog ForLocal(Bundle savedInstanceState)
        {
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.FileActionList, null);

            var retrieveButton = view.FindViewById<Button>(Resource.Id.actionsheet_retrieve);
            var replaceButton = view.FindViewById<Button>(Resource.Id.actionsheet_replace);
            var renameButton = view.FindViewById<Button>(Resource.Id.actionsheet_rename);
            var clearButton = view.FindViewById<Button>(Resource.Id.actionsheet_clear);

            retrieveButton.Click += OnClickRetrieve;
            replaceButton.Click += OnClickReplace;
            renameButton.Click += OnClickRename;
            clearButton.Click += OnClickClear;

            return new AlertDialog.Builder(Activity)
                                  .SetTitle(Title)
                                  .SetView(view)
                                  .SetNeutralButton(Resource.String.cancel, OnClickCancel)
                                  .Create();
        }

        private Dialog ForRemote(Bundle savedInstanceState)
        {
            
            var view = Activity.LayoutInflater.Inflate(Resource.Layout.FileActionList, null);

            var retrieveButton = view.FindViewById<Button>(Resource.Id.actionsheet_retrieve);
            var replaceButton = view.FindViewById<Button>(Resource.Id.actionsheet_replace);
            var renameButton = view.FindViewById<Button>(Resource.Id.actionsheet_rename);
            var clearButton = view.FindViewById<Button>(Resource.Id.actionsheet_clear);

            retrieveButton.Click += OnClickRetrieve;
            replaceButton.Click += OnClickReplace;
            renameButton.Click += OnClickRename;
            clearButton.Click += OnClickClear;

            return new AlertDialog.Builder(Activity)
                                  .SetTitle(Title)
                                  .SetView(view)
                                  .SetNeutralButton(Resource.String.cancel, OnClickCancel)
                                  .Create();
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;
            return ForLocal(savedInstanceState);
        }

        public override void OnPause()
        {
            base.OnPause();
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

        public void ShowOn(Activity activity)
        {
            var mng = activity.FragmentManager;
            Show(mng, "TAG");
        }

        public static FileActionDialogFragment Create(string title)
        {
            return new FileActionDialogFragment(title);
        }

        void OnClickRetrieve(object sender, EventArgs ev) {
            OnRetrieve?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickReplace(object sender, EventArgs ev) {
            OnReplace?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickRename(object sender, EventArgs ev) {
            OnRename?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickClear(object sender, EventArgs ev) {
            OnClear?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickAnyToClose() {
            Dismiss();
        }
    }


    public class RemoteFileActionDialogFragment : DialogFragment
    {
        public string Title { get; }
        /// <summary>
        /// Retrieveボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRetrieve;
        /// <summary>
        /// Replaceボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnReplace;
        /// <summary>
        /// Renameボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRename;
        /// <summary>
        /// Clearボタンが押された時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnClear;

        /// <summary>
        /// キャンセルされた時.
        /// このイベントが発行された時点でダイアログが閉じられることになります.
        /// </summary>
        public event Action OnRequestedCancel;


        string[] Items => new string[] {
            GetString(Resource.String.label_retrieve),
            GetString(Resource.String.label_replace),
            GetString(Resource.String.label_rename),
            GetString(Resource.String.label_clear)
        };

        public RemoteFileActionDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public RemoteFileActionDialogFragment()
        {
        }

        public RemoteFileActionDialogFragment(string title)
        {
            Title = title;
        }

        public Dialog ForRemote(Bundle savedInstanceState)
        {

            var view = Activity.LayoutInflater.Inflate(Resource.Layout.FileActionList, null);

            var retrieveButton = view.FindViewById<Button>(Resource.Id.actionsheet_retrieve);
            var replaceButton = view.FindViewById<Button>(Resource.Id.actionsheet_replace);
            var renameButton = view.FindViewById<Button>(Resource.Id.actionsheet_rename);
            var clearButton = view.FindViewById<Button>(Resource.Id.actionsheet_clear);
            clearButton.Visibility = ViewStates.Visible;

            retrieveButton.Click += OnClickRetrieve;
            replaceButton.Click += OnClickReplace;
            renameButton.Click += OnClickRename;
            clearButton.Click += OnClickClear;

            return new AlertDialog.Builder(Activity)
                                  .SetTitle(Title)
                                  .SetView(view)
                                  .SetNeutralButton(Resource.String.cancel, OnClickCancel)
                                  .Create();
        }
        
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            RetainInstance = true;
            return ForRemote(savedInstanceState);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override void OnPause()
        {
            base.OnPause();
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

        public void ShowOn(Activity activity)
        {
            var mng = activity.FragmentManager;
            Show(mng, "TAG");
        }

        public static RemoteFileActionDialogFragment Create(string title)
        {
            return new RemoteFileActionDialogFragment(title);
        }

        void OnClickRetrieve(object sender, EventArgs ev)
        {
            OnRetrieve?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickReplace(object sender, EventArgs ev)
        {
            OnReplace?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickRename(object sender, EventArgs ev)
        {
            OnRename?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickClear(object sender, EventArgs ev)
        {
            OnClear?.Invoke();
            OnClickAnyToClose();
        }

        void OnClickAnyToClose()
        {
            Dismiss();
        }
    }
}
