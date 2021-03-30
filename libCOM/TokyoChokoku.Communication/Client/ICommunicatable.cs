using System;
using System.Threading.Tasks;
using System.Net;

namespace TokyoChokoku.Communication
{
	public interface ICommunicatable
	{
		void Send(byte[] command);
        void BeginReceive(Func<byte[], Boolean> callback);
        void Close();
    }
}

