using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public struct BigEndWord
    {
        // 上位バイトから順番に
        public byte Byte0;
        public byte Byte1;


        public Word SystemWord {
            get {
                Word word = default(Word);
                if (BitConverter.IsLittleEndian) {
                    word.Byte0 = Byte1;
                    word.Byte1 = Byte0;
                } else {
                    word.Byte0 = Byte0;
                    word.Byte1 = Byte1;
                }
                return word;
            }
            set {
                if (BitConverter.IsLittleEndian) {
                    Byte0 = value.Byte1;
                    Byte1 = value.Byte0;
                } else {
                    Byte0 = value.Byte0;
                    Byte1 = value.Byte1;
                }
            }
        }

        public Int16 Int16 {
            get {
                return SystemWord.Int16;
            }
            set {
                Word word = default(Word);
                word.Int16 = value;
                SystemWord = word;
            }
        }

        public UInt16 UInt16 {
            get {
                return SystemWord.UInt16;
            }
            set {
                Word word = default(Word);;
                word.UInt16 = value;
                SystemWord = word;
            }
        }

        public Char Char {
            get {
                return SystemWord.Char;
            }
            set {
                Word word = default(Word);;
                word.Char = value;
                SystemWord = word;
            }
        }

        public Byte[] Array {
            get {
                return new Byte[] { Byte0, Byte1 };
            }
            set {
                Byte0 = value [0];
                Byte1 = value [1];
            }
        }
    }
}

