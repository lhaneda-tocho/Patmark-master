using System;
namespace TokyoChokoku.Structure
{
    public enum FieldType: byte
    {
        Text          = 0,  // 文字列
        OuterArcText  = 1,  // 文字列（外弧）
        InnerArcText  = 2,  // 内弧
        XVerticalText = 3,  // 文字列（縦書き 文字の底辺が x のプラス方向へ向くのが標準）
        YVerticalText = 4,  // 文字列（縦書き 文字の底辺が y のプラス方向へ向くのが標準）
        QrCode        = 5,  // QRコード
        DataMatrix    = 6,  // データマトリクス
        Rectangle     = 10, // 矩形
        Circle        = 11, // 完全な円
        Line          = 12, // 直線
        Ellipse       = 13, // 楕円
        Triangle      = 14, // 三角
        Bypass        = 15, // バイパス
        Unknown       = 255 // 不明
	}
}
