using System;
namespace TokyoChokoku.ControllerIO
{
    /// <summary>
    /// ControllerがBusyの状態など，通信してはならないタイミングで通信を試みた場合に発生します.
    /// コマンドごとに条件, タイミングは変化します.
    /// </summary>
    public class CommunicationProtectedException: System.IO.IOException
    {
        public CommunicationProtectedException()
        {
        }

        public CommunicationProtectedException(string message) : base(message)
        {
        }

        public CommunicationProtectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommunicationProtectedException(string message, int hresult) : base(message, hresult)
        {
        }

        public static CommunicationProtectedException WhileMarking()
        {
            return new CommunicationProtectedException("Reason: marking now.");
        }
    }
}
