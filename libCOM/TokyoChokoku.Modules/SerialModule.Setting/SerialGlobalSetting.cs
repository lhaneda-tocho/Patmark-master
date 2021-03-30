using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox;

using TokyoChokoku.SerialModule.Counter;
using TokyoChokoku.SerialModule.Ast;
namespace TokyoChokoku.SerialModule.Setting
{
    public static class SerialGlobalSetting
    {
        static readonly object TheLock = new object();
        volatile static SerialSetting Data = new SerialSetting.Mutable();


        public static SerialNodeProcessor CreateNodeProcessor()
        {
            lock (TheLock)
            {
                return Data.CreateNodeProcessor();
            }
        }

        static void NewReference(SerialSetting reference)
        {
            lock (TheLock)
            {
                Data = reference;
            }
        }

        public static void SetSetting(SerialSetting setting) {
            NewReference(setting.MutableCopy());
        }

        /// <summary>
        /// Copy to mutable object.
        /// </summary>
        /// <returns>The copy.</returns>
        public static SerialSetting.Mutable MutableCopy() {
            lock (TheLock) { return Data.MutableCopy(); }
        }

        /// <summary>
        /// Copy to immutable object.
        /// </summary>
        /// <returns>The copy.</returns>
        public static SerialSetting.Immutable ImmutableCopy() {
            lock (TheLock) { return Data.ImmutableCopy(); }
        }

        public static void Log() 
        {
            Data.Log();
        }

        /// <summary>
        /// Retrieve the setting from controller.
        /// </summary>
        /// <returns>The from controller.</returns>
        /// <param name="fileNumber">File number.</param>
        public static async Task<bool> RetrieveFromController(int? fileNumber = null)
        {
            var res = await SerialSettingIO.RetrieveFromController(fileNumber);
            if (res == null)
                return false;
            NewReference(res);
            return true;
        }

        /// <summary>
        /// Sends to controller.
        /// </summary>
        /// <returns>The to controller.</returns>
        /// <param name="fileNumber">File number.</param>
        public static async Task<bool> SendToController(int? fileNumber = null)
        {
            if(fileNumber != null)
                SetFileNumber((int)fileNumber);
            return await SerialSettingIO.SendToController(MutableCopy(), fileNumber);
        }

        /// <summary>
        /// Sets the file no.
        /// </summary>
        /// <param name="fileNumber">File number.</param>
        public static void SetFileNumber(int fileNumber) {
            lock(TheLock) {
                Data.SetFileNumber(fileNumber);
            }
        }
    }
}
