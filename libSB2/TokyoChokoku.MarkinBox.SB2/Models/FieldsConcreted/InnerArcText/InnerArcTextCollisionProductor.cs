using System;

using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class InnerArcTextCollisionProductor
	{

        /// <summary>
        /// タッチの判定用に衝突判定を作成します．
        /// </summary>
        /// <param name="target">Target.</param>
        public static ICollision Create (InnerArcText.Constant target)
        {
            return CreatePrecision (target);
        }

		/// <summary>
        /// はみ出し判定用に衝突判定を作成します．
        /// </summary>
        /// <returns>The precision.</returns>
        /// <param name="target">Target.</param>
        public static ICollision CreatePrecision (InnerArcText.Constant target) {
            var p       = target.Parameter;
			var factory = new CollisionFactory ();

			// ベースポイントの位置を合わせる
			factory.Transform.Translate ( (float)p.X, (float) p.Y);
			factory.Transform.Rotate ((float) -p.Angle);

			// 円の中心位置，始点半径，始点角度，終点半径，終点角度，扇型 中心点の角の大きさ[deg]を求める．
			var origin = new Homogeneous2D (0, p.DistanceBetweenBasePointAndCircleCenter);

			var startRadius = p.InnerRadius;
			var endRadius = p.OuterRadius;

			double startPhase;
			switch (p.BasePoint) {
			default:
			case Consts.FieldBasePointLB:
			case Consts.FieldBasePointLM:
			case Consts.FieldBasePointLT:
				startPhase = 0;
				break;

			case Consts.FieldBasePointCB:
			case Consts.FieldBasePointCM:
			case Consts.FieldBasePointCT:
				startPhase = -p.OuterTextLength * 0.5 / p.OuterRadius;
				startPhase = UnitConverter.Radians.ToDegrees(startPhase);
				break;

			case Consts.FieldBasePointRB:
			case Consts.FieldBasePointRM:
			case Consts.FieldBasePointRT:
				startPhase = -p.OuterTextLength / p.OuterRadius;
				startPhase = UnitConverter.Radians.ToDegrees(startPhase);
				break;
			}

			double endPhase = p.OuterTextLength / p.OuterRadius;
			endPhase = UnitConverter.Radians.ToDegrees(endPhase);
			endPhase += startPhase;

			return factory.NewArcText (origin, startRadius, startPhase, endRadius, endPhase);
		}
	}
}

