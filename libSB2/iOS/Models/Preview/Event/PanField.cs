namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class PanField
    {
        public iOSFieldManager FieldManager { get; }
        public To              Action { get; }

        public enum To
        {
            SelectOther,
            SelectEditing,
            Deselect,
            BePanning,
            HavePanned
        }

        public PanField (iOSFieldManager fieldManager, To doing)
        {
            FieldManager = fieldManager;
            Action = doing;
        }
    }

    public static class PanFieldExt
    {
        public static PanField Create (this PanField.To todo, iOSFieldManager manager)
        {
            return new PanField (manager, todo);
        }
    }
}

