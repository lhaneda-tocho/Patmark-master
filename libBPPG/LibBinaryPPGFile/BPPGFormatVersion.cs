using System;
using System.Collections.Generic;

namespace TokyoChokoku.Bppg
{
    /// <summary>
    /// BPPGライブラリ内で使用されるバージョンバリューオブジェクトです。
    /// アプリ側とのやり取りで使用します。
    /// </summary>
    public class BPPGFormatVersion : IEquatable<BPPGFormatVersion>
    {
        /// <summary>
        /// フォーマット名。 "BPPG_FIELD" などが入ります。
        /// </summary>
        public string Name  { get; }

        /// <summary>
        /// メジャーバージョン番号
        /// </summary>
        public int    Major { get; }

        /// <summary>
        /// マイナーバージョン番号
        /// </summary>
        public int    Minor { get; }


        public BPPGFormatVersion(string name, int major, int minor)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Major = major;
            Minor = minor;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as BPPGFormatVersion);
        }

        public bool Equals(BPPGFormatVersion other)
        {
            return other != null &&
                   Name == other.Name &&
                   Major == other.Major &&
                   Minor == other.Minor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Major, Minor);
        }

        public override string ToString()
        {
            return $"[{Name}] {Major}.{Minor}";
        }

        public static bool operator ==(BPPGFormatVersion left, BPPGFormatVersion right)
        {
            return EqualityComparer<BPPGFormatVersion>.Default.Equals(left, right);
        }

        public static bool operator !=(BPPGFormatVersion left, BPPGFormatVersion right)
        {
            return !(left == right);
        }
    }
}
