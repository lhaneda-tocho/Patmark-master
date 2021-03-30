using System;
using System.Linq;
using Newtonsoft.Json;
using TokyoChokoku.CalendarModule;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.CalendarModule.Replacement;

using System.Collections.Generic;

namespace TokyoChokoku.CalendarModule.Setting
{
    /// <summary>
    /// カレンダーの置換設定を表すクラスです.
    /// </summary>
    public class CRSetting
    {
        public class Immutable: CRSetting
        {
            public Immutable(CRYearList yearList, CRMonthList monthList, CRDayList dayList, CRShiftList shiftList)
                : base(
                    yearList.ImmutableCopy(),
                    monthList.ImmutableCopy(),
                    dayList.ImmutableCopy(),
                    shiftList.ImmutableCopy()
                ) { }
            
            public Immutable() : base(
                new CRYearList.Immutable(),
                new CRMonthList.Immutable(),
                new CRDayList.Immutable(),
                new CRShiftList.Immutable()
            ) {}
        }
        public class Mutable : CRSetting
        {
            public Mutable(CRYearList yearList, CRMonthList monthList, CRDayList dayList, CRShiftList shiftList)
                : base(
                    yearList.MutableCopy(),
                    monthList.MutableCopy(),
                    dayList.MutableCopy(),
                    shiftList.MutableCopy()
                )
            { }

            public Mutable() : base(
                new CRYearList.Mutable(),
                new CRMonthList.Mutable(),
                new CRDayList.Mutable(),
                new CRShiftList.Mutable()
            )
            { }
        }
        public class JsonFormat {
            [JsonProperty("years")]
            IList<string>    YearList   { get; set; } = new List<string>();
            [JsonProperty("months")]
            IList<string>    MonthsList { get; set; } = new List<string>();
            [JsonProperty("days")]
            IList<string>    DayList    { get; set; } = new List<string>();
            [JsonProperty("shifts")]
            IList<Shift>     ShiftList  { get; set; } = new List<Shift>();

            public void FixError() {
                var yl = YearList;
                var ml = MonthsList;
                var dl = DayList;
                var sl = ShiftList;
                if(yl.Count != CRYearList.RequiredSize) {
                    int size  = yl.Count;
                    int index = 0;
                    foreach(var e in new CRYearList.Immutable()) {
                        if (index >= size)
                            yl.Add(e.Code.ToString());

                        index++;
                    }
                    YearList = yl.Take(CRYearList.RequiredSize).ToList();
                }
                if (ml.Count != CRMonthList.RequiredSize)
                {
                    int size = ml.Count;
                    int index = 0;
                    foreach (var e in new CRMonthList.Immutable())
                    {
                        if (index >= size)
                            ml.Add(e.Code.ToString());

                        index++;
                    }
                    MonthsList = ml.Take(CRMonthList.RequiredSize).ToList();
                }
                if (dl.Count != CRDayList.RequiredSize)
                {
                    int size = dl.Count;
                    int index = 0;
                    foreach (var e in new CRDayList.Immutable())
                    {
                        if (index >= size)
                            dl.Add(e.Code.ToString());

                        index++;
                    }
                    DayList = dl.Take(CRDayList.RequiredSize).ToList();
                }
                if (sl.Count != CRShiftList.RequiredSize)
                {
                    int size = sl.Count;
                    int index = 0;
                    foreach (var e in new CRShiftList.Immutable())
                    {
                        if (index >= size)
                            sl.Add(Shift.From(e));

                        index++;
                    }
                    ShiftList = sl.Take(CRShiftList.RequiredSize).ToList();
                }
            }

            public CRSetting Construct() {
                FixError();
                var yl = from e in YearList
                         select CRYear.Init(FirstOrNullChar(e));
                var ml = from e in MonthsList
                         select CRMonth.Init(FirstOrNullChar(e));
                var dl = from e in DayList
                         select CRDay.Init(FirstOrNullChar(e));
                var sl = from e in ShiftList
                         select e.Construct();

                return new CRSetting.Mutable(
                    new CRYearList.Mutable(yl),
                    new CRMonthList.Mutable(ml),
                    new CRDayList.Mutable(dl),
                    new CRShiftList.Mutable(sl)
                );
            }

            private static char FirstOrNullChar(string text) {
                if (string.IsNullOrEmpty(text))
                    return '\0';
                else
                    return text.First();
            }

            public static JsonFormat From(CRSetting setting) {
                var yl = from e in setting.YearList
                         select e.Code.ToString();

                var ml = from e in setting.MonthList
                         select e.Code.ToString();

                var dl = from e in setting.DayList
                         select e.Code.ToString();

                var sl = from s in setting.ShiftList
                         select Shift.From(s);

                var jf = new JsonFormat();
                jf.YearList   = yl.ToList();
                jf.MonthsList = ml.ToList();
                jf.DayList    = dl.ToList();
                jf.ShiftList  = sl.ToList();
                return jf;
            }


