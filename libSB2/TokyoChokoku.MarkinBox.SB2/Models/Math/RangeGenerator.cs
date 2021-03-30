using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class RangeGenerator
    {
        static RangeGenerator ()
        {
        }

        public static List<int> DivideNumberToSteps(int number, int step)
        {
            var res = new List<int>();
            var division = (int)Math.Floor((decimal)(number / step));
            for (var i = 0; i < division; i++) {
                res.Add (step);
            }
            var remaining = number % step;
            if (remaining != 0) {
                res.Add (remaining);
            }
            return res;
        }
    }
}

