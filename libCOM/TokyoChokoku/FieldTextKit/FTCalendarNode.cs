using System;
using TokyoChokoku.CalendarModule;
using TokyoChokoku.CalendarModule.Ast;
using System.Collections.Generic;

namespace TokyoChokoku.FieldTextKit
{
    public class FTCalendarNode: CalendarNode, IFieldTextNode
    {
        public FieldTextType FieldTextType => FieldTextType.Calendar;

        public FTCalendarNode(IEnumerable<CalendarType> types): base(types)
        {}
    }
}
