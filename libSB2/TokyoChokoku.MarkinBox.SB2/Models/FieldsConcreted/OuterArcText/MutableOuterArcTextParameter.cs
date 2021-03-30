using System;

using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.Parameters
{
	public partial class MutableOuterArcTextParameter
	{
		
		public void ApplyPivotRotation(decimal degrees) {
			MatrixContext transform = new MatrixContext ();

			// 新しいベースポイントの位置を求める
			transform.Translate ((double)X, (double)Y);
			transform.Rotate ((double)-Angle);

			{
				var distance = DistanceBetweenBasePointAndCircleCenter;
				transform.Translate (0, distance);
				transform.Rotate ((double)degrees);
				transform.Translate (0, -distance);
			}

			var newBasePoint = transform.CurrentMatrix * new Position2D (0, 0);

			// 更新前の値
			var oldX = X;
			var oldY = Y;
			var oldAngle = Angle;

			// 新しい値
			var newX = (decimal)newBasePoint.X;
			var newY = (decimal)newBasePoint.Y;
			var newAngle = Angle - degrees;


			// バリデータの応答を見つつ 代入
			Boolean failed = false;
			ModificationListener listener = (result, sender) => failed = true;


			XStore.OnFailure += listener;
			X = newX;
			XStore.OnFailure -= listener;
			if (failed) {
				return;
			}


			YStore.OnFailure += listener;
			Y = newY;
			YStore.OnFailure -= listener;
			if (failed) {
				X = oldX;
				return;
			}

			AngleStore.OnFailure += listener;
			Angle = newAngle;
			AngleStore.OnFailure -= listener;
			if (failed) {
				X = oldX;
				Y = oldY;
				return;
			}

		}
	}
}

