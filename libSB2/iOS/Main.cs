using System;

using UIKit;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{	
			// ロガーの実装を設定します。
			Log.I = new LogPlatform ();

            // 設定情報の取得ロジックを設定します。
            PreferenceManager.Init(new PreferenceAccessor());

			// 通信クライアントの実装を共通部分に渡します。
            CommunicationClientManager.Init(
                CommunicationClientFactory.Instance,
                CommunicationManager.Instance
            );

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
                Console.WriteLine (e);
            }
		}
	}
}
