using System;
using System.Collections.Generic;
using System.Collections.Immutable;
namespace TokyoChokoku.CalendarModule.Replacement
{
    public struct CRDay
    {
        // 1~9 → 1~9
        // 10~31 → A~L
        static readonly IDictionary<int, CRDay> DefaultDict;
        static readonly IList<CRDay> DefaultList;

        public static IList<CRDay> DefaultMutableList   => new List<CRDay>(DefaultList);
        public static IList<CRDay> DefaultImmutableList => DefaultList;

        static CRDay()
        {
            var dic = new Dictionary<int, CRDay>();
            var list = new List<CRDay>(31);

            // 1~9 → 1~9
            // 10~31 → A~L
            for (int i = 1; i < 10; i++)
            {
                var code = (char)(i + '0');
                var data = Init(code);
                dic.Add(i, data);
                list.Add(data);
            }
            for (int i = 10; i <= 31; i++)
            {
                var code = (char)(i - 10 + 'A');
                var data = Init(code);
                dic.Add(i, data);
                list.Add(data);
            }

            DefaultDict = dic.ToImmutableDictionary();
            DefaultList = list.ToImmutableList();
        }

        /// <summary>
        /// The code.
        /// </summary>
        public char Code;

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
        /// Init the specified code.
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="code">Code.</param>
        public static CRDay Init(char code) {
            var v = new CRDay();
            v.Code = code;
            return v;
        }

        /// <summary>
        /// Default the specified day.
        /// </summary>
        /// <returns>The default.</returns>
        /// <param name="day">Day.</param>
        public static CRDay Default(int day) {
            var def  = DefaultDict;
            var valid = def.ContainsKey(day);
            if (valid)
                return def[day];
            throw new KeyNotFoundException();
        }

    }
}
