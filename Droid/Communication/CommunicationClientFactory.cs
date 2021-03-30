using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using TokyoChokoku.Communication;
using Android.Content;

namespace TokyoChokoku.Patmark.Droid.Communication
{
    public class CommunicationClientFactory : ICommunicatableSupplier
	{
        Context Ctx;

        public static void SetUp(Context ctx){
            Instance = new CommunicationClientFactory(ctx);
        }
        
        public static CommunicationClientFactory Instance { get; private set; }
        private CommunicationClientFactory (Context ctx)
		{
            Ctx = ctx;
		}

        public ICommunicatable Create(){
            return new PatmarkCommunicatable (Ctx, CommunicationManager.RemoteAddr, CommunicationManager.Port);
		}
	}
}

