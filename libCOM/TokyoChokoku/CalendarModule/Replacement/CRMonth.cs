using System;
using System.Collections.Generic;
using System.Collections.Immutable;
namespace TokyoChokoku.CalendarModule.Replacement
{
    public struct CRMonth
    {
        // 1~9 → 1~9
        // 10~31 → A~L
        static readonly IDictionary<int, CRMonth> DefaultDict;
        static readonly IList<CRMonth> DefaultList;

        public static IList<CRMonth> DefaultMutableList   => new List<CRMonth>(DefaultList);
        public static IList<CRMonth> DefaultImmutableList => DefaultList;

        static CRMonth()
        {
            var dic  = new Dictionary<int, CRMonth>();
            var list = new List<CRMonth>(31);
            // M:1〜12 ⇒ A~L
            for (int i = 1; i <= 12; i++)
            {
                var code = (char)(i - 1 + 'A');
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
        public static CRMonth Init(char code)
        {
            var v = new CRMonth();
            v.Code = code;
            return v;
        }

        /// <summary>
        /// Default the specified month.
        /// </summary>
        /// <returns>The default.</returns>
        /// <param name="month">Month.</param>
        public static CRMonth Default(int month)
        {
            var def = DefaultDict;
            var valid = def.ContainsKey(month);
            if (valid)
                return def[month];
            throw new KeyNotFoundException();
        }
    }
}
