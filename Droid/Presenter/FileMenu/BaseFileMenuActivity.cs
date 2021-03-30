

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
using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.Patmark.Presenter.Embossment;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    [Activity]
    public abstract class BaseFileMenuActivity<T> : Activity
    {
        public ArrayAdapter<string> Adapter{ get; private set; }

        public IBaseFileMenuDialogManager<T> TheDialogManager { get; private set; }
        public ListView                      TheListView      { get; private set; }
        public Button                        TheAddButton     { get; private set; }
        public LoadingOverlayView            TheOverlayFrame  { get; private set; }

        public abstract EmbossmentFileRepository TheFileRepository { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Class Init
            TheDialogManager = OnCreateDialogManager() ?? throw new NullReferenceException();
            OnSetupRepository();

            // view init
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.FileMenu);

            Adapter = OnCreateAdapter();

            // Create your application here
            TheListView = FindViewById<ListView>(Resource.Id.listView_fileList);
            TheOverlayFrame = FindViewById<LoadingOverlayView>(Resource.Id.progressBarView);
            InitViewsOnCreate();
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateFileList();
        }

        void InitViewsOnCreate()
        {
            {
                // FIXME: Loadに対応
                var adapter = Adapter;
                TheListView.Adapter = adapter;

                TheListView.ItemClick += (sender, ev) =>
                {
                    var item = ev.Position;
                    OnClickItem(item);
                };
            }
        }


        #region ui callbacks
        protected void OnClickSaveAs()
        {
            if (TheFileRepository.IsEmpty)
            {
                OnEmptyError();
                return;
            }
            TheDialogManager.ShowSaveAsDialog();
        }

        protected void OnClickItem(int index)
        {
            Console.WriteLine($"item={index}");
            T name;
            if(GetNameOrNull(index, out name)){
                TheDialogManager.ShowActionSheet(index, name);
            }
        }
        #endregion




        #region Menu Callback
        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnMenuItemSelected(featureId, item);
            }
        }

        #endregion



        #region request callbacks

        protected abstract IBaseFileMenuDialogManager<T> OnCreateDialogManager();
        protected abstract ArrayAdapter<string> OnCreateAdapter();


        protected abstract bool GetNameOrNull(int pos, out T data);

        /// <summary>
        /// リスト更新
        /// </summary>
        protected abstract void UpdateFileList();

        protected abstract void OnSetupRepository();

        protected abstract void OnEmptyError();
        #endregion
    }
}
