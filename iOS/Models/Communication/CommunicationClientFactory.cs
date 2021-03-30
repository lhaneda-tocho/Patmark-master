using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;

namespace TokyoChokoku.Patmark.iOS
{
    public class CommunicationClientFactory : ICommunicatableSupplier
	{
		private static CommunicationClientFactory _Instance = new CommunicationClientFactory();
		public static CommunicationClientFactory Instance { get { return _Instance; } }
		private CommunicationClientFactory ()
		{
		}

        public ICommunicatable Create(){
			return new PatmarkCommunicatable (CommunicationManager.RemoteAddr, CommunicationManager.Port);
		}
	}
}

