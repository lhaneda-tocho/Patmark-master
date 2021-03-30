using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public sealed class CommonFileMenuSource
    {
        public Holder<FieldSource>  FieldSourceHolder { get; }
        public iOSFieldManager      FieldManager { get; }
        public FileMenuActionSource FileMenuActionSource { get; }

        public FieldSource FieldSource { get { return FieldSourceHolder.Content; } }

        public CommonFileMenuSource (Holder<FieldSource> fieldSourceHolder, iOSFieldManager fieldManager, FileMenuActionSource fileMenuActionSource)
        {
            FieldSourceHolder = fieldSourceHolder;
            FieldManager = fieldManager;
            FileMenuActionSource = fileMenuActionSource;
        }

        public LocalFileMenuSource ToLocalFileMenuSource ()
        {
            return new LocalFileMenuSource (
                FieldSource,
                FieldManager.FieldListCopy,
                FileMenuActionSource
            );
        }

        public RemoteFileMenuSource ToRemoteFileMenuSource ()
        {
            return new RemoteFileMenuSource (
                FieldSource,
                FieldManager.FieldListCopy,
                FileMenuActionSource
            );
        }
    }
}

