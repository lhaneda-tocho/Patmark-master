using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class BarcodeWrapper
    {
        protected readonly MBDataStructure data;



        public BarcodeWrapper (MBDataStructure data) {
            this.data = data;
        }


        private BigEndDWord BarcodeStatus {
            get {
                var dword = default (BigEndDWord);
                dword.Float = data.Height;
                return dword;
            }

            set {
                data.Height = value.Float;
            }
        }

        private BigEndWord Spare0 {
            get {
                var word = default (BigEndWord);
                word.Int16 = data.Spares [0];
                return word;
            }

            set {
                data.Spares [0] = value.Int16;
            }
        }

        public byte BarcodeTypeByte
        {
            get
            {
                var dword = BarcodeStatus;
                return dword.Byte0;
            }
        }

        public BarcodeType BarcodeType  {
            get {
                var dword = BarcodeStatus;

                if (BarcodeTypeExt.IsDefined (dword.Byte0))
                    return (BarcodeType) dword.Byte0;
                else
                    return 0;
            }

            set {
                var dword   = BarcodeStatus;
                dword.Byte0 = (byte) value;
                BarcodeStatus = dword;
            }
        }

        public byte HorizontalDotCount {
            get {
                return BarcodeStatus.Byte1;
            }

            set {
                var dword = BarcodeStatus;
                dword.Byte1 = value;
                BarcodeStatus = dword;
            }
        }

        public byte DotPower {
            get {
                return BarcodeStatus.Byte2;
            }

            set {
                var dword = BarcodeStatus;
                dword.Byte2 = value;
                BarcodeStatus = dword;
            }
        }

        public byte VerticalDotCount
        {
            get {
                return Spare0.Byte0;
            }
            set {
                var word = Spare0;
                word.Byte0 = value;
                Spare0 = word;
            }
        }

        public void SetPrmFlBarcodeMode () {
            data.PrmFl = 50;
        }
    }
}

