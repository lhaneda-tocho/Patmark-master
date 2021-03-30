using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    using Fields;
    using Parameters;

    public static class TextFieldCollector
    {
        static Checker    MyChecker    { get; } = new Checker ();
        static TextGetter MyTextGetter { get; } = new TextGetter ();
        static TextParser MyTextParser { get; } = new TextParser ();


        public static bool IsSerialContainer (Owner owner)
        {
            return owner.Accept (MyChecker, null);
        }

        public static List<Owner> Collect (List<Owner> owners)
        {
            return owners.Select (
                (owner) => IsSerialContainer (owner) ? owner : null
            ).Where (
                (owner) => owner != null
            ).ToList ();
        }

        public static string GetText (Owner owner)
        {
            return owner.Accept (MyTextGetter, null);
        }

        public static RootFieldTextNode ParseText (Owner owner)
        {
            return owner.Accept (MyTextParser, null);
        }

        public static bool HasSerial (Owner owner)
        {
            return ParseText (owner).HasSerial;
        }

        public static bool HasLogo (Owner owner)
        {
            return ParseText (owner).HasLogo;
        }

        public static bool HasCalender (Owner owner)
        {
            return ParseText (owner).HasCalendar;
        }


        sealed class Checker : IFieldVisitor<bool, Nil>
        {
            public bool Visit (XVerticalText.Constant target, Nil arg)
            {
                return true;
            }

            public bool Visit (OuterArcText.Constant target, Nil arg)
            {
                return true;
            }

            public bool Visit (InnerArcText.Constant target, Nil arg)
            {
                return true;
            }

            public bool Visit (YVerticalText.Constant target, Nil arg)
            {
                return true;
            }

            public bool Visit (HorizontalText.Constant target, Nil arg)
            {
                return true;
            }



            public bool Visit (QrCode.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (DataMatrix.Constant target, Nil arg)
            {
                return true;
            }







            public bool Visit (Triangle.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (Line.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (Ellipse.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (Bypass.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (Circle.Constant target, Nil arg)
            {
                return false;
            }

            public bool Visit (Rectangle.Constant target, Nil arg)
            {
                return false;
            }
        }

        sealed class TextGetter : IFieldVisitor<string, Nil>
        {
            public string Visit (XVerticalText.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }

            public string Visit (OuterArcText.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }

            public string Visit (InnerArcText.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }

            public string Visit (YVerticalText.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }

            public string Visit (HorizontalText.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }



            public string Visit (QrCode.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }

            public string Visit (DataMatrix.Constant target, Nil arg)
            {
                return target.Parameter.Text;
            }







            public string Visit (Triangle.Constant target, Nil arg)
            {
                return null;
            }

            public string Visit (Line.Constant target, Nil arg)
            {
                return null;
            }

            public string Visit (Ellipse.Constant target, Nil arg)
            {
                return null;
            }

            public string Visit (Bypass.Constant target, Nil arg)
            {
                return null;
            }

            public string Visit (Circle.Constant target, Nil arg)
            {
                return null;
            }

            public string Visit (Rectangle.Constant target, Nil arg)
            {
                return null;
            }
        }


        sealed class TextParser : IFieldVisitor<RootFieldTextNode, Nil>
        {
            public RootFieldTextNode Visit (XVerticalText.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }

            public RootFieldTextNode Visit (OuterArcText.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }

            public RootFieldTextNode Visit (InnerArcText.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }

            public RootFieldTextNode Visit (YVerticalText.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }

            public RootFieldTextNode Visit (HorizontalText.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }




            public RootFieldTextNode Visit (QrCode.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (DataMatrix.Constant target, Nil arg)
            {
                return target.Parameter.ParseText ();
            }




            public RootFieldTextNode Visit (Triangle.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (Line.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (Ellipse.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (Bypass.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (Circle.Constant target, Nil arg)
            {
                return null;
            }

            public RootFieldTextNode Visit (Rectangle.Constant target, Nil arg)
            {
                return null;
            }
        }
    }
}

