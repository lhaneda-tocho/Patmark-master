using Foundation;
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class CalendarSettingsInsertionController : UIViewController
	{
		public CalendarSettingsInsertionController (IntPtr handle) : base (handle)
        {
        }

        public delegate void InsertionDelegate(string text);

        public ViewModel Model { get; set; }

        public class ViewModel
        {
            public InsertionDelegate Insert;
            public ViewModel(InsertionDelegate insert)
            {
                this.Insert = insert;
            }
        }

        override
        public void ViewDidLoad()
        {
            NavigationItem.Title = NSBundle.MainBundle.LocalizedString("ctrl-calendar-insertion.title", "");
            // ナビゲーションバーを無視してスクロールビューのサイズを決定します。
            this.AutomaticallyAdjustsScrollViewInsets = false;

            // 
            foreach (var container in Containers) {
                container.Button.TouchUpInside += (object sender, EventArgs e) => {
                    this.InsertionTextBox.Text += "@C[" + container.Code + "]";
                };
            }

            // X ボタンを押して、１文字削除
            this.TextDeletionButton.TouchUpInside += (object sender, EventArgs e) => {
                DeleteOneCharacter();
            };
            // X ボタン長押しで 連続削除．
            this.TextDeletionButton.AddGestureRecognizer (
                GestureFactory.CreateStandardRepeater(DeleteOneCharacter)
            );

            // OKボタンを押した場合、入力内容をテキストにセットします。
            this.OKButton.TouchUpInside += (object sender, EventArgs e) => {
                Model.Insert(InsertionTextBox.Text);
            };
		}

        private struct Container
        {
            public UIButton Button;
            public string Code;
            
            public Container(UIButton button, string code)
            {
                this.Button = button;
                this.Code = code;
            }
        }

        private Container[] Containers{
            get{
                return new Container[] {
                    new Container(InsertionButton_S, "S"),
                    new Container(InsertionButton_DD, "DD"),
                    new Container(InsertionButton_D, "D"),
                    new Container(InsertionButton_jjj, "jjj"),
                    new Container(InsertionButton_MM, "MM"),
                    new Container(InsertionButton_M, "M"),
                    new Container(InsertionButton_YYYY, "YYYY"),
                    new Container(InsertionButton_YY, "YY"),
                    new Container(InsertionButton_Y, "Y"),
                    new Container(InsertionButton_hh, "hh"),
                    new Container(InsertionButton_mm, "mm"),
                    new Container(InsertionButton_ss, "ss"),
                };
            }
        }


        private void DeleteOneCharacter () {
            var text = InsertionTextBox.Text;

            if (text.Length > 0) {
                InsertionTextBox.Text = text.Remove (text.Length - 1);
            }
        }
	}
}
