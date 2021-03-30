//using System;
//using System.Text;
//using System.Text.RegularExpressions;
//using Foundation;
//using UIKit;
//namespace TokyoChokoku.Patmark.iOS.Presenter.TextInputFilter
//{
//    public class RemoteFileNameInputFilter: UITextFieldDelegate
//    {
//        public static RemoteFileNameInputFilter CreateAndAttach(UITextField textField) 
//        {
//            var filter = new RemoteFileNameInputFilter();
//            filter.Attach(textField);
//            return filter;
//        }


//        UITextField textField = null;
//        string      previousText;
//        string      lastReplacement;
//        NSRange     lastRange;
//        EventHandler EditingChangedDelegate { get; }


//        public RemoteFileNameInputFilter()
//        {
//            EditingChangedDelegate = EditingChanged;
//        }

//        public void Attach(UITextField textField)
//        {
//            this.textField = textField;
//            textField.Delegate = this;
//            textField.AddTarget(EditingChangedDelegate, UIControlEvent.EditingChanged);
//        }

//        public void Detach()
//        {
//            var textField = this.textField;
//            this.textField = null;
//            if (textField.Delegate == this)
//                textField.Delegate = null;
//            textField.RemoveTarget(EditingChangedDelegate, UIControlEvent.EditingChanged);
//        }

//        public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
//        {
//            if (this.textField != textField)
//                return true;
//            previousText = textField.Text;
//            lastRange    = range;
//            lastReplacement = replacementString;
//            return true;
//        }

//        private void EditingChanged(object sender, EventArgs e)
//        {
//            UITextField textField = (UITextField)sender;

//            //if (textField.MarkedTextRange != null)
//            //{
//            //    return;
//            //}

//            var prev = previousText;
//            var repl = lastReplacement;
//            var curr = textField.Text;

//            if()

//            if (textField.Text > h)
//            {
//                NSInteger offset = _maxLength - [textField.text length];

//                NSString* replacementString = [_lastReplacementString substringToIndex: ([_lastReplacementString length] + offset)];
//                NSString* text = [_previousText stringByReplacingCharactersInRange: _lastReplaceRange withString: replacementString];

//                UITextPosition* position = [textField positionFromPosition: textField.selectedTextRange.start offset: offset];
//                UITextRange* selectedTextRange = [textField textRangeFromPosition: position toPosition: position];

//                textField.text = text;
//                textField.selectedTextRange = selectedTextRange;
//            }
//        }


//    }
//}
