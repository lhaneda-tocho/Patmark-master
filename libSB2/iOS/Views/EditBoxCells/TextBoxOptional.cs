using System;
using System.Text.RegularExpressions;

using Foundation;
using UIKit;
using CoreGraphics;
using ObjCRuntime;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class TextBoxOptional : UITableViewCell, IEditBoxCell
    {
        public static readonly NSString Key = new NSString ("TextBoxOptional");
        public static readonly UINib Nib;


        TextBoxCell Parent  { get; set; }
        iOSFieldManager FieldManager { get; set; }

        string section;
        public string SectionName {
            get {
                return section;
            }
        }

        public string Identifier {
            get {
                return SectionName + ".Context";
            }
        }

        static TextBoxOptional ()
        {
            Nib = UINib.FromName ("TextBoxOptional", NSBundle.MainBundle);
        }

        protected TextBoxOptional (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }


        protected void InitProperties (iOSFieldManager mng, TextBoxCell parent)
        {
            section = parent.SectionName;
            FieldManager = mng;
            Parent = parent;
        }


        /// <summary>
        /// 新しいインスタンスを初期化して 返します
        /// 
        /// </summary>
        public static TextBoxOptional Create (
            iOSFieldManager manager, TextBoxCell parent)
        {
            var arr = NSBundle.MainBundle.LoadNib ("TextBoxOptional", null, null);
            var cell = Runtime.GetNSObject<TextBoxOptional> (arr.ValueAt (0));

            cell.InitProperties (manager, parent);

            return cell;
        }




        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();
            Calendar.SetTitle ("ctrl-field-calendar.title".Localize (), UIControlState.Normal);
            Serial.SetTitle ("ctrl-field-serial.title".Localize (), UIControlState.Normal);
        }

        public UITableViewCell AsTableViewCell ()
        {
            return this;
        }

        partial void Calender_TouchUpInside (BorderButton sender)
        {
            var model = new CalendarSettingsViewController.ViewModel (Parent.TryInsertText);
            CalendarSettingsViewController.Popover (model, new CGSize (300, 300), Parent.GetTextBox());
        }

        partial void Serial_TouchUpInside (BorderButton sender)
        {
            SerialSettingsController.Modal (
                fieldManager: FieldManager,
                text: Parent.Delegate.GetText (),

                insertAfter: (tag) => {
                    Parent.GetTextBox ().Text += tag;
                    Parent.Delegate.ChangedText (Parent.GetTextBox ().Text);
                },

                replaceSerial: (tag) => {
                    // 文字列取得
                    var text = Parent.Delegate.GetText ();

                    // 置換する
                    Parent.GetTextBox ().Text = Regex.Replace (text, Sketchbook.Serial.Consts.FormatRegEx, tag);

                    Parent.Delegate.ChangedText (Parent.GetTextBox ().Text);
                }
            );
        }

        public CellType As<CellType> () where CellType : UITableViewCell
        {
            return this as CellType;
        }

        public void UpdateContains (IEditBoxCellDelegate source)
        {
            // 何もしない
        }
    }
}
