using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public interface FieldDrawable
	{
        void Draw          (FieldCanvas canvas);
        void DrawBorder    (FieldCanvas canvas);
        void DrawBasePoint (FieldCanvas canvas);
	}
}

