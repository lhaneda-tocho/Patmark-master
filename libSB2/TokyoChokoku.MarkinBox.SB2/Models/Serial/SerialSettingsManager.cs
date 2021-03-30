using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;

using TokyoChokoku.MarkinBox.Sketchbook.Serial;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class SerialSettingsManager
    {
        private SerialSettingsManager ()
        {
            Init();
        }

        static Regex RegSerial { get; } = new Regex(@"@S\[(\d{1,4})-(\d)\]");

        public static SerialSettingsManager Instance { get; } = new SerialSettingsManager();

        public readonly List<MBSerialData> Settings = new List<MBSerialData>();
        public readonly List<MBSerialCounterData> Counters = new List<MBSerialCounterData>();

        public delegate void OnReloadedDelegate();
        public OnReloadedDelegate OnReloaded;

        private void Init()
        {
            // 設定値初期化
            Settings.Clear();
            for (var i = 0; i < Serial.Consts.NumOfSerial; i++)
            {
                Settings.Add(new MBSerialData());
            }
            // 現在値初期化
            Counters.Clear();
            foreach (var counter in
                Enumerable.Range(1, Serial.Consts.NumOfSerial)
                    .Select((i) =>
                    {
                        return new MBSerialCounterData();
                    })
                    .OrderBy((counter) =>
                    {
                        return counter.SerialNo;
                    })
            )
            {
                Counters.Add(counter);
            }
        }

        public async Task<bool> Reload(int? fileNumber){
            Init();
            if (CommunicationClientManager.Instance.IsOnline())
            {
                if (fileNumber != null)
                {
                    // ファイルに対応した設定値をSDカードからコントローラへロード

                    if (!(await CommandExecuter.SetSerialSettingsFileNo((short)fileNumber)).IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Reload]失敗 - SetSerialSettingsFileNo");
                        return false;
                    }

                    if (!(await CommandExecuter.LoadSerialSettingsOfFileFromSdCard()).IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Reload]失敗 - LoadSerialSettingsOfFileFromSdCard");
                        return false;
                    }
                }

                {
                    var tmpSettings = await CommandExecuter.ReadSerialSettings();
                    if (!tmpSettings.IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Reload]失敗 - ReadSerialSettings");
                        return false;
                    }
                    Settings.Clear();
                    foreach (var setting in tmpSettings.Value)
                    {
                        Settings.Add(setting);
                    }
                }
                {
                    var tmpCounters = await CommandExecuter.ReadSerialCounters();
                    if (!tmpCounters.IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Reload]失敗 - ReadSerialCounters");
                        return false;
                    }
                    Counters.Clear();
                    foreach (var counters in tmpCounters.Value)
                    {
                        Counters.Add(counters);
                    }
                }
            }

            // リロードが完了したことをコールバックします。
            if (OnReloaded != null)
                OnReloaded ();
            else
                throw new NullReferenceException ("OnReloaded must be set callback.");

            return true;
        }

        public async Task<bool> Save(int? fileNumber){
            if (CommunicationClientManager.Instance.IsOnline ()) 
            {
                if (!(await CommandExecuter.SetSerialSettings(new MBSerialSettingsDataBinarizer(Settings))).IsOk)
                {
                    Log.Error("[SerialSettingsManager - Save]失敗 - SetSerialSettings");
                    return false;
                }
                if (!(await CommandExecuter.SetSerialCounters(new MBSerialCountersDataBinarizer(Counters))).IsOk)
                {
                    Log.Error("[SerialSettingsManager - Save]失敗 - SetSerialCounters");
                    return false;
                }

                if (fileNumber != null)
                {
                    // ファイルに対応した設定値をコントローラからSDカードへ保存

                    if (!(await CommandExecuter.SetSerialSettingsFileNo((short)fileNumber)).IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Save]失敗 - SetSerialSettingsFileNo");
                        return false;
                    }

                    if (!(await CommandExecuter.SaveSerialSettingsOfFileToSdCard()).IsOk)
                    {
                        Log.Error("[SerialSettingsManager - Save]失敗 - SaveSerialSettingsOfFileToSdCard");
                        return true;
                    }
                }
            }
            return true;
        }

        public List<SerialStores> CreateSerialStores()
        {
            var res = new List<SerialStores>();
            for (var i = 0; i < Settings.Count; i++)
            {
                res.Add(new SerialStores(Settings[i], Counters[i]));
            }
            return res;
        }

        public List<MBData> UpdateFieldText(List<MBData> source)
        {
            var ans = source.Select(it =>
            {
                var txt = UpdateFieldText(it.Text);
                var m = it.ToMutable();
                m.Text = txt;
                return new MBData(m);
            });
            return ans.ToList();
        }

        public string UpdateFieldText(string text)
        {
            int Digits(int v)
            {
                var i = 0;
                int r = v;
                while(r != 0)
                {
                    ++i;
                    r /= 10;
                }
                return Math.Max(1, i); 
            }

            int CountOrDefault(int index, int def)
            {
                var cs = Counters;
                if (0 <= index && index < cs.Count)
                {
                    return cs[index].CurrentValue;
                }
                else
                {
                    return def;
                }
            }

            string CountToString(int count, int id)
            {
                id = id - 1;
                count = CountOrDefault(id, count);
                var settings = Settings;
                if(0 <= id && id < settings.Count)
                {
                    var s = settings[id];
                    var format = s.Format;
                    var mx     = s.MaxValue;
                    var digits = Digits(mx);
                    if (format == Consts.SERIAL_FORMAT_ZERO)
                    {
                        return count.ToString($"D{digits}");
                    }
                    else
                    {
                        return count.ToString();
                    }
                } else
                {
                    return count.ToString();
                }
            }

            string IDToString(int id)
            {
                return id.ToString();
            }

            var sb = new StringBuilder();
            var rem = text;
            
            while(true)
            {
                var m = RegSerial.Match(rem);
                if(!m.Success)
                {
                    sb.Append(rem);
                    rem = "";
                    break;
                }
                var g = m.Groups;
                var i = m.Index;
                var l = m.Length;
                var head = rem.Substring(0, i);
                var count = int.Parse(g[1].Value);
                var id    = int.Parse(g[2].Value);
                rem = rem.Substring(i + l);

                sb.Append(head);
                sb.Append($"@S[{CountToString(count, id)}-{IDToString(id)}]");
                
            }
            return sb.ToString();
        }
    }
}

