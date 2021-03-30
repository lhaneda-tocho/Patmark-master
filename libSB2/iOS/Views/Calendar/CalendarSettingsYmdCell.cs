using System;
using System.Linq;
using System.CodeDom.Compiler;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Connections;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class CalendarSettingsYmdCell : UICollectionViewCell
    {
        public static readonly NSString CellId = new NSString ("CalendarSettingsYmdCell");
        public static UINib Nib;

        private Connection<char> ValueConnection;

        static CalendarSettingsYmdCell ()
        {
            Nib = UINib.FromName ("CalendarSettingsYmdCell", NSBundle.MainBundle);
        }

        public CalendarSettingsYmdCell (IntPtr handle) : base (handle)
        {
        }


        public void Update(
            string title,
            Store<char> store
        ) {
            
            // タイトルを設定します。
            Title.Text = title;
            // 値とテキストボックスをバインドします。
            if (ValueConnection != null) {
                ValueConnection.Dispose ();
            } else {
                TextBox.ValueChanged += this.OnTextBoxValueChanged;
            }
            ValueConnection = new Connection<char> (
                store,
                char.TryParse,
                (result, sender) => {
                },
                (result, sender) => {
                }
            );
            // テキストボックスに値をセットします。
            TextBox.Text = ValueConnection.Content.ToString();


        }

        private void OnTextBoxValueChanged(object sender, EventArgs arg){
            if (ValueConnection != null) {
                ValueConnection.TrySet (TextBox.Text);
            }
        }

        public void OnSelected(){
            TextBox.BecomeFirstResponder();
        }

        override
        protected void Dispose (bool disposing) {
            if (disposing) {
                if (ValueConnection != null) {
                    ValueConnection.Dispose ();
                }
                base.Dispose ();
            }
        }

        override
        public void RemoveFromSuperview ()
        {
            if (ValueConnection != null) {
                ValueConnection.Dispose ();
            }
            base.RemoveFromSuperview ();
        }
    }
}
