using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using Smdn.Formats.Ini;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
	public partial class MBDataTextizer
	{
		private readonly IniDocument doc;

		private MBDataTextizer (IniDocument doc)
		{
			this.doc = doc;
		}

		public static MBDataTextizer CreateEmpty () {
			var empty = new MBDataTextizer ( new IniDocument () );

			// セクション作成
			var baseSection = empty.doc["BASE"];
			baseSection ["Kishu"] = "0";

			return empty;
		}

		public static MBDataTextizer Of ( StringReader input ) {
			var doc = IniDocument.Load (input);
			return new MBDataTextizer (doc);
		}



		public List<MBData> ToMBData () {
			var loadedData = new List<MBData> ();

			foreach ( var section in doc.Sections ) {
				if ( ReadableSection (section) ) {
					loadedData.Add ( Read (section) );
				}
			}

			return loadedData;
		}

		// [Field…] セクションの判定に利用．
		private static bool ReadableSection(IniSection section)
		{
			return !String.IsNullOrEmpty (section.Name) 
				&& section.Name.Contains ("FIELD");
		}



		// セーブデータ(iniファイル)から読み込んだデータを用いてパラメータを初期化
		private static MBData Read (IniSection section)
		{
			MBDataStructure model = new MBDataStructure ();

            model.Type          = ReadType (section, model);
			model.Text          = ReadText (section, model);
            model.Mode          = ReadMode (section, model);
            model.Id            = ReadId (section, model);
            model.X             = ReadX (section, model);
            model.Y             = ReadY (section, model);
            model.Height        = ReadHeight (section, model);
            model.Pitch         = ReadPitch (section, model);
            model.Aspect        = ReadAspect (section, model);
            model.Angle         = ReadAngle (section, model);
            model.ArcRadius     = ReadRadius (section, model);
            model.Speed         = ReadSpeed(section, model);
            model.Density       = ReadDensity(section, model);
            model.Power         = ReadPower (section, model);
            model.LinkFlug      = ReadLinkEnables (section, model);
            model.Links [0]     = ReadLink1 (section, model);
            model.Links [1]     = ReadLink2 (section, model);
            model.Links [2]     = ReadLink3 (section, model);
            model.Links [3]     = ReadLink4 (section, model);
            model.Links [4]     = ReadLink5 (section, model);
            model.OptionSw      = ReadRectSw(section, model);
            model.SerialNo      = ReadSerialSetNo (section, model);
            model.BasePoint     = ReadBasePoint (section, model);
            model.PrmFl         = ReadPrmFl (section, model);

			// タイプごとの固有処理
            ReadEachStructure (section, model);

			return new MBData (model);
		}


        static byte ReadType (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Type, model.Type, (val) => Byte.Parse (val));
        }
        static string ReadText (IniSection section, MBDataStructure model)
        {
            return section.Get (Name.Text, model.Text, (val) => val);
        }
        static ushort ReadMode (IniSection section, MBDataStructure model)
        {
            return (ushort)section.Get (Name.Mode, model.Mode, (val) => Convert.ToInt32 (val, 16));
        }
        static short ReadId (IniSection section, MBDataStructure model)
        {
            return section.Get (Name.Id, model.Id, (val) => Convert.ToInt16 (val));
        }
        static float ReadX (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.X, model.X, (val) => Convert.ToDouble (val));
        }
        static float ReadY (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Y, model.Y, (val) => Convert.ToDouble (val));
        }
        static float ReadHeight (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Height, model.Height, (val) => Convert.ToDouble (val));
        }
        static float ReadPitch (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Pitch, model.Pitch, (val) => Convert.ToDouble (val));
        }
        static float ReadAspect (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Aspect, model.Aspect, (val) => Convert.ToDouble (val));
        }
        static float ReadAngle (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Angle, model.Angle, (val) => Convert.ToDouble (val));
        }
        static float ReadRadius (IniSection section, MBDataStructure model)
        {
            return (float)section.Get (Name.Radius, model.ArcRadius, (val) => Convert.ToDouble (val));
        }
        static short ReadSpeed(IniSection section, MBDataStructure model)
        {
            return section.Get(Name.Speed, model.Speed, (val) => Convert.ToInt16(val));
        }
        static short ReadDensity(IniSection section, MBDataStructure model)
        {
            return section.Get(Name.Density, model.Density, (val) => Convert.ToInt16(val));
        }
        static short ReadPower (IniSection section, MBDataStructure model)
        {
            return section.Get (Name.Power, model.Power, (val) => Convert.ToInt16 (val));
        }
        static byte ReadLinkEnables (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.LinkEnables, model.LinkFlug, (val) => Convert.ToByte (val));
        }
        static byte ReadLink1 (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Link1, 0, (val) => Convert.ToByte (val));
        }
        static byte ReadLink2 (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Link2, 0, (val) => Convert.ToByte (val));
        }
        static byte ReadLink3 (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Link3, 0, (val) => Convert.ToByte (val));
        }
        static byte ReadLink4 (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Link4, 0, (val) => Convert.ToByte (val));
        }
        static byte ReadLink5 (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.Link5, 0, (val) => Convert.ToByte (val));
        }
        static byte ReadRectSw (IniSection section, MBDataStructure model)
        {
            return section.Get (Name.RectSw, model.OptionSw, (val) => Convert.ToByte (val));
        }
        static byte ReadSerialSetNo (IniSection section, MBDataStructure model)
        {
            return section.Get (Name.SerialSetNo, model.SerialNo, (val) => Convert.ToByte (val));
        }
        static byte ReadBasePoint (IniSection section, MBDataStructure model)
        {
            return (byte)section.Get (Name.BasePoint, model.BasePoint, (val) => Convert.ToByte (val));
        }
        static short ReadPrmFl (IniSection section, MBDataStructure model)
        {
            return (short)section.Get (Name.ParameterFlag, model.PrmFl, (val) => Convert.ToInt16 (val));
        }



        private IniSection InitFieldSection (int id) {
			var fieldSection = doc[ "FIELD" +id ];

			fieldSection[ Name.Id               ] =    "1";
			fieldSection[ Name.ParameterFlag            ] =    "0";
			fieldSection[ Name.X                ] =    "0";
			fieldSection[ Name.Y                ] =    "2";
			fieldSection[ Name.Height           ] =    "2";
			fieldSection[ Name.Pitch            ] =    "4";
			fieldSection[ Name.Aspect           ] =  "100";
			fieldSection[ Name.Mode             ] =    "0x00000000";
			fieldSection[ Name.Angle            ] =    "0";
			fieldSection[ Name.Radius           ] =    "0";
			fieldSection[ Name.Speed            ] =    "5";
			fieldSection[ Name.Power            ] =    "0";
			//fieldSection[ Name.HostVersion      ] =    "1";
			fieldSection[ Name.dd               ] =     "";
			fieldSection[ Name.Text             ] = "Text";
			fieldSection[ Name.SerialSetNo      ] =    "0";
			fieldSection[ Name.NowSerialNo      ] =    "0";
			fieldSection[ Name.ArcCenterX       ] =    "0";
			fieldSection[ Name.ArcCenterY       ] =    "0";
			fieldSection[ Name.FieldNumber      ] =     "";
			fieldSection[ Name.MrkDir           ] =    "0";
			fieldSection[ Name.LinkEnables      ] =    "0";
			fieldSection[ Name.Link1            ] =    "0";
			fieldSection[ Name.Link2            ] =    "0";
			fieldSection[ Name.Link3            ] =    "0";
			fieldSection[ Name.Link4            ] =    "0";
			fieldSection[ Name.Link5            ] =    "0";
			fieldSection[ Name.PauseExecute     ] =    "0";
			fieldSection[ Name.ProportF         ] =    "0";
			fieldSection[ Name.RectSw           ] =    "0";
			fieldSection[ Name.stx              ] =    "0";
			fieldSection[ Name.sty              ] =  "0.0";
			fieldSection[ Name.enx              ] =  "0.0";
			fieldSection[ Name.eny              ] =  "0.0";
			fieldSection[ Name.p_ht             ] =  "0.0";
			fieldSection[ Name.wd               ] =  "0.0";
			fieldSection[ Name.OpMode           ] =    "0";
			fieldSection[ Name.MouseClickX      ] =    "0";
			fieldSection[ Name.MouseClickY      ] =    "0";
			fieldSection[ Name.Type             ] =    "0";
			fieldSection[ Name.p_CenterX        ] =  "0.0";
			fieldSection[ Name.p_CenterY        ] =  "0.0";
			fieldSection[ Name.p_Radius         ] =  "0.0";
			fieldSection[ Name.p_X3             ] =  "0.0";
			fieldSection[ Name.p_Y3             ] =  "0.0";
			fieldSection[ Name.p_X4             ] =  "0.0";
			fieldSection[ Name.p_Y4             ] =  "0.0";
			fieldSection[ Name.p_X13            ] =  "0.0";
			fieldSection[ Name.p_Y13            ] =  "0.0";
			fieldSection[ Name.p_X14            ] =  "0.0";
			fieldSection[ Name.p_Y14            ] =  "0.0";
			fieldSection[ Name.p_ArcLng         ] =  "0.0";
			fieldSection[ Name.rel_Angle        ] =  "0.0";
			fieldSection[ Name.ZoomFct          ] =  "0.0";
			fieldSection[ Name.BasePoint        ] =    "0";
			fieldSection[ Name.Layer_WORK_SHAPE ] =    "0";

			return fieldSection;
		}

		public void Put (int sectionId, MBData data) {

			var fieldSection = InitFieldSection (sectionId);
			fieldSection[ Name.Id          ] = data.Id          .ToString ();
			fieldSection[ Name.X           ] = data.X           .ToString ();
			fieldSection[ Name.Y           ] = data.Y           .ToString ();
			fieldSection[ Name.Height      ] = data.Height      .ToString ();
			fieldSection[ Name.Pitch       ] = data.Pitch       .ToString ();
			fieldSection[ Name.Aspect      ] = data.Aspect      .ToString ();
            fieldSection[ Name.Mode        ] = string.Format ("0x{0:X8}", data.Mode);
			fieldSection[ Name.Angle       ] = data.Angle       .ToString ();
			fieldSection[ Name.Radius      ] = data.ArcRadius   .ToString ();
			fieldSection[ Name.Speed       ] = data.Speed       .ToString ();
			fieldSection[ Name.Power       ] = data.Power       .ToString ();
            fieldSection[ Name.Density     ] = data.Density.ToString();
            fieldSection[ Name.HostVersion ] = data.HostVersion .ToString ();
			fieldSection[ Name.Text        ] = data.Text;
			fieldSection[ Name.SerialSetNo ] = data.SerialNo    .ToString ();
			fieldSection[ Name.LinkEnables ] = data.LinkFlug    .ToString ();
			fieldSection[ Name.Link1       ] = data.Links[0]    .ToString ();
			fieldSection[ Name.Link2       ] = data.Links[1]    .ToString ();
			fieldSection[ Name.Link3       ] = data.Links[2]    .ToString ();
			fieldSection[ Name.Link4       ] = data.Links[3]    .ToString ();
			fieldSection[ Name.Link5       ] = data.Links[4]    .ToString ();
			// fieldSection[ "PauseExe" ] = ;
			fieldSection[ Name.RectSw      ] = data.OptionSw    .ToString ();
			fieldSection[ Name.Type        ] = data.Type        .ToString ();
			fieldSection[ Name.BasePoint   ] = data.BasePoint   .ToString ();

            // フィールドごとの固有処理
            WriteEachStructure (data, fieldSection);
		}


		public void Save (TextWriter output) {
			doc.Save(output);
		}


		public static void Save (TextWriter output, MBData[] data) {
			var doc = CreateEmpty ();

			{
				int i = 1;
				foreach (var e in data) {
					doc.Put (i++, e);
				}
			}

			doc.Save ( output );
		}

        public static void Save (TextWriter output, IEnumerable <MBData> data)
        {
            var doc = CreateEmpty ();

            {
                int i = 1;
                foreach (var e in data) {
                    doc.Put (i++, e);
                }
            }

            doc.Save (output);
        }


		public static void Save (TextWriter output, Owner[] owners) {
			var doc = CreateEmpty ();


			{
				int i = 1;
				foreach (var e in owners) {
					var data = e.ToSerializable ();
					doc.Put (i++, data);
				}
			}

			doc.Save ( output );
		}


        static void ReadTextFlag (IniSection section, MBDataStructure raw)
        {
            var flag = (int)section.Get (Name.ParameterFlag, 0, Convert.ToInt32);
            switch (flag) {
            default:
                raw.PrmFl = FontMode.FontTC.ToPrmFl ();
                break;
            case 1:
                raw.PrmFl = FontMode.Font5x7Dot.ToPrmFl ();
                break;
            case 4:
                raw.PrmFl = FontMode.FontPC.ToPrmFl ();
                break;
            }
        }
        static void WriteTextFlag (MBData raw, IniSection section)
        {
            var font = FontModeExt.ValueOf (raw.PrmFl);
            section [Name.ParameterFlag] = font.ToPpgParameterFlag ().ToString ();
        }


        // Type別に 行う処理．バイナリ形式と仕様を合わせる．
        // この時点で mbdataに Type値が入っている．

        static partial void ReadOuterArcText (IniSection section, MBDataStructure raw)
        {
            ReadTextFlag (section, raw);
        }

        static partial void WriteOuterArcText (MBData raw, IniSection section)
        {
            WriteTextFlag (raw, section);
        }



        static partial void ReadInnerArcText (IniSection section, MBDataStructure raw)
        {
            ReadTextFlag (section, raw);
        }

        static partial void WriteInnerArcText (MBData raw, IniSection section)
        {
            WriteTextFlag (raw, section);
        }



        static partial void ReadXVerticalText (IniSection section, MBDataStructure raw)
        {
            ReadTextFlag (section, raw);
        }

        static partial void WriteXVerticalText (MBData raw, IniSection section)
        {
            WriteTextFlag (raw, section);
        }



        static partial void ReadYVerticalText (IniSection section, MBDataStructure raw)
        {
            ReadTextFlag (section, raw);
        }

        static partial void WriteYVerticalText (MBData raw, IniSection section)
        {
            WriteTextFlag (raw, section);
        }



        static partial void ReadHorizontalText (IniSection section, MBDataStructure raw)
        {
            ReadTextFlag (section, raw);
        }

        static partial void WriteHorizontalText (MBData raw, IniSection section)
        {
            WriteTextFlag (raw, section);
        }



        static partial void ReadQrCode (IniSection section, MBDataStructure raw)
        {
            var defaultValue = DataMatrixConstant.DotCount10x10;

            raw.PrmFl = FontMode.FontBarcode.ToPrmFl ();

            // データマトリクス用に 初期値を設定しておく．
            var dm = new DataMatrixWrapper (raw);
            dm.HorizontalDotCount = defaultValue.Horizontal;
            dm.VerticalDotCount   = defaultValue.Vertical;
        }

        static partial void WriteQrCode (MBData raw, IniSection section) {
            section [Name.ParameterFlag] = FontMode.FontBarcode.ToPpgParameterFlag ().ToString ();
        }


        static partial void ReadDataMatrix (IniSection section, MBDataStructure raw) {

            var defaultValue = DataMatrixConstant.DotCount10x10;

            raw.PrmFl = FontMode.FontBarcode.ToPrmFl ();

            var dm = new DataMatrixWrapper (raw);
            var flnm = section.Get (Name.FieldNumber);

            string [] values = flnm.Split ('x');

            switch (values.Length) {
                case 0: {
                    dm.VerticalDotCount = defaultValue.Vertical;
                    dm.HorizontalDotCount = defaultValue.Horizontal;
                    break;
                }
                case 1: {
                    byte vertical;
                    if (!byte.TryParse (values [0], out vertical))
                        vertical = defaultValue.Horizontal;
                    dm.VerticalDotCount   = vertical;
                    dm.HorizontalDotCount = vertical;
                    break;
                }
                default: {
                    byte horizontal, vertical;
                    if (!byte.TryParse (values [0], out vertical))
                        vertical   = defaultValue.Vertical;
                    if (!byte.TryParse (values [1], out horizontal))
                        horizontal = defaultValue.Horizontal;
                    dm.VerticalDotCount   = vertical;
                    dm.HorizontalDotCount = horizontal;
                    break;
                }
            }
        }

        static partial void WriteDataMatrix (MBData raw, IniSection section)
        {
            var dm = new DataMatrixWrapper (raw.ToMutable());
            var horizontal = dm.HorizontalDotCount;
            var vertical = dm.VerticalDotCount;

            section [Name.ParameterFlag] = FontMode.FontBarcode.ToPpgParameterFlag ().ToString ();

            if (horizontal == vertical) {
                section [Name.FieldNumber] = vertical.ToString ();
            } else {
                section [Name.FieldNumber] = vertical.ToString () + 'x' + horizontal;
            }

        }


        static partial void ReadLine (IniSection section, MBDataStructure raw) {
            raw.Angle     = (float) section.Get (Name.ArcCenterX , 0, (val) => Convert.ToDouble (val));
            raw.ArcRadius = (float) section.Get (Name.ArcCenterY , 0, (val) => Convert.ToDouble (val));
        }

        static partial void WriteLine (MBData raw, IniSection section)
        {
            section [Name.ArcCenterX] = raw.Angle.ToString ();
            section [Name.ArcCenterY] = raw.ArcRadius.ToString();
        }


		private static class Name {
			public static readonly string Id               =               "ID";
            public static readonly string ParameterFlag    =              "flg";
			public static readonly string X                =                "X";
			public static readonly string Y                =                "Y";
			public static readonly string Height           =               "Ht";
			public static readonly string Pitch            =            "Pitch";
			public static readonly string Aspect           =           "Aspect";
			public static readonly string Mode             =             "mode";
			public static readonly string Angle            =            "Angle";
			public static readonly string Radius           =           "Radius";
            public static readonly string Speed            = "Speed";
            public static readonly string Density          = "Density";
            public static readonly string Power            =            "Power";
			public static readonly string HostVersion      =     "Host_Version";
			public static readonly string dd               =               "dd";
			public static readonly string Text             =            "ddorg";
			public static readonly string SerialSetNo      =      "SerialSetNo";
			public static readonly string NowSerialNo      =      "NowSerialNo";
			public static readonly string ArcCenterX       =       "ArcCenterX";
			public static readonly string ArcCenterY       =       "ArcCenterY";
			public static readonly string FieldNumber      =             "FlNm";
			public static readonly string MrkDir           =           "MrkDir";
			public static readonly string LinkEnables      =        "FldLnkFlg";
			public static readonly string Link1            =          "FldLnk1";
			public static readonly string Link2            =          "FldLnk2";
			public static readonly string Link3            =          "FldLnk3";
			public static readonly string Link4            =          "FldLnk4";
			public static readonly string Link5            =          "FldLnk5";
			public static readonly string PauseExecute     =         "PauseExe";
			public static readonly string ProportF         =         "ProportF";
			public static readonly string RectSw           =           "RectSw";
			public static readonly string stx              =              "stx";
			public static readonly string sty              =              "sty";
			public static readonly string enx              =              "enx";
			public static readonly string eny              =              "eny";
			public static readonly string p_ht             =             "p_ht";
			public static readonly string wd               =               "wd";
			public static readonly string OpMode           =           "OpMode";
			public static readonly string MouseClickX      =      "MouseClickX";
			public static readonly string MouseClickY      =      "MouseClickY";
			public static readonly string Type             =             "Type";
			public static readonly string p_CenterX        =        "p_CenterX";
			public static readonly string p_CenterY        =        "p_CenterY";
			public static readonly string p_Radius         =         "p_Radius";
			public static readonly string p_X3             =             "p_X3";
			public static readonly string p_Y3             =             "p_Y3";
			public static readonly string p_X4             =             "p_X4";
			public static readonly string p_Y4             =             "p_Y4";
			public static readonly string p_X13            =            "p_X13";
			public static readonly string p_Y13            =            "p_Y13";
			public static readonly string p_X14            =            "p_X14";
			public static readonly string p_Y14            =            "p_Y14";
			public static readonly string p_ArcLng         =         "p_ArcLng";
			public static readonly string rel_Angle        =        "rel_Angle";
			public static readonly string ZoomFct          =          "ZoomFct";
			public static readonly string BasePoint        =      "KijyunPoint";
			public static readonly string Layer_WORK_SHAPE = "Layer_WORK_SHAPE";
		}

	}
}

