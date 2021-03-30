using System;
using System.Collections.Generic;
using System.Collections.Immutable;
namespace TokyoChokoku.CalendarModule.Replacement
{
    public struct CRYear
    {
        // 1~9 → 1~9
        // 10~31 → A~L
        static readonly IDictionary<int, CRYear> DefaultDict;
        static readonly IList<CRYear> DefaultList;

        public static IList<CRYear> DefaultMutableList   => new List<CRYear>(DefaultList);
        public static IList<CRYear> DefaultImmutableList => DefaultList;

        static CRYear()
        {
            var dic = new Dictionary<int, CRYear>();
            var list = new List<CRYear>(31);
            // Y:0~9
            for (int i = 0; i <= 9; i++)
            {
                var code = (char)(i + '0');
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
        public static CRYear Init(char code)
        {
            var v = new CRYear();
            v.Code = code;
            return v;
        }

        /// <summary>
        /// Default the specified year.
        /// </summary>
        /// <returns>The default.</returns>
        /// <param name="year">Year.</param>
        public static CRYear Default(int year)
        {
            var def = DefaultDict;
            var valid = def.ContainsKey(year);
            if (valid)
                return def[year];
            throw new KeyNotFoundException();
        }
    }
}
