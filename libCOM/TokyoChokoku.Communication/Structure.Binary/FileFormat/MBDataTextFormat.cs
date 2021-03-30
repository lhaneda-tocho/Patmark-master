using System;
using TokyoChokoku.Text;
namespace TokyoChokoku.Structure.Binary.FileFormat
{
    public enum MBDataTextFormat
    {
        Wide, Barcode
    }

    public static class MBDataTextFormatExt
    {
        /// <summary>
        /// Get text encode.
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="self">Self.</param>
        public static TextEncodings GetTextEncode(this MBDataTextFormat self) {
            switch (self)
			{
				case MBDataTextFormat.Wide:
                    return TextEncodings.Byte2;

                case MBDataTextFormat.Barcode:
                    return TextEncodings.Byte1;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int GetMaxCharCount(this MBDataTextFormat self) {
			switch (self)
			{
				case MBDataTextFormat.Wide:
					return 50;

				case MBDataTextFormat.Barcode:
					return 75;

				default:
					throw new ArgumentOutOfRangeException();
			}
        }
    }
}
