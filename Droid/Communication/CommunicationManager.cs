using System;
using System.Net;
using Android.Net.Wifi;
using Android.App;
using TokyoChokoku.Communication;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace TokyoChokoku.Patmark.Droid.Communication
{
    public class CommunicationManager : ICommunicationChecker
	{
		public const String RemoteAddr = "172.31.0.1";
		public const int Port = 55000;

        private CommunicationManager (){
        }
        public static CommunicationManager Instance { get; } = new CommunicationManager();

		public bool IsConnectable(){

            try
            {
                WifiManager manager = (WifiManager)Android.App.Application.Context.GetSystemService(Service.WifiService);
                String ip = new IPAddress((long)manager.ConnectionInfo.IpAddress).ToString();
                if (Regex.IsMatch(ip, @"^172\.31\.0\.\d+$"))
                {
                    return true;
                }

                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (var addr in adapter.GetIPProperties().GatewayAddresses)
                    {
                        if (Regex.IsMatch(addr.Address.MapToIPv4().ToString(), @"^172\.31\.0\.\d+$"))
                        {
                            return true;
                        }
                    }
                }
                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var addressList = adapter?.GetIPProperties()?.DnsAddresses;
                    if(addressList == null) {
                        return false;
                    }
                    foreach(var addr in addressList){
                        //Log.Debug(String.Format("Self IP Address : {0}", addr.MapToIPv4()));
                        if (Regex.IsMatch(addr.MapToIPv4().ToString(), @"^172\.31\.0\.\d+$"))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (SocketException e)
            {
                // ホストが解決出来なかった場合は、失敗とします。
                Log.Debug("[CommunicationManager] ホストが解決できません ... " + e);
                return false;
            }
		}
	}
}