            public class Shift {
                [JsonProperty("code")]
                public string Code     { get; set; }
                [JsonProperty("enable")]
                public bool   Enable   { get; set; }
                [JsonProperty("startHour")]
                public byte   StartHour   { get; set; }
                [JsonProperty("startMinute")]
                public byte StartMinute { get; set; }
                [JsonProperty("endHour")]
                public byte   EndHour     { get; set; }
                [JsonProperty("endMinute")]
                public byte   EndMinute   { get; set; }

                public void FixError() {
                    if(string.IsNullOrEmpty(Code)){
                        Code   = "\0";
                        Enable = false;
                    } else {
                        Code = Code.First().ToString();
                    }
                }

                public CRShift Construct() {
                    FixError();
                    var range = TimeRange.Init();
                    range.From.Hour = StartHour;
                    range.From.Min  = StartMinute;
                    range.To.Hour = EndHour;
                    range.To.Min  = EndMinute;
                    return CRShift.Init(Code.First(), ref range);
                }

                public static Shift From(CRShift s) {
                    var js = new Shift();
                    var range = s.Range;

                    js.Code        = s.Code.ToString();
                    js.StartHour   = range.From.Hour;
                    js.StartMinute = range.From.Min;
                    js.EndHour     = range.To.Hour;
                    js.EndMinute   = range.To.Min;

                    return js;
                }
            }
        }

        public CRYearList  YearList  { get; }
        public CRMonthList MonthList { get; }
        public CRDayList   DayList   { get; }
        public CRShiftList ShiftList { get; }



        public void SetYearReplacement(int year, CRYear value)
        {
            YearList[year] = value;
        }

        public void SetMonthReplacement(int month, CRMonth value)
        {
            MonthList[month] = value;
        }

        public void SetDayReplacement(int day, CRDay value)
        {
            DayList[day] = value;
        }

        public void SetShiftReplacement(int shiftID, CRShift value)
        {
            ShiftList[shiftID] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.CalendarModule.Setting.CRSetting"/> class.
        /// </summary>
        /// <param name="yearList">Year list.</param>
        /// <param name="monthList">Month list.</param>
        /// <param name="dayList">Day list.</param>
        /// <param name="shiftList">Shift list.</param>
        public CRSetting(CRYearList yearList, CRMonthList monthList, CRDayList dayList, CRShiftList shiftList)
        {
            YearList = yearList;
            MonthList = monthList;
            DayList = dayList;
            ShiftList = shiftList;
        }

        /// <summary>
        /// Creates the node processor constructor.
        /// </summary>
        /// <returns>The node processor constructor.</returns>
        public CalendarNodeProcessor.Constructor CreateNodeProcessorConstructor()
        {
            return new CalendarNodeProcessor.Constructor(
                YearList ,
                MonthList,
                DayList  ,
                ShiftList);
        }


        /// <summary>
        /// Copy to mutable object.
        /// </summary>
        /// <returns>The copy.</returns>
        public CRSetting MutableCopy() {
            return new Mutable(YearList, MonthList, DayList, ShiftList);
        }

        /// <summary>
        /// Copy to immutable object;
        /// </summary>
        /// <returns>The copy.</returns>
        public CRSetting ImmutableCopy() {
            return new Immutable(YearList, MonthList, DayList, ShiftList);
        }

        public void Log()
        {
            var err = Console.Error;
            var yearLogs  = Enumerable.Range(0, YearList.Count).Zip(YearList, (i, e) => {
                return ("[" + i + "] = " + e.Code);
            });
            var monthLogs = Enumerable.Range(1, MonthList.Count).Zip(MonthList, (i, e) => {
                return ("[" + i + "] = " + e.Code);
            });
            var dayLogs   = Enumerable.Range(1, DayList.Count).Zip(DayList, (i, e) => {
                return ("[" + i + "] = " + e.Code);
            });
            var shiftLogs = Enumerable.Range(1, ShiftList.Count).Zip(ShiftList, (i, e) => {
                return ("[" + i + "] = " + e);
            });

            err.WriteLine("Year:");
            foreach (var log in yearLogs)
            {
                err.Write("  ");
                err.WriteLine(log);
            }
            err.WriteLine();
            err.WriteLine("Month:");
            foreach (var log in monthLogs)
            {
                err.Write("  ");
                err.WriteLine(log);
            }
            err.WriteLine();
            err.WriteLine("Day:");
            foreach (var log in dayLogs)
            {
                err.Write("  ");
                err.WriteLine(log);
            }
            err.WriteLine();
            err.WriteLine("Shift:");
            foreach (var log in shiftLogs)
            {
                err.Write("  ");
                err.WriteLine(log);
            }
        }
    }
}
