using System;
using System.Linq;
using System.IO;

using TokyoChokoku;
using TokyoChokoku.Text;
using TokyoChokoku.Communication;
using TokyoChokoku.Structure.Binary.FileFormat;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.Communication.Text;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Structure.Binary
{
	public sealed class MBDataBinarizer
	{
        private static ILog Log = CommunicationLogger.Supply();

		public static class ByteSize
		{
			public const int Text = 2 * 52;
			// 52 word / max 50 char count.
			// if Type is QrCode or DataMatrix, max 75 char count.

			public const int FileSize = 88 * 2;
		}

        public Programmer Data => data;

		// nonnull
		readonly Programmer   data;

		// mapper
		readonly WordMapper   text;
        readonly ByteMapper   monoText;
		readonly SInt16Mapper textCharCount;
		readonly SInt16Mapper mode;
		readonly SInt16Mapper prmFl;
		readonly SInt16Mapper id;

		readonly FloatMapper  x;
		readonly FloatMapper  y;
		readonly FloatMapper  height;
		readonly FloatMapper  pitch;
		readonly FloatMapper  aspect;
		readonly FloatMapper  angle;
		readonly FloatMapper  arcRadius;

		readonly SInt16Mapper speed;
		readonly SInt16Mapper density;
		readonly SInt16Mapper power;
		readonly UInt16Mapper hostVersion;

		readonly ByteMapper   linkFlug;
		readonly ByteMapper   links;

		readonly ByteMapper   optionSw;
		readonly ByteMapper   serialNo;
		readonly SInt16Mapper zDepth;
		readonly ByteMapper   type;
		readonly ByteMapper   basePoint;
       
		readonly SInt16Mapper spares;


		public String Text {
			get {
				if (Type == (int) FieldType.DataMatrix || Type == (int) FieldType.QrCode) {
                    // 2Dコードの場合は1バイト文字として読み込む
                    int count = TextCharCount;
                    var bytes = monoText.ReadFrom(data).Take(count).ToArray();
                    return new MonoByteText(bytes).ToString();
				} else {
                    // その他の場合は2バイト文字として読み込む
                    int count = TextCharCount;
                    var words = text.ReadFrom(data).Take(count).ToArray();
                    var wtext = new WideText(words);
                    return wtext.ToString();
				}
			}
		}


		public short TextCharCount {
            get
            {
                int count = textCharCount.ReadFrom(data).First();
                if (Type == (int)FieldType.DataMatrix || Type == (int)FieldType.QrCode)
                {
                    return (short) Math.Min(75, Math.Max(0, count));
                }
                else
                {
                    return (short) Math.Min(50, Math.Max(0, count));
                }
            }
		}


		public short Mode {
            get{ return mode.ReadFrom(data).First(); }
            private set{ mode.PutTo(data, value); }
		}


		public short PrmFl {
			get { return prmFl.ReadFrom(data).First(); }
			private set{ prmFl.PutTo(data, value); }
		}


		public short Id {
            get { return id.ReadFrom(data).First(); }
			private set{ id.PutTo(data, value); }
		}


		public float X {
            get { return x.ReadFrom(data).First(); }
			private set{ x.PutTo(data, value); }
		}


		public float Y {
            get { return y.ReadFrom(data).First(); }
            private set{ y.PutTo(data, value); }
		}


		public float Height {
            get { return height.ReadFrom(data).First(); }
			private set{ height.PutTo(data, value); }
		}


		public float Pitch {
            get { return pitch.ReadFrom(data).First(); }
			private set{ pitch.PutTo(data, value); }
		}


		public float Aspect {
            get { return aspect.ReadFrom(data).First(); }
			private set{ aspect.PutTo(data, value); }
		}


		public float Angle {
            get { return angle.ReadFrom(data).First(); }
			private set{ angle.PutTo(data, value); }
		}


		public float ArcRadius {
            get { return arcRadius.ReadFrom(data).First(); }
			private set{ arcRadius.PutTo(data, value); }
		}


		public short Speed {
            get { return speed.ReadFrom(data).First(); }
			private set{ speed.PutTo(data, value); }
		}


		public short Density {
            get { return density.ReadFrom(data).First(); }
			private set{ density.PutTo(data ,value); }
		}


		public short Power {
            get { return power.ReadFrom(data).First(); }
			private set{ power.PutTo(data, value); }
		}


		public ushort HostVersion {
            get { return hostVersion.ReadFrom(data).First(); }
			private set{ hostVersion.PutTo(data, value); }
		}


		public byte LinkFlug {
            get { return linkFlug.ReadFrom(data).First(); }
			private set{ linkFlug.PutTo(data, value); }
		}
		

		public byte OptionSw {
            get { return optionSw.ReadFrom(data).First(); }
			private set{ optionSw.PutTo(data, value); }
		}


		public byte SerialNo {
            get { return serialNo.ReadFrom(data).First(); }
            private set{ serialNo.PutTo(data, value); }
		}


		public short ZDepth {
            get { return zDepth.ReadFrom(data).First(); }
            private set{ zDepth.PutTo(data, value); }
		}


		public byte Type {
            get { return type.ReadFrom(data).First(); }
			private set{ type.PutTo(data, value); }
		}
			

		public byte BasePoint {
            get { return basePoint.ReadFrom(data).First(); }
            private set{ basePoint.PutTo(data, value); }
		}


		public byte[] GetLinks ()
		{
            return links.ReadFrom(data).ToArray();
		}

		public short[] GetSpares ()
		{
            return spares.ReadFrom(data).ToArray();
		}





		void SetTextProperties (String data, short type)
		{
			if (type == (int) FieldType.DataMatrix || type == (int)FieldType.QrCode) {
                // 2Dコードの場合は1バイト文字として書き込む．
                var mtext = MonoByteText.Encode(data);
                var count = (short)Math.Min(75, mtext.Count);
                textCharCount.PutTo(this.data, count);
                monoText.PutTo(this.data, mtext, 0, count);
			} else {
				// その他の場合は2バイト文字として読み込む
				var wtext = WideText.Encode(data);
				var count = Math.Min(50, wtext.Count);
				textCharCount.PutTo(this.data, (short)count);
                text.PutTo(this.data, wtext, 0, count);
			}
		}



		void SetLink (int i, byte link)
		{
            links.PutTo(data, link, i);
		}

		void SetSpare (int i, short spare)
		{
            spares.PutTo(data, spare, i);
		}



		public MBDataBinarizer (Programmer programmer)
		{
            data = programmer;

            if (programmer.ByteCount < MBDataFileSize.ByteCount)
                throw new IndexOutOfRangeException();

            var chain = new MapperChain();

            // ===== Def
            // TODO: Union に対応する
            text          = chain.Alloc(MBDataTextSize.Words, IndexType.Word).AsWord();
            monoText      = text.CreateByteUnion();
            textCharCount = chain.AllocSInt16();
            mode          = chain.AllocSInt16();
            prmFl         = chain.AllocSInt16();
            id            = chain.AllocSInt16();

			x = chain.AllocFloat();
			y = chain.AllocFloat();
			height = chain.AllocFloat();
            pitch         = chain.AllocFloat();
            aspect        = chain.AllocFloat();
            angle         = chain.AllocFloat();
            arcRadius     = chain.AllocFloat();

            speed         = chain.AllocSInt16();
            density       = chain.AllocSInt16();
            power         = chain.AllocSInt16();
            hostVersion   = chain.AllocUInt16();

            linkFlug      = chain.AllocByte();
            links         = chain.Alloc(MBDataStructure.NumberOfLinks, IndexType.Byte).AsByte();

            optionSw      = chain.AllocByte();
            serialNo      = chain.AllocByte();
            zDepth        = chain.AllocSInt16();
            type          = chain.AllocByte();
            basePoint     = chain.AllocByte();

            spares        = chain.Alloc(MBDataStructure.NumberOfSpares, IndexType.Word).AsSInt16();

            // ===== Validation
            var totalCount = chain.TotalByteCount();
            if(totalCount != MBDataFileSize.ByteCount) throw new InvalidProgramException("MBData size validation error: " + chain.TotalByteCount());

		}

        public static MBDataBinarizer Read(EndianFormatter formatter, Byte[] array)
        {
			Byte[] store = new Byte[array.Length];
			Array.Copy(array, 0, store, 0, MBDataFileSize.ByteCount);
            var programmer = Programmer.ReadCommandData(formatter, store, CommandDataType.R);
            return new MBDataBinarizer(programmer);
        }


		public MBDataBinarizer (EndianFormatter formatter, MBData src) : this (
            new Programmer(formatter, new byte[MBDataFileSize.ByteCount])
        ){
			SetTextProperties(src.Text, src.Type);
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
            SerialNo = src.SerialNo;
            //var nullableSerialNumber = MatchingGrammer.SearchSerialNo(Text);
            //SerialNo = (nullableSerialNumber != null) ? (byte)nullableSerialNumber : (byte)0;

            ZDepth = src.ZDepth;
			Type = src.Type;
			BasePoint = src.BasePoint;

			for (int i = 0; i < MBDataStructure.NumberOfSpares; i++) {
				SetSpare (i, src.Spares [i]);
			}
            {
				var bytes = Data.GetAllBytes();
                CommunicationLogger.Supply().Warn(BinalizerUtil.BytesToString(bytes));
                foreach( var quad in bytes.TetraPack()) 
                {
                    var mes = BinalizerUtil.BytesToString(quad.ToBytes());
                    CommunicationLogger.Supply().Info(mes);
                }
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
				Log.Debug ("PPTの読み込み" + "テキストの変換" + e);
			}

			return new MBData (model);
		}

        public byte[] ToCommandFormat()
        {
            return data.ToCommandFormat(CommandDataType.R);
        }
	}
}
