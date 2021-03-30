using System;
namespace TokyoChokoku.FieldModel
{
    /// <summary>
    /// 図形基準点
    /// </summary>
    public enum FieldBasePoint : byte
    {
        LB = 0, // 基準点（左下） 
        LM = 1, // 基準点（左中）
        LT = 2, // 基準点（左上）
        CB = 3, // 基準点（中央下）
        CM = 4, // 基準点（中央中）
        CT = 5, // 基準点（中央上）
        RB = 6, // 基準点（右下）
        RM = 7, // 基準点（右中）
        RT = 8, // 基準点（右上）
    }

    public static class FieldBasePointExt {
        public static bool Verify(int value) {
            return 0 <= value && value <= 8;
        }
        public static FieldBasePoint ToBasePoint(this int value) {
            if(Verify(value))
                return (FieldBasePoint)value;
            throw new InvalidOperationException(value + " is not base point");
        }
        public static FieldBasePoint ToBasePoint(this byte value)
        {
            if (Verify(value))
                return (FieldBasePoint)value;
            throw new InvalidOperationException(value + " is not base point");
        }
    }
}
