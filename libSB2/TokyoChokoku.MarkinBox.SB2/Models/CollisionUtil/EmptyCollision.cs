using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
	public class EmptyCollision : ICollision
	{
		private static readonly EmptyCollision Instance;

		static EmptyCollision () {
			Instance = new EmptyCollision ();
		}

		public static EmptyCollision Create () {
			return Instance;
		}

		private EmptyCollision ()
		{
        }

        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }

        public bool At (Homogeneous2D point)
		{
			return false;
		}

		public bool OnCircle (Homogeneous2D origin, double radius)
		{
			return false;
		}

        public bool InBox (RectangleArea rect)
        {
            return false;
        }
	}
}

