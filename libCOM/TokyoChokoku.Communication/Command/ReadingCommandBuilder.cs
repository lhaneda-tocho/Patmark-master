using System;
using System.Linq;
using System.Collections.Generic;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Communication
{
	public class ReadingCommandBuilder : CommandBuilder
	{
        private static readonly ILog Log = CommunicationLogger.Supply();

        public CommandDataType DataType { get; private set; }
        public int Addr { get; private set; } = 0;
		short DataValueSize = 0;


        public ReadingCommandBuilder (MemoryAddress address, short dataValueSize)
		{
			this.DataType = address.DataType;
			this.Addr = address.Address;
			this.DataValueSize = dataValueSize;
        }

		override protected List<Byte> GetBody (){
			List<Byte> result = new List<Byte>();
			result.Add( (byte)CommandMode.R );
			result.Add( (byte)DataType );
			result.AddRange( BigEndian.GetBytes(Addr) );
			result.AddRange( BigEndian.GetBytes(DataValueSize) );
			return result;
		}

        public List<ICommand> Build(int timeOut = 500, int numOfRetry = 3, int delayAfter = 10, bool enableBeforeExcluding = false)
        {
            return Divide().Select((builder) => {
                return (ICommand)new Command(builder.ToBytes(), true, timeOut, numOfRetry, delayAfter, (int)builder.DataType.ReceivedDataByteSize() * builder.DataValueSize, enableBeforeExcluding);
            }).ToList();
        }

        /// <summary>
        /// 読み込みサイズが大きすぎるコマンドは、複数回に分割します。
        /// </summary>
        /// <returns>The command.</returns>
        private List<ReadingCommandBuilder> Divide()
        {
            var readings = new List<ReadingCommandBuilder> ();
            var max = (int)Math.Floor ((decimal)(Sizes.MaxReadingBytes / DataType.DataSize()));
            if (DataValueSize <= max) {
                readings.Add(this);
            } else {
                Log.Debug ("[Reading]コマンドを分割します ... " + "Addr:"+Addr+", DataSize:"+DataValueSize);
                var steps = DivideNumberToSteps (DataValueSize, max);
                var totalSteps = 0;
                foreach(var step in steps) {
                    readings.Add(
                        new ReadingCommandBuilder(
                            new MemoryAddress(
                                DataType,
                                Addr + totalSteps
                            ),
                            (short)step
                        )
                    );
                    Log.Debug ("Addr:"+(Addr+totalSteps)+", DataSize:"+step);
                    totalSteps += step;
                }
            }
            return readings;
        }



        private static List<int> DivideNumberToSteps(int number, int step)
        {
            var res = new List<int>();
            var division = (int)Math.Floor((decimal)number / (decimal)step);
            for (var i = 0; i < division; i++) {
                res.Add (step);
            }
            var remaining = number % step;
            if (remaining != 0) {
                res.Add (remaining);
            }
            return res;
        }
	}
}

