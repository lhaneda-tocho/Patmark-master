using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    /// <summary>
    /// 【参考】
    /// How to easily get Device IP Address in Xamarin.Forms using DependencyService | ÇøŋfuzëÐ SøurcëÇødë
    /// https://goo.gl/nuhgwR
    /// </summary>
	public class CommunicationClient : ICommunicationClient
	{
        private IPEndPoint RemoteEp;
		private UdpClient Client = null;
		private UdpClient ReceiverFixed = null;

        private List<Task<byte[]>> ReadingTasks = new List<Task<byte[]>>();

		public CommunicationClient (string remoteAddr, int port)
		{
            var localIp = NetworkInterface.GetAllNetworkInterfaces().Select((ni) =>
            {
                return ni.GetIPProperties().UnicastAddresses.FirstOrDefault((addr) =>
                {
                    return Regex.IsMatch(addr.Address.ToString(), @"^172\.31\.0\.\d+$");
                });
            }).Where((addr)=>
            {
                return addr != null;
            }).FirstOrDefault();

            var localEp = new IPEndPoint(localIp != null ? localIp.Address : IPAddress.Any, port);
			RemoteEp = new IPEndPoint (IPAddress.Parse (remoteAddr), port);

			Log.Debug(String.Format("Remote IP ... {0}:{1}", RemoteEp.Address, RemoteEp.Port));
			Log.Debug(String.Format("Local IP ... {0}:{1}", localEp.Address, localEp.Port));
			
            Client = CreateClient(null);
            ReceiverFixed = CreateClient(localEp);
		}

		public void BeginReceive(){
			ReadingTasks.Add(Task.Factory.StartNew(() =>
			{
				return Client.Receive(ref RemoteEp);
			}));
			ReadingTasks.Add(Task.Factory.StartNew(() =>
			{
				return ReceiverFixed.Receive(ref RemoteEp);
			}));
		}

		public void Send(byte[] command){
			Client.Send(command, command.Length, RemoteEp);
		}

		public async Task<byte[]> ReceiveAsync(int timeout){
            var resultTaskIndex = Task.WaitAny(ReadingTasks.ToArray(), timeout);
            if (resultTaskIndex >= 0)
            {
                return await ReadingTasks[resultTaskIndex].ContinueWith((result) =>
                {
                    return result.Result;
                });
            }
            else
            {
                throw new TimeoutException("[CommunicationClient - ReceiveAsync] コントローラからの応答がありません。");
            }
		}

        public void Close()
        {
            if (Client != null)
            {
                Client.Client.Disconnect(true);
                Client.Close();
                Client = null;
            }
            if (ReceiverFixed != null)
            {
                ReceiverFixed.Client.Disconnect(true);
                ReceiverFixed.Close();
                ReceiverFixed = null;
            }
        }

        private static UdpClient CreateClient(IPEndPoint localIp)
        {
            var client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            if (localIp != null)
            {
                client.Client.Bind(localIp);
            }
            return client;
        }
	}
}

