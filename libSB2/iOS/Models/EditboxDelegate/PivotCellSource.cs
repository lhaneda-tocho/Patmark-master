using System;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class PivotCellSource : IEditBoxCellDelegate
    {
        // Nonnull
        private IEditBoxCommonDelegate commonDelegate = EmptyEditBoxCommonDelegate.Instance;

        /// <summary>
        /// EditBoxCell 全てにおいて 共通で呼び出されるデリゲートを表します，
        /// このプロパティは null を返しません．
        /// もし，null を設定したのであれば，その代わりに EmptyEditBoxCommonDelegate.Instance が設定されます
        /// null チェックを行う場合，代わりに CommonDelegate == EmptyEditBoxCommonDelegate.Instance
        /// で実現できます．
        /// 
        /// </summary>
        /// <value>The common delegate.</value>
        protected IEditBoxCommonDelegate CommonDelegate {
            get {
                return commonDelegate;
            }
            set {
                if (value == null)
                    commonDelegate = EmptyEditBoxCommonDelegate.Instance;
                else
                    commonDelegate = value;
            }
        }

        public PivotCellSource (IEditBoxCommonDelegate commonDelegate)
        {
            CommonDelegate = commonDelegate;
        }

        public abstract void TouchedCCW ();
        public abstract void TouchedCW ();


        public static PivotCellSource Of (MutableInnerArcTextParameter p, IEditBoxCommonDelegate commonDelegate)
        {
            if (p == null)
                throw new System.NullReferenceException ();
            return new InnerArcTextSource (p, commonDelegate);
        }


        public static PivotCellSource Of (MutableOuterArcTextParameter p, IEditBoxCommonDelegate commonDelegate)
        {
            if (p == null)
                throw new System.NullReferenceException ();
            return new OuterArcTextSource (p, commonDelegate);
        }


        public void SetUpdatedListener (Action<IEditBoxCellDelegate> action)
        {
            // 状態を持たないので，何も起きません．
        }


        private class InnerArcTextSource : PivotCellSource {
            private readonly MutableInnerArcTextParameter p;

            public InnerArcTextSource (MutableInnerArcTextParameter parameter, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
            {
                p = parameter;
            }

            public override void TouchedCCW ()
            {
                p.ApplyPivotRotation (-10m);
                CommonDelegate.Apply ();
            }

            public override void TouchedCW ()
            {
                p.ApplyPivotRotation (+10m);
                CommonDelegate.Apply ();
            }
        }

        private class OuterArcTextSource : PivotCellSource
        {
            private readonly MutableOuterArcTextParameter p;

            public OuterArcTextSource (MutableOuterArcTextParameter parameter, IEditBoxCommonDelegate commonDelegate)
                : base (commonDelegate)
            {
                p = parameter;
            }

            public override void TouchedCCW ()
            {
                p.ApplyPivotRotation (-10m);
                CommonDelegate.Apply ();
            }

            public override void TouchedCW ()
            {
                p.ApplyPivotRotation (+10m);
                CommonDelegate.Apply ();
            }
        }
    }
}

