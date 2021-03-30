using System;
using System.Text;

using TokyoChokoku.CalendarModule.Replacement;

namespace TokyoChokoku.CalendarModule.Ast
{
    public class CalendarNodeProcessor
    {
        /// <summary>
        /// Constructor class of CalendarNodeConverter.
        /// </summary>
        public class Constructor {
            public CRYearList  YearRepList  { get; }
            public CRMonthList MonthRepList { get; }
            public CRDayList   DayRepList   { get; }
            public CRShiftList TimeRepList  { get; }

            public Constructor(CRYearList y, CRMonthList m, CRDayList d, CRShiftList s){
                TimeRepList  = s;
                YearRepList  = y;
                MonthRepList = m;
                DayRepList   = d;
            }

            public Constructor() {
                TimeRepList  = new CRShiftList.Immutable();
                YearRepList  = new CRYearList.Immutable();
                MonthRepList = new CRMonthList.Immutable();
                DayRepList   = new CRDayList.Immutable();
            }

            public CalendarNodeProcessor Create(DateTime date) {
                var yrep = YearRepList [date];
                var mrep = MonthRepList[date];
                var drep = DayRepList  [date];
                var trep = TimeRepList.FirstAtTime(date, CRShift.Init(' ')); // TODO: 該当するシフトがなかった場合の初期値の設定
                return new CalendarNodeProcessor(time: date, 
                                                 yearRep : yrep,
                                                 monthRep: mrep,
                                                 dayRep  : drep,
                                                 timeRep : trep);
            }

            public CalendarNodeProcessor CreateWithCurrentTime()
            {
                return Create(DateTime.Now);
            }
        }

        public DateTime   Date;
        public CRYear     YearRep;
        public CRMonth    MonthRep;
        public CRDay      DayRep;
        public CRShift    TimeRep;

        public CalendarNodeProcessor(DateTime time, CRYear yearRep, CRMonth monthRep, CRDay dayRep, CRShift timeRep)
        {
            Date     = time;
            YearRep  = yearRep;
            MonthRep = monthRep;
            DayRep   = dayRep;
            TimeRep  = timeRep;
        }

        CalendarNode Traverse(CalendarNode node, StringBuilder sb)
        {
            switch (node.Type)
            {
                case CalendarType.S:
                    {
                        char symbol = TimeRep.Code;
                        sb.Append(symbol);
                        break;
                    }
                case CalendarType.YYYY:
                    {
                        sb.Append(Date.Year.ToString().PadLeft(4, '0'));
                        break;
                    }
                case CalendarType.YY:
                    {
                        sb.Append((Date.Year % 100).ToString().PadLeft(2, '0'));
                        break;
                    }
                case CalendarType.Y:
                    {
                        sb.Append(YearRep.Code);
                        break;
                    }
                case CalendarType.MM:
                    {
                        sb.Append(Date.Month.ToString().PadLeft(2, '0'));
                        break;
                    }
                case CalendarType.M:
                    {
                        sb.Append(MonthRep.Code);
                        break;
                    }
                case CalendarType.DD:
                    {
                        sb.Append(Date.Day.ToString().PadLeft(2, '0'));
                        break;
                    }
                case CalendarType.JJJ:
                    {
                        sb.Append(Date.DayOfYear.ToString().PadLeft(3, '0'));
                        break;
                    }
                case CalendarType.D:
                    {
                        sb.Append(DayRep.Code);
                        break;
                    }
            }
            return node.Next;
        }

        /// <summary>
        /// Convert the specified node to string.
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="node">Node.</param>
        public string Convert(CalendarNode node)
        {
            var sb = new StringBuilder();
            while (node != null)
                node = Traverse(node, sb);
            return sb.ToString();
        }

    }
}
