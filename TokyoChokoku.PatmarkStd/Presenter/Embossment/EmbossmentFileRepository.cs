using System;
using System.Linq;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Patmark.EmbossmentKit;
using Monad;
namespace TokyoChokoku.Patmark.Presenter.Embossment
{
    public class EmbossmentFileRepository
    {
        public FileContext     CurrentFile { get; set; }
        public bool IsEmpty
        {
            get
            {
                return CurrentFile.IsEmpty;
            }
        }


        /// <summary>
        /// ファイルのリストア
        /// </summary>
        public bool RestoreFile(FileContext def)
        {
            var f = AutoSaveManager.LoadFileAutoSaved();
            if (f.HasValue())
            {
                CurrentFile = f.Value();
                return true;
            }
            CurrentFile = def;
            return false;
        }

        /// <summary>
        /// クイックモードならUIControllerの入力内容をCurrentFileに反映します。
        /// current file が null の場合は EmbossmentData.Empty を返します.
        /// </summary>
        public EmbossmentData CreateEmbossmentData()
        {
            var file = CurrentFile;
            if(file==null) {
                WarnFileNotSet();
                return EmbossmentData.Empty;
            }
            // ファイルの内容に合わせてプレビュー・打刻用データを生成します。
            var fields = file.Owner.Serializable;
            if (fields.Count() > 1)
            {
                var first = fields.First();
                ushort mode = first.Mode;
                // ushort mode is flnm in MBDATA defined as a hex value
                // mode : 0 = Text or Logo; 400 = QR code; 800 = DM
                if (mode == 0)
                {
                    ///
                    var dataEnu = from field in fields.Take(1)
                                  select EmbossmentData.Create(
                                             EmbossmentMode.FromMBData(field),
                                             field.Text
                                         );
                    return dataEnu.FirstOrDefault() ?? EmbossmentData.Empty;
                }
                else
                {
                    //var dataEnu = EmbossmentData.Create(
                    //                    new EmbossmentMode(),
                    //                    ""
                    //                    );
                    return EmbossmentData.Empty;
                }
            } else
            {
                return EmbossmentData.Empty;
            }
            //} else {
            //    // 参照型の場合, FirstOrDefaultは 要素がない時に null を返す.
            //    return dataEnu.FirstOrDefault() ?? EmbossmentData.Empty;
            //}
        }

        /// <summary>
        /// クイックモードならUIControllerの入力内容をCurrentFileに反映します。
        /// current file が null の場合は何もしません (警告は出る)
        /// </summary>
        public void CommitIfQuickMode(EmbossmentData data) {
            var file = CurrentFile;
            if(file == null) {
                WarnFileNotSet();
                return;
            }
            if (file.isLocalFile)
            {
                CurrentFile = file.QuickEditor.Apply((src) => {
                    return EmbossmentToolKit.ApplyEmbossmentData(data, null, new MBData(src)).ToMutable();
                });
            }
        }

        /// <summary>
        /// Saves the quick mode text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void SaveQuickModeText(string text) {
            // クイックモードの場合は、編集したテキストをセットします。
            CurrentFile = CurrentFile.QuickEditor.ApplyText(text);
            // ファイルを保存
            AutoSaveManager.Overwrite(CurrentFile.Owner);
        }

        /// <summary>
        /// QuickModeでCurrentFileが空の時は 空の文字を挿入して保存します.
        /// (フェイルセーフ用の実装)
        /// </summary>
        /// <param name="text">Text.</param>
        public void SaveQuickModeAsBlankIfNeeded() {
            var file = CurrentFile;
            if(file.isLocalFile) {
                if (file.Owner.IsEmpty)
                {
                    SaveQuickModeText("");
                }
            }
        }

        /// <summary>
        /// オートセーブの実行.
        /// current file が null の場合は何もしません
        /// </summary>
        public void AutoSave()
        {
            var file = CurrentFile;
            if (file == null)
            {
                WarnFileNotSet();
                return;
            }
            if(file.Owner.IsEmpty)
            {
                return;
            }
            file.AutoSave();
        }

        void WarnFileNotAttached()
        {
            Console.Error.WriteLine("[EmbossmentFileRepository] Warning: File not attached.");
        }

        void WarnFileNotSet()
        {
            Console.Error.WriteLine("[EmbossmentFileRepository] Warning: File not set.");
        }
    }
}
