using System;
using System.Threading.Tasks;
using System.Net;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public interface ICommunicationChecker
	{
        bool IsConnectable();
	}
}

