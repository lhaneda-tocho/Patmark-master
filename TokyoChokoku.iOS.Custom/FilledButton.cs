using System;
using System.ComponentModel;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using UIKit;

namespace TokyoChokoku.iOS.Custom
{
    [Register("FilledButton"), DesignTimeVisible(true)]
    public class FilledButton : BorderedButton //, IComponent
	{
        bool EnableChangeTextColorField = false;
		[Export("EnableChangeTextColor"), Browsable(true)]
        public bool EnableChangeTextColor
        {
            get
            {
                return EnableChangeTextColorField;
            }
            set
            {
				EnableChangeTextColorField = value;
                UpdateTextColorNeeded();
				UpdateColor();
            }
        }

		bool EnableChangeBorderButtonField = false;
		[Export("EnableChangeBorderColor"), Browsable(true)]
        public bool EnableChangeBorderColor
        {
            get
            {
                return EnableChangeBorderButtonField;
            }
            set
            {
                EnableChangeBorderButtonField = value;
                UpdateColor();
            }
        }

        UIColor ButtonColorField = UIColor.Clear;
		[Export("ButtonColor"), Browsable(true)]
		public UIColor ButtonColor
		{
            get
            {
                return ButtonColorField;
            } 
            set
            {
                if(value == null)
                    ButtonColorField = UIColor.Clear;
                else
                    ButtonColorField = value;
                UpdateTextColorNeeded();
                UpdateColor();
            }
		}

        UIColor HighlightColorField = UIColor.Clear;
		[Export("HighlightColor"), Browsable(true)]
		public UIColor HighlightColor
		{
			get
			{
				return HighlightColorField;
			}
			set
			{
				if (value == null)
					HighlightColorField = UIColor.Clear;
				else
					HighlightColorField = value;
                UpdateTextColorNeeded();
				UpdateColor();
			}
		}

        UIColor DisabledColorField = UIColor.Clear;
		[Export("DisabledColor"), Browsable(true)]
		public UIColor DisabledColor
		{
			get
			{
				return DisabledColorField;
			}
			set
			{
				if (value == null)
					DisabledColorField = UIColor.Clear;
				else
					DisabledColorField = value;
                UpdateTextColorNeeded();
				UpdateColor();
			}
		}

        UIColor SelectedColorField = UIColor.Clear;
		[Export("SelectedColor"), Browsable(true)]
        public UIColor SelectedColor
        {
			get
			{
				return SelectedColorField;
			}
			set
			{
				if (value == null)
					SelectedColorField = UIColor.Clear;
				else
					SelectedColorField = value;
                UpdateTextColorNeeded();
				UpdateColor();
			}
        }

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                UpdateColor();
            }
        }

		public override bool Highlighted
		{
			get
			{
				return base.Highlighted;
			}
			set
			{
				base.Highlighted = value;
                UpdateColor();
			}
		}

		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
                UpdateColor();
			}
		}

		#region constructor extension
		public FilledButton(CoreGraphics.CGRect frame) : base(frame)  { Initialize(); }
		public FilledButton(NSCoder coder)             : base(coder)  { Initialize(); }
		public FilledButton(IntPtr handle)             : base(handle) { Initialize(); }

		public FilledButton()                          : base()       { Initialize(); }
		public FilledButton(UIButtonType type)         : base(type)   { Initialize(); }
		#endregion

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

   //     UIImage ImageFromColor(UIColor color)
   //     {
			//var rect = new CGRect(0, 0, 1, 1);
			//UIGraphics.BeginImageContext(rect.Size);
			//var context = UIGraphics.GetCurrentContext();
			//context.SetFillColor(HighlightColor.CGColor);
			//context.FillRect(rect);
			//var image = UIGraphics.GetImageFromCurrentImageContext();
			//UIGraphics.EndImageContext();
        //    return image;
        //}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		void Initialize()
		{
            ButtonColor    = UIColor.Clear;
			DisabledColor  = UIColor.Clear;
			HighlightColor = UIColor.Clear;
            SelectedColor  = UIColor.Clear;
		}

        bool NeedsColorAnimate()
        {
            return State == UIControlState.Normal;
        }

        UIColor GetColorFromState(UIControlState state)
        {
			switch (state)
			{
				case UIControlState.Disabled:
					return DisabledColor;
				case UIControlState.Highlighted:
					return HighlightColor;
				case UIControlState.Selected:
					return SelectedColor;
				case UIControlState.Normal:
					return ButtonColor;
				default:
					return ButtonColor;
			}
        }

        UIColor CurrentColor()
        {
			var state = State;
            return GetColorFromState(state);
        }

        CABasicAnimation BorderAnimation(double duration)
        {
            var startBorder = BorderColor.CGColor;
            var endBorder   = CurrentColor().CGColor;

            // borderColorを指定
            var anim = CABasicAnimation.FromKeyPath("borderColor");
            // 開始時の色
            anim.SetFrom(startBorder);
            // 終了時の色
            anim.SetTo(endBorder);
            // アニメーションの持続時間
            anim.Duration = duration;
            // アニメーションのカーブ
            anim.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
            // 完了後にリセットされないようにする (デフォルトはkCAFillModeRemoved)
            anim.FillMode = CAFillMode.Forwards;
            anim.RemovedOnCompletion = false;
            return anim;
        }

        void UpdateBorder(double duration, bool animate)
        {
            if (animate)
            {
                CATransaction.Begin();

                var anim = BorderAnimation(duration);
                CATransaction.CompletionBlock = () =>
                {
                    Layer.RemoveAnimation("FilledButtonColorAnimation");
                    var c = CurrentColor();
                    BorderColor = c;
                };

                Layer.AddAnimation(anim, "FilledButtonColorAnimation");
                CATransaction.Commit();
            } else {
				var c = CurrentColor();
				BorderColor = c;
            }
        }

        void UpdateTint(double duration, bool animate)
        {
            if (animate)
            {
                Animate(duration, 0.0, UIViewAnimationOptions.CurveEaseOut, () =>
                {
                    TintColor = CurrentColor();
                }, () =>
                {
                    TintColor = CurrentColor();
                });
            } else {
                TintColor = CurrentColor();
            }
        }

        void UpdateTextColorNeeded()
        {
            if (EnableChangeTextColor)
            {
                UIControlState state = UIControlState.Normal;
                SetTitleColor(null, state);

                state = UIControlState.Disabled;
                SetTitleColor(null, state);

                state = UIControlState.Highlighted;
                SetTitleColor(null, state);

                state = UIControlState.Selected;
                SetTitleColor(null, state);
            }
        }



        void UpdateText(double duration, bool animate)
        {
            var c = CurrentTitleColor;
			//if (c != null && c != UIColor.Clear)
			//return;
			var l = TitleLabel;

            if (animate)
            {
				Transition(l, duration, UIViewAnimationOptions.CurveEaseOut, () =>
				{
					l.TextColor = CurrentColor();
				}, () =>
				{
					l.TextColor = CurrentColor();
				});
            } else {
                l.TextColor = CurrentColor();
            }
        }

        void UpdateColor()
        {
            var duration = 0.3;
            var animate = NeedsColorAnimate();

            if(EnableChangeBorderColor)
                UpdateBorder(duration, animate);
            if (EnableChangeTextColor)
                UpdateText(duration, animate);
            UpdateTint(duration, animate);
        }
	}
}
