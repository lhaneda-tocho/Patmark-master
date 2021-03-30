using System;
using System.Collections.Generic;

namespace TokyoChokoku.CalendarModule.Ast
{
    public class CalendarNode
    {
        // null when I am final element. otherwise
        public CalendarNode Next { get; private set; }

        // nonnull
        public CalendarType Type { get; }

        /// <summary>
        /// Gets the char count.
        /// </summary>
        /// <value>The char count.</value>
        public int CharCount { get; }

        /// <summary>
        /// Gets the type of the field text.
        /// </summary>
        /// <value>The type of the field text.</value>
        //public FieldTextType FieldTextType => FieldTextType.Calendar;

        /// <summary>
        /// ノードを初期化します．必ず1つ以上の要素がなければなりません．
        /// </summary>
        /// <param name="types">Types.</param>
        public CalendarNode (IEnumerable <CalendarType> types) {

            var it = types.GetEnumerator ();

            // 必ず1つ要素が必要
            if (!it.MoveNext ())
                throw new ArgumentException ();

            Type = it.Current;
            var node = this;
           
            while (it.MoveNext())
                node = node.CreateNext (it.Current);

            CharCount = CalendarTypeExt.CharCount(types);
        }


        private CalendarNode (CalendarType type)
        {
            Next = null;
            Type = type;
        }

        private CalendarNode CreateNext (CalendarType type) {
            return Next = new CalendarNode (type);
        }
    }
}

