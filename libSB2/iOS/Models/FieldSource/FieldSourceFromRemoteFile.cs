using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class FieldSourceFromRemoteFile : FieldSource
    {
        /// <summary>
        /// パーマネント打刻ファイル用の設定
        /// </summary>
        /// <returns></returns>
        public static FieldSourceFromRemoteFile ForParmanent()
        {
            return new FieldSourceFromRemoteFile(255, "<<Parmanent>>");
        }


        // ========

        readonly int indexOfFile;
        readonly string name;

        public string From {
            get {
                return "file-category.remote".Localize () + " No." + (indexOfFile + 1) + " : " + name;
            }
        }

        public FieldSourceFromRemoteFile (int indexOfFile, string name)
        {
            this.indexOfFile = indexOfFile;
            this.name = name;
        }

        public Maybe<IList<iOSOwner>> TryLoad ()
        {
            //var task = TryLoadAsync();
            //task.Start();
            //task.Wait();
            //return task.Result;

            //FIXME: 2019/12/27 これよりも、上の処理の方が良いような... これだとシリアル設定が読み込まれません.
            var task = RemoteFileManager.Instance.LoadFile (indexOfFile);
            task.Start ();
            task.Wait ();
            var data = task.Result;
            if (data == null || data.Count <= 0) {
                return Maybe<IList<iOSOwner>>.Nothing;
            }
            return iOSOwner.From (FieldFactory.Create (data)).ToMaybe <IList<iOSOwner>> ();
        }

        public async Task<Maybe<IList<iOSOwner>>> TryLoadAsync ()
        {
            // ファイルの読み取り
            var data = await RemoteFileManager.Instance.LoadFile (indexOfFile);
            // データが空だったとき
            if (data == null || data.Count <= 0) {
                return Maybe<IList<iOSOwner>>.Nothing;
            }
            // シリアル設定を読み込みます。
            await SerialSettingsManager.Instance.Reload(indexOfFile);
            // シリアル設定を反映
            data = SerialSettingsManager.Instance.UpdateFieldText(data);
            // 結果をリストに設定
            var list = iOSOwner.From(FieldFactory.Create(data));
            return list.ToMaybe<IList<iOSOwner>>();
        }

        public void Autosave (iOSFieldContext context)
        {
            AutoSaveManager.SaveFileName (name.ToMaybe ());
            AutoSaveManager.SaveControllerNumber (indexOfFile.ToMaybe ());
            AutoSaveManager.Overwrite (context);
        }
    }
}

