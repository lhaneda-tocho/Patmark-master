namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public sealed class FileMenuDismissedEvent
    {
        public enum BecauseOf { FileLoaded, CloseButtonPushed }

        public BecauseOf Cause { get; }

        public FileMenuDismissedEvent (BecauseOf cause)
        {
            Cause = cause;
        }
    }
}

