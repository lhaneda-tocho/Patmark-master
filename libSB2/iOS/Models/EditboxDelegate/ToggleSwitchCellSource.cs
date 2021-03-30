using System;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class ToggleSwitchCellSource : IEditBoxCellDelegate
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

        public ToggleSwitchCellSource (IEditBoxCommonDelegate commonDelegate)
        {
            CommonDelegate = commonDelegate;
        }

        public abstract void SetUpdatedListener (Action<IEditBoxCellDelegate> action);

        public abstract bool CurrentState  ();
        public abstract void ChangedState  (bool state);
    }
}

