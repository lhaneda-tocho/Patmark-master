using System;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class PreferenceManager
    {
        private PreferenceManager (IPreferenceAccessor accessor)
        {
            this.Accessor = accessor;
        }
        public static PreferenceManager Instance;

        public IPreferenceAccessor Accessor {get;}

        public static void Init(IPreferenceAccessor accessor){
            Instance = new PreferenceManager (accessor);
        }
    }
}

