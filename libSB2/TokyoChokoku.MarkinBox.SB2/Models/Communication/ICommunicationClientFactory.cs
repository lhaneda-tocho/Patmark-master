using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public interface ICommunicationClientFactory
	{
		ICommunicationClient Create();
	}
}

