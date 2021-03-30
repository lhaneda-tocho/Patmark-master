using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarTypeNode
    {
        // null when I am final element. otherwise
        public CalendarTypeNode Next {get; private set;}

        // nonnull
        public CalendarType     Type {get;}



        /// <summary>
        /// ノードを初期化します．必ず1つ以上の要素がなければなりません．
        /// </summary>
        /// <param name="types">Types.</param>
        public CalendarTypeNode (IEnumerable <CalendarType> types) {

            var it = types.GetEnumerator ();

            // 必ず1つ要素が必要
            if (!it.MoveNext ())
                throw new ArgumentException ();

            Type = it.Current;
            var node = this;

            while (it.MoveNext())
                node = node.CreateNext (it.Current);
            
        }


        private CalendarTypeNode (CalendarType type)
        {
            Next = null;
            Type = type;
        }



        private CalendarTypeNode CreateNext (CalendarType type) {
            return Next = new CalendarTypeNode (type);
        }
    }
}

