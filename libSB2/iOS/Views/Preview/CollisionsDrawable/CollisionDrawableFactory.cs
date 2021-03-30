using System;
using TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class CollisionDrawableFactory
    {
        public static CollisionDrawableFactory Default { get; } = new CollisionDrawableFactory(
#pragma warning disable RECS0110 // 条件が常に 'true' か、常に 'false' です
            (AppMode.EnableCollisionVisualization) ?
#pragma warning restore RECS0110 // 条件が常に 'true' か、常に 'false' です
                new CollisionDrawableCreationVisitor()
            :
                new EmptyVisitor()
        );


        ICollisionVisitor<CollisionDrawableFactory, ICollisionDrawable> Visitor { get; }

        public CollisionDrawableFactory(ICollisionVisitor<CollisionDrawableFactory, ICollisionDrawable> visitor)
        {
            Visitor = visitor ?? throw new ArgumentNullException("null not allowed");
        }

        public ICollisionDrawable CreateFrom(ICollision collision)
        {
            return collision.Accept(Visitor, this);
        }

        public static ICollisionDrawable DefaultCreateFrom(ICollision collision)
        {
            return Default.CreateFrom(collision);
        }


        class EmptyVisitor : ICollisionVisitor<CollisionDrawableFactory, ICollisionDrawable>
        {
            protected ICollisionDrawable EmptyDrawable { get; } = new EmptyCollisionDrawable();

            public virtual ICollisionDrawable Visit(ConvexHull collision, /** NonNull */ CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }

            public virtual ICollisionDrawable Visit(EmptyCollision collision, CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }

            public virtual ICollisionDrawable Visit(CombinedCollision collision, CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }

            public virtual ICollisionDrawable Visit(PointCollision collision, CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }

            public virtual ICollisionDrawable Visit(TriangulateStrip collision, CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }

            public virtual ICollisionDrawable Visit(LineStrip collision, CollisionDrawableFactory args)
            {
                return EmptyDrawable;
            }
        }

        class CollisionDrawableCreationVisitor : EmptyVisitor
        {
            public override ICollisionDrawable Visit(ConvexHull collision, CollisionDrawableFactory args)
            {
                return new ConvexHullDrawable(collision);
            }

            public ICollisionDrawable Visit(CombinedCollision collision, CollisionDrawableFactory args)
            {
                // TODO: 実装
                return new CombinedCollisionDrawable(collision, args);
            }

            //public ICollisionDrawable Visit(PointCollision collision, CollisionDrawableFactory args)
            //{
            //    // TODO: 実装
            //    throw new NotImplementedException();
            //}

            //public ICollisionDrawable Visit(TriangulateStrip collision, CollisionDrawableFactory args)
            //{
            //    // TODO: 実装
            //    throw new NotImplementedException();
            //}

            //public ICollisionDrawable Visit(LineStrip collision, CollisionDrawableFactory args)
            //{
            //    // TODO: 実装
            //    throw new NotImplementedException();
            //}
        }
    }

    
}
