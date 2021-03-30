using Foundation;

namespace TokyoChokoku.Patmark.iOS
{
    public class KeyValueStore : IKeyValueStore
    {
        NSUserDefaults Prefs = NSUserDefaults.StandardUserDefaults;

        public bool HasKey(string key)
        {
            return (Prefs[key] != null);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return HasKey(key) ? Prefs.FloatForKey(key) : defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            return HasKey(key) ? (int)Prefs.IntForKey(key) : defaultValue;
        }

        public string GetString(string key, string defaultValue)
        {
            return HasKey(key) ? Prefs.StringForKey(key) : defaultValue;
        }

        public void Set(string key, int val)
        {
            Prefs.SetInt(val, key);
        }

        public void Set(string key, float val)
        {
            Prefs.SetFloat(val, key);
        }

        public void Set(string key, string val)
        {
            Prefs.SetString(val, key);
        }

        public bool Commit()
        {
            return Prefs.Synchronize();
        }

        public class Factory : IKeyValueStoreFactory
        {
            public IKeyValueStore Create()
            {
                return new KeyValueStore();
            }
        }
    }
}
