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
using TokyoChokoku.Patmark.AvailableCharacters;

namespace TokyoChokoku.Patmark.Droid.Presenter.FileMenu
{
    public interface IBaseFileMenuDialogManager<FileName>
    {
        void ShowEditTextDialog(int index, FileName target);
        void ShowSaveAsDialog  (               );
        void ShowSaveOverDialog(int index, FileName text);
        void ShowRenameOverDialog(PathName target, PathName next);
        void ShowActionSheet   (int index, FileName name  );
    }

    /// <summary>
    /// Local file menu dialog manager.
    /// </summary>
    public class LocalFileMenuDialogManager: IBaseFileMenuDialogManager<PathName>
    {
        public interface IListener {
            void OnRequestedSaveAs(string text);

            void OnRequestedRetrieve(PathName name);

            void OnRequestedReplace(PathName name);

            void OnRequestedRename(PathName name, string next);

            void OnRequestedRenameOver(PathName name, PathName next);

            void OnRequestedClear(PathName name);

            void OnReceiveInvalidFileName(string text);
        }

        public DialogManager TheDialogManager { get; } = new DialogManager();
        public Activity      TheContext {get;}

        public IListener Listener { get; }

        public LocalFileMenuDialogManager(Activity theContext, IListener listener)
        {
            TheContext = theContext ?? throw new NullReferenceException();
            Listener = listener;
        }

