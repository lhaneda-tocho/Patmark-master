using System;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.MarkinBox;
using TokyoChokoku.Communication;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.Structure.Binary
{
    public class MBSerialSettingDataBinarizer
    {
        const int SerialCount = MBSerial.NumOfSerial;
        const int CommandSize = SerialCount * MBSerial.BytesOfSerialSetting;

        public Programmer Data { get; }
        public Settings   SettingData { get; }

        public MBSerialSettingDataBinarizer(Programmer data)
        {
            Data = data;
            if (data.ByteCount < CommandSize)
                throw new IndexOutOfRangeException();

            var chain = new MapperChain();
            SettingData = new Settings(data, chain);

            var totalByteCount = chain.TotalByteCount();
            if (totalByteCount != CommandSize)
                throw new InvalidProgramException();
        }

        public MBSerialSettingDataBinarizer(EndianFormatter formatter, IEnumerable<MBSerialData> sdata): this(
            new Programmer(formatter, new byte[CommandSize]))
        {
            SettingData.PutAll(sdata);
        }

        public MBSerialSettingDataBinarizer(EndianFormatter formatter, SCSettingList sdata): this(formatter, sdata.ToMBForm())
        {
        }

        /// <summary>
        /// To the command format.
        /// </summary>
        /// <returns>The command format.</returns>
        public byte[] ToCommandFormat()
        {
            return Data.ToCommandFormat(CommandDataType.R);
        }

        public List<MBSerialData> ConstructObject()
        {
            return SettingData.GetAll();
        }

        public class Settings
        {
            public Programmer Data;
            List<SettingMapper> Mappers;

            public MBSerialData this[int index]
            {
                get
                {
                    return Mappers[index].Read(Data);
                }
                set
                {
                    Mappers[index].Put(Data, value);
                }
            }

            public Settings(Programmer data, MapperChain chain)
            {
                Data = data;
                var mappers = new List<SettingMapper>(SerialCount);
                for (int i = 0; i < SerialCount; i++)
                {
                    var m = new SettingMapper(chain);
                    mappers.Add(m);
                }
                Mappers = mappers;
            }

            public void PutAll(IEnumerable<MBSerialData> values)
            {
                int i = 0;
                foreach (var data in values.Take(SerialCount))
                {
                    this[i] = data;
                    i++;
                }
            }

            public List<MBSerialData> GetAll()
            {
                var data = Data;
                var enu = from m in Mappers
                          select m.Read(data);
                return enu.ToList();
            }
        }

        class SettingMapper
        {
            UInt16Mapper format;
            UInt16Mapper resetRule;
            UInt32Mapper maxValue;
            UInt32Mapper minValue;
            ByteMapper   skipNumber;
            ByteMapper   repeatCount;
            UInt16Mapper clearingTime;

            public MBSerialData Read(Programmer data)
            {
                var ans = new MBSerialData();

                ans.Format         = format            .ReadFrom(data).First();
                ans.ResetRule      = resetRule         .ReadFrom(data).First();
                ans.MaxValue       = maxValue          .ReadFrom(data).First();
                ans.MinValue       = minValue          .ReadFrom(data).First();
                ans.SkipNumber     = skipNumber        .ReadFrom(data).First();
                ans.RepeatCount    = repeatCount       .ReadFrom(data).First();
                ans.ResetTime      = clearingTime      .ReadFrom(data).First();

                return ans;
            }

            public void Put(Programmer data, MBSerialData value)
            {
                format            .PutTo(data, value.Format            );
                resetRule         .PutTo(data, value.ResetRule         );
                maxValue          .PutTo(data, value.MaxValue          );
                minValue          .PutTo(data, value.MinValue          );
                skipNumber        .PutTo(data, value.SkipNumber        );
                repeatCount       .PutTo(data, value.RepeatCount       );
                clearingTime      .PutTo(data, value.ResetTime         );
            }

            public SettingMapper(MapperChain chain)
            {
                format            = chain.AllocUInt16 ();
                resetRule         = chain.AllocUInt16 ();
                maxValue          = chain.AllocUInt32 ();
                minValue          = chain.AllocUInt32 ();
                skipNumber        = chain.AllocByte   ();
                repeatCount       = chain.AllocByte   ();
                clearingTime      = chain.AllocUInt16 ();
            }
        }
    }
}

//using System;
//using System.Collections.Generic;

//namespace TokyoChokoku.MarkinBox.Sketchbook
//{
//    public class MBSerialSettingsDataBinarizer
//    {
//        private List<byte> Data = new List<byte>();

//        public MBSerialSettingsDataBinarizer (List<MBSerialData> serials)
//        {
//            foreach (var serial in serials)
//            {
//                Data.AddRange(serial.GetBytes());
//            }
//        }

//        public byte[] GetBytes()
//        {
//            return Data.ToArray ();
//        }
//    }
//}

