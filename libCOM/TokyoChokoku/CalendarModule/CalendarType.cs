using System;

namespace TokyoChokoku.CalendarModule
{
    public enum CalendarType
    {
        S,
        MM,
        M,
        DD,
        D,
        JJJ,
        YYYY,
        YY,
        Y
    }

    public static class CalendarTypeExt{
        public static CalendarType Parse (string source) {
            return (CalendarType) Enum.Parse (typeof(CalendarType), source);
        }


        public static int CharCount (this CalendarType type) {
            switch (type) {
            case CalendarType.S:
            case CalendarType.M:
            case CalendarType.D:
            case CalendarType.Y:
                return 1;

            case CalendarType.MM:
            case CalendarType.DD:
            case CalendarType.YY:
                return 2;

            case CalendarType.JJJ:
                return 3;

            case CalendarType.YYYY:
                return 4;

            default:
                return 0;
            }
        }


        public static int CharCount (System.Collections.Generic.IEnumerable<CalendarType> types) {
            int count = 0;
            foreach (var type in types) 
                count += type.CharCount ();
            return count;
        }

        public static string GetName (this CalendarType type) {
            return Enum.GetName (typeof(CalendarType), type);
        }

        public static string ToString (System.Collections.Generic.IEnumerable<CalendarType> types) {
            var sb = new System.Text.StringBuilder ();
            foreach (var type in types)
                sb.Append (type.GetName ());
            return sb.ToString ();
        }
    }
}

