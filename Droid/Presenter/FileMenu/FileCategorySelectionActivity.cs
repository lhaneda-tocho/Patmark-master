
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.Patmark.Presenter.FileMenu;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    [Activity(Label = "@string/fileCategorySelectionActivity_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Nosensor, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class FileCategorySelectionActivity : Activity
    {
        void HandleAction()
        {

        }

        public RemoteFileRepository Repo { get; private set; }
        public ListView TheCategoryListView { get; private set; }

        string[] CategoryArray => new string[] {
            GetString(Resource.String.label_file_menu_latest),
            GetString(Resource.String.label_file_menu_remote),
            GetString(Resource.String.label_file_menu_local),
        };

        Action[] ActionArray => new Action[] {
            OnSelectedLatest,
            OnSelectedRemote,
            OnSelectedLocal
        };

        public LoadingOverlayView TheOverlayFrame { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.FileCategorySelection);

            // retreive
            TheCategoryListView = FindViewById<ListView>(Resource.Id.listView_fileCategory);
            TheOverlayFrame = FindViewById<LoadingOverlayView>(Resource.Id.progressBarView);

            Repo = new RemoteFileRepository();

            InitViewsOnCreate();
        }

        void InitViewsOnCreate()
        {
            {
                var adapter   = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, CategoryArray);
                var callbacks = ActionArray;
                TheCategoryListView.Adapter = adapter;
                TheCategoryListView.ItemClick += (sender, ev) =>
                {
                    var item = ev.Position;
                    System.Console.WriteLine($"item={item}");
                    ActionArray[item]();
                };
            }
        }



        #region callbacks

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

        void OnSelectedLatest() {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(()=>TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var res = await Repo.OnRetrieveLatest();
                        Application.SynchronizationContext.Post(_ =>
                        {
                            if (res)
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_read), ToastLength.Long).Show();
                                Finish();
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_file_is_empty), ToastLength.Long).Show();
                            }
                        }, null);
                    }
                    catch (ControllerIO.CommunicationProtectedException ex)
                    {
                        Application.SynchronizationContext.Post(_ =>
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.toast_communication_protected), ToastLength.Long).Show();
                        }, null);
                    }
                    catch (Exception)
                    {
                        Application.SynchronizationContext.Post(_ =>
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.toast_communication_timed_out), ToastLength.Long).Show();
                        }, null);
                    }
                }));
            }
            else
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_communication_required), ToastLength.Long).Show();
            }
            Console.WriteLine("OnSelectedLatest");
        }

        void OnSelectedRemote() {
            Console.WriteLine("OnSelectedRemote");
            if (CommunicationClient.Instance.Ready)
            {
                StartActivity(new Intent(this, typeof(RemoteFileMenuActivity)));
            }else
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.toast_communication_required), ToastLength.Long).Show();
            }
        }

        void OnSelectedLocal()
        {
            Console.WriteLine("OnSelectedLocal");
            StartActivity(new Intent(this, typeof(LocalFileMenuActivity)));
        }

        #endregion
    }
}
