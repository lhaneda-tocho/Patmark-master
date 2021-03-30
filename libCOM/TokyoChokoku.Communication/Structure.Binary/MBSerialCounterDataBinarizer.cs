using System;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox;
using TokyoChokoku.Communication;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.Structure.Binary
{
    public class MBSerialCounterDataBinarizer
    {
        const int SerialCount = MBSerial.NumOfSerial;
        const int CommandSize = SerialCount * MBSerial.BytesOfSerialCounter;

        public Programmer Data { get; }
        public Counters   CounterData { get; }

        public MBSerialCounterDataBinarizer(Programmer data)
        {
            Data = data;
            if (data.ByteCount < CommandSize)
                throw new IndexOutOfRangeException();

            var chain = new MapperChain();
            CounterData = new Counters(data, chain);

            var totalByteCount = chain.TotalByteCount();
            if (totalByteCount != CommandSize)
                throw new InvalidProgramException();
        }

        public MBSerialCounterDataBinarizer(EndianFormatter formatter, SCCountStateList sdata): this(
            new Programmer(formatter, new byte[CommandSize]))
        {
            CounterData.PutAll(sdata.ToMBForm());
        }

        /// <summary>
        /// To the command format.
        /// </summary>
        /// <returns>The command format.</returns>
        public byte[] ToCommandFormat()
        {
            return Data.ToCommandFormat(CommandDataType.R);
        }

        public List<MBSerialCounterData> ConstructObject() {
            return CounterData.GetAll();
        }

        public class Counters {
            public Programmer Data;
            List<CounterMapper> Mappers;

            public MBSerialCounterData this[int index] {
                get {
                    return Mappers[index].Read(Data);
                }
                set {
                    Mappers[index].Put(Data, value);
                }
            }

            public Counters(Programmer data, MapperChain chain) {
                Data = data;
                var mappers = new List<CounterMapper>(SerialCount);
                for (int i = 0; i < SerialCount; i++)
                {
                    var m = new CounterMapper(chain);
                    mappers.Add(m);
                }
                Mappers = mappers;
            }

            public void PutAll(IEnumerable<MBSerialCounterData> values) {
                int i = 0;
                foreach(var data in values.Take(SerialCount)) {
                    this[i] = data;
                    i++;
                }
            }

            public List<MBSerialCounterData> GetAll() {
                var data = Data;
                var enu = from m in Mappers
                          select m.Read(data);
                return enu.ToList();
            }
        }

        class CounterMapper
        {
            public UInt16Mapper serialNo;
            public UInt16Mapper repeatingCount;
            public UInt32Mapper currentValue;
            public UInt32Mapper lastUpdateDate;
            public UInt16Mapper lastUpdateTime;
            public UInt16Mapper fileNo;

            public MBSerialCounterData Read(Programmer data) {
                var ans = new MBSerialCounterData();

                ans.SerialNo       = serialNo      .ReadFrom(data).First();
                ans.RepeatingCount = repeatingCount.ReadFrom(data).First();
                ans.CurrentValue   = currentValue  .ReadFrom(data).First();
                ans.LastUpdateDate = lastUpdateDate.ReadFrom(data).First();
                ans.LastUpdateTime = lastUpdateTime.ReadFrom(data).First();
                ans.FileNo         = fileNo        .ReadFrom(data).First();

                return ans;
            }

            public void Put(Programmer data, MBSerialCounterData value) {
                serialNo      .PutTo(data, value.SerialNo);
                repeatingCount.PutTo(data, value.RepeatingCount);
                currentValue  .PutTo(data, value.CurrentValue);
                lastUpdateDate.PutTo(data, value.LastUpdateDate);
                lastUpdateTime.PutTo(data, value.LastUpdateTime);
                fileNo        .PutTo(data, value.FileNo);
            }

            public CounterMapper(MapperChain chain) {
                serialNo       = chain.AllocUInt16();
                repeatingCount = chain.AllocUInt16();
                currentValue   = chain.AllocUInt32();
                lastUpdateDate = chain.AllocUInt32();
                lastUpdateTime = chain.AllocUInt16();
                fileNo         = chain.AllocUInt16();
            }
        }
    }
}

//using System;
//using System.Collections.Generic;

//namespace TokyoChokoku.MarkinBox.Sketchbook
//{
//    public class MBSerialCountersDataBinarizer
//    {
//        private List<byte> Data = new List<byte>();

//        public MBSerialCountersDataBinarizer(List<MBSerialCounterData> counters)
//        {
//            foreach (var counter in counters)
//            {
//                Data.AddRange(counter.GetBytes());
//            }
//        }

//        public byte[] GetBytes()
//        {
//            return Data.ToArray ();
//        }
//    }
//}

