using Functional.Maybe;
using UIKit;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public static class UIViewExt
    {
        public static Maybe<UIView> FindFirstResponder <T> (this T view) where T: UIView
        {
            if (view.IsFirstResponder)
                return view.ToMaybe <UIView> ();
            foreach (UIView subView in view.Subviews) {
                if (subView.IsFirstResponder)
                    return subView.ToMaybe ();
                var res = subView.FindFirstResponder ();
                if (res.HasValue) 
                    return res;
            }
            return Maybe<UIView>.Nothing;
        }

        public static void ResignFirstResponderWithAnimation (this UIResponder active, double duration)
        {
            UIView.BeginAnimations ("HideKeyboard");
            UIView.SetAnimationDuration (duration);
            active.ResignFirstResponder ();
            UIView.CommitAnimations ();
        }
    }
}

