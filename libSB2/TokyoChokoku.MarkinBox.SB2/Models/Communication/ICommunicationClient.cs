using System;
using System.Threading.Tasks;
using System.Net;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public interface ICommunicationClient
	{
		void BeginReceive ();
		void Send(byte[] command);
		Task<byte[]> ReceiveAsync(int timeout);
        void Close();
    }
}

