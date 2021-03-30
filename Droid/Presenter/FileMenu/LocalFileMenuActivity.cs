
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

using TokyoChokoku.Patmark.Presenter.FileMenu;
using TokyoChokoku.Patmark.Droid.Presenter.Alert;
using TokyoChokoku.Patmark.Presenter.Embossment;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    [Activity(Label = "@string/localFileMenuActivity_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Nosensor, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class LocalFileMenuActivity : BaseFileMenuActivity<PathName>, LocalFileMenuDialogManager.IListener
    {
        public PathNameAdapterManager AdapterManager { get; private set; }
        public LocalFileRepository Repo { get; private set; }

        public override EmbossmentFileRepository TheFileRepository => Repo;

        #region request callbacks
        protected override IBaseFileMenuDialogManager<PathName> OnCreateDialogManager()
        {
            return new LocalFileMenuDialogManager(this, this);
        }

        protected override void OnSetupRepository()
        {
            Repo = new LocalFileRepository();
        }

        protected override bool GetNameOrNull(int pos, out PathName data)
        {
            var name = AdapterManager.GetPathNameOrNull(pos);
            if (name == null)
                return false;
            
            data = (PathName) name;
            return true;
        }

        protected override ArrayAdapter<string> OnCreateAdapter()
        {
            AdapterManager = new PathNameAdapterManager(this);
            return AdapterManager.Adapter;
        }

        /// <summary>
        /// リスト更新
        /// </summary>
        protected override void UpdateFileList()
        {
            var list = OnRetrieveFileList();
            AdapterManager.UpdateWithList(list);
        }


        protected List<PathName> OnRetrieveFileList()
        {
            return Repo.FileList();
        }


        public void OnRequestedSaveAs(string text) {
            var success = Repo.OnSaveAsWithText(text,
                                  onInvalidName: ()=>OnReceiveInvalidFileName(text), 
                                  onSaveOver   : (path)=>TheDialogManager.ShowSaveOverDialog(0, path), // 番号は無視される
                                  onEmpty      : ()=>OnEmptyError(),
                                  onImcompatibleSerial: ()=>OnImcompatibleSerial()
            ); 
            if(success) {
                UpdateFileList();
            }
        }

        public void OnRequestedRetrieve(PathName name)
        {
            Repo.OnLoad(name);
            Finish();
        }

        public void OnRequestedReplace(PathName name)
        {
            var success = Repo.OnSaveOver(name,
                onEmpty: ()=>OnEmptyError(),
                onImcompatibleSerial: () => OnImcompatibleSerial()

            );
            if(success)
                UpdateFileList();
        }

        public void OnRequestedRename(PathName name, string next)
        {
            var success = Repo.OnRenameWithText(
                name,
                next,
                () => OnReceiveInvalidFileName(next),
                () => OnReceiveSameFileName(),
                (targetPath, nextPath) => TheDialogManager.ShowRenameOverDialog(targetPath, nextPath)
            );
            if (success)
            {
                UpdateFileList();
            }
        }

        public void OnRequestedRenameOver(PathName name, PathName next){
            Repo.OnRenameOver(name, next);
            UpdateFileList();
        }

        public void OnRequestedClear(PathName name)
        {
            var success = Repo.OnDelete(name);

            if (success)
            {
                UpdateFileList();
            }
        }

        void OnImcompatibleSerial()
        {
            Toast.MakeText(this, Resource.String.msg_serialField_writeFileSelectorLocal, ToastLength.Long).Show();
        }

        public void OnReceiveInvalidFileName(string text)
        {
            Toast.MakeText(this, Resource.String.toast_invalidFileName, ToastLength.Short).Show();
        }

        protected override void OnEmptyError()
        {
            Toast.MakeText(this, Resource.String.toast_file_is_empty, ToastLength.Short).Show();
        }

        public void OnReceiveSameFileName()
        {
            // 何もしません。
        }

        #endregion




        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.FileAddMenu, menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menuButton_add)
            {
                OnClickSaveAs();
                return true;
            }
            else
                return false;
        }
    }


    
}
