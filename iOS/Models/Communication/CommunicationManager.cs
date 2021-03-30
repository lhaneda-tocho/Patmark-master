using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.iOS
{
    public class CommunicationManager : ICommunicationChecker
	{
		public const String RemoteAddr = "172.31.0.1";
		public const int Port = 55000;

        private CommunicationManager (){
        }
        public static CommunicationManager Instance { get; } = new CommunicationManager();

		public bool IsConnectable(){
			try{
                foreach(var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (var addr in adapter.GetIPProperties().GatewayAddresses)
                    {
                        if (addr.Address.ToString().Contains(RemoteAddr))
                        {
                            return true;
                        }
                    }
                }
                return false;
			}catch(SocketException e){
                // ホストが解決出来なかった場合は、失敗とします。
                Log.Debug("[CommunicationManager] ホストが解決できません ... " + e);
				return false;
			}
		}
	}
}

