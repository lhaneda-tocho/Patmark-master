using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class CommandTaskManager
    {
        /// <summary>
        /// 同時に複数のリクエストを送らないように制限するため、
        /// シングルトンでリクエストコマンドのキューを管理します。
        /// </summary>
        private static CommandTaskManager _Instance = new CommandTaskManager();
        public static CommandTaskManager Instance { get { return _Instance; } }

        /// <summary>
        /// 最後に送信した時刻です。
        /// </summary>
        private int LastSentAt = DateTime.Now.Millisecond;

        /// <summary>
        /// 最短100ミリ秒間隔で送信します。
        /// </summary>
        private const int SendingInterval = 100;

        /// <summary>
        /// コマンドキュー
        /// </summary>
        private Queue<ICommand> Commands;

        /// <summary>
        /// 
        /// </summary>
        private CommandTaskManager()
        {
            Commands = new Queue<ICommand>();
        }

        public async Task<byte[]> StartCommand(ICommand command)
        {
            return await StartCommand(command, 0);
        }

        private async Task<byte[]> StartCommand(ICommand command, int trialCount)
        {
            var udpClient = CommunicationClientManager.Instance.CreateClient();
            // 
            Log.Debug("[CommandTaskManager.StartCommand] キューにコマンドを登録します ... " + command.ToString());
            Commands.Enqueue(command);
            // 先頭になるまで待ちます。
            while (Commands.Peek() != command)
            {
                await Task.Delay(5);
            }
            Log.Debug("[CommandTaskManager.StartCommand] コマンドを実行します ... " + BitConverter.ToString(command.ToBytes()));

			// データの受信を待機します。
			if (command.NeedsResponse)
			{
				Log.Debug("[CommandTaskManager.StartCommand] コマンドの受信待機を開始します。");
				udpClient.BeginReceive();
			}

            // 受信の待機が完了するまで待ちます。
            await Task.Delay(30);

            // 最短の送信間隔に合わせて、送信を遅らせます。
            //var pastTime = (DateTime.Now.Millisecond - LastSentAt);
            //if (pastTime < SendingInterval)
            //{
            //    await Task.Delay(SendingInterval - pastTime);
            //}

            // 実行します。
            udpClient.Send(command.ToBytes());

            // 受信を待機します。
            try
            {
                var result = new byte[] { };
                if (command.NeedsResponse)
                {
					Log.Debug("[CommandTaskManager.StartCommand] コマンドの受信を待機します。");
					result = await udpClient.ReceiveAsync(command.Timeout);
                    udpClient.Close();
                    udpClient = null;
                }
                else
                {
					// ACK, FRM(適当) ... 通信に成功したことにします。
					result = new byte[] { 0x06, 0x01 };
                }
                LastSentAt = DateTime.Now.Millisecond;
                // 完了したら、キューから取り除きます。
                Commands.Dequeue();
                Log.Debug("[CommandTaskManager.StartCommand] コマンドが完了しました ... " + command.ToString());
                Log.Debug("[CommandTaskManager.StartCommand] 結果 ... " + BitConverter.ToString(result));
                //
                return result;
            }
            catch (TimeoutException ex)
            {
                LastSentAt = DateTime.Now.Millisecond;
                // 失敗したら、キューから取り除きます。
                Commands.Dequeue();
                Log.Error(ex.ToString());

                // コマンドをやり直します。
                if (trialCount < command.NumOfRetry)
                {
                    udpClient.Close();
                    udpClient = null;
                    return await StartCommand(command, trialCount + 1);
                }
                else
                {
                    // 例外を上位層に投げます
                    throw ex;
                }
            }
            finally
            {
                if (udpClient != null)
                {
                    udpClient.Close();
                    udpClient = null;
                }
            }
        }
    }
}

