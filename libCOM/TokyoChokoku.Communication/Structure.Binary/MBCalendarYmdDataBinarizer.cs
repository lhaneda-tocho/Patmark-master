using System;
using System.Collections.Generic;
using System.Linq;
using TokyoChokoku.MarkinBox;
using TokyoChokoku.Communication;
using TokyoChokoku.Communication.Binalizer;


namespace TokyoChokoku.Structure.Binary
{
    /// <summary>
    /// C(1Byte)領域のデータ
    /// </summary>
    public class MBCalendarYmdDataBinarizer
    {
        const int CommandSize = MBCalendar.TotalBytesOfYMD;

        public Programmer Data { get; }
        public Replacement YearReplacement;
        public Replacement MonthReplacement;
        public Replacement DayReplacement;


        public MBCalendarYmdDataBinarizer(Programmer data)
        {
            Data = data;
            if (data.ByteCount < CommandSize)
                throw new IndexOutOfRangeException();
            
            var chain = new MapperChain();
            YearReplacement  = Replacement.Calloc(data, chain, MBCalendar.CharsOfYearReplacements);
            MonthReplacement = Replacement.Calloc(data, chain, MBCalendar.CharsOfMonthReplacements);
            DayReplacement   = Replacement.Calloc(data, chain, MBCalendar.CharsOfDayReplacements);

            var totalByteCount = chain.TotalByteCount();
            if (totalByteCount != CommandSize)
                throw new InvalidProgramException();
        }

        public MBCalendarYmdDataBinarizer(EndianFormatter formatter, MBCalendarData src) : this(new Programmer(formatter, CommandSize))
        {
            Put(src);
        }

        public void Put(MBCalendarData cdata)
        {
            YearReplacement.PutAll(from i in cdata.YearReplacements.Values.AsEnumerable()
                                   select (byte)i );

            MonthReplacement.PutAll(from i in cdata.MonthReplacements.Values.AsEnumerable()
                                    select (byte)i );

            DayReplacement.PutAll(from i in cdata.DayReplacements.Values.AsEnumerable()
                                  select (byte)i );
        }

        /// <summary>
        /// To the command format.
        /// </summary>
        /// <returns>The command format.</returns>
        public byte[] ToCommandFormat()
        {
            return Data.ToCommandFormat(CommandDataType.C);
        }

        public class Replacement {
            public readonly Programmer Data;
            public readonly List<ByteMapper> Mapper;
             
            public byte this[int index] {
                get {
                    return Mapper[index].ReadFrom(Data).First();
                }
                set {
                    Mapper[index].PutTo(Data, value);
                }
            }

            public Replacement(Programmer data, List<ByteMapper> mapper)
            {
                if (data == null || mapper == null)
                    throw new NullReferenceException();
                Data   = data;
                Mapper = mapper;
            }

            public void PutAll(IEnumerable<byte> bytes) {
                int index = 0;
                foreach(var b in bytes) {
                    if (index < Mapper.Count)
                        this[index++] = b;
                    else
                        break;
                }
            }

            public List<byte> GetAll() {
                var dest = new List<byte>(Mapper.Count);
                for (int i = 0; i < Mapper.Count; i++) 
                {
                    dest.Add(this[i]);
                }
                return dest;
            }

            public static Replacement Calloc(Programmer data, MapperChain chain, int size)
            {
                var list = new List<ByteMapper>(size);
                for (int i = 0; i < size; i++)
                {
                    list.Add(chain.AllocByte());
                }
                return new Replacement(data, list);
            }
        }
    }
}
