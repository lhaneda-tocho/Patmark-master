using System;
namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class OptionWrapper
    {
        private readonly MBDataStructure data;

        private const byte MIRROR_BIT      = 0x01; // Bit 0
        private const byte NOT_MIRROR_BIT  = 0xFE;

        private const byte REVERSE_BIT     = 0x04; // Bit 2
        private const byte NOT_REVERSE_BIT = 0xFB; // Bit 2

        private const byte PAUSE_BIT       = 0x80; // Bit 7
        private const byte NOT_PAUSE_BIT   = 0x7F;

        public OptionWrapper (MBDataStructure data)
        {
            if (data == null)
                throw new NullReferenceException ();
            this.data = data;
        }

        public bool Mirrored {
            get {
                return (data.OptionSw & MIRROR_BIT) != 0;
            }
            set {
                if (value)
                    data.OptionSw |=  MIRROR_BIT;
                else
                    data.OptionSw &=  NOT_MIRROR_BIT;
            }
        }

        public bool Reverse {
            get {
                return (data.OptionSw & REVERSE_BIT) != 0;
            }
            set {
                if (value)
                    data.OptionSw |= REVERSE_BIT;
                else
                    data.OptionSw &= NOT_REVERSE_BIT;
            }
        }

        /// <summary>
        /// 鏡写しかどうか
        /// </summary>
        /// <value><c>true</c> 鏡写しの時; そうでなければ, <c>false</c>.</value>
        public bool Pause {
            get {
                return (data.OptionSw & PAUSE_BIT) != 0;
            }
            set {
                if (value)
                    data.OptionSw |= PAUSE_BIT;
                else
                    data.OptionSw &= NOT_PAUSE_BIT;
            }
        }


    }
}

