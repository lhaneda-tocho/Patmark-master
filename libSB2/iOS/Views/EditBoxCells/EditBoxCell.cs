using System;
using UIKit;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public abstract class EditBoxCell <DelegateType> : UITableViewCell, IEditBoxCell
        where DelegateType :class, IEditBoxCellDelegate  {

        DelegateType myDelegate;
        public virtual DelegateType Delegate { 
            get {
                return myDelegate;
            }
            set {
                myDelegate = value;
                if (value == null) {
                    myDelegate.SetUpdatedListener (null);
                    return;
                }
                myDelegate.SetUpdatedListener (UpdateContains);
            }
        }

        /// <summary>
        /// セクション名
        /// </summary>
        /// <returns>The section name.</returns>
        public abstract string SectionName { get; }

        /// <summary>
        /// セルに割り当てる ID． セルを探す際に利用します
        /// </summary>
        /// <returns>The identifier.</returns>
        public abstract string Identifier { get; }

        protected EditBoxCell (UITableViewCellStyle style, string reuseIdentifier) : base (style, reuseIdentifier)
        {
        }

        protected EditBoxCell ()
        {
        }

        protected EditBoxCell (Foundation.NSCoder coder) : base (coder)
        {
        }

        protected EditBoxCell (Foundation.NSObjectFlag t) : base (t)
        {
        }

        protected EditBoxCell (IntPtr handle) : base (handle)
        {
        }

        public EditBoxCell (CoreGraphics.CGRect frame) : base (frame)
        {
        }

        public EditBoxCell (UITableViewCellStyle style, Foundation.NSString reuseIdentifier) : base (style, reuseIdentifier)
        {
        }


        public abstract void UpdateContains (IEditBoxCellDelegate source);

        public UITableViewCell AsTableViewCell ()
        {
            return this;
        }

        public CellType As<CellType> () where CellType : UITableViewCell
        {
            return this as CellType;
        }


        /// <summary>
        /// 自身を タッチできないようにする．
        /// </summary>
        /// <returns>The test.</returns>
        /// <param name="point">Point.</param>
        /// <param name="uievent">Uievent.</param>
        public override UIView HitTest(CoreGraphics.CGPoint point, UIEvent uievent)
        {
            UIView ans;          
            UIView view = base.HitTest(point, uievent);
            if (object.ReferenceEquals(view, this) || object.ReferenceEquals(view, this.ContentView))
                ans = null;
            else
                ans = view;
            return ans;
        }
    }
}

