using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox
{
    public class MBCalendarData
    {
        const int NumofShift =  MBCalendar.NumOfShift;

        public List<MBCalendarShiftData> ShiftReplacements = new List<MBCalendarShiftData>();
        public Dictionary<string, char> YearReplacements = new Dictionary<string, char>();
        public Dictionary<string, char> MonthReplacements = new Dictionary<string, char>();
        public Dictionary<string, char> DayReplacements = new Dictionary<string, char>();


        public static char GetDefautMonthReplacement(int index)
        {
            if (1 <= index && index <= 9)
                // M:1〜12 ⇒ A~L
                return (char)(index + 64);
            throw new ArgumentOutOfRangeException();
        }

        public static char GetDefautYearReplacement(int index)
        {
            if (1 <= index && index <= 9)
                // Y:0~9
                return (char)(index + 48);
            throw new ArgumentOutOfRangeException();
        }

        public MBCalendarData ()
        {
            // シフト
            ShiftReplacements.Clear();
            for(int i = 0; i < NumofShift; i++){
                ShiftReplacements.Add (new MBCalendarShiftData ());
            }
            // D:1~9
            for( int i = 1; i <= 9; i++ ){
                DayReplacements.Add(i.ToString(), (char)(i + 48));
            }
            // D:10~31 ⇒ A~V
            for( int i = 10; i <= 31; i++ ){
                DayReplacements.Add(i.ToString(),(char)(i + 64));
            }
            // M:1〜12 ⇒ A~L
            for( int i = 1; i <= 12; i++ ){
                MonthReplacements.Add(i.ToString(),(char)(i + 64));
            }
            // Y:0~9
            for( int i = 0; i <= 9; i++ ){
                YearReplacements.Add(i.ToString(), (char)(i + 48));
            }
        }

        public void SetReplacements(List<MBCalendarShiftData> shift, List<char> year, List<char> month, List<char> day){
            // shift
            for(int i = 0; i < Math.Min(ShiftReplacements.Count, NumofShift); i++){
                ShiftReplacements [i] = shift [i];
            }
            // year
            for (var i = 0; i < year.Count; i++) {
                YearReplacements [i.ToString ()] = year[i];
            }
            // month
            for (var i = 0; i < month.Count; i++) {
                MonthReplacements [(i+1).ToString ()] = month[i];
            }
            // day
            for (var i = 0; i < day.Count; i++) {
                DayReplacements [(i+1).ToString ()] = day[i];
            }
        }

        public int AvailableShiftsCount
        {
            get
            {
                return ShiftReplacements.Where( (shift) => {
                    return shift.Code != '\0';
                }).Count();
            }
        }
    }
}

