using System;
using Foundation;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class PreferenceAccessor : IPreferenceAccessor
    {
        public PreferenceAccessor ()
        {
        }

        public void SetString(string key, string value){
            var prefs = NSUserDefaults.StandardUserDefaults;
            prefs.SetString(value, key);
            prefs.Synchronize();
        }


        public string GetString(string key){
            return NSUserDefaults.StandardUserDefaults.StringForKey (key);
        }
    }
}

