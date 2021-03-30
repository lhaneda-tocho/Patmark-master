using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoChokoku.Communication
{
    public class WritingCommandDivider
    {
        public static List<WritingCommandBuilder> Divide(WritingCommandBuilder source)
        {
            var divider = new WritingCommandDivider(source);
            return divider.Invoke();
        }

        // ================

        public    CommandDataType          DataType { get; }
        public    int                      Addr     { get; }
		public    Programmer               Data     { get; } // nonnull

        private long ElementCount
        {
            get
            {
                return Data.ByteCount / DataType.DataSize();
            }
        }

        private uint MaxElementCount {
            get
            {
                return Sizes.MaxWritingDataSize / DataType.DataSize();
            }
        }

        private List<uint> StepSizeList
        {
            get
            {
                var maxElementsPerStep = MaxElementCount;
                var elements = ElementCount;

                var res = new List<uint>();
                var division = elements / maxElementsPerStep;
                for (var i = 0; i < division; i++)
                {
                    res.Add(maxElementsPerStep);
                }
                var remaining = elements % maxElementsPerStep;
                if (remaining != 0)
                {
                    res.Add((uint)remaining);
                }
                return res;
            }
        }

        public WritingCommandDivider(WritingCommandBuilder builder)
        {
            DataType = builder.DataType;
            Addr     = builder.Addr;
            Data     = builder.Data ?? throw new ArgumentNullException("Data should not be null");
        }

        public List<WritingCommandBuilder> Invoke()
        {

            var commands = new List<WritingCommandBuilder>();
            var max = MaxElementCount;
            var elementCount = ElementCount;
            if (elementCount <= max)
            {
                commands.Add(new WritingCommandBuilder(DataType, Addr, Data));
                return commands;
            }
            else
            {
                Log.Debug("[Writing]コマンドを分割します ... " + "Addr:" + Addr + ", DataSize[count]:" + elementCount);
                Log.Debug(Data.GetAllBytes().ToString());
                var stepSizeList = StepSizeList;
                foreach (var (index, elements) in Enumerable.Range(0, stepSizeList.Count).Zip(stepSizeList, (x, y) => (x, y)))
                {
                    var addressOffset   = (int)(index * max);
                    var byteOffset      = (int)(addressOffset * DataType.DataSize());
                    var byteLen         = (int)(elements * DataType.DataSize());
                    var address         = Addr + addressOffset;
                    var content = Data.GetAllBytes()
                                      .Skip(byteOffset)
                                      .Take(byteLen)
                                      .ToArray();
                    commands.Add(new WritingCommandBuilder(
                        new MemoryAddress(
                            DataType,
                            Addr + addressOffset
                        ),
                        new Programmer(
                            Data.TheFormatter,
                            content
                        )
                    ));
                    Log.Debug($"Addr:{DataType}[{address}], DataSize:{elements} ({byteLen}[Byte])");
                    Log.Debug(content.ToString());
                }
                return commands;
            }
        }
    }
}
