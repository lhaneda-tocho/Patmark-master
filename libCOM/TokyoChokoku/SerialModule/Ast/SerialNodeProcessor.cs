using System;

using TokyoChokoku.SerialModule.Counter;
namespace TokyoChokoku.SerialModule.Ast
{
    public class SerialNodeProcessor
    {
        public SCSettingList    SettingList { get; }
        public SCCountStateList List        { get; }

        public SerialNodeProcessor(SCSettingList settingList, SCCountStateList list)
        {
            SettingList = settingList.ToImmutable();
            List        = list.ToImmutable();
        }

        /// <summary>
        /// Convert the specified node to string.
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="node">Node.</param>
        public string Convert(SerialNode node)
        {
            var def = node.CurrentValue;
            var serial = node.SerialNumber;
            var value  = GetCurrentValueOrDefault(serial, def);
            var format = SupplyFormat(serial) ?? SCFormat.FillZero;
            var range  = SupplyRange(serial) ?? SCCountRange.Init(0, 9999);

            switch (format)
            {
                case SCFormat.FillZero:
                    {
                        int width = range.MaxValue.ToString().Length;
                        int count = value;
                        return count.ToString().PadLeft(width, '0');
                    }
                case SCFormat.RightJustify:
                    {
                        int width = range.MaxValue.ToString().Length;
                        int count = value;
                        return count.ToString().PadLeft(width, ' ');
                    }
                case SCFormat.LeftJustify:
                    {
                        int count = value;
                        return count.ToString();
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }

        }

        /// <summary>
        /// Gets the current value from  serial number.
        /// return "def" if cannot retreive a current serial value or the serial number out of range.
        /// </summary>
        /// <returns>The current value or default.</returns>
        /// <param name="serial">Serial.</param>
        /// <param name="def">Def.</param>
        public int GetCurrentValueOrDefault(int serial, int def)
        {
            var maybeValue = SupplyValue(serial);
            Console.Write("SerialValue: "); Console.WriteLine(maybeValue);
            return maybeValue ?? def;
        }


        public int? SupplyValue(int serialNo) {
            if (MarkinBox.MBSerialNumber.Verify(serialNo))
                return (int?)List[serialNo].CurrentValue;
            return null;
        }

        public SCFormat? SupplyFormat(int serialNo) {
            if (MarkinBox.MBSerialNumber.Verify(serialNo))
                return (SCFormat?)SettingList[serialNo].Format;
            return null;
        }

        public SCCountRange? SupplyRange(int serialNo) {
            if (MarkinBox.MBSerialNumber.Verify(serialNo))
                return (SCCountRange?)SettingList[serialNo].Range;
            return null;
        }
    }
}
