using System;

using Foundation;
using UIKit;
using ObjCRuntime;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class DropdownListCell : EditBoxCell<DropdownListCellSource>, IEditBoxInteractionDelegate
    {
        public static readonly NSString Key = new NSString ("DropdownListCell");
        public static readonly UINib Nib;
        static nfloat Height {
            get {
                return 40;
            }
        }


        public event Action<string, IEditBoxInteractionDelegate> RequestInteraction;


        public override string SectionName {
            get {
                return TitleFrame.Title;
            }
        }
        public override string Identifier {
            get {
                return SectionName;
            }
        }


        public override DropdownListCellSource Delegate {
            set {
                base.Delegate = value;
                if (value != null) {
                    DropdownButton.SetTitle (value.GetCurrentValue (), UIControlState.Normal);
                }
            }
        }

        /// <summary>
        /// EditBoxの操作を行います
        /// </summary>
        /// <param name="interactionId">Interaction identifier.</param>
        /// <param name="source">Source.</param>
        public void Interact (string interactionId, PropertyEditBoxSource source)
        {
            switch (interactionId) {
            case "toggle": {
                    Console.WriteLine ("Section of " + SectionName);
                    Console.WriteLine ("Section of " + SectionName);
                    nint row = source.SearchRowInSection (SectionName, SectionName + ".Dropdown");
                    if (row == -1)
                        Dropdown (source);
                    else
                        CloseDropdown (source);
                    break;
                }
            }
        }

        public void ClearListeners ()
        {
            RequestInteraction = null;
        }

        void Dropdown (PropertyEditBoxSource source)
        {
            if (Delegate.IsSingleOrEmpty())
                return;

            nint row = source.SearchRowInSection (SectionName, SectionName);

            source.Insert (
                row + 1,
                source.SearchSection (SectionName),
                () =>  PickerViewCell.Create (
                         SectionName,
                         SectionName + ".Dropdown",
                         new MyPickerViewCellDelegate (this)
                )
            );
        }

        void CloseDropdown (PropertyEditBoxSource source)
        {
            // セルの取得を試みる
            var cell = source.GetCellFromId (SectionName, SectionName + ".Dropdown");
            if (cell != null && cell.AsTableViewCell() is PickerViewCell) {

                // 該当するセルが見つかり，それが PickerViewCell である場合
                var pcell = cell.As<PickerViewCell> ();
                pcell.StopAndCommit ();

                nint row = source.SearchRowInSection (SectionName, SectionName + ".Dropdown");
                source.Remove (row, source.SearchSection (SectionName));
            }
        }

        static DropdownListCell ()
        {
            Nib = UINib.FromName ("DropdownListCell", NSBundle.MainBundle);
        }

        protected DropdownListCell (IntPtr handle) : base (handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            DropdownButton.TouchUpInside += (sender, e) => {
                if (Delegate == null)
                    return;
                RequestInteraction ("toggle", this);
            };
        }

        public override void UpdateContains (IEditBoxCellDelegate source)
        {
            DropdownButton.SetTitle (Delegate.GetCurrentValue (), UIControlState.Normal);
        }

        public static DropdownListCell Create (string title, DropdownListCellSource mydelegate)
        {
            var arr = NSBundle.MainBundle.LoadNib ("DropdownListCell", null, null);
            var cell = Runtime.GetNSObject<DropdownListCell> (arr.ValueAt (0));

            cell.TitleFrame.Title = title;
            cell.Delegate = mydelegate;

            cell.DropdownButton.ContentEdgeInsets = new UIEdgeInsets((nfloat) 8, (nfloat)25, (nfloat)8, (nfloat)25);

            return cell;
        }



        sealed class MyPickerViewCellDelegate : IPickerViewCellDelegate
        {
            DropdownListCell current;

            public MyPickerViewCellDelegate (DropdownListCell current)
            {
                this.current = current;
            }

            public void Commit (PickerViewCell cell, UIPickerView pickerView)
            {
                current.Delegate.Commit (cell, pickerView);
                current.DropdownButton.SetTitle (current.Delegate.GetCurrentValue (), UIControlState.Normal);
            }

            public UIPickerViewModel CreatePickerViewModel (PickerViewCell cell, UIPickerView pickerView)
            {
                return current.Delegate.CreatePickerViewModel (cell, pickerView);
            }

            public string GetCurrentValue ()
            {
                return current.Delegate.GetCurrentValue ();
            }

            public void InitializePickerView (PickerViewCell cell, UIPickerView pickerView)
            {
                current.Delegate.InitializePickerView (cell, pickerView);
            }
        }

    }
}
