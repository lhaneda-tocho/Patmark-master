using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public static class MetaSerialStores
    {
        // serial

        public class SettingDefinition
        {
            public string Name { get; }
            public string Type { get; }

            public SettingDefinition(string name, string type)
            {
                this.Name = name;
                this.Type = type;
            }
        }

        public static List<SettingDefinition> SettingDefinitions
        {
            get
            {
                var res = new List<SettingDefinition>();
                res.Add(new SettingDefinition("Format", "short"));
                res.Add(new SettingDefinition("ClearingCondition", "short"));
                res.Add(new SettingDefinition("MaxValue", "int"));
                res.Add(new SettingDefinition("MinValue", "int"));
                res.Add(new SettingDefinition("RepeatingCount", "short"));
                res.Add(new SettingDefinition("ClearingTimeHH", "short"));
                res.Add(new SettingDefinition("ClearingTimeMM", "short"));
                return res;
            }
        }


        public class CounterDefinition
        {
            public string Name { get; }
            public string Type { get; }

            public CounterDefinition(string name, string type)
            {
                this.Name = name;
                this.Type = type;
            }
        }

        public static List<CounterDefinition> CounterDefinitions
        {
            get
            {
                var res = new List<CounterDefinition>();
                res.Add(new CounterDefinition("SerialNo", "short"));
                res.Add(new CounterDefinition("CurrentValue", "int"));
                return res;
            }
        }
    }
}

