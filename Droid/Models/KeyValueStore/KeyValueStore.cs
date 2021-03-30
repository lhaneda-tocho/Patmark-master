using System;
using Android.Content;

namespace TokyoChokoku.Patmark.Droid
{
    public class KeyValueStore : IKeyValueStore
    {
        ISharedPreferences Prefs;
        ISharedPreferencesEditor Editor;

        public KeyValueStore(Context ctx){
            Prefs = ctx.GetSharedPreferences("Patmark", FileCreationMode.Private);
            Editor = Prefs.Edit();
        }

        public bool HasKey(string key)
        {
            return Prefs.Contains(key);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return Prefs.GetFloat(key, defaultValue);
        }

        public int GetInt(string key, int defaultValue)
        {
            return Prefs.GetInt(key, defaultValue);
        }

        public string GetString(string key, string defaultValue)
        {
            return Prefs.GetString(key, defaultValue);
        }

        public void Set(string key, int val)
        {
            Editor.PutInt(key, val);
        }

        public void Set(string key, float val)
        {
            Editor.PutFloat(key, val);
        }

        public void Set(string key, string val)
        {
            Editor.PutString(key, val);
        }

        public bool Commit()
        {
            return Editor.Commit();
        }

        public class Factory : IKeyValueStoreFactory
        {
            Context Ctx;

            public Factory(Context ctx)
            {
                Ctx = ctx;
            }

            public IKeyValueStore Create()
            {
                return new KeyValueStore(Ctx);
            }
        }
    }
}
