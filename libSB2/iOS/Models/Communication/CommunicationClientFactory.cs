using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class CommunicationClientFactory : ICommunicationClientFactory
	{
		private static CommunicationClientFactory _Instance = new CommunicationClientFactory();
		public static CommunicationClientFactory Instance { get { return _Instance; } }
		private CommunicationClientFactory ()
		{
		}

		public ICommunicationClient Create(){
			return new CommunicationClient (CommunicationManager.RemoteAddr, CommunicationManager.Port);
		}
	}
}

