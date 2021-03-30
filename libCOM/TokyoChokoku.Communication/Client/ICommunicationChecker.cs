using System;
using System.Threading.Tasks;
using System.Net;

namespace TokyoChokoku.Communication
{
    public interface ICommunicationChecker
	{
        bool IsConnectable();
	}
}

