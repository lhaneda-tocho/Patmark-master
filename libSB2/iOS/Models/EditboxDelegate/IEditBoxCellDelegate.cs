using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface IEditBoxCellDelegate
    {

        /// <summary>
        /// このデリゲートで所持する状態が変更された時に呼び出されるデリゲートメソッドを設定します．
        /// 設定できるメソッドは1つのみです．この呼び出しより前に設定されたデリゲートメソッドは削除されます．
        /// null を設定した場合は デリゲートメソッドの削除を行います．
        /// 
        /// デリゲートの実装によっては，状態を持たないものがあります．
        /// そのデリゲートに対してこのメソッドを呼び出しても何も起きないことを保証します．
        /// </summary>
        void SetUpdatedListener (Action<IEditBoxCellDelegate> action);
    }
}

