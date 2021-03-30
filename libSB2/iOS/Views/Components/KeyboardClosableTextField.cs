using System;
using UIKit;
namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class KeyboardClosableTextField : UITextField
    {
        public KeyboardClosableTextField (IntPtr handle) : base (handle)
        {
            EditingDidEndOnExit += (sender, e) => {
                this.EndEditing(true);
            };

            ShouldReturn += t =>
            {
                t.ResignFirstResponder();
                return true;
            };
        }
    }
}