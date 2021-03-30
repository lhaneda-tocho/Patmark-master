using System;

using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public interface IPreferenceAccessor
    {
        void SetString(string key, string value);
        string GetString(string key);
    }
}

