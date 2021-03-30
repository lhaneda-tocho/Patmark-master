using System;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.iOS.RenderKitForIOS;
using TokyoChokoku.Patmark.StorageUtil;
using TokyoChokoku.Patmark.iOS.Models.Logging;
using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.iOS.Injection
{
    public static class Injecter
    {
		public static void ProgramInit()
		{
			CommonInit();
            ScreenUtil.Inject(new iOSScreenUtil());
            FontFactory.Inject(new iOSFontFactory());

            // ロガーの実装を注入します。
            Log.I = new LogPlatform();

            // TokyoChokoku.Patmark 側で利用する文言を注入します。
            CommonStrings.Inject(new CommonStringsImpl());

            // ローカルファイルのパス取得方法を注入します。
            LocalFilePathGeneratorPublisher.Inject(new LocalFilePathGenerator());

            // 通信クライアントの実装を注入します。
            CommunicationClient.InitPatmarkClient(
                CommunicationClientFactory.Instance,
                CommunicationManager.Instance
            );

            // KeyValueStoreのアクセス方法を注入します。
            KeyValueStoreFactoryHolder.Inject(new KeyValueStore.Factory());
                                      
            // デフォルト値の取得方法を注入します。
            DefaultParameterProvider.Init(
                (arg) => new AppSettingMarkingParameterDB().GetTextSize(arg),
                (arg) => new AppSettingMarkingParameterDB().GetForce(arg),
                (arg) => new AppSettingMarkingParameterDB().GetQuality(arg)
            );
        }

        /// <summary>
        /// インスペクタで表示するために用いる．複数回呼び出しても例外が起きません．
        /// </summary>
        public static void DesignTimeInit()
        {
            SetAcceptReinject();
            CommonInit();
			ScreenUtil.InjectNeeded(new iOSScreenUtil());
			FontFactory.InjectNeeded(new iOSFontFactory());


            // KeyValueStoreのアクセス方法を注入します。
            KeyValueStoreFactoryHolder.Inject(new KeyValueStore.Factory());

            // デフォルト値の取得方法を注入します。
            DefaultParameterProvider.Init(
                (arg) => new AppSettingMarkingParameterDB().GetTextSize(arg),
                (arg) => new AppSettingMarkingParameterDB().GetForce(arg),
                (arg) => new AppSettingMarkingParameterDB().GetQuality(arg)
            );
		}

		static void SetAcceptReinject()
		{
			StorageProvider.AcceptReinject = true;
		}

        static void CommonInit()
		{
			StorageProvider.FieldStorage = new FieldStorageForiOS();
        }
    }
}
