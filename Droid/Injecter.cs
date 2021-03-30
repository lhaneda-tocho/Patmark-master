using System;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.StorageUtil;
using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Patmark.EmbossmentKit;
using Android.Content;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.Droid
{
    using Droid.Communication;
    using Droid.Logging;
    using RenderKitForDroid;


    public static class Injecter
	{
        static object TheLock = new object();
        static volatile bool Initialized = false;

		/// <summary>
		/// Android用 Injection. 複数回呼び出しても例外が起きません．
		/// </summary>
        public static void InjectIfNeeded(Context ctx)
		{
            lock (TheLock)
            {
                if (Initialized)
                    return;
                Initialized = true;

                SetAcceptReinject();
                CommonInit();
                //ScreenUtil.InjectNeeded(new iOSScreenUtil());
                FontFactory.InjectNeeded(new DroidFontFactory());

				// ロガーの実装を設定します。
				Log.I = new LogPlatform();

                // TokyoChokoku.Patmark 側で利用する文言を注入します。
                CommonStrings.Inject(new CommonStringsImpl(ctx));

                // ローカルファイルのパス取得方法を注入します。
                LocalFilePathGeneratorPublisher.Inject(new LocalFilePathGenerator());


                // 通信クライアントの実装を共通部分に渡します。
                CommunicationClientFactory.SetUp(ctx);
				CommunicationClient.InitPatmarkClient(
					CommunicationClientFactory.Instance,
					CommunicationManager.Instance
				);

                // KeyValueStoreのアクセス方法を注入します。
                KeyValueStoreFactoryHolder.Inject(new KeyValueStore.Factory(ctx));

                // デフォルト値の取得方法をセットします。
                DefaultParameterProvider.Init(
                    (arg) => new AppSettingMarkingParameterDB().GetTextSize(arg),
                    (arg) => new AppSettingMarkingParameterDB().GetForce(arg),
                    (arg) => new AppSettingMarkingParameterDB().GetQuality(arg)
                );

			}
		}

		static void SetAcceptReinject()
		{
			//StorageProvider.AcceptReinject = true;
		}

		static void CommonInit()
		{
			//StorageProvider.FieldStorage = new FieldStorageForiOS();
		}
    }
}
