using System;
using TokyoChokoku;


namespace TokyoChokoku.Communication
{
    public static class CommunicationLogger
	{
        static readonly ILog log = None.Run(() =>
        {
            return new LogDefault();
        });

        static CommunicationLogger()
        {
        }

 		public static ILog Supply() {
            return log;
        }

        public static void SelfTest()
		{
            //log.Fatal("fatal level output");
            log.Error("error level output");
            log.Warn ("warn  level output");
			log.Info ("info  level output");
            log.Debug("debug level output");
        }

        public static bool isConfigured()
        {
            return log != null;
        }
    }
}
