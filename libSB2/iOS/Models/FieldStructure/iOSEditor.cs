using System;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class iOSEditor : Editor
    {
        public FieldDrawable Drawable {
            get {
                return Field.CreateDrawable ();
            }
        }

        protected iOSEditor (IMutableField<IMutableParameter> field) : base (field)
        {
            if (field == null)
                throw new NullReferenceException ();
        }

        iOSEditor (iOSEditor editor) : this (editor.Field.ToGenericConstant ().ToGenericMutable ())
        {
        }

        public iOSEditor (iOSOwner owner) : this (owner.Field.ToGenericMutable ()) {
        }

        public override NextType ToOwner<NextType> ()
        {
            var owner = new iOSOwner (this) as NextType;
            if (owner == null)
                throw new ArgumentOutOfRangeException ();
            return owner;
        }

        public override NextType Duplicate<NextType> ()
        {
            var editor = new iOSEditor (this) as NextType;
            if (editor == null)
                throw new ArgumentOutOfRangeException ();
            return editor;
        }
    }
}

