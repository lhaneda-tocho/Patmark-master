using System;
namespace TokyoChokoku.Communication
{
    public struct RomVersion
    {
        public byte   Major;
        public byte   Middle;
        public byte   Minor;
        public UInt16 Revision; 

        public static RomVersion Init(byte major, byte middle, byte minor, UInt16 revision)
        {
            var v = new RomVersion();
            v.Major    = major;
            v.Middle   = middle;
            v.Minor    = minor;
            v.Revision = revision;
            return v;
        }

        /// <summary>
        /// 以下の形式のデータから RomVersionを初期化します.こちらは表示用です。
        /// Version  D0[10] 0x3305 : Ver 3.3.05 (BCD)
        ///                 0x3415 : Ver 3.4.15 (BCD)
        /// Revision D0[9]  0ｘ04 : Rev4
        ///                 0ｘ0A : Rev10
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="versionSet">Version set.</param>
        /// <param name="revision">Revision.</param>
        public static RomVersion Deformat(UInt16 versionSet, UInt16 revision)
        {
            byte major = (byte)((versionSet >> 12) & 0xF);
            byte middle = (byte)((versionSet >> 8) & 0xF);
            // フューチャー様からの下記依頼に対応します。
            // 現在はD0[10] = 0x4A00　→　Ver.4.10.00　に変換して頂いていると思いますが
            // D0[10] = 0x4A00　→　Ver.4.03.00、　0x4B00　→　Ver.4.04.00、　0x4C00　→　Ver.4.05.00　の様に変換して頂きたい
            if (middle >= (byte)10)
            {
                middle -= (byte)7;
            }

            byte minor = (byte)(versionSet & 0xFF);
            return Init(major, middle, minor, revision);
        }

        /// <summary>
        /// 以下の形式のデータから RomVersionを初期化します.こちらは機種判定用です。
        /// Version  D0[10] 0x3305 : Ver 3.3.05 (BCD)
        ///                 0x3415 : Ver 3.4.15 (BCD)
        /// Revision D0[9]  0ｘ04 : Rev4
        ///                 0ｘ0A : Rev10
        /// </summary>
        /// <returns>The from.</returns>
        /// <param name="versionSet">Version set.</param>
        /// <param name="revision">Revision.</param>
        public static RomVersion DeformatForMachineModel(UInt16 versionSet, UInt16 revision)
        {
            byte major = (byte)((versionSet >> 12) & 0xF);
            byte middle = (byte)((versionSet >> 8) & 0xF);
            byte minor = (byte)(versionSet & 0xFF);
            return Init(major, middle, minor, revision);
        }
    }
}
