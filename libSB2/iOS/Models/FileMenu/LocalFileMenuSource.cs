using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using ToastIOS;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{

    public class LocalFileMenuSource
    {
        
        // 読み込み場所を記憶
        FieldSource Source { get; set; }

        // FileLoaded 後の時のみ nonnull
        // 読み込み結果を記憶
        IList<iOSOwner> Loaded { get; set; }

        // 保存するリスト
        List<iOSOwner> OwnerList { get; }

        // ファイル操作の結果に応じて呼び出す必要のあるメソッドたち
        FileMenuActionSource Actions { get; }

        /// <summary>
        /// Complex にファイルパスを与えてリストを構成します．
        /// </summary>
        /// <value>The file list.</value>
        public List<PathName> FileList {
            get {
                return (
                    from e in LocalFileManager.GetLocalPpgPathList ()
                    select PathName.FromPath(e)
                ).ToList ();
            }
        }

        /// <summary>
        /// コンストラクタ．null を入れると例外が発生します．
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="ownerList">Owner list.</param>
        /// <param name="actions">Actions.</param>
        public LocalFileMenuSource (FieldSource source, List<iOSOwner> ownerList, FileMenuActionSource actions)
        {
            if (source == null || ownerList == null || actions == null)
                throw new NullReferenceException ();
            Source = source;
            OwnerList = ownerList;
            Actions = actions;
        }

        /// <summary>
        /// ViewDidLoadで呼び出されます．
        /// </summary>
        public void Setup ()
        {
            LocalFilePathExt.ReadyDirectory ();
        }

        /// <summary>
        /// 指定されたファイルを削除します．
        /// 引数の文字列は FileList から選ばれたものです．
        /// </summary>
        /// <param name="file">File.</param>
        public void Delete (PathName file)
        {
            File.Delete (file.Full);
        }

        /// <summary>
        /// 指定されたファイルが存在するときは true を返すメソッドです．
        /// 存在するときは true, そうでなければ false を返します．
        /// </summary>
        /// <param name="file">File.</param>
        public bool Exists (PathName file)
        {
            return File.Exists (file.Full);
        }

        /// <summary>
        /// 指定されたファイル名からCoamplexNameオブジェクトとして返します．
        /// 変換できない場合は 空 を返します．
        /// </summary>
        /// <param name="file">File.</param>
        public Maybe<PathName> GetName (string file)
        {
            return
                from pathName in PathName.FromPath (file.ToLocalPpgPath ())
                select pathName;
        }

        /// <summary>
        /// 指定されたファイルの読み込みを行います．
        /// </summary>
        /// <param name="file">File.</param>
        public void Load (PathName file)
        {
            Source = new FieldSourceFromLocalFile (file);
            var data = Source.TryLoad ();
            if (data.HasValue) {
                Loaded = data.Value;
                Toast.MakeText (
                    "Edited successfully.".Localize (),
                    ToastDuration.Medium
                ).Show ();
            } else {
                Toast.MakeText (
                    "Failed to edit name.".Localize (),
                    ToastDuration.Medium
                ).Show ();
            }
        }

        /// <summary>
        /// ファイル名を変更します．
        /// 引数の名前は Exists によって ファイルが存在しないことを確認され，
        /// かつ IsValid によって ファイル名として適切であることを証明されたものです．
        /// </summary>
        /// <param name="targetFile">Rename target filie.</param>
        /// <param name="nextName">New file name</param>
        public void Rename (PathName targetFile, PathName nextName)
        {
            File.Move (
                targetFile.Full,
                nextName.Full
            );
        }

        /// <summary>
        /// 新しいファイルに保存します．
        /// 引数の名前は Exists によって ファイルが存在しないことを確認され，
        /// かつ IsValid によって ファイル名として適切であることを証明されたものです．
        /// </summary>
        /// <param name="file">File.</param>
        public void SaveAs (PathName file)
        {
            LocalFileManager.SaveAs (OwnerList, file.Full);
            Source = new FieldSourceFromLocalFile (file);
        }

        /// <summary>
        /// 指定されたファイルに上書き保存します．
        /// 引数の文字列は FileList から選ばれたものです．
        /// </summary>
        /// <param name="file">File.</param>
        public void SaveOver (PathName file)
        {
            LocalFileManager.SaveOver (OwnerList, file.Full);
        }

        /// <summary>
        /// コントローラが dismiss された時に呼ばれます．
        /// イベントリスナを呼び出すためのアクションを返します．
        /// 何もしないときは 空 を返します．
        /// 返り値のアクションは このメソッド実行直後に呼び出されます，
        /// </summary>
        /// <returns>The sender.</returns>
        public Maybe<Action> GetAction (FileMenuDismissedEvent ev)
        {
            switch (ev.Cause) {
                case FileMenuDismissedEvent.BecauseOf.FileLoaded: {
                    if (Loaded == null)
                        throw new NullReferenceException ();
                    return Actions.WillExitAfterLoad (Source, Loaded).ToMaybe ();
                }

                default: {
                    return Actions.WillExit (Source).ToMaybe ();
                }
            }
        }
    }
}

