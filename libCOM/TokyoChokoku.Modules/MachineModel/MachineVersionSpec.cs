using System;
using System.Collections;
using System.Collections.Generic;

namespace TokyoChokoku.MachineModel
{
    /// <summary>
    /// コントローラのバージョンを表す抽象クラスです. 具象クラスは製品ごとに設定されます.
    /// バージョンは3段階取得できることが保証されており，0から順にMajorバージョン, Middle バージョン, Minorバージョン と取得できます.
    /// </summary>
    public abstract class MachineVersionSpec: IEnumerable<int>
    {
        private readonly int[] version;

        /// <summary>
        /// 製品の名前を返すプロパティです. この名前はユニークである必要はありません.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// バージョンを取得します. 0からMajorバージョン, Minorバージョン, Update回数 と取得できます. 少なくとも3桁は必ず取得できます. 
        /// 3桁以降で存在しない段階にアクセスした場合は例外となります. 3段以内で存在しない段がある場合, その段から3段目までは0を返します
        /// 段の指定は 0 から始まる点に注意してください.
        /// </summary>
        /// <param name="i">The index.</param>
        public int this[int i]
        {
            get
            {
                return version[i];
            }
        }

        /// <summary>
        /// バージョン3桁を指定して初期化します.
        /// </summary>
        /// <param name="v0">V0.</param>
        /// <param name="v1">V1.</param>
        /// <param name="v2">V2.</param>
        public MachineVersionSpec(string target, params int[] vn) 
        {
            if (vn.Length < 3) {
                version = new int[3];
                for (int i = 0; i < vn.Length; ++i)
                    version[i] = vn[i];
            } else
                version = (int[])vn.Clone();
            Target = target ?? "";
        }

        /// <summary>
        /// 桁数を返します.
        /// </summary>
        /// <returns>The count.</returns>
        public int Count => version.Length;

        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)version).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<int>)version).GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            var spec = obj as MachineVersionSpec;
            if (spec == null)
                return false;
            if(GetType() != obj.GetType())
                return false;
            if (Count != spec.Count)
                return false;
            var cnt = Count;
            for (int i = 0; i < cnt; ++i) {
                if (this[i] != spec[i])
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 1380981039 + EqualityComparer<int[]>.Default.GetHashCode(version);
        }
    }
}
