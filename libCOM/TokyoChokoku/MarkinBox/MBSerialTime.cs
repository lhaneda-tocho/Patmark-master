using System;
namespace TokyoChokoku.MarkinBox
{

    /// <summary>
    /// MBSerialTime.
    /// </summary>
    public struct MBSerialTime
    {
        public const UInt16 StencilH10 = 0x0FFF;
        public const UInt16 StencilH1  = 0xF0FF;
        public const UInt16 StencilM10 = 0xFF0F;
        public const UInt16 StencilM1  = 0xFFF0;


        /// <summary>
        /// Init with the specified hhmm.
        /// </summary>
        /// <returns>the initialized value.</returns>
        /// <param name="hhmm">Hhmm.</param>
        public static MBSerialTime Init(UInt16 hhmm) {
            var ins = new MBSerialTime();
            ins.HHMM = hhmm;
            return ins;
        }

        /// <summary>
        /// Init with the specified hour and minute.
        /// </summary>
        /// <returns>the initialized value.</returns>
        /// <param name="hour">Hour.</param>
        /// <param name="minute">Minute.</param>
        public static MBSerialTime Init(byte hour, byte minute)
        {
            var ins = new MBSerialTime();
            ins.Hour   = hour;
            ins.Minute = minute;
            return ins;
        }

        /// <summary>
        /// The hhmm.
        /// </summary>
        public UInt16 HHMM;

        internal byte H10 {
            get
            {
                var v = ((HHMM >> 12) & 0xF) % 10;
                return (byte)v;
            }
            set 
            {
                var tetra   = (UInt16)( (value % 10) << 12 );
                var stencil = StencilH10;
                HHMM = (UInt16)( HHMM & stencil | tetra );
            }
        }

        internal byte H1 {
            get
            {
                var v = ((HHMM >>  8) & 0xF) % 10;
                return (byte)v;
            }
            set
            {
                var tetra   = (UInt16)( (value % 10) <<  8 );
                var stencil = StencilH1;
                HHMM = (UInt16)( HHMM & stencil | tetra );
            }
        }

        internal byte M10 {
            get 
            {
                var v = ((HHMM >>  4) & 0xF) % 10;
                return (byte)v;
            }
            set
            {
                var tetra   = (UInt16)( (value % 10) <<  4 );
                var stencil = StencilM10;
                HHMM = (UInt16)( HHMM & stencil | tetra );
            }
        }

        internal byte M1 {
            get
            {
                var v = ((HHMM      ) & 0xF) % 10;
                return (byte)v;
            }
            set
            {
                var tetra   = (UInt16)( (value % 10)       );
                var stencil = StencilM1;
                HHMM = (UInt16)( HHMM & stencil | tetra );
            }
        }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public byte Hour {
            get
            {
                return (byte)(H10 * 10 + H1);
            }
            set
            {
                H10 = (byte)(value / 10);
                H1  = (byte)(value     );
            }
        }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        /// <value>The minute.</value>
        public byte Minute {
            get
            {
                return (byte)(M10 * 10 + M1);
            }
            set
            {
                M10 = (byte) (value / 10);
                M1  = (byte) (value     );
            }
        }
    }
}