        /// <summary>
        /// Shows the save as dialog.
        /// </summary>
        public void ShowEditTextDialog(int index, PathName target)
        {
            TheDialogManager.StartShowing(() => {
                var frag = RenameFragment.Create(target.Simple);

                frag.OnRequestedRename += (file) => {
                    TheDialogManager.EndShowing(); // 先に呼ばないと コールバック内で ダイアログを表示できなくなる.
                    Listener?.OnRequestedRename(target, file.Text);
                };

                frag.OnRequestedCancel += () =>
                {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the save as dialog.
        /// </summary>
        public void ShowSaveAsDialog()
        {
            TheDialogManager.StartShowing(() => {
                var frag = SaveAsFragment.Create();

                frag.OnRequestedSave += (file) => {
                    TheDialogManager.EndShowing(); // 先に呼ばないと コールバック内で ダイアログを表示できなくなる.
                    Listener?.OnRequestedSaveAs(file.Text);
                };

                frag.OnRequestedCancel += () =>
                {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the save over dialog.
        /// </summary>
        public void ShowSaveOverDialog(int index, PathName text)
        {
            TheDialogManager.StartShowing(() => {
                var frag = SaveOverDialogFragment.Create(text.Simple);

                frag.OnRequestedSaveOver += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedReplace(text);
                };

                frag.OnRequestedCancel += () => {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the rename over dialog.
        /// </summary>
        public void ShowRenameOverDialog(PathName target, PathName next)
        {
            TheDialogManager.StartShowing(() => {
                var frag = SaveOverDialogFragment.Create(next.Simple);

                frag.OnRequestedSaveOver += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedRenameOver(target, next);
                };

                frag.OnRequestedCancel += () => {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the action sheet.
        /// </summary>
        /// <param name="name">Name.</param>
        public void ShowActionSheet(int index, PathName name)
        {
            TheDialogManager.StartShowing(() => {
                var frag = FileActionDialogFragment.Create(name.Simple);

                frag.OnRetrieve += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedRetrieve(name);
                };
                frag.OnReplace += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedReplace(name);
                };
                frag.OnRename += () =>
                {
                    TheDialogManager.EndShowing();
                    ShowEditTextDialog(index, name);
                };
                frag.OnClear += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedClear(name);
                };

                frag.OnRequestedCancel += () => {
                    TheDialogManager.EndShowing();
                };
                frag.ShowOn(TheContext);

                return frag;
            });
        }
    }



    /// <summary>
    /// Remote file menu dialog manager.
    /// </summary>
    public class RemoteFileMenuDialogManager : IBaseFileMenuDialogManager<RemoteFileInfo>
    {
        public interface IListener
        {
            void OnRequestedRetrieve(int index, RemoteFileInfo name);

            void OnRequestedReplace(int index, RemoteFileInfo name);

            void OnRequestedRename(int index, RemoteFileInfo name, string next);

            void OnRequestedClear(int index, RemoteFileInfo name);

            void OnReceiveInvalidFileName(string text);
        }

        public DialogManager TheDialogManager { get; } = new DialogManager();
        public Activity TheContext { get; }

        public IListener Listener { get; }

        public RemoteFileMenuDialogManager(Activity theContext, IListener listener)
        {
            TheContext = theContext ?? throw new NullReferenceException();
            Listener = listener;
        }

        public void ShowEditTextDialog(int index, RemoteFileInfo target)
        {
            ShowEditTextDialog(index, target, "");
        }

        /// <summary>
        /// Shows the save as dialog.
        /// </summary>
        public void ShowEditTextDialog(int index, RemoteFileInfo target, string errorMessage, string previousText = null)
        {
            TheDialogManager.StartShowing(() => {
                var frag = RenameFragment.Create(target.Name, errorMessage, previousText);

                frag.OnRequestedRename += (file) => {
                    TheDialogManager.EndShowing(); // 先に呼ばないと コールバック内で ダイアログを表示できなくなる. (また，iOSと処理を合わせるためでもある)
                    var newText = file.Text;
                    if (newText != null)
                    {
                        newText = newText.Replace("\0", "");
                    }

                    if (AvailableRemoteFileNameChar.IsValid(newText)) {
                        Listener?.OnRequestedRename(index, target, newText);
                    } else {
                        var errMsg = AvailableRemoteFileNameChar.ToStringAvailableChar(
                            TheContext.GetString(Resource.String.RemoteFileMenuDialogManager_ErrorMessage),
                            TheContext.GetString(Resource.String.RemoteFileMenuDialogManager_Space)
                        );
                        ShowEditTextDialog(index, target, errMsg, newText);
                    }
                };

                frag.OnRequestedCancel += () =>
                {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the save as dialog.
        /// </summary>
        public void ShowSaveAsDialog()
        {
            
        }

        /// <summary>
        /// Shows the save over dialog.
        /// </summary>
        public void ShowSaveOverDialog(int index, RemoteFileInfo text)
        {
            TheDialogManager.StartShowing(() => {
                var frag = SaveOverDialogFragment.Create(text.Name);

                frag.OnRequestedSaveOver += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedReplace(index, text);
                };

                frag.OnRequestedCancel += () => {
                    TheDialogManager.EndShowing();
                };

                frag.ShowOn(TheContext);

                return frag;
            });
        }

        /// <summary>
        /// Shows the rename over dialog.
        /// </summary>
        public void ShowRenameOverDialog(PathName target, PathName next)
        {
            throw new NotImplementedException("RemoteFileMenuDialogManager.ShowRenameOverDialog を実装してください。");
        }

        /// <summary>
        /// Shows the action sheet.
        /// </summary>
        /// <param name="name">Name.</param>
        public void ShowActionSheet(int index, RemoteFileInfo name)
        {
            TheDialogManager.StartShowing(() => {
                var frag = RemoteFileActionDialogFragment.Create(name.GetDisplayNameWithIndex(index));

                frag.OnRetrieve += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedRetrieve(index, name);
                };
                frag.OnReplace += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedReplace(index, name);
                };
                frag.OnRename += () =>
                {
                    TheDialogManager.EndShowing();
                    ShowEditTextDialog(index, name);
                };
                frag.OnClear += () =>
                {
                    TheDialogManager.EndShowing();
                    Listener?.OnRequestedClear(index, name);
                };
                frag.OnRequestedCancel += () => {
                    TheDialogManager.EndShowing();
                };
                frag.ShowOn(TheContext);

                return frag;
            });
        }
    }
}
