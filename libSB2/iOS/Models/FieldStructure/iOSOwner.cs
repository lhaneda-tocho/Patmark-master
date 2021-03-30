using System;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class iOSOwner : Owner
    {
        public FieldDrawable Drawable { get; }


        public iOSOwner (IField<IConstantParameter> field) : base (field) {
            if (field == null)
                throw new NullReferenceException ();
            
            Drawable = field.CreateDrawable ();
        }

        iOSOwner (iOSOwner owner) : this(owner.Field)
        {

        }

        public iOSOwner (iOSEditor editor) : this (editor.Field.ToGenericConstant () ) 
        {

        }

        public static List<iOSOwner> From (List<IField<IConstantParameter>> fields)
        {
            var list = new List<iOSOwner> ();
            foreach (var field in fields)
                list.Add (new iOSOwner (field));
            return list;
        }

        public iOSEditor ToEditor () {
            return new iOSEditor (this);
        }

        public override NextType ToEditor<NextType> ()
        {
            var editor = new iOSEditor (this) as NextType;
            if (editor == null)
                throw new ArgumentOutOfRangeException ();
            return editor;
        }

        public override NextType Duplicate<NextType> ()
        {
            var owner = new iOSOwner (this) as NextType;
            if (owner == null)
                throw new ArgumentOutOfRangeException ();
            return owner;
        }
    }
}

