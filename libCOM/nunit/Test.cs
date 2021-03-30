using NUnit.Framework;
using System;
using static BitConverter.EndianBitConverter;
using static NUnit.Framework.Assert;

namespace TokyoChokoku.Communication
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void LogSelfTest()
        {
            CommunicationLogger.SelfTest();
            True(CommunicationLogger.isConfigured());
        }

        [Test()]
        public void CheckEndian()
        {
            Console.Write("Native : ");
            Console.WriteLine(BinalizerUtil.NativeEndian());

            uint neko = 0x88006400;
            var big = BigEndian.GetBytes(neko);
			var lit = LittleEndian.GetBytes(neko);
			BinalizerUtil.DumpBytes(big);
			BinalizerUtil.DumpBytes(lit);

            True("88-00-64-00" == BinalizerUtil.BytesToString(big));
            True("00-64-00-88" == BinalizerUtil.BytesToString(lit));
        }

    }
}
