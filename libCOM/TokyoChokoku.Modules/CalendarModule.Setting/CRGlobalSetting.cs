using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku.CalendarModule.Ast;
using TokyoChokoku.CalendarModule.Replacement;
namespace TokyoChokoku.CalendarModule.Setting
{
    /// <summary>
    /// カレンダー置換設定です.
    /// </summary>
    public static class CRGlobalSetting
    {
        static object theLock = new object();

        volatile static CRSetting Setting = new CRSetting.Mutable();

        public static readonly Property.CRYearStoreList  YearStoreList  = new Property.CRYearStoreList ();
        public static readonly Property.CRMonthStoreList MonthStoreList = new Property.CRMonthStoreList();
        public static readonly Property.CRDayStoreList   DayStoreList   = new Property.CRDayStoreList  ();
        public static readonly Property.CRShiftStoreList ShiftStoreList = new Property.CRShiftStoreList();

        static void NewReference(CRSetting setting)
        {
            lock (theLock)
            {
                Setting = setting;
            }
        }

        public static void SetSetting(CRSetting setting) {
            NewReference(setting.MutableCopy());
        }

        public static CalendarNodeProcessor.Constructor CreateNodeProcessorConstructor()
        {
            var data = MutableCopy();
            return data.CreateNodeProcessorConstructor();
        }

        public static CalendarNodeProcessor CreateNodeProcessor(DateTime date)
        {
            return CreateNodeProcessorConstructor().Create(date);
        }

        public static CalendarNodeProcessor CreateNodeProcessorWithCurrentTime()
        {
            return CreateNodeProcessorConstructor().CreateWithCurrentTime();
        }

        public static CRSetting ImmutableCopy() {
            lock (theLock)
            {
                return Setting.ImmutableCopy();
            }
        }

        public static CRSetting MutableCopy() {
            lock (theLock)
            {
                return Setting.MutableCopy();
            }
        }



        public static CRYear GetYear(int year)
        {
            lock (theLock)
            {
                return Setting.YearList[year];
            }
        }

        public static CRMonth GetMonth(int month)
        {
            lock (theLock)
            {
                return Setting.MonthList[month];
            }
        }

        public static CRDay GetDay(int day)
        {
            lock (theLock)
            {
                return Setting.DayList[day];
            }
        }

        public static CRShift GetShift(int shiftID)
        {
            lock (theLock)
            {
                return Setting.ShiftList[shiftID];
            }
        }

        public static void SetYear(int year, CRYear value)
        {
            lock (theLock)
            {
                Setting.SetYearReplacement(year, value);
            }
        }

        public static void SetMonth(int month, CRMonth value)
        {
            lock (theLock)
            {
                Setting.SetMonthReplacement(month, value);
            }
        }

        public static void SetDay(int day, CRDay value)
        {
            lock (theLock)
            {
                Setting.SetDayReplacement(day, value);
            }
        }

        public static void SetShift(int shiftID, CRShift value)
        {
            lock (theLock)
            {
                Setting.SetShiftReplacement(shiftID, value);
            }
        }

        public static async Task<bool> RetrieveFromController() {
            var maybeSetting = await CRSettingIO.RetrieveFromController();
            if (maybeSetting == null)
                return false;
            NewReference(maybeSetting);
            return true;
        }

        public static async Task<bool> SendToController() {
            var data = MutableCopy();
            return await CRSettingIO.SendToController(data);
        }

        public static void Log() {
            var copy = MutableCopy();
            copy.Log();
        }
    }
}
