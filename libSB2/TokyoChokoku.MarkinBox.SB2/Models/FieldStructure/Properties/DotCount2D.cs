using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// DataMatrixの ドット数を表すクラスです．
    /// </summary>
    public class DotCount2D: IEquatable<DotCount2D>
    {
        public bool Invalid { get; }
        public byte Horizontal { get; }
        public byte Vertical   { get; }

        public double AspectWidth
        {
            get
            {
                if (Invalid)
                    return 1;
                return (Horizontal - 1) / (double)(Vertical - 1); 
            }
        }

        public double AspectHeight
        {
            get
            {
                if (Invalid)
                    return 1;
                return (Vertical - 1) / (double)(Horizontal - 1);
            }
        }

        public DotCount2D (byte horizontal, byte vertical)
        {
            if (horizontal <= 1 || vertical <= 1)
                Invalid = true;
            else
                Invalid = false;
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public DotCount2D () : this (10, 10)
        {

        }

        /// <summary>
        /// デバッグ用に表示する文字列に変換します．
        /// 
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString ()
        {
            return string.Format ("[DotCount2D: Horizontal={0}, Vertical={1}]", Horizontal, Vertical);
        }

        /// <summary>
        /// GUIで表示する文字列に変換します．
        /// </summary>
        /// <returns>The title.</returns>
        public string ToTitle ()
        {
            if (Horizontal == Vertical) {
                return string.Format ("{0}", Horizontal);
            } else {
                return string.Format ("{0} x {1}", Vertical, Horizontal);
            }
        }

        public override bool Equals (object obj)
        {
            return Equals(other: obj as DotCount2D);
        }

        public bool Equals(DotCount2D other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (ReferenceEquals(other, null))
                return false;
            return Horizontal == other.Horizontal && Vertical == other.Vertical;
        }

        public override int GetHashCode ()
        {
            int code = 17;
            code = 31 * code + Horizontal;
            code = 31 * code + Vertical;
            return code;
        }

        public static bool operator == (DotCount2D left, DotCount2D right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            if (ReferenceEquals(right, null))
                return false;
            return left.Equals(other: right);
        }

        public static bool operator != (DotCount2D left, DotCount2D right)
        {
            return !(left == right);
        }
    }
}

