using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static BitConverter.EndianBitConverter;

namespace TokyoChokoku.Communication
{
    /// <summary>
    /// 書き込みコマンドを構築するビルダです。
    /// </summary>
	public class WritingCommandBuilder : CommandBuilder
	{
        /// <summary>
        /// セクタ名です。
        /// </summary>
        public    CommandDataType          DataType { get; private set; }

        /// <summary>
        /// 上記セクタないのアドレスです。
        /// </summary>
        public    int                      Addr     { get; private set; }

        /// <summary>
        /// 送信するデータを書き出すオブジェクト
        /// </summary>
		public    Programmer               Data     { get; private set; }


        /// <summary>
        /// セクタ・アドレス・プログラマを指定して書き込みコマンドビルダを作成します。
        /// </summary>
        /// <param name="dataType">セクタ名</param>
        /// <param name="address">上記セクタ内のアドレス</param>
        /// <param name="data">送信するデータを書き出すオブジェクト</param>
        public WritingCommandBuilder(CommandDataType dataType, int address, Programmer data)
        {
            this.DataType = dataType;
            this.Addr = address;
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// メモリアドレスと、プログラマを指定して、書き込みコマンドビルダを作成します。
        /// </summary>
        /// <param name="address">メモリアドレス</param>
        /// <param name="data">送信するデータを書き出すオブジェクト</param>
        public WritingCommandBuilder (MemoryAddress address, Programmer data): this(
            address.DataType,
            address.Address,
            data
        )
		{
        }

        /// <summary>
        /// コマンドのデータ部サイズ。
        /// </summary>
		private short ElementCount
        {
			get
            {
                return (short) (Data.ByteCount / DataType.DataSize());
            }
		}

        /// <summary>
        /// メッセージボディを取得します。
        /// </summary>
        /// <returns>メッセージボディのバイト列</returns>
		override protected List<Byte> GetBody (){
			List<Byte> result = new List<Byte>();
			result.Add( (byte)CommandMode.W );
			result.Add( (byte)DataType );
			result.AddRange( BigEndian.GetBytes(Addr) );
			result.AddRange( BigEndian.GetBytes(ElementCount) );
            result.AddRange( Data.ToCommandFormat(DataType) );
			return result;
		}

        /// <summary>
        /// 送信設定を指定して、送信可能なコマンド形式に変換します。
        /// </summary>
        /// <param name="needsResponse">応答を待つかどうか</param>
        /// <param name="timeout">タイムアウト時間 [ms]</param>
        /// <param name="numOfRetry">最大再試行回数</param>
        /// <param name="delayAfter">コマンド送信後の待機時間 [ms]</param>
        /// <param name="expectedDataSize">期待する返答データサイズ</param>
        /// <param name="enableBeforeExcluding">排他前に送信して良いコマンドである場合は true を指定します。</param>
        /// <returns>コマンドリスト. コマンドサイズが十分小さければ、要素数1のリスト。そうでなければ、送信可能な単位に分割されたコマンドリスト。</returns>
        public List<ICommand> Build(bool needsResponse, int timeout = 500, int numOfRetry = 3, int delayAfter = 10, int expectedDataSize = 1, bool enableBeforeExcluding = false)
        {
            //BinalizerUtil.DumpBytes(Data.ToCommandFormat(DataType));
            //BinalizerUtil.DumpBytes(GetBody().ToArray());
            new Command(ToBytes(), needsResponse, timeout, numOfRetry, delayAfter, expectedDataSize, enableBeforeExcluding);
            return Divide().Select((builder) => {
                return (ICommand)new Command(builder.ToBytes(), needsResponse, timeout, numOfRetry, delayAfter, expectedDataSize, enableBeforeExcluding);
            }).ToList();
        }

        /// <summary>
        /// (内部実装) 読み込みサイズが大きすぎるコマンドは、複数回に分割します。
        /// </summary>
        /// <returns>The command.</returns>
        private List<WritingCommandBuilder> Divide()
        {
            return WritingCommandDivider.Divide(this);
        }
    }
}

