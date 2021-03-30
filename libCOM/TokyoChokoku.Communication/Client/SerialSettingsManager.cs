using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    [Obsolete("互換性のためダミーとして実装されています. 将来このクラスは使用不能になります.", true)]
    public class SerialSettingsManager
	{
        CommunicationClient Client;
		CommandExecutor Commands;

		internal SerialSettingsManager(CommunicationClient client, CommandExecutor commands)
		{
            Client = client;
			Commands = commands;
			Init();
		}

		//public readonly List<MBSerialData> Settings = new List<MBSerialData>();
		//public readonly List<MBSerialCounterData> Counters = new List<MBSerialCounterData>();

		public delegate void OnReloadedDelegate();
		public OnReloadedDelegate OnReloaded;

		void Init()
		{
		}

        [Obsolete("互換性のためダミーとして実装されています. 将来このメソッドは使用不能になります.", true)]
		public async Task<bool> Reload(int? fileNumber)
		{
			Init();
			
			// リロードが完了したことをコールバックします。
			if (OnReloaded != null)
				OnReloaded();
			else
				throw new NullReferenceException("OnReloaded must be set callback.");

			return true;
		}

        [Obsolete("互換性のためダミーとして実装されています. 将来このメソッドは使用不能になります.", true)]
		public async Task<bool> Save(int? fileNumber)
		{
			return true;
		}

        void Error(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
