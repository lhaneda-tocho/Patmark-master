using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public struct BigEndDWord
    {
        // 上位バイトから順番に
        public byte Byte0;
        public byte Byte1;
        public byte Byte2;
        public byte Byte3;


        public DWord SystemDWord {
            get {
                DWord dword = default(DWord);
                if (BitConverter.IsLittleEndian) {
                    dword.Byte0 = Byte3;
                    dword.Byte1 = Byte2;
                    dword.Byte2 = Byte1;
                    dword.Byte3 = Byte0;
                } else {
                    dword.Byte0 = Byte0;
                    dword.Byte1 = Byte1;
                    dword.Byte2 = Byte2;
                    dword.Byte3 = Byte3;
                }
                return dword;
            }
            set {
                if (BitConverter.IsLittleEndian) {
                    Byte0 = value.Byte3;
                    Byte1 = value.Byte2;
                    Byte2 = value.Byte1;
                    Byte3 = value.Byte0;
                } else {
                    Byte0 = value.Byte0;
                    Byte1 = value.Byte1;
                    Byte2 = value.Byte2;
                    Byte3 = value.Byte3;
                }
            }
        }

        public Int32 Int32 {
            get {
                return SystemDWord.Int32;
            }
            set {
                DWord dword = default(DWord);
                dword.Int32 = value;
                SystemDWord = dword;
            }
        }

        public UInt32 UInt32 {
            get {
                return SystemDWord.UInt32;
            }
            set {
                DWord dword = default(DWord);;
                dword.UInt32 = value;
                SystemDWord = dword;
            }
        }

        public float Float {
            get {
                return SystemDWord.Float;
            }
            set {
                DWord dword = default(DWord);;
                dword.Float = value;
                SystemDWord = dword;
            }
        }

        public Byte[] Array {
            get {
                return new Byte[] { Byte0, Byte1, Byte2, Byte3 };
            }
            set {
                Byte0 = value [0];
                Byte1 = value [1];
                Byte2 = value [2];
                Byte3 = value [3];
            }
        }
    }
}

