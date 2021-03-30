using System;
using System.Threading.Tasks;

namespace TokyoChokoku.Communication
{
    public abstract class CommandTaskManager
    {
        public abstract Task<byte[]> StartCommand(ICommand command, Func<byte[], byte[]> extractResult);
    }
}
