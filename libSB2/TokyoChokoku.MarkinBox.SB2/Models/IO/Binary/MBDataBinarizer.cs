using System;
using System.IO;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public sealed class MBDataBinarizer
	{
		public static class ByteSize
		{
			public const int Text = 2 * 52;
			// 52 word / max 50 char count.
			// if Type is QrCode or DataMatrix, max 75 char count.

			public const int FileSize = 88 * 2;
		}


		// nonnull
		private readonly Byte[] data;



		private readonly MappedString text;
		private readonly MappedBigEndShort textCharCount;
		private readonly MappedBigEndShort mode;
		private readonly MappedBigEndShort prmFl;
		private readonly MappedBigEndShort id;
		private readonly MappedBigEndFloat x;
		private readonly MappedBigEndFloat y;
		private readonly MappedBigEndFloat height;
		private readonly MappedBigEndFloat pitch;
		private readonly MappedBigEndFloat aspect;
		private readonly MappedBigEndFloat angle;
		private readonly MappedBigEndFloat arcRadius;
		private readonly MappedBigEndShort speed;
		private readonly MappedBigEndShort density;
		private readonly MappedBigEndShort power;
		private readonly MappedBigEndUnsignedShort hostVersion;
		private readonly MappedByte linkFlug;
		private readonly MappedByte[] links;

		private MappedByte LastLink {
			get { return links [MBDataStructure.NumberOfLinks - 1]; } 
		}

		private readonly MappedByte optionSw;
		private readonly MappedByte serialNo;
		private readonly MappedBigEndShort zDepth;
		private readonly MappedByte type;
		private readonly MappedByte basePoint;
		private readonly MappedBigEndShort[] spares;

		private MappedBigEndShort LastSpare {
			get { return spares [spares.Length - 1]; }
		}



		public String Text {
			get {
				if (Type == Consts.FieldTypeDataMatrix || Type == Consts.FieldTypeQrCode) {
					// 2Dコードの場合は1バイト文字として読み込む
					return text.GetAs (TextEncode.Byte1, TextCharCount);
				} else {
					// その他の場合は2バイト文字として読み込む
					return text.GetAs (TextEncode.Byte2, TextCharCount);
				}
			}
		}


		public short TextCharCount {
			get{ return textCharCount.Value; }
		}


		public short Mode {
            get{ return (short) mode.Value; }
            private set{ mode.Value =  (short) value; }
		}


		public short PrmFl {
			get { return prmFl.Value; }
			private set{ prmFl.Value = value; }
		}


		public short Id {
			get { return id.Value; }
			private set{ id.Value = value; }
		}


		public float X {
			get { return x.Value; }
			private set{ x.Value = value; }
		}


		public float Y {
			get { return y.Value; }
			private set{ y.Value = value; }
		}


		public float Height {
			get { return height.Value; }
			private set{ height.Value = value; }
		}


		public float Pitch {
			get { return pitch.Value; }
			private set{ pitch.Value = value; }
		}


		public float Aspect {
			get { return aspect.Value; }
			private set{ aspect.Value = value; }
		}


		public float Angle {
			get { return angle.Value; }
			private set{ angle.Value = value; }
		}


		public float ArcRadius {
			get { return arcRadius.Value; }
			private set{ arcRadius.Value = value; }
		}


		public short Speed {
			get { return speed.Value; }
			private set{ speed.Value = value; }
		}


		public short Density {
			get { return density.Value; }
			private set{ density.Value = value; }
		}


		public short Power {
			get { return power.Value; }
			private set{ power.Value = value; }
		}


		public ushort HostVersion {
			get { return hostVersion.Value; }
			private set{ hostVersion.Value = value; }
		}


		public byte LinkFlug {
			get { return linkFlug.Value; }
			private set{ linkFlug.Value = value; }
		}
		

		public byte OptionSw {
			get { return optionSw.Value; }
			private set{ optionSw.Value = value; }
		}


		public byte SerialNo {
			get { return serialNo.Value; }
			private set{ serialNo.Value = value; }
		}


		public short ZDepth {
			get { return zDepth.Value; }
			private set{ zDepth.Value = value; }
		}


		public byte Type {
			get { return type.Value; }
			private set{ type.Value = value; }
		}
			

		public byte BasePoint {
			get { return basePoint.Value; }
			private set{ basePoint.Value = value; }
		}


		public byte[] GetLinks ()
		{
			byte[] array = new byte[MBDataStructure.NumberOfLinks];

			for (int i = 0; i < array.Length; i++) {
				array [i] = links [i].Value;
			}

			return array;
		}

		public short[] GetSpares ()
		{
			short[] array = new short[MBDataStructure.NumberOfSpares];

			for (int i = 0; i < array.Length; i++) {
				array [i] = spares [i].Value;
			}

			return array;
		}





		private void SetTextProperties (String data, short type)
		{
			if (data.Length > MBDataStructure.MaxTextCharCounts(type)) {
				throw new IndexOutOfRangeException ();
			}

			short charCount = (short)data.Length;
			textCharCount.Value = charCount;

			if (type == Consts.FieldTypeDataMatrix || type == Consts.FieldTypeQrCode) {
				// 2Dコードの場合は1バイト文字として書き込む．
				text.SetStringAs (data, TextEncode.Byte1, charCount);
			} else {
				// その他の場合は2バイト文字として読み込む
				text.SetStringAs (data, TextEncode.Byte2, charCount);
			}
		}



		private void SetLink (int i, byte link)
		{
			links [i].Value = link;
		}

		private void SetSpare (int i, short spare)
		{
			spares [i].Value = spare;
		}



		public MBDataBinarizer ()
		{
			data = new byte[ByteSize.FileSize];

			text = new MappedString (data, 0, ByteSize.Text);
			textCharCount = new MappedBigEndShort (data, text.NextOffset);
			mode = new MappedBigEndShort (data, textCharCount.NextOffset);
			prmFl = new MappedBigEndShort (data, mode.NextOffset);
			id = new MappedBigEndShort (data, prmFl.NextOffset);

			x = new MappedBigEndFloat (data, id.NextOffset);
			y = new MappedBigEndFloat (data, x.NextOffset);
			height = new MappedBigEndFloat (data, y.NextOffset);
			pitch = new MappedBigEndFloat (data, height.NextOffset);
			aspect = new MappedBigEndFloat (data, pitch.NextOffset);
			angle = new MappedBigEndFloat (data, aspect.NextOffset);
			arcRadius = new MappedBigEndFloat (data, angle.NextOffset);

			speed = new MappedBigEndShort (data, arcRadius.NextOffset);
			density = new MappedBigEndShort (data, speed.NextOffset);
			power = new MappedBigEndShort (data, density.NextOffset);
			hostVersion = new MappedBigEndUnsignedShort (data, power.NextOffset);
			linkFlug = new MappedByte (data, hostVersion.NextOffset);

			links = MappedByte.newArray (data, linkFlug.NextOffset, MBDataStructure.NumberOfLinks);
			optionSw = new MappedByte (data, LastLink.NextOffset);
			serialNo = new MappedByte (data, optionSw.NextOffset);
			zDepth = new MappedBigEndShort (data, serialNo.NextOffset);
			type = new MappedByte (data, zDepth.NextOffset);

			basePoint = new MappedByte (data, type.NextOffset);
			spares = MappedBigEndShort.newArray (data, basePoint.NextOffset, MBDataStructure.NumberOfSpares);
		}


		public MBDataBinarizer (Byte[] src) : this ()
		{
			if (src.Length != ByteSize.FileSize) {
				throw new ArgumentException ();
			}

			Array.Copy (src, data, ByteSize.FileSize);
		}

		public MBDataBinarizer (MBData src) : this ()
		{
			SetTextProperties (src.Text, src.Type);
            Mode = (short) src.Mode;
			PrmFl = src.PrmFl;
			Id = src.Id;
			X = src.X;
			Y = src.Y;
			Height = src.Height;
			Pitch = src.Pitch;
			Aspect = src.Aspect;
			Angle = src.Angle;
			ArcRadius = src.ArcRadius;
			Speed = src.Speed;
			Density = src.Density;
			Power = src.Power;
			HostVersion = src.HostVersion;
			LinkFlug = src.LinkFlug;

			for (int i = 0; i < MBDataStructure.NumberOfLinks; i++) {
				SetLink (i, src.Links [i]);
			}

			OptionSw = src.OptionSw;

			// テキストに埋め込まれたシリアル番号をセットします。
			//SerialNo = src.SerialNo;
			var nullableSerialNumber = MatchingGrammer.SearchSerialNoOrNull(Text);
			if(nullableSerialNumber != null)
				SerialNo = (byte)(int)nullableSerialNumber;

			ZDepth = src.ZDepth;
			Type = src.Type;
			BasePoint = src.BasePoint;

			for (int i = 0; i < MBDataStructure.NumberOfSpares; i++) {
				SetSpare (i, src.Spares [i]);
			}
		}

		/// <summary>
		/// このオブジェクトを MBDataとして 出力します．
		/// </summary>
		/// <returns>The MB data.</returns>
		public MBData ToMBData ()
		{
			MBDataStructure model = new MBDataStructure ();
            model.Mode = (ushort) this.Mode;
			model.PrmFl = this.PrmFl;
			model.Id = this.Id;

			model.X = this.X;
			model.Y = this.Y;
			model.Height = this.Height;
			model.Pitch = this.Pitch;
			model.Aspect = this.Aspect;
			model.Angle = this.Angle;
			model.ArcRadius = this.ArcRadius;

			model.Speed = this.Speed;
			model.Density = this.Density;
			model.Power = this.Power;
			model.HostVersion = this.HostVersion;

			model.LinkFlug = this.LinkFlug;

			byte[] linksArray = GetLinks ();
			for (int i = 0; i < MBDataStructure.NumberOfLinks; i++) {
				model.Links [i] = linksArray [i];
			}

			model.OptionSw = this.OptionSw;
			model.SerialNo = this.SerialNo;

			model.ZDepth = this.ZDepth;
			model.Type = this.Type;
			model.BasePoint = this.BasePoint;

			short[] sparesArray = GetSpares ();
			for (int i = 0; i < MBDataStructure.NumberOfSpares; i++) {
				model.Spares [i] = sparesArray [i];
			}

			// FIXME: 例外の再スローを検討．
			try {
				model.Text = this.Text;
			} catch (Exception e) {
				Log.Debug ("PPTの読み込み", "テキストの変換", e.ToString ());
			}

			return new MBData (model);
		}


		public byte[] GetBytes(){
			var result = new byte[]{ };
			Array.Resize (ref result, data.Length);
			data.CopyTo (result, 0);
			return result;
		}

	}
}
