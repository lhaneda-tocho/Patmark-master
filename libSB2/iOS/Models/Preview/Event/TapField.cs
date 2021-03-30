namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public sealed class TapField
    {
        public iOSFieldManager FieldManager { get; }
        public To              Action       { get; }

        public enum To
        {
            SelectOther,
            SelectEditing,
            Deselect
        }

        public TapField (iOSFieldManager manager, To doing)
        {
            FieldManager = manager;
            Action = doing;
        }
    }

    public static class TapFieldExt
    {
        public static TapField Create (this TapField.To todo, iOSFieldManager manager)
        {
            return new TapField (manager, todo);
        }
    }
}

