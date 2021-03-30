using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox;

using TokyoChokoku.CalendarModule;
using TokyoChokoku.CalendarModule.Replacement;

namespace TokyoChokoku.CalendarModule.Setting
{
    // TODO: エラー発生時のコールバックメソッドの実装
    public static class CRSettingIO
    {
        public static async Task<bool> SendToController(CRSetting setting)
        {
            try {
                var mbdata = ConvertToMBForm(setting);
                var client = CommunicationClient.Instance;
                if (!client.Ready)
                {
                    Console.Error.WriteLine("[CRSettingIO - SendToController]The client not ready yet.");
                    return false;
                }
                var exec = client.CreateCommandExecutor();
                var ymdres   = await exec.SetCalendarYmdReplacements  (mbdata);
                if (!ymdres.IsOk)
                {
                    Console.Error.WriteLine("[CRSettingIO - SendToController]Failure to set ymd replacements.");
                    return false;
                }
                var shiftres = await exec.SetCalendarShiftReplacements(mbdata);
                if (!shiftres.IsOk)
                {
                    Console.Error.WriteLine("[CRSettingIO - SendToController]Failure to set shift replacements");
                    return false;
                }
                var saveres = await exec.SaveBasiceSettingsToSdCard();
                if (!saveres.IsOk)
                {
                    Console.Error.WriteLine("[CRSettingIO - SendToController]Failure to write ymd and shift on sdcard");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Retrieves a calendar replacement settings from a remote controller.
        /// return null if cannot communicate with the controller.
        /// </summary>
        /// <returns>Mutable CRSetting instance.</returns>
        public static async Task<CRSetting> RetrieveFromController()
        {
            try {
                var client = CommunicationClient.Instance;
                if (!client.Ready)
                {
                    Console.Error.WriteLine("[CRSettingIO - RetrieveFromController]The client not ready yet.");
                    return null;
                }
                var exec = client.CreateCommandExecutor();
                var yr = await exec.ReadCalendarYearReplacements();
                if (!yr.IsOk)
                {
                    Log.Error("[CRSettingIO - RetrieveFromController]年設定の読み込みに失敗しました。");
                    return null;
                }
                var mr = await exec.ReadCalendarMonthReplacements();
                if (!mr.IsOk)
                {
                    Log.Error("[CRSettingIO - RetrieveFromController]月設定の読み込みに失敗しました。");
                    return null;
                }
                var dr = await exec.ReadCalendarDayReplacements();
                if (!dr.IsOk)
                {
                    Log.Error("[CRSettingIO - RetrieveFromController]日付設定の読み込みに失敗しました。");
                    return null;
                }
                var sr = await exec.ReadCalendarShiftReplacements();
                if (!sr.IsOk)
                {
                    Log.Error("[CRSettingIO - RetrieveFromController]シフト設定の書き込みに失敗しました。");
                    return null;
                }

                var ylist = from i in yr.Value
                            select CRYear.Init(i);
                var mlist = from i in mr.Value
                            select CRMonth.Init(i);
                var dlist = from i in dr.Value
                            select CRDay.Init(i);
                var slist = from i in sr.Value
                            select From(i);
                var availableCount = sr.AvailableCount;

                var years  = new CRYearList.Mutable(ylist);
                var months = new CRMonthList.Mutable(mlist);
                var days   = new CRDayList.Mutable(dlist);
                var shifts = new CRShiftList.Mutable(slist);
                shifts.ApplyAvailableCount(availableCount);

                return new CRSetting(years, months, days, shifts);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return null;
            }
        }

        public static CRShift From(MBCalendarShiftData shift) {
            var c = shift.Code;
            var sh = shift.StartingHour;
            var sm = shift.StartingMinute;
            var eh = shift.EndingHour;
            var em = shift.EndingMinute;

            var range = TimeRange.Init();
            range.From.Hour = sh;
            range.From.Min = sm;
            range.To.Hour = eh;
            range.To.Min = em;

            return CRShift.Init(c, ref range);
        }

        /// <summary>
        /// Converts to MBCalendarData
        /// </summary>
        public static MBCalendarData ConvertToMBForm(CRSetting setting)
        {
            var cdata = new MBCalendarData();
            var y = setting.YearList .Select((it) => it.Code).ToList();
            var m = setting.MonthList.Select((it) => it.Code).ToList();
            var d = setting.DayList  .Select((it) => it.Code).ToList();
            var s = ConvertToMBForm(setting.ShiftList);
            cdata.SetReplacements(s,y,m,d);
            return cdata;
        }

        /// <summary>
        /// Converts to MBCalendarShiftData
        /// </summary>
        public static List<MBCalendarShiftData> ConvertToMBForm(CRShiftList shifts)
        {
            var enu = from e in shifts.CopyEnables()
                      select ConvertToMBForm(e);
            return enu.ToList();
        }

        /// <summary>
        /// Converts to MBCalendarShiftData
        /// </summary>
        public static MBCalendarShiftData ConvertToMBForm(CRShift shift) 
        {
            var sdata = new MBCalendarShiftData();
            var s = shift.Range.From;
            var e = shift.Range.To;

            sdata.Code = shift.Code;

            sdata.StartingHour   = s.Hour;
            sdata.StartingMinute = s.Min;

            sdata.EndingHour   = e.Hour;
            sdata.EndingMinute = e.Min;

            return sdata;
        }
    }
}
