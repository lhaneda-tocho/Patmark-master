using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
namespace TokyoChokoku.CalendarModule.Replacement
{
    public struct CRShift
    {
        static IList<CRShift> DefaultList;
        public static IList<CRShift> DefaultMutableList   => new List<CRShift>(DefaultList);
        public static IList<CRShift> DefaultImmutableList => DefaultList;

        static CRShift() {
            var list = new List<CRShift>(5);
            var data = InitDisabled();
            for (int i = 0; i < 5; i++) {
                list.Add(data);
            }
            DefaultList = list.ToImmutableList();
        }

        /// <summary>
        /// The code.
        /// </summary>
        public char Code;

        /// <summary>
        /// The condition of this shift is enable or not.
        /// </summary>
        public bool Enable;

        /// <summary>
        /// The time range.
        /// </summary>
        public TimeRange Range;

        /// <summary>
        /// Ascii code
        /// </summary>
        /// <value>The ASCII.</value>
        public byte Ascii
        {
            get => (byte)Code;
            set => Code = (char)value;
        }

        /// <summary>
        /// Initialize struct as disabled.
        /// Code = null char
        /// Range = 0:00 ~ 0:00
        /// </summary>
        /// <returns>The disabled.</returns>
        public static CRShift InitDisabled() {
            var v = new CRShift();
            v.Code = '\0';
            v.Enable = false;
            v.Range = TimeRange.Init();
            return v;
        }

        /// <summary>
        /// Init the specified code.
        /// Range = 0:00 ~ 0:00
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="code">Code.</param>
        public static CRShift Init(char code)
        {
            var v = new CRShift();
            v.Code = code;
            v.Enable = true;
            v.Range = TimeRange.Init();
            return v;
        }

        /// <summary>
        /// Init the specified code and range.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="code">Code.</param>
        /// <param name="range">Range.</param>
        public static CRShift Init(char code, ref TimeRange range)
        {
            var v = new CRShift();
            v.Code = code;
            v.Enable = true;
            v.Range = range;
            return v;
        }

        public override string ToString()
        {
            char code = Code;
            string codestr;
            if(code == 0) {
                codestr = "<NULL>";
            } else {
                codestr = code.ToString();
            }
            return "Code=" + codestr + ", Enable=" + Enable + ", Range=" + Range;
        }
    }
}
