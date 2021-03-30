using System;
using System.Runtime.CompilerServices;

namespace Unicast.ProgressKit
{
    /// <summary>
    /// <c>ProgressViewController</c> への弱参照を表します。
    /// </summary>
    public struct ProgressViewControllerWeakReference : IEquatable<ProgressViewControllerWeakReference>
    {
        /// <summary>
        /// 弱参照を指定して、この構造体を初期化します。
        /// </summary>
        /// <param name="reference">弱参照したいオブジェクト</param>
        /// <returns><c>reference</c> への弱参照</returns>
        public static ProgressViewControllerWeakReference Init(ProgressViewController reference)
        {
            ProgressViewControllerWeakReference weak;
            weak.reference = new WeakReference<ProgressViewController>(reference);
            return weak;
        }

        // ====

        /// <summary>
        /// 弱参照オブジェクトです。 (nullable)
        /// </summary>
        private WeakReference<ProgressViewController> reference;

        /// <summary>
        /// 参照の期限切れの場合は true を返します。
        /// </summary>
        public bool IsExpired => ReferenceEquals(GetOrNull(), null);

        /// <summary>
        /// 参照を取得します。保持中の参照が無い場合は null です。
        /// reference === null は、 参照の期限切れと同一視します。
        /// </summary>
        /// <returns></returns>
        public ProgressViewController GetOrNull()
        {
            ProgressViewController ans;
            var weak = reference;
            if (ReferenceEquals(weak, null))
                return null;
            if (reference.TryGetTarget(out ans))
                return ans;
            return null;
        }

        /// <summary>
        /// 右辺値と同じオブジェクトを弱参照するかどうか調べます。
        ///
        /// 未初期化の弱参照は、期限切れとみなして比較します。
        /// 
        /// </summary>
        /// <param name="other">右辺値</param>
        /// <returns>
        /// a. true : 両辺が期限切れの場合
        /// b. true : 両辺ともに同じオブジェクトを弱参照する場合
        /// c. false: Otherwise
        /// </returns>
        public bool Equals(ProgressViewControllerWeakReference other)
        {
            var a = GetOrNull();
            var b = other.GetOrNull();
            return ReferenceEquals(a, b);
        }

        /// <summary>
        /// 右辺値と同じオブジェクトを弱参照するかどうか調べます。
        ///
        /// 未初期化の弱参照は、期限切れとみなして比較します。
        /// 
        /// </summary>
        /// <param name="other">右辺値</param>
        /// <returns>
        /// a. false: 右辺に null が指定された場合
        /// b. true : 両辺が期限切れの場合
        /// c. true : 両辺ともに同じオブジェクトを弱参照する場合
        /// d. false: Otherwise
        /// </returns>
        public override bool Equals(object other)
        {
            var r = other as ProgressViewControllerWeakReference?;
            if (ReferenceEquals(r, null))
                return false;
            return Equals((ProgressViewControllerWeakReference)r);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(GetOrNull());
        }

        /// <summary>
        /// left.Equals(right) と同じ条件で比較します。
        /// </summary>
        /// <param name="left">左辺値</param>
        /// <param name="right">右辺値</param>
        /// <returns>
        /// left.Equals(right) と同じ返り値
        /// </returns>
        public static bool operator ==(ProgressViewControllerWeakReference left, ProgressViewControllerWeakReference right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// !(left == right) と同じ条件で比較します。
        /// </summary>
        /// <param name="left">左辺値</param>
        /// <param name="right">右辺値</param>
        /// <returns>
        /// !(left == right) と同じ返り値
        /// </returns>
        public static bool operator !=(ProgressViewControllerWeakReference left, ProgressViewControllerWeakReference right)
        {
            return !(left == right);
        }
    }
}
