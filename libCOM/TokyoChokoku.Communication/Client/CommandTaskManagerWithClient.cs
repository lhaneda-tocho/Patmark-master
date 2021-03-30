using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoChokoku;

namespace TokyoChokoku.Communication
{
    public class CommandTaskManagerWithClient: CommandTaskManager
    {
        static readonly ILog Log = CommunicationLogger.Supply();


        /// <summary>
        /// 最後に送信した時刻です。
        /// </summary>
        long LastSentAt = DateTime.Now.Ticks;

        /// <summary>
        /// 最短100ミリ秒間隔で送信します。
        /// </summary>
        const int SendingInterval = 100;

        /// <summary>
        /// コマンドキュー
        /// </summary>
        Queue<ICommand> Commands;

        readonly CommunicationClient ClientManager;




        /// <summary>
        /// 
        /// </summary>
        public CommandTaskManagerWithClient(CommunicationClient clientManager)
        {
            Commands = new Queue<ICommand>();
            ClientManager = clientManager;
        }

        public override async Task<byte[]> StartCommand(ICommand command, Func<byte[], byte[]> extractResult)
        {
            return await StartCommand(command, 0, extractResult);
        }

        async Task<byte[]> StartCommand(ICommand command, int trialCount, Func<byte[], byte[]> extractResult)
        {
            ICommunicatable udpClient = null;

			// 受信を待機します。
			try
            {
                udpClient = ClientManager.CreateCommunicatable();
                // 
                Debug("[CommandTaskManager.StartCommand] キューにコマンドを登録します ... " + command);
                Commands.Enqueue(command);

                //// 先頭になるまで待ちます。
                //while (Commands.Peek() != command)
                //{
                //       await Task.Delay(10);
                //}

                Debug("[CommandTaskManager.StartCommand] コマンドを実行します ... " + BinalizerUtil.BytesToString(command.ToBytes()));
                // データの受信を待機します。

                var result = new List<byte>();

                if (command.NeedsResponse)
                {
                    udpClient.BeginReceive((data) =>
                    {
                        var tmpData = extractResult(data);
                        result.AddRange(tmpData);
                        Log.Debug(String.Format("読み取り中 ... {0} / {1} bytes : {2}", result.Count, command.ExpectedDataSize, BinalizerUtil.BytesToString(tmpData)));
                        return result.Count >= command.ExpectedDataSize;
                    });
                }else{
                    result.AddRange(new byte[] { 0x06, 0x00 });
                }

                //// 最短の送信間隔に合わせて、送信を遅らせます。
                //var pastTime = (DateTime.Now.Ticks - LastSentAt);
                //if (pastTime < SendingInterval)
                //{
                //    await Task.Delay(SendingInterval - pastTime);
                //}

                // 送信します。
                udpClient.Send(command.ToBytes());

                LastSentAt = DateTime.Now.Ticks;
                // コマンド完了まで待ちます。
                while (result.Count < command.ExpectedDataSize)
                {
                    Log.Debug(String.Format("待機中 ... {0} / {1} bytes , timeout {2} / {3}", result.Count, command.ExpectedDataSize, (DateTime.Now.Ticks - LastSentAt) / 10000, command.Timeout));
                    if((DateTime.Now.Ticks - LastSentAt) / 10000 > command.Timeout) {
                        System.Diagnostics.Debug.WriteLine($"repeat before excluding: {command.EnableBeforeExcluding}");
                        throw new TimeoutException();
                    }
                    await Task.Delay(50);
                }

                if (command.DelayAfter > 0)
                {
                    await Task.Delay(command.DelayAfter);
                }

                // 完了したら、キューから取り除きます。
                Commands.Dequeue();
                Debug(String.Format("コマンドが完了しました ... {0}", command));
                Debug(String.Format("結果 ... {0} / {1} bytes ... {2}", result.Count, command.ExpectedDataSize, BinalizerUtil.BytesToString(result.ToArray())));
                //
                return result.ToArray();
            }
            catch (TimeoutException ex)
            {
                LastSentAt = DateTime.Now.Ticks;
                // 失敗したら、キューから取り除きます。
                Commands.Dequeue();
                Log.Error(ex.ToString());

                // コマンドをやり直します。
                if (trialCount < command.NumOfRetry)
                {
                    if (!ClientManager.IsOnline() || !command.EnableBeforeExcluding && !ClientManager.Ready)
                    {
                        throw new TimeoutException("接続が切れているため、タイムアウト扱いにします。");
                    }

                    udpClient.Close();
                    udpClient = null;

                    await Task.Delay(100);
                    return await StartCommand(command, trialCount + 1, extractResult);
                }
                else {
                    // 例外を上位層に投げます
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw new TimeoutException(String.Format("通信中に例外「{0}」が発生したため、タイムアウト扱いにします。", ex));
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

        private void Debug(String str){
            Console.WriteLine(str);
        }
    }
}

