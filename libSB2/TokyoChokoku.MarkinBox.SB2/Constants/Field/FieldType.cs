using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	/// <summary>
	/// 図形の種類を表す 定数です．
	/// </summary>
	public enum FieldType : byte
	{
		// 図形タイプ（種類）
		HorizontalText   = 0,  // 文字列
		OuterArcText     = 1,  // 文字列（外弧）
		InnerArcText     = 2,  // 内弧
		XVerticalText    = 3,  // 文字列（縦書き 文字の底辺が x のプラス方向へ向くのが標準）
		YVerticalText    = 4,  // 文字列（縦書き 文字の底辺が y のプラス方向へ向くのが標準）
		QrCode           = 5,  // QRコード
		DataMatrix       = 6,  // データマトリクス
		Rectangle        = 10, // 矩形
		Circle           = 11, // 完全な円
		Line             = 12, // 直線
		Ellipse          = 13, // 楕円
		Triangle         = 14, // 三角
		Bypass           = 15, // バイパス
	}

    public static class FieldTypeExt
    {
        public static bool IsDefined(byte i)
        {
            return Enum.IsDefined ( typeof(FieldType), i );
        }

        public  static String GetName (this FieldType type)
        {
            return Enum.GetName (typeof(FieldType), (int)type);
        }

        public static NotSupportedTypeException generateNotSupportedTypeException(int value)
        {
            return new NotSupportedTypeException ("'" + value + "' is not defined as FieldType.");
        }


        public class NotSupportedTypeException : System.Exception
        {
            public NotSupportedTypeException(String mes) : base(mes) {

            }
        }
    }
}

