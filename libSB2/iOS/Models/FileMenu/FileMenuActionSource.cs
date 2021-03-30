using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface FileMenuActionSource
    {
        /// <summary>
        /// 最新打刻ファイルの読み出しが発生した時．
        /// </summary>
        /// <returns>The load newest.</returns>
        /// <param name="source">Source.</param>
        /// <param name="list">List.</param>
        Action DidLoadNewest (FieldSource source, IList<iOSOwner> list);

        /// <summary>
        /// ファイルメニューで読み込みが発生し，その後，メニューが閉じられた時に呼び出されるアクションを返します．
        /// 
        /// </summary>
        /// <returns>The load and exit.</returns>
        Action WillExitAfterLoad (FieldSource source, IList<iOSOwner> context);

        /// <summary>
        /// ファイルメニューで 新規作成 が発生し，その後，メニューが閉じられた時に呼び出されるアクションを返します．
        /// 
        /// </summary>
        /// <returns>The exit after do anything.</returns>
        /// <param name="source">Source.</param>
        Action WillExit (FieldSource source);

    }
}

