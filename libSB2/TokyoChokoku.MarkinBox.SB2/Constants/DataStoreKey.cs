using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// Preferenceの値を取得する際に必要な文字列キーです．
    /// </summary>
	public enum DataStoreKey
	{
        /// <summary>
        /// 未使用
        /// </summary>
        CurrentFileNumber,
        /// <summary>
        /// 打刻機の型番
        /// </summary>
        MachineModelNumber,
        /// <summary>
        /// 管理者 or 操作者
        /// </summary>
        OperationMode,
        /// <summary>
        /// バーコードの打刻モード
        /// </summary>
        BarcodeMarkingMode,
        /// <summary>
        /// ソレノイドタイプ
        /// </summary>
        SolenoidType
    }

    /// <summary>
    /// DataStoreKey列挙型の拡張
    /// </summary>
	public static class DataStoreKeyExt{
		/// <summary>
        /// キーを文字列に変換します．
        /// </summary>
        /// <returns>The key.</returns>
        /// <param name="key">Key.</param>
        public static string ToKey(this DataStoreKey key){
			return key.GetType ().ToString () + key.ToString ();
		}
	}
}

