using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook;

using TokyoChokoku.Communication;
//using TokyoChokoku.SerialModule.Setting;

namespace TokyoChokoku.ControllerIO
{
    public class EmbossmentIO
    {
        public CommunicationClient Client { get; internal set; }
        public CommandExecutor     Exec   { get; }
        public FileIO              FileIO { get; internal set; } // Required

        internal EmbossmentIO(CommandExecutor exec)
        {
            Exec = exec ?? throw new NullReferenceException();
        }

        public static EmbossmentIO Create(CommunicationClient client)
        {
            return FileIO.Create(client).EmbossmentIO;
        }

        /// <summary>
        /// Setups the permanent marking.
        /// </summary>
        /// <returns>The permanent marking.</returns>
        /// <param name="fields">Fields.</param>
        public async Task<bool> SetupPermanentMarking(List<MBData> fields)
        {
            //var list = await FileIO.GetCachedOrLoadFileInfo();
            return await FileIO.SavePermanentFile(fields);
        }


        /// <summary>
        /// Clears the permanent marking.
        /// </summary>
        /// <returns>The permanent marking.</returns>
        public async Task<bool> ClearPermanentMarking()
        {
            //var list = await FileIO.GetCachedOrLoadFileInfo();
            return await FileIO.ClearPermanentFile();
        }
    }
}
