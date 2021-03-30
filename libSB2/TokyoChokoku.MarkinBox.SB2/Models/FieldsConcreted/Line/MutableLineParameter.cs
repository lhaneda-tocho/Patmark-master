using System;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
    public partial class MutableLineParameter : IMutableParameter
    {
        public decimal X {
            get {
                return StartX;
            }

            set {
                CenterX = CenterX - StartX + value;
                EndX    = EndX    - StartX + value;
                StartX  = value;
            }
        }

        public decimal Y {
            get {
                return StartY;
            }

            set {
                CenterY = CenterY - StartY + value;
                EndY    = EndY    - StartY + value;
                StartY  = value;
            }
        }

        public XStore XStore { get; private set; }


        public YStore YStore { get; private set; }


        partial void Initialize (ClosedValidator validator) {
            XStore = new XStore (
                GeometryValidator.ValidateX,
                (     ) => { return X;  },
                (value) => { X = value; }
            );

            YStore = new YStore (
                GeometryValidator.ValidateY,
                (     ) => { return Y;  },
                (value) => { Y = value; }
            );
        }
    }
}

