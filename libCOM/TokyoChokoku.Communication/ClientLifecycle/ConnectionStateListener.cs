using System;
namespace TokyoChokoku.Communication
{
    public interface ConnectionStateListener
    {
        /// <summary>
        /// リスナが登録された後に呼び出されます.
        /// </summary>
        void DidSumbit(ConnectionState current);

        /// <summary>
        /// 状態が変化した時に呼び出されます.
        /// </summary>
        void OnStateChanged(ConnectionState next, ConnectionState prev);
        /// <summary>
        /// 排他処理に失敗した時に呼び出されます.
        /// </summary>
        void OnFailOpening(ExclusionError error);
        /// <summary>
        /// 排他解除に失敗した時に呼び出されます.
        /// </summary>
        void OnFailReleasing(ExclusionError error);
    }
}
