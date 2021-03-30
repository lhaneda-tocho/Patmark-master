using System;

using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public class CalendarSettingsYmdDataContainer
    {
        public string Title { get; private set; }
        public Store<char> ValueStore { get; private set; }

        public CalendarSettingsYmdDataContainer (string title, Store<char> valueStore)
        {
            this.Title = title;
            this.ValueStore = valueStore;
        }
    }
}

