using System;
namespace TokyoChokoku.MarkinBox
{
    public class MBSerialNumber
    {
        public static bool Verify(int number)
        {
            return 0 < number && number <= MBSerial.NumOfSerial;
        }
    }
}
