using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
	public interface ICommand
	{
		Byte[] ToBytes();

        /// <summary>
        /// true:enable waiting response.
        /// </summary>
        bool NeedsResponse { get; }

        /// <summary>
        /// </summary>
        int Timeout { get; }

        /// <summary>
        /// </summary>
        int NumOfRetry { get; }
    }
}

