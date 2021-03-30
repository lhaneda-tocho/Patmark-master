using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.iOS.Presenter.Loading;
using TokyoChokoku.Communication;


namespace TokyoChokoku.Patmark.iOS
{
    using Presenter;

    public partial class FileMenuController : UITableViewController
    {
        public FileContext Loaded;
        public FileManagementActions Actions;

        public FileMenuController (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        override
        public void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "ctrl-files.title".Localize();

            if(Loaded == null || Actions == null){
                throw new NullReferenceException("LoadedとActionsをセットしてください。");
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            var selected = tableView.CellAt(indexPath);
            if(selected == BtnLatest){
                if (CommunicationClient.Instance.Ready)
                {
                    InvokeOnMainThread(async () => { 
                        await LoadingOverlay.ShowWithTask(async () =>
                        {
                            try
                            {
                                var manager = new LatestFileManager(Loaded, Actions);
                                if (await manager.Load())
                                {
                                    InvokeOnMainThread(() =>
                                    {
                                        MyToast.ShowMessage("Read successfully.", duration: MyToast.Short);
                                        NavigationController.PopViewController(true);
                                    });
                                }
                                else
                                {
                                    InvokeOnMainThread(() =>
                                    {
                                        MyToast.ShowMessage("File is empty.", duration: MyToast.Short);
                                        NavigationController.PopViewController(true);
                                    });
                                }
                            }
                            catch (ControllerIO.CommunicationProtectedException ex)
                            {
                                InvokeOnMainThread(() =>
                                {
                                    MyToast.ShowMessage("Protected to communicate.", duration: MyToast.Long);
                                });
                            }
                            catch(Exception){
                                InvokeOnMainThread(() =>
                                {
                                    MyToast.ShowMessage("There is no response from the Patmark. Check the connection.", duration: MyToast.Long);
                                });
                            }
                        });
                    });
                }
                else{
                    MyToast.ShowMessage("communication.required", duration: MyToast.Short);
                }
            }
            else if (selected == BtnRemote)
            {
                if (CommunicationClient.Instance.Ready)
                {
                    //
                    var storyboard = UIStoryboard.FromName("RemoteFile", NSBundle.MainBundle);
                    var ctrl = storyboard.InstantiateInitialViewController() as RemoteFileMenuController;
                    ctrl.Source = new RemoteFileManager(Loaded, Actions);
                    NavigationController.PushViewController(ctrl, true);
                }
                else
                {
                    MyToast.ShowMessage("communication.required", duration: MyToast.Short);
                }
            }
            else if (selected == BtnLocal)
            {
                //
                var storyboard = UIStoryboard.FromName("LocalFile", NSBundle.MainBundle);
                var ctrl = storyboard.InstantiateInitialViewController() as LocalFileMenuController;
                ctrl.Source = new LocalFileManager(Loaded, Actions);
                NavigationController.PushViewController(ctrl, true);
            }
        }
    }
}