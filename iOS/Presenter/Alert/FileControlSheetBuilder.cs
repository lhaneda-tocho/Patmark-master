using System;
using UIKit;



namespace TokyoChokoku.Patmark.iOS
{
    public class FileControlSheetBuilder
    {
        static Action<UIAlertAction> EmptyAction {
            get {
                return (obj) => { };
            }
        }

        static string EmptyTitle { get; } = "NO-TITLE";

        string title = EmptyTitle;
        Action<UIAlertAction> read = EmptyAction;
        Action<UIAlertAction> saveOver = EmptyAction;
        Action<UIAlertAction> rename = EmptyAction;
        Action<UIAlertAction> clear = EmptyAction;

        public string Title {
            set {
                title = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction> Read {
            set {
                read = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction> SaveOver {
            set {
                saveOver = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction> Rename {
            set {
                rename = NullToEmpty (value);
            }
        }

        public Action<UIAlertAction> Clear {
            set {
                clear = NullToEmpty (value);
            }
        }

        static string NullToEmpty (string value)
        {
            if (value == null)
                return EmptyTitle;
            else
                return value;
        }

        static Action<UIAlertAction> NullToEmpty (Action<UIAlertAction> value)
        {
            if (value == null)
                return EmptyAction;
            else
                return value;
        }

        public UIAlertController Build (UIView sourceView)
        {
            var alert = UIAlertController.Create (
                title,
                "Choose your operation.".Localize (),
                UIAlertControllerStyle.ActionSheet);

            // Add Actions
            AddButton (alert, "Read", read);

            AddButton (alert, "Overwrite", saveOver);

            AddButton (alert, "Rename", rename);

            AddDestructiveButton (alert, "Delete", clear);

            AddCancelButton (alert);

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null) {
                presentationPopover.SourceView = sourceView;
                presentationPopover.SourceRect = sourceView.Bounds;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up | UIPopoverArrowDirection.Down;
            }

            return alert;
        }

        public UIAlertController BuildWithoutClear (UIView sourceView)
        {
            var alert = UIAlertController.Create (
                title,
                "Choose your operation.".Localize (),
                UIAlertControllerStyle.ActionSheet);

            // Add Actions
            AddButton (alert, "Read", read);

            AddButton (alert, "Overwrite", saveOver);

            AddButton (alert, "Rename", rename);

            AddCancelButton (alert);

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null) {
                presentationPopover.SourceView = sourceView;
                presentationPopover.SourceRect = sourceView.Bounds;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up | UIPopoverArrowDirection.Down;
            }

            return alert;
        }

        static void AddCancelButton (UIAlertController alert)
        {
            alert.AddAction (UIAlertAction.Create (
                "Cancel".Localize (), UIAlertActionStyle.Cancel, null));
        }

        static void AddButton (
            UIAlertController alert, string label, Action<UIAlertAction> action)
        {
            alert.AddAction (
                UIAlertAction.Create (
                    label.Localize(),
                    UIAlertActionStyle.Default,
                    action)
            );
        }

        static void AddDestructiveButton (
            UIAlertController alert, string label, Action<UIAlertAction> action)
        {
            alert.AddAction (
                UIAlertAction.Create (
                    label.Localize(),
                    UIAlertActionStyle.Destructive,
                    action)
            );
        }
    }
}

