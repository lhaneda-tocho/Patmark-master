using System;

namespace TokyoChokoku.Communication
{
	public interface ICommunicatableSupplier
	{
		ICommunicatable Create();
	}
}

