using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public static class MetaCalendarStores
    {
        // ymd

        public class YmdDefinition
        {
            public string Name { get; }
            public IEnumerable<int> Numbers { get; }

            public YmdDefinition(string name, IEnumerable<int> numbers)
            {
                this.Name = name;
                this.Numbers = numbers;
            }
        }
 
        public static YmdDefinition YearDefinition { get; } = new YmdDefinition (
            name: "Year",
            numbers: Enumerable.Range (0, 10)
        );

        public static YmdDefinition MonthDefinition { get; } = new YmdDefinition (
            name: "Month",
            numbers: Enumerable.Range (1, 12)
        );

        public static YmdDefinition DayDefinition { get; } = new YmdDefinition (
            name: "Day",
            numbers: Enumerable.Range (1, 31)
        );

        public static ReadOnlyCollection <YmdDefinition> YmdDefinitions { get; }


        // shift

        public class ShiftDefinition
        {
            public string Name { get; }
            public string TypeName { get; }

            public ShiftDefinition(string name, string typeName)
            {
                this.Name = name;
                this.TypeName = typeName;
            }
        }

        public static ReadOnlyCollection <ShiftDefinition> ShiftDefinitions { get; }

        // 

        static MetaCalendarStores () {

            // ymd

            List<YmdDefinition> ymdList = new List <YmdDefinition> ();

            ymdList.Add(YearDefinition);
            ymdList.Add(MonthDefinition);
            ymdList.Add(DayDefinition);

            YmdDefinitions = ymdList.AsReadOnly ();

            // shift

            List<ShiftDefinition> shiftList = new List <ShiftDefinition> ();

            shiftList.Add(new ShiftDefinition("Code", "char"));
            shiftList.Add(new ShiftDefinition("StartingHour", "byte"));
            shiftList.Add(new ShiftDefinition("StartingMinute", "byte"));
            shiftList.Add(new ShiftDefinition("EndingHour", "byte"));
            shiftList.Add(new ShiftDefinition("EndingMinute", "byte"));

            ShiftDefinitions = shiftList.AsReadOnly ();
        }
    }
}

