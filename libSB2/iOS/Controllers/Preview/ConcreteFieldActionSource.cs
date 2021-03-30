using System;
using System.Collections.Generic;
using UIKit;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    class ConcreteFieldActionSource : FileMenuActionSource
    {
        readonly CanvasPresentationManager PresentationManager;
        readonly iOSFieldContext     FieldContext;
        readonly Holder<FieldSource> FieldSourceHolder;
        readonly UILabel             FileNameLabel;

        public ConcreteFieldActionSource (CanvasPresentationManager presentationManager, iOSFieldContext fieldContext, Holder<FieldSource> fieldSourceHolder, UILabel fileNameLabel)
        {
            PresentationManager = presentationManager;
            FieldContext = fieldContext;
            FieldSourceHolder = fieldSourceHolder;
            FileNameLabel = fileNameLabel;
        }

        public Action DidLoadNewest (FieldSource source, IList<iOSOwner> list)
        {
            return () => {
                FieldSourceHolder.Content = source;
                FieldContext.ForceDeleteAll ();
                FieldContext.TrySubmitAll (list);
                FileNameLabel.Text = source.From;
                PresentationManager.DrawRequest ();
            };
        }

        public Action WillExit (FieldSource source)
        {
            return () => {
                FieldSourceHolder.Content = source;
                FileNameLabel.Text = source.From;
            };
        }

        public Action WillExitAfterLoad (FieldSource source, IList<iOSOwner> list)
        {
            return () => {
                FieldSourceHolder.Content = source;
                FieldContext.ForceDeleteAll ();
                FieldContext.TrySubmitAll (list);
                FileNameLabel.Text = source.From;
                PresentationManager.DrawRequest ();
            };
        }
    }
}

