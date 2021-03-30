using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ReponseMBCalendarShiftData : Response
    {
        public ReponseMBCalendarShiftData (IRawResponse raw) : base(raw)
        {
        }

        public List<MBCalendarShiftData> Value{
            get{
                var res = new List<MBCalendarShiftData> ();
                for (var i = 0; i < Calendar.Consts.NumOfShift; i++) {
                    res.Add (
                        new MBCalendarShiftData (
                            Raw.Data
                                .Skip (i * Calendar.Consts.BytesOfShift)
                                .Take (Calendar.Consts.BytesOfShift).ToArray ()
                        )
                    );
                }
                return res;
            }
        }
    }
}

