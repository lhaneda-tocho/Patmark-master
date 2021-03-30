using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class ConvexHull : ICollision
	{
		private Cartesian2D[] vertices;
		private Cartesian2D[] normals;

		public Cartesian2D[] Vertices {
			get{
				return vertices.Clone () as Cartesian2D[];
			}
		}

		public static ConvexHull NewTriangle(
			Homogeneous2D p0, Homogeneous2D p1, Homogeneous2D p2)
		{
			return new ConvexHull (new Homogeneous2D[]{p0, p1, p2});
		}


		public static ConvexHull NewRectangle(
			Homogeneous2D p0, Homogeneous2D p1, Homogeneous2D p2, Homogeneous2D p3)
		{
			return new ConvexHull (new Homogeneous2D[]{p0, p1, p2, p3});
		}


		public ConvexHull (Homogeneous2D[] vs)
		{
			var vert = (Homogeneous2D[]) vs.Clone ();

			this.vertices = vert.ToCartesian ();
			this.normals = new Cartesian2D[vertices.Length];

			var rot = AffineMatrix2D.NewRotation (-90);

			for (int i = 0; i < vertices.Length; i++) {
				int j = (i + 1) % vertices.Length;

				var edge = vertices [j] - vertices [i];
				var normal = (rot * edge.ToHomogeneous ()).ToCartesian ().Normalize (2);

				normals [i] = normal;
			}
        }

        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }



        public bool At (Homogeneous2D point)
		{
			var pointCartesian = point.ToCartesian ();
			return PointInConvexHull(pointCartesian, vertices);
		}



		public bool OnCircle (Homogeneous2D targetOrigin, double radius)
		{
			var targetOriginCartesian = targetOrigin.ToCartesian ();

			bool collided = false;

			// 点と円の衝突判定
			for (int i = 0; i < vertices.Length; i++) 
				collided |= PointInCircle (targetOriginCartesian, vertices [i], radius);
			

			// 点と凸包の衝突判定
			collided |= PointInConvexHull (targetOriginCartesian, Increase(radius));

			return collided;
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


		
		private Cartesian2D[] Increase(double size)
		{
			// 元の頂点数の2倍
			var inc = new Cartesian2D[vertices.Length * 2];

			for (int i = 0; i < vertices.Length; i++) {
				
				int j = (i + 1) % vertices.Length;

				var currentVertex = vertices [i];
				var nextVertex = vertices [j];
				var increase = size * normals [i];

				inc [i * 2] = currentVertex + increase;
				inc [i * 2 + 1] = nextVertex + increase;
			}

			return inc;
		}




		// Utilities



		private static bool PointInCircle (Cartesian2D point, Cartesian2D origin, double radius)
		{
			var distance = (point - origin).Norm (2);
			return distance <= radius;
		}

		private static bool PointInRightSide (Cartesian2D point, Cartesian2D start, Cartesian2D end)
		{
			var edge = end - start;
			var toPoint = point - end;

			var outer = edge.OuterProduct (toPoint);
			return outer >= 0;
		}


		private static bool PointInConvexHull (Cartesian2D point, Cartesian2D[] vs)
		{
			for (int i = 0; i < vs.Length; i++)
            { 
				int j = (i + 1) % vs.Length;
                // すべて満たす必要がある
                bool result = PointInRightSide (point, vs[i], vs[j]);
                if (!result) 
                    return false;
			}
            return true;
		}


		override
		public String ToString()
		{
			var mes = GetType ().FullName + "(\n";

			foreach( var e in vertices )
			{
				mes += "  " + e.ToString () + "\n";
			}

			mes += ") end of " + GetType ().Name;

			return mes;
		}

	}
}

