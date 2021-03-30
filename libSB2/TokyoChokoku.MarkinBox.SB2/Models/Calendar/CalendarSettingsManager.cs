using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class CalendarSettingsManager
    {
        private CalendarSettingsManager ()
        {
        }
        public static CalendarSettingsManager Instance { get; } = new CalendarSettingsManager();

        private MBCalendarData CalendarData = new MBCalendarData();

        public async Task<Nil> Reload(){
            CalendarData = await MBCalendarDataFactory.Create();
            return null;
        }

        public async Task<bool> Save(){
            if (CommunicationClientManager.Instance.IsOnline ()) 
            {
                {
                    var res = await CommandExecuter.SetCalendarYmdReplacements (
                                   new MBCalendarDataBinarizer (CalendarData)
                               );
                    if (!res.IsOk) {
                        Log.Error ("[CalendarSettingsManager - Save]日付設定の書き込みに失敗しました。");
                        return false;
                    }
                }

                {
                    var res = await CommandExecuter.SetCalendarShiftReplacements (
                                    new MBCalendarShiftDataBinarizer (CalendarData)
                                );
                    if (!res.IsOk) {
                        Log.Error ("[CalendarSettingsManager - Save]シフト設定の書き込みに失敗しました。");
                        return false;
                    }
                }

                {
                    if (!(await CommandExecuter.SaveBasiceSettingsToSdCard()).IsOk)
                    {
                        Log.Error("[CalendarSettingsManager - Save]カレンダー設定とシフト設定のSDカードへの書き込みに失敗しました。");
                        return false;
                    }
                }
            }
            return true;
        }

        public List<CalendarYearStore> CreateYearStores(){
            return CalendarYearStoresFactory.Create (CalendarData);
        }

        public List<CalendarMonthStore> CreateMonthStores(){
            return CalendarMonthStoresFactory.Create (CalendarData);
        }

        public List<CalendarDayStore> CreateDayStores(){
            return CalendarDayStoresFactory.Create (CalendarData);
        }

        public List<CalendarShiftStore> CreateShiftStores(){
            return CalendarShiftStoresFactory.Create (CalendarData);
        }
    }
}

