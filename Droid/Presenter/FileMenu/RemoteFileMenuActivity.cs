
using System;
using System.Threading.Tasks;
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
using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.Presenter.Embossment;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    [Activity(Label = "@string/remoteFileMenuActivity_name", ScreenOrientation = Android.Content.PM.ScreenOrientation.Nosensor, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class RemoteFileMenuActivity : BaseFileMenuActivity<RemoteFileInfo>, RemoteFileMenuDialogManager.IListener
    {
        public RemoteFileRepository  Repo { get; private set; }
        public RemoteFileInfoAdapter AdapterManager { get; private set; }

        public override EmbossmentFileRepository TheFileRepository => Repo;

        #region request callbacks

        protected override IBaseFileMenuDialogManager<RemoteFileInfo> OnCreateDialogManager()
        {
            return new RemoteFileMenuDialogManager(this, this);
        }

        protected override void OnSetupRepository()
        {
            Repo = new RemoteFileRepository();
        }

        protected override ArrayAdapter<string> OnCreateAdapter()
        {
            AdapterManager = new RemoteFileInfoAdapter(this);
            return AdapterManager.Adapter;
        }

        protected override bool GetNameOrNull(int pos, out RemoteFileInfo data)
        {
            var info = AdapterManager.GetPathNameOrNull(pos);
            if (info == null)
            {
                data = null;
                return false;
            }
            data = info;
            return true;
        }

        protected async Task<List<RemoteFileInfo>> OnRetrieveFileList()
        {
            return await Repo.FileList();
        }

        protected override void UpdateFileList()
        {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(()=>TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var list = await OnRetrieveFileList();
                        Application.SynchronizationContext.Post(_ => { // async不可
                            AdapterManager.UpdateWithList(list);
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
        }
        public void OnRequestedRetrieve(int index, RemoteFileInfo name)
        {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(() => TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var success = await Repo.OnRetrieve(index, name);
                        Application.SynchronizationContext.Post(_ => {
                            if (success)
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
        }

        public void OnRequestedReplace(int index, RemoteFileInfo name)
        {
            Console.WriteLine($"OnRequestedReplace: {name.NumOfField}, {name.Name}");
            var newName = name.NumOfField > 0 ? name.Name : "";

            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(() => TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var success = (await Repo.OnReplace(index, onEmpty: () => OnEmptyError())) && (await Repo.OnRename(index, newName));
                        Application.SynchronizationContext.Post(async _ => {
                            if (success)
                            {
                                AdapterManager.UpdateWithList(await Repo.FileList());
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_saved), ToastLength.Long).Show();
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_failed_to_save), ToastLength.Long).Show();
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
        }

        public void OnRequestedRename(int index, RemoteFileInfo name, string next)
        {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(()=>TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var success = await Repo.OnRename(index, next);
                        var fileName = Repo.ToFileName(index, next);
                        Application.SynchronizationContext.Post(_ => {
                            if (success)
                            {
                                AdapterManager.Update(index, new RemoteFileInfo(fileName, name.NumOfField));
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_edited), ToastLength.Long).Show();
                            }else{
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_failed_to_edit), ToastLength.Long).Show();
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
        }

        public void OnReceiveInvalidFileName(string text)
        {
            Toast.MakeText(this, Resource.String.toast_invalidFileName, ToastLength.Short).Show();
        }

        public void OnClickReload()
        {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(()=>TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        await Repo.OnReload();
                        Application.SynchronizationContext.Post(_ =>
                        {
                            UpdateFileList();
                            Toast.MakeText(this, Resources.GetString(Resource.String.toast_read), ToastLength.Long).Show();
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
        }

        public void OnRequestedClear(int index, RemoteFileInfo name)
        {
            if (CommunicationClient.Instance.Ready)
            {
                Task.Run(() => TheOverlayFrame.ShowWhileProcessing(async () =>
                {
                    try
                    {
                        var success = await Repo.OnClear(index);
                        Application.SynchronizationContext.Post(_ =>
                        {
                            UpdateFileList();
                            if(success)
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_cleared), ToastLength.Long).Show();
                            else
                                Toast.MakeText(this, Resources.GetString(Resource.String.toast_failed_to_clear), ToastLength.Long).Show();
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
        }

        protected override void OnEmptyError()
        {
            PatmarkApplication.RunOnMain(() =>
            {
                Toast.MakeText(this, Resource.String.toast_file_is_empty, ToastLength.Short).Show();
            });
        }

        #endregion



        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.RemoteReloadMenu, menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menuButton_reload)
            {
                OnClickReload();
                return true;
            }
            else
                return false;
        }
    }
}
