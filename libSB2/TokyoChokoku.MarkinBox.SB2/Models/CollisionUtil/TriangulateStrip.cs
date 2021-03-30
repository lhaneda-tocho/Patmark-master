using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class TriangulateStrip : ICollision
	{
		private readonly Cartesian2D[] vertices;

		public Cartesian2D[] Vertices {
			get {
				return (Cartesian2D[])vertices.Clone ();
			}
		}

		public TriangulateStrip (Homogeneous2D[] vertices)
		{
			this.vertices = vertices.ToCartesian ();
        }

        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }


        public bool At (Homogeneous2D hgpoint)
		{
			var point = hgpoint.ToCartesian ();
			var Next = NewRaster (vertices);


			bool result = false;

			for (var triangle = Next (); triangle != null; triangle = Next ()) {
				result |= PointInTriangle (point, triangle);
			}

			return result;
		}

		public bool OnCircle (Homogeneous2D hgorigin, double radius)
		{
			var origin = hgorigin.ToCartesian ();
			var Next = NewRaster (vertices);


			bool result = false;

			for (var triangle = Next (); triangle != null; triangle = Next ()) {
				result |= CircleOnTriangle (origin, radius, triangle);
			}

			return result;
		}

        /// <summary>
        /// 箱との衝突判定
        /// </summary>
        /// <returns><c>true</c>, if box was oned, <c>false</c> otherwise.</returns>
        public bool InBox (RectangleArea rect)
        {
            var sx = rect.X;
            var sy = rect.Y;
            var ex = sx + rect.Width;
            var ey = sy + rect.Height;

            bool collided = true;

            // 各点が箱の中にいるかを計算
            foreach (var point in vertices) {
                var m = sx <= point.X && point.X <= ex &&
                        sy <= point.Y && point.Y <= ey;
                collided &= m;
            }

            return collided;
        }


		private bool PointInTriangle(Cartesian2D point, Cartesian2D[] triangle)
		{
			var outer0 = (triangle [1] - triangle [0]).OuterProduct (point - triangle [1]);
			var outer1 = (triangle [2] - triangle [1]).OuterProduct (point - triangle [2]);
			var outer2 = (triangle [0] - triangle [2]).OuterProduct (point - triangle [0]);

			return (outer0 >= 0) && (outer1 >= 0) && (outer2 >= 0);
		}


		private bool CircleOnTriangle(Cartesian2D origin, double radius, Cartesian2D[] triangle)
		{
			bool result = true;

			result &= CircleOnSegmentsRightSide (origin, radius, triangle[0], triangle[1]);
			result &= CircleOnSegmentsRightSide (origin, radius, triangle[1], triangle[2]);
			result &= CircleOnSegmentsRightSide (origin, radius, triangle[2], triangle[0]);

			return result;
		}


		private bool CircleOnSegmentsRightSide(
			Cartesian2D origin, double radius, Cartesian2D start, Cartesian2D end)
		{
			var SE = end - start;
			var EP = origin - end;
			var SP = origin - start;

			var dotSE_EP = SE * EP;
			var dotSE_SP = SE * SP;


			// FIXME: コメント文の表現を もうすこし明確に！

			// 3つに場合分けする

			// 円が 点Aより外側
			if (dotSE_SP < 0 && dotSE_EP <= 0) {
				var distanceSP = SP.Norm (2);
				return distanceSP <= radius;
			} 

			// 円が 点A,Bの 間に存在．
			else if (dotSE_SP >= 0 && dotSE_EP <= 0) {
				var distanceBetweenOriginAndSegment = SE.Normalize (2).OuterProduct (EP);
				return distanceBetweenOriginAndSegment <= radius;
			}

			// 円が 点Bより外側
			else { // if (dotSE_SP >= 0 && dotSE_EP >= 0) {
				var distanceEP = EP.Norm (2);
				return distanceEP <= radius;
			}
		}



		private Func<Cartesian2D[]> NewRaster( Cartesian2D[] vertices )
		{
			int i = 2;

			var buffer = new Cartesian2D [3];
			buffer[1] = vertices[0];
			buffer[2] = vertices[1];

			return () => {
						if( i < vertices.Length ) {

							buffer[0] = buffer[1];
							buffer[1] = buffer[2];
							buffer[2] = vertices[i];

							++i;

							return (Cartesian2D[]) buffer.Clone ();
						} else {
							return null;
						}
			};
		}



	}
}

