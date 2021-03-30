using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class HorizontalTextCollisionProductor
	{

        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (HorizontalText.Constant target)
        {
            return CreatePrecision (target);
        }

        /// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (HorizontalText.Constant target) {
            var p       = target.Parameter;
			var factory = new CollisionFactory ();

			// ベースポイントの位置と回転を設定する．
			factory.Transform.Translate ( (float)p.X, (float) p.Y);
			factory.Transform.Rotate ((float)-p.Angle);

			// テキストの位置を調整する．
			float startX, startY;

			switch (p.BasePoint) {
			default:
			case Consts.FieldBasePointLB:
				startX = 0f;
				startY = -p.BoxHeight;
				break;

			case Consts.FieldBasePointLM:
				startX = 0f;
				startY = -p.BoxHeight/2;
				break;

			case Consts.FieldBasePointLT:
				startX = 0f;
				startY = 0f;
				break;

			case Consts.FieldBasePointCB:
				startX = -p.BoxWidth/2;
				startY = -p.BoxHeight;
				break;

			case Consts.FieldBasePointCM:
				startX = -p.BoxWidth/2;
				startY = -p.BoxHeight/2;
				break;

			case Consts.FieldBasePointCT:
				startX = -p.BoxWidth/2;
				startY = 0f;
				break;

			case Consts.FieldBasePointRB:
				startX = -p.BoxWidth;
				startY = -p.BoxHeight;
				break;

			case Consts.FieldBasePointRM:
				startX = -p.BoxWidth;
				startY = -p.BoxHeight/2;
				break;

			case Consts.FieldBasePointRT:
				startX = -p.BoxWidth;
				startY = 0f;
				break;
			}

			factory.Transform.Translate (startX, startY);

			var p0 = new Homogeneous2D (         0,           0);
			var p1 = new Homogeneous2D (p.BoxWidth,           0);
			var p2 = new Homogeneous2D (p.BoxWidth, p.BoxHeight);
			var p3 = new Homogeneous2D (         0, p.BoxHeight);

			return factory.NewConvexRectangle (p0, p1, p2, p3);
		}
	}
}

