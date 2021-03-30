using System;
namespace TokyoChokoku.Patmark
{
    public interface IKeyValueStore
    {
        bool HasKey(string key);

        int    GetInt(string key, int defaultValue);
        float  GetFloat(string key, float defaultValue);
        string GetString(string key, string defaultValue);

        void Set(string key, int val);
        void Set(string key, float val);
        void Set(string key, string val);

        bool Commit();
    }
}
