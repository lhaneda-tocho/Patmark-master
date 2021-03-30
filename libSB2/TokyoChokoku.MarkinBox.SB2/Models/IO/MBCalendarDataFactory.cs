using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public static class MBCalendarDataFactory
    {
        public static async Task<MBCalendarData> Create ()
        {
            var calendar = new MBCalendarData ();

            if (CommunicationClientManager.Instance.IsOnline ()) {
                calendar.SetReplacements (
                    (await CommandExecuter.ReadCalendarShiftReplacements ()).Value,
                    (await CommandExecuter.ReadCalendarYearReplacements ()).Value,
                    (await CommandExecuter.ReadCalendarMonthReplacements ()).Value,
                    (await CommandExecuter.ReadCalendarDayReplacements ()).Value
                );
            }

            return calendar;
        }
    }
}

