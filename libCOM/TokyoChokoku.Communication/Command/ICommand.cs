using System;

namespace TokyoChokoku.Communication
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

        /// <summary>
        /// </summary>
        int DelayAfter { get; }

        /// <summary>
        /// </summary>
        int ExpectedDataSize { get; }

        bool EnableBeforeExcluding { get; }
    }
}

