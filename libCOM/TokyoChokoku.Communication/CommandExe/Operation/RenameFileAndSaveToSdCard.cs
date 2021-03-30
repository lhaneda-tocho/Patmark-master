using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    partial class CommandExecutor
    {
        /// <summary>
        /// ファイル名を書き換えるメソッドです。この実行でSDカードへ保存されます。
        /// 処理に失敗した場合のコントローラの状態は不定となります。
        ///
        /// なお、このメソッドではファイル名の修正は行いません。
        /// この呼び出しの時点で、ファイル情報のリストアが行われていない場合は、この処理の初めに実行します。
        /// </summary>
        /// <param name="fileIndex">ファイルインデックス</param>
        /// <param name="numOfFields">フィールド数</param>
        /// <param name="onChangeFileList"><c>fileIndex</c>で指定されたファイルが、ファイル一覧に表示されるファイルである場合に呼ばれるアクションです。(引数1: fileIndex, 引数2: fileName)</param>
        /// <returns>成功したら true, 失敗したら false</returns>
        /// <exception cref="ArgumentException"><c>fileIndex</c>が定義域外である場合 (ファイルリストの外である場合)</exception>
        public Task<bool> RenameFileAndSaveToSdCard(int fileIndex, string fileName, Action<int, string> onChangeFileListOrNull)
        {
            void Require(bool condition, Func<string> message)
            {
                if (!condition)
                    throw new ArgumentException(message?.Invoke() ?? "");
            }
            bool FileIndexIsInFileList()
            {
                return 0 <= fileIndex && fileIndex < Sizes.NumOfRemoteFile;
            }
            async Task<bool> SetFileName()
            {
                if (FileIndexIsInFileList())
                {
                    onChangeFileListOrNull?.Invoke(fileIndex, fileName);
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                    return (await this.SetRemoteFileName((short)fileIndex, fileName)).IsOk;
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                }
                return false;
            }
            async Task<bool> NameToSdCard()
            {
                return (await this.SaveFileNameToSdCard()).IsOk;
            }
            async Task<bool> Block()
            {
                Require(FileIndexIsInFileList(), () => $"{nameof(fileIndex)} (={fileIndex}) is out of range");

                // ファイル情報の展開を行う (これを行わないと、別のコントローラに接続した後にファイルの名前を変更することでファイルリストが完全に消失してしまう)
                // NOTE: 特定のファイル名だけ SDカードへ書き込むように実装できるなら、このロジックは不要になる。
                var success = await LoadFileNameAndMapOnSdCardIfNeeded();
                if (!success)
                    return false;

                // ファイル名の送信
                success = await SetFileName();
                if (!success)
                    return false;
                success = await NameToSdCard();
                if (!success)
                    return false;
                return success;
            }
            return Block();
        }
    }
}

