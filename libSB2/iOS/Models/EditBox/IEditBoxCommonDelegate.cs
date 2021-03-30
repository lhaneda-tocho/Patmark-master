using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public interface IEditBoxCommonDelegate
    {

        /// <summary>
        /// 編集中のデータを iOSOwnerリストに書き込みます．(適用します)
        /// </summary>
        void Apply ();

        /// <summary>
        /// 編集Boxを 再構成します．
        /// 加えて編集中のデータを iOSOwnerリストに書き込みます．(適用します)
        /// </summary>
        void Rebuild ();
    }
}

