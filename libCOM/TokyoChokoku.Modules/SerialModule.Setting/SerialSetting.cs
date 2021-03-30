using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.SerialModule.Counter;
using TokyoChokoku.SerialModule.Ast;
namespace TokyoChokoku.SerialModule.Setting
{
    public class SerialSetting
    {
        /// <summary>
        /// Mutable.
        /// </summary>
        public class Mutable: SerialSetting {
            public Mutable(SCSettingList settingList, SCCountStateList countStateList): base(settingList.ToMutable(), countStateList.ToMutable()) {}
            public Mutable(): this(new SCSettingList.Mutable(), new SCCountStateList.Mutable()) {}
        }

        /// <summary>
        /// Immutable.
        /// </summary>
        public class Immutable: SerialSetting {
            public Immutable(SCSettingList settingList, SCCountStateList countStateList): base(settingList.ToImmutable(), countStateList.ToImmutable()) { }
            public Immutable(): this(new SCSettingList.Immutable(), new SCCountStateList.Immutable()) { }
        }

        public SCSettingList    SettingList;
        public SCCountStateList CountStateList;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.SerialModule.Setting.SerialSetting"/> class.
        /// </summary>
        /// <param name="settingList">Setting list.</param>
        /// <param name="countStateList">Count state list.</param>
        public SerialSetting(SCSettingList settingList, SCCountStateList countStateList) {
            SettingList = settingList ?? throw new NullReferenceException();
            CountStateList = countStateList ?? throw new NullReferenceException();
        }

        /// <summary>
        /// Creates the node processor.
        /// </summary>
        /// <returns>The node processor.</returns>
        public SerialNodeProcessor CreateNodeProcessor() {
            return new SerialNodeProcessor(SettingList, CountStateList);
        }

        /// <summary>
        /// Mutable copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public SerialSetting.Mutable MutableCopy() => new Mutable(SettingList, CountStateList);

        /// <summary>
        /// Immutable copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public SerialSetting.Immutable ImmutableCopy() => new Immutable(SettingList, CountStateList);

        public void Log() {
            Console.WriteLine("Serial Settings");
            Console.WriteLine(SettingList);
            Console.WriteLine(CountStateList);
        }

        /// <summary>
        /// Sets the file number.
        /// </summary>
        /// <param name="fileNumber">File number.</param>
        public void SetFileNumber(int fileNumber)
        {
            int count = CountStateList.Count;
            for (int i = 1; i <= count; i++) {
                var e         = CountStateList[i];
                var data      = e.ToMutable();
                data.FileNo   = (ushort)fileNumber;
                CountStateList[i] = data.ToImmutable();
            }
        }
    }
}
