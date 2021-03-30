using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    /// <summary>
    /// 衝突判定を保存するリストを表すインターフェースです．
    /// ICollision型のリストを使わずに衝突判定リストの管理を行うことができます．
    /// 
    /// </summary>
    public interface CollisionKitDataSource <Type> where Type:class
    {
        /// <summary>
        /// 任意のデータのリストを取得する．
        /// </summary>
        /// <returns>The list.</returns>
        IList<Type>      DataList ();
        /// <summary>
        /// 任意のデータから，衝突判定を取得するメソッド．
        /// </summary>
        /// <returns>The collision.</returns>
        /// <param name="type">Type.</param>
        ICollision       GetCollision (Type type);
    }
}

