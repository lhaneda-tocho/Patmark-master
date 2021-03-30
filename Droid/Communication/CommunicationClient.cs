using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Communication;
using Android.Content;
using Android.Net.Wifi;
using Android.Runtime;

namespace TokyoChokoku.Patmark.Droid.Communication
{
	/// <summary>
	/// 【参考】
	/// How to easily get Device IP Address in Xamarin.Forms using DependencyService | ÇøŋfuzëÐ SøurcëÇødë
	/// https://goo.gl/nuhgwR
	/// </summary>
	public class PatmarkCommunicatable : ICommunicatable
	{
        WifiManager.MulticastLock multiLock;
		IPEndPoint RemoteEp;
		UdpClient Client = null;

		private List<Task<byte[]>> ReadingTasks = new List<Task<byte[]>>();

        public PatmarkCommunicatable(Context ctx, string remoteAddr, int port)
		{
            var manager = ctx.GetSystemService(Context.WifiService) as WifiManager;
            multiLock = manager.CreateMulticastLock("Patmark");
            multiLock.SetReferenceCounted(true);
            multiLock.Acquire();

			var localIp = NetworkInterface.GetAllNetworkInterfaces().Select((ni) =>
			{
				return ni.GetIPProperties().UnicastAddresses.FirstOrDefault((addr) =>
				{
					return Regex.IsMatch(addr.Address.ToString(), @"^172\.31\.0\.\d+$");
				});
			}).Where((addr) =>
			{
				return addr != null;
			}).FirstOrDefault();

			var localEp = new IPEndPoint(localIp != null ? localIp.Address : IPAddress.Any, port);
			RemoteEp = new IPEndPoint(IPAddress.Parse(remoteAddr), port);

			Log.Debug(String.Format("Remote IP ... {0}:{1}", RemoteEp.Address, RemoteEp.Port));
			Log.Debug(String.Format("Local IP ... {0}:{1}", localEp.Address, localEp.Port));

			Client = CreateClient(localEp);
		}

        public void BeginReceive(Func<byte[], Boolean> callback)
        {
            Client.BeginReceive(async (ar) =>
            {
                try
                {
                    var client = (UdpClient)ar.AsyncState;
                    if (!callback(client.EndReceive(ar, ref RemoteEp)))
                    {
                        BeginReceive(callback);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }, Client);
        }

        public void Send(byte[] command)
        {
            Client.Send(command, command.Length, RemoteEp);
        }

		public void Close()
		{
            if(multiLock != null)
            {
                multiLock.Release();
                multiLock = null;
            }
			if (Client != null)
			{
				Client.Client.Disconnect(true);
				Client.Close();
				Client = null;
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