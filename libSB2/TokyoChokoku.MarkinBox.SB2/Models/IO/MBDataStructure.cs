using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public sealed class MBDataStructure
	{

		public const int NumberOfLinks = 5;
		public const int NumberOfSpares = 8;

		public const int FigTypeIndex = 79 * 2;
		public const int NumOfLink = 5;
		public const int NumOfSpares = 8;

		public static int MaxTextCharCounts(int type)
		{
			if (type == Consts.FieldTypeQrCode || type == Consts.FieldTypeDataMatrix) {
				return 75;
			} else {
				return 50;
			}
		}

		public String Text = "";
		public ushort Mode = 0;
		public short PrmFl = 0;
		public short Id = 0;

		public float X = 0f;
		public float Y = 0f;
		public float Height = 0f;
		public float Pitch = 0f;
		public float Aspect = 0f;
		public float Angle = 0f;
		public float ArcRadius = 0f;

		public short Speed = 0;
		public short Density = 5;
		public short Power = 0;
		public ushort HostVersion = 0x9999;

		public byte LinkFlug = 0;

		public byte[] Links { get; private set;} = new byte[MBDataStructure.NumberOfLinks];

		public byte OptionSw = 0;
		public byte SerialNo = 0;

		public short ZDepth= 0;

		public byte Type = byte.MaxValue;
		public byte BasePoint = 0;

		public short[] Spares { get; private set;} = new short[MBDataStructure.NumberOfSpares];


		public MBDataStructure Clone()
		{
			var ins = (MBDataStructure) MemberwiseClone ();

			ins.Links = (byte[])this.Links.Clone ();
			ins.Spares = (short[]) this.Spares.Clone ();

			return ins;
		}



		/// <summary>
		/// Modeの値を元に、図形タイプを更新します。
		/// </summary>
		public void UpdateType ()
		{
			//if (Type > 0) {
			//	return;			// 明示されているタイプを優先
			//} else {
			//	// -1:MBではこの値は存在しない
			//	// メモリクリア状態が0で、Character と被っている為、差別化が必要
			//	Type = byte.MaxValue;
			//}

			//// アスペクト比を再計算　判定に使用する模様
			//int calc_aspect_rato = 999;
			//try {
			//	calc_aspect_rato = (int)((Math.Abs (Pitch - Y) / Math.Abs (Height - Y)) * 100);
			//} catch (Exception e) {
			//	Log.Debug ("PPTの読み込み", "アスペクト比の再計算", e.ToString ());
			//}

			////Log.d(this.toString(), aspect + ":" + calc_aspect_rato + ":" + mode);

			//// 文字列が入っていれば、textを疑う
			//if (Text != null && Text.Length > 0) {
			//	Type = Consts.FieldTypeText;
			//}

			//// シリアルの場合
			//if ((Mode & Consts.FieldModeSerial) != 0) {

			//}

			//if ((Mode & Consts.FieldModeOuterArc) != 0) {
			//	Type = Consts.FieldTypeOuterArcText;
			//	return;
			//}
			//if ((Mode & Consts.FieldModeInnerArc) != 0) {
			//	Type = Consts.FieldTypeInnerArcText;
			//	return;
			//}
			//if ((Mode & Consts.FieldModeTriangle) != 0) {
			//	Type = Consts.FieldTypeTriangle;
			//	return;
			//}
			//if ((Mode & Consts.FieldModeDataMatrix) != 0) {
			//	Type = Consts.FieldTypeDataMatrix;
			//	return;
			//}
			//if ((Mode & Consts.FieldModeQrCode) != 0) {
			//	Type = Consts.FieldTypeQrCode;
			//	return;
			//}
			//if ((Type & Consts.FieldModeCircleOrEllipse) != 0) {
			//	if (Aspect == -100.0f) {
			//		Type = Consts.FieldTypeCircle;
			//		return;
			//	} else if (Aspect == calc_aspect_rato) {
			//		Type = Consts.FieldTypeEllipse;
			//		return;
			//	}
			//}
		}


	}
}

