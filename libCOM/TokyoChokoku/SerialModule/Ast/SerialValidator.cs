using System;
using TokyoChokoku.MarkinBox;
namespace TokyoChokoku.SerialModule.Ast
{
    public static class SerialValidator
    {
        public static bool VerifySerialNumber(int number) {
            return MBSerialNumber.Verify(number);
        }
    }
}
