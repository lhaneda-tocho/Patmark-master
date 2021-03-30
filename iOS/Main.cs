using System;
using UIKit;


namespace TokyoChokoku.Patmark.iOS
{
    using Injection;

    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Injecter.ProgramInit();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			try
			{
				UIApplication.Main(args, null, "AppDelegate");
			}
			catch (TimeoutException e)
			{
				Console.WriteLine("コントローラへの接続がタイムアウトしました。");
				Console.WriteLine(e);
			}
			catch (System.Net.Sockets.SocketException e)
			{
				Console.WriteLine("ネットワークに接続できません。");
				Console.WriteLine(e);
			}
        }
    }
}
