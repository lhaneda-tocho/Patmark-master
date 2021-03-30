using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// Big endian bytes.
    /// </summary>
	public class BigEndianBytes : List<byte>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.MarkinBox.Sketchbook.BigEndianBytes"/> class.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
		public BigEndianBytes(IEnumerable<byte> bytes)
		{
			if(bytes != null){
				this.AddRange (bytes);
			}
		}

        /// <summary>
        /// Tos the system endian.
        /// </summary>
        /// <returns>The system endian.</returns>
		public IEnumerable<byte> ToSystemEndian(){
			if (BitConverter.IsLittleEndian)
				return this.Reverse<byte>();
			else
				return this;
		}
	}
}

