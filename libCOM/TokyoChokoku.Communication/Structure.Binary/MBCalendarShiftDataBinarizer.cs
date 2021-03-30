using System;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox.Sketchbook.Calendar;

using TokyoChokoku.MarkinBox;
using TokyoChokoku.Structure.Binary.FileFormat;
using TokyoChokoku.Communication;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Structure.Binary
{
    /// <summary>
    /// (2Byte領域) R領域のデータ
    /// </summary>
    public class MBCalendarShiftDataBinarizer
    {
        const int CommandSize = MBCalendar.BytesOfAvailableShiftSize + MBCalendar.TotalBytesOfShifts;
        const int NumOfShift = MBCalendar.NumOfShift;

        public Programmer Data { get; }
        readonly UInt16Mapper availableShiftsCount;



        public UInt16 AvailableShiftsCount {
            get {
                return availableShiftsCount.ReadFrom(Data).First();
            }
            set {
                availableShiftsCount.PutTo(Data, value);
            }
        }

        public readonly List<ShiftMemMap> Shifts;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.Structure.Binary.MBCalendarShiftDataBinarizer"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        public MBCalendarShiftDataBinarizer(Programmer data)
        {
            Data = data;
            if (data.ByteCount < CommandSize)
                throw new IndexOutOfRangeException("Invalid data size: " + data.ByteCount);

            var chain = new MapperChain();
            availableShiftsCount = chain.AllocUInt16();
            Shifts = ShiftMemMap.Calloc(data, chain, MBCalendar.NumOfShift);

            // ===== Validation
            var totalCount = chain.TotalByteCount();
            if (totalCount != CommandSize) throw new InvalidProgramException("MBCalendarShiftData size validation error: " + chain.TotalByteCount());
        }

        public MBCalendarShiftDataBinarizer(EndianFormatter formatter, MBCalendarData src): this(
            new Programmer(formatter, CommandSize)
        )
        {
            Put(src);
        }

        /// <summary>
        /// Put the specified calendar.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="calendar">Calendar.</param>
        public void Put(MBCalendarData calendar)
        {
            AvailableShiftsCount = (UInt16)calendar.AvailableShiftsCount;

            Shifts.Zip(calendar.ShiftReplacements, (mem, input)=>{
                mem.Code          = input.Code;
                mem.StartingValue = input.StartingValue;
                mem.EndingValue   = input.EndingValue;
                return mem;
            });
        }

        public List<MBCalendarShiftData> ConstructObject()
        {
            var enu =  from mem in Shifts
                       select new MBCalendarShiftData().Also((it) =>
                       {
                           it.Code = (char) mem.Code;
                           it.StartingValue = mem.StartingValue;
                           it.EndingValue = mem.EndingValue;
                       });
            return enu.ToList();
        }


        /// <summary>
        /// Tos the command format.
        /// </summary>
        /// <returns>The command format.</returns>
        public byte[] ToCommandFormat()
        {
            return Data.ToCommandFormat(CommandDataType.R);
        }



        /// <summary>
        /// Shift mem map.
        /// </summary>
        public class ShiftMemMap
        {
            Programmer Data;
            readonly UInt16Mapper code;
            readonly UInt16Mapper startingValue;
            readonly UInt16Mapper endingValue;

            public UInt16 Code
            {
                get
                {
                    return code.ReadFrom(Data).First();
                }
                set
                {
                    code.PutTo(Data, value);
                }
            }
            public UInt16 StartingValue
            {
                get
                {
                    return startingValue.ReadFrom(Data).First();
                }
                set
                {
                    startingValue.PutTo(Data, value);
                }
            }
            public UInt16 EndingValue
            {
                get
                {
                    return endingValue.ReadFrom(Data).First();
                }
                set
                {
                    endingValue.PutTo(Data, value);
                }
            }

            public ShiftMemMap(Programmer data, UInt16Mapper code, UInt16Mapper startingValue, UInt16Mapper endingValue)
            {
                Data = data;
                this.code = code;
                this.startingValue = startingValue;
                this.endingValue = endingValue;
            }

            public static ShiftMemMap Malloc(Programmer data, MapperChain chain)
            {
                var code = chain.AllocUInt16();
                var sv = chain.AllocUInt16();
                var ev = chain.AllocUInt16();
                return new ShiftMemMap(data, code, sv, ev);
            }

            public static List<ShiftMemMap> Calloc(Programmer data, MapperChain chain, int count)
            {
                var list = new List<ShiftMemMap>(count);
                for (int i = 0; i < count; i++)
                {
                    var m = Malloc(data, chain);
                    list.Add(m);
                }
                return list;
            }
        }
    }
}
//namespace TokyoChokoku.MarkinBox.Sketchbook
//{
//    public class MBCalendarShiftDataBinarizer
//    {
//        const int NumofShift =  Calendar.Consts.NumOfShift;

//        private List<byte> Data = new List<byte>();

//        public MBCalendarShiftDataBinarizer (MBCalendarData calendar)
//        {
//            Data.AddRange (BigEndianBitConverter.GetBytes ((char)calendar.AvailableShiftsCount));
//            foreach (var shift in calendar.ShiftReplacements) {
//                Data.AddRange (BigEndianBitConverter.GetBytes (shift.Code));
//                Data.AddRange (BigEndianBitConverter.GetBytes (shift.StartingValue));
//                Data.AddRange (BigEndianBitConverter.GetBytes (shift.EndingValue));
//            }
//        }

//        public byte[] GetBytes()
//        {
//            return Data.ToArray ();
//        }
//    }
//}

