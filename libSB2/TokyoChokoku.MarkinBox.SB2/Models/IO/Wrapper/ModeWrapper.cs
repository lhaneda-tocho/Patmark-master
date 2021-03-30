using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class ModeWrapper
    {
        protected readonly MBDataStructure data;


        public ModeWrapper (MBDataStructure data)
        {
            this.data = data;
        }

        public bool Calender {
            get { return (data.Mode & ModeBit.CALENDER) == ModeBit.CALENDER; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.CALENDER;
                else
                    data.Mode &= (ushort) ~ModeBit.CALENDER;
            }
        }

        public bool Serial {
            get { return (data.Mode & ModeBit.SERIAL) == ModeBit.SERIAL; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.SERIAL;
                else
                    data.Mode &= (ushort) ~ModeBit.SERIAL;
            }
        }

        public bool OuterArc {
            get { return (data.Mode & ModeBit.OUTER_ARC) == ModeBit.OUTER_ARC; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.OUTER_ARC;
                else
                    data.Mode &= (ushort) ~ModeBit.OUTER_ARC;
            }
        }

        public bool InnerArc {
            get { return (data.Mode & ModeBit.INNER_ARC) == ModeBit.INNER_ARC; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.INNER_ARC;
                else
                    data.Mode &= (ushort) ~ModeBit.INNER_ARC;
            }
        }

        public bool Triangle {
            get { return (data.Mode & ModeBit.TRIANGLE) == ModeBit.TRIANGLE; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.TRIANGLE;
                else
                    data.Mode &= (ushort) ~ModeBit.TRIANGLE;
            }
        }

        public bool QrCode {
            get { return (data.Mode & ModeBit.QR_CODE) == ModeBit.QR_CODE; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.QR_CODE;
                else
                    data.Mode &= (ushort) ~ModeBit.QR_CODE;
            }
        }

        public bool DataMatrix {
            get { return (data.Mode & ModeBit.DATA_MATRIX) == ModeBit.DATA_MATRIX; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.DATA_MATRIX;
                else
                    data.Mode &= (ushort) ~ModeBit.DATA_MATRIX;
            }
        }

        public bool VerticalY {
            get { return (data.Mode & ModeBit.VERTICAL_Y) == ModeBit.VERTICAL_Y; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.VERTICAL_Y;
                else
                    data.Mode &= (ushort) ~ModeBit.VERTICAL_Y;
            }
        }

        public bool VerticalX {
            get { return (data.Mode & ModeBit.VERTICAL_X) == ModeBit.VERTICAL_X; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.VERTICAL_X;
                else
                    data.Mode &= (ushort) ~ModeBit.VERTICAL_X;
            }
        }

        public bool RectangleLineTriangle {
            get { return (data.Mode & ModeBit.RECTANGLE_LINE_TRIANGLE) == ModeBit.RECTANGLE_LINE_TRIANGLE; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.RECTANGLE_LINE_TRIANGLE;
                else
                    data.Mode &= (ushort) ~ModeBit.RECTANGLE_LINE_TRIANGLE;
            }
        }

        public bool CircleEllipse {
            get { return (data.Mode & ModeBit.CIRCLE_ELLIPSE) == ModeBit.CIRCLE_ELLIPSE; }
            set { 
                if (value)
                    data.Mode |=  ModeBit.CIRCLE_ELLIPSE;
                else
                    data.Mode &= (ushort) ~ModeBit.CIRCLE_ELLIPSE;
            }
        }

        public ushort BitSet {
            get {
                return data.Mode;
            }
            set {
                data.Mode = value;
            }
        }

        public void Clear () {
            BitSet = 0;
        }
    }
}

