using System;
using System.Diagnostics;
using ToastIOS;

using Foundation;
using UIKit;
using ObjCRuntime;

using TokyoChokoku.MarkinBox.Sketchbook.Validators;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class PickerViewCell : UITableViewCell, IEditBoxCell
    {
        public static readonly NSString Key = new NSString ("PickerViewCell");
        public static readonly UINib Nib;

        IPickerViewCellDelegate myDelegate;
        IPickerViewCellDelegate Delegate { 
            get {
                return myDelegate;
            }
            set {
                myDelegate = value;
                if (value != null) {
                    Picker.Model = value.CreatePickerViewModel (this, Picker);
                    value.InitializePickerView (this, Picker);
                } else {
                    Picker.Model = null;
                }
            }
        }

        bool NeedsInitialize
        {
            get {
                return Delegate == null;
            }
        }


        string section;
        public string SectionName {
            get {
                return section;
            }
        }

        string rowIdentifier;
        public string Identifier {
            get {
                return rowIdentifier;
            }
        }



        static PickerViewCell ()
        {
            Nib = UINib.FromName ("PickerViewCell", NSBundle.MainBundle);
        }

        protected PickerViewCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();
        }

        public static PickerViewCell Create (string section, string row, IPickerViewCellDelegate cellDelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib ("PickerViewCell", null, null);
            var cell = Runtime.GetNSObject<PickerViewCell> (arr.ValueAt (0));

            cell.Initialize (section, row, cellDelegate);

            return cell;
        }

        void Initialize (string section, string row, IPickerViewCellDelegate cellDelegate)
        {
            this.section = section;
            rowIdentifier = row;
            Delegate = cellDelegate;
        }



        void StopPicker ()
        {
            nint componentCount = Picker.NumberOfComponents;
            for (int i = 0; i < componentCount; i++) {
                nint rowCount = Picker.SelectedRowInComponent (i);
                Picker.Select (rowCount, i, false);
            }
        }

        public void StopAndCommit ()
        {
            StopPicker ();
            Delegate.Commit (this, Picker);
        }

        public override void RemoveFromSuperview ()
        {
            base.RemoveFromSuperview ();
        }

        public UITableViewCell AsTableViewCell ()
        {
            return this;
        }

        public void PopupErrorMessage (ValidationResult result)
        {
            string message = Localize (result);
            Toast.MakeText (message)
                 .SetFontSize (15)
                 .SetDuration (3000)
                 .SetGravity (ToastGravity.Center)
                 .Show (ToastType.Error);
        }

        string Localize (ValidationResult result)
        {
            var sb = new System.Text.StringBuilder ();
            bool start = true;
            foreach (var e in result.ErrorCodes) {
                if (!start)
                    sb.AppendLine ();
                sb.Append (e.GetLocalizationId ().Localize ());
            }
            return sb.ToString ();
        }

        public CellType As<CellType> () where CellType : UITableViewCell
        {
            return this as CellType;
        }

        public void UpdateContains (IEditBoxCellDelegate source)
        {
            // なにもしない．
        }

        sealed class EmptyDelegate : UIPickerViewModel, IPickerViewCellDelegate
        {
            public void Commit (PickerViewCell cell, UIPickerView pickerView)
            {
                
            }

            public UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView picker)
            {
                return this;
            }

            public string GetCurrentValue ()
            {
                return "empty";
            }

            public void InitializePickerView (PickerViewCell cell, UIPickerView pickerView)
            {
            }
        }
    }
}
