using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarNode : IFieldTextNode
    {
        public FieldTextType FieldTextType {
            get {
                return FieldTextType.Calendar;
            }
        }

        public IEnumerable<CalendarType> TypeEnumerable { get; }


        public int CharCount () {
            return CalendarTypeExt.CharCount (TypeEnumerable);
        }


        public CalendarNode (IEnumerable<CalendarType> typeEnumerable)
        {
            this.TypeEnumerable = typeEnumerable;
        }

        override
        public string ToString () {
            var shifts = CalendarSettingsManager.Instance.CreateShiftStores ();
            var years = CalendarSettingsManager.Instance.CreateYearStores  ();
            var month = CalendarSettingsManager.Instance.CreateMonthStores ();
            var days  = CalendarSettingsManager.Instance.CreateDayStores   ();

            DateTime date = DateTime.Now;

            var sb = new System.Text.StringBuilder ();
            foreach (var type in TypeEnumerable) {
                switch (type) {
                    case CalendarType.S: {

                        // 一番始めに該当する シフトを採用する

                        char symbol = ' ';
                        foreach (var shift in shifts) {
                            // s : start  e : end
                            // H : Hour   M : Minute
                            int nowH = date.Hour;
                            int nowM = date.Minute;
                            int sH   = shift.StartingHourStore  .Content;
                            int sM   = shift.StartingMinuteStore.Content;
                            int eH   = shift.EndingHourStore    .Content;
                            int eM   = shift.EndingMinuteStore  .Content;

                            // 23:59 ~ 0:01 対策
                            if (sH > eH) eH += 24;

                            bool isEqualsOrGreaterThanStart = (sH < nowH) || (sH == nowH && sM <= nowM);
                            bool isLessThanEnd              = (nowH < eH) || (nowH == eH && nowM <  eM);

                            // 現在のシフトにいる場合
                            if (isEqualsOrGreaterThanStart && isLessThanEnd) {
                                symbol = shift.CodeStore.Content;
                                break;
                            }

                        }
                        sb.Append (symbol);
                        break;
                    }
                    case CalendarType.YYYY: {
                        sb.Append (date.Year.ToString ().PadLeft (4, '0'));
                        break;
                    }
                    case CalendarType.YY: {
                        sb.Append ((date.Year % 100).ToString ().PadLeft (2, '0'));
                        break;
                    }
                    case CalendarType.Y: {
                        if (years.Count >= 10)
                            sb.Append (years [date.Year % 10].Content);
                        else
                            sb.Append ('Y');
                        break;
                    }
                    case CalendarType.MM: {
                        sb.Append (date.Month.ToString ().PadLeft (2, '0'));
                        break;
                    }
                    case CalendarType.M: {
                        if (month.Count >= 12)
                            sb.Append (month [date.Month - 1].Content); // 月は 1から番号が始まる．
                        else
                            sb.Append ('M');
                        break;
                    }
                    case CalendarType.DD: {
                        sb.Append (date.Day.ToString ().PadLeft (2, '0'));
                        break;
                    }
                    case CalendarType.JJJ: {
                        sb.Append (date.DayOfYear.ToString ().PadLeft (3, '0'));
                        break;
                    }
                    case CalendarType.D: {
                        if (days.Count >= 31)
                            sb.Append (days [date.Day - 1].Content); // 日付は 1から番号が始まる．
                        else
                            sb.Append ('D');
                        break;
                    }
                }
            }
            return sb.ToString ();
        }
    }
}

