using System;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public static class Validators
    {
        public static bool IsRectangleInCanvas (RectangleArea rect) {
            double startX = rect.X;
            double startY = rect.Y;
            double endX   = startX + rect.Width;
            double endY   = startY + rect.Height;


            var size = GetCanvasSize ();


            return 
                startX >= 0      &&
                endX   <= size.X &&
                startY >= 0      &&
                endY   <= size.Y  ;
        }

        static IntSize2D GetCanvasSize () {
            return new IntSize2D(
                MachineModelNoManager.Get().LatticeSize().X,
                MachineModelNoManager.Get().LatticeSize().Y
            );
        }

        public static Movement GetMovementRectangleIntoCanvas (RectangleArea rect) {
            double startX = rect.X;
            double startY = rect.Y;
            double endX   = startX + rect.Width;
            double endY   = startY + rect.Height;


            var size = GetCanvasSize ();

            double movementX = 0.0;
            double movementY = 0.0;


            // Canvas 右側・下側に位置調整．
            if (endX > size.X) 
                movementX = endX - size.X;
            
            if (endY > size.Y) 
                movementX = endX - size.Y;
            

            // Canvas 上側・左側に位置調整．
            if (startX < 0) 
                movementX = -startX;
            
            if (startY < 0) 
                movementX = -startY;


            // 右左共にはみ出す場合は 左側を優先．
            // 右左共にはみ出す場合は 上側を優先．
            return new Movement ((decimal) movementX, (decimal) movementY);
        }


        public sealed class Movement {
            public decimal X { get; }
            public decimal Y { get; }

            public Movement (decimal x, decimal y) {
                this.X = x;
                this.Y = y;
            }
        }
    }
}

