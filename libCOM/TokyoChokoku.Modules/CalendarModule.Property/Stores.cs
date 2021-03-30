using System;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook.Properties;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;
using TokyoChokoku.CalendarModule.Setting;
using TokyoChokoku.CalendarModule.Replacement;
using System.Collections;

namespace TokyoChokoku.CalendarModule.Property
{
    public class CRYearStore: Store<CRYear>
    {
        public CRYearStore(int year): base(
            Getter: (     ) => CRGlobalSetting.GetYear(year),
            Setter: (value) => CRGlobalSetting.SetYear(year, value),
            validator: (value) => ValidationResult.Empty
        ){}
    }

    public class CRMonthStore: Store<CRMonth>
    {
        public CRMonthStore(int month): base(
            Getter: (     ) => CRGlobalSetting.GetMonth(month),
            Setter: (value) => CRGlobalSetting.SetMonth(month, value),
            validator: (value) => ValidationResult.Empty
        ){}
    }

    public class CRDayStore : Store<CRDay>
    {
        public CRDayStore(int day) : base(
            Getter: (     ) => CRGlobalSetting.GetDay(day),
            Setter: (value) => CRGlobalSetting.SetDay(day, value),
            validator: (value) => ValidationResult.Empty
        ){}
    }

    public class CRShiftStore: Store<CRShift>
    {
        public CRShiftStore(int shiftID) : base(
            Getter: (     ) => CRGlobalSetting.GetShift(shiftID),
            Setter: (value) => CRGlobalSetting.SetShift(shiftID, value),
            validator: (value) => ValidationResult.Empty
        ){}
    }

    public class CRYearStoreList : IEnumerable<CRYearStore>
    {
        const int RequiredSize = 9;

        IList<CRYearStore> StoreList = new List<CRYearStore>(RequiredSize);

        public CRYearStore this[int index]
        {
            get
            {
                return StoreList[index];
            }
        }

        public CRYearStoreList()
        {
            for (int i = 0; i <= 9; i++)
            {
                var s = new CRYearStore(i);
                StoreList.Add(s);
            }
        }

        public IEnumerator<CRYearStore> GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }
    }

    public class CRMonthStoreList : IEnumerable<CRMonthStore>
    {
        const int RequiredSize = 12;

        IList<CRMonthStore> StoreList = new List<CRMonthStore>(RequiredSize);

        public CRMonthStore this[int index]
        {
            get
            {
                return StoreList[index-1];
            }
        }

        public CRMonthStoreList()
        {
            for (int i = 1; i <= 12; i++)
            {
                var s = new CRMonthStore(i);
                StoreList.Add(s);
            }
        }

        public IEnumerator<CRMonthStore> GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }
    }

    public class CRDayStoreList : IEnumerable<CRDayStore>
    {
        const int RequiredSize = 31;

        IList<CRDayStore> StoreList = new List<CRDayStore>(RequiredSize);

        public CRDayStore this[int index]
        {
            get
            {
                return StoreList[index-1];
            }
        }

        public CRDayStoreList()
        {
            for (int i = 1; i <= 31; i++)
            {
                var s = new CRDayStore(i);
                StoreList.Add(s);
            }
        }

        public IEnumerator<CRDayStore> GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }
    }


    public class CRShiftStoreList : IEnumerable<CRShiftStore>
    {
        const int RequiredSize = 5;

        IList<CRShiftStore> StoreList = new List<CRShiftStore>(RequiredSize);

        public CRShiftStore this[int index]
        {
            get
            {
                return StoreList[index-1];
            }
        }

        public CRShiftStoreList()
        {
            for (int i = 1; i <= 5; i++)
            {
                var s = new CRShiftStore(i);
                StoreList.Add(s);
            }
        }

        public IEnumerator<CRShiftStore> GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StoreList.GetEnumerator();
        }
    }
}
