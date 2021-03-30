using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public class WritingCommandBuilder : CommandBuilder
	{
        public CommandDataType DataType { get; private set; }
        public int Addr { get; private set; }
		protected byte[] Datas;
        public ReadOnlyCollection<byte> ReadOnlyDatas { get { return new ReadOnlyCollection<byte> (new List<byte> (Datas)); } }


        public WritingCommandBuilder (MemoryAddress address, byte[] datas)
		{
            this.DataType = address.DataType;
            this.Addr = address.Address;

            if (address.DataType == CommandDataType.C){
                // エリアCは、2バイトに拡張して送信します（MB2コントローラの仕様です）
                var extractedData = new List<byte>();
                foreach (var data in datas)
                {
                    extractedData.Add(0x0);
                    extractedData.Add(data);
                }
                this.Datas = extractedData.ToArray();
            }
            else{
                this.Datas = datas;
            }
        }

        public WritingCommandBuilder (MemoryAddress address, List<byte> datas) : this(
			address,
			datas.ToArray ()
		)
		{
		}

        public WritingCommandBuilder (MemoryAddress address, byte data) : this(
            address,
			new byte[]{ data } // 
		)
		{
		}

		private short DataValueSize {
			get{
				if (Datas == null || Datas.Length == 0) {
					return 0;
				} else {
					return (short)(Datas.Length / DataType.DataSize ());
				}
			}
		}

		override protected List<Byte> GetBody (){
			List<Byte> result = new List<Byte>();
			result.Add( (byte)CommandMode.W );
			result.Add( (byte)DataType );
			result.AddRange( BigEndianBitConverter.GetBytes(Addr) );
			result.AddRange( BigEndianBitConverter.GetBytes(DataValueSize) );
			result.AddRange( Datas );
			return result;
		}

        public ICommand Build(bool needsResponse, int timeout = 1000, int numOfRetry = 3)
        {
            return new Command(this.ToBytes(), needsResponse, timeout, numOfRetry);
        }
	}
}

