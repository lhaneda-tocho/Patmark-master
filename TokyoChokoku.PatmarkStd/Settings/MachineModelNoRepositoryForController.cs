using System;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.Settings
{
    [Obsolete("Disabled_A001", error: true)]
    public class MachineModelNoRepositoryForController: IMachineModelNoRepository
    {
        private CommunicationClient TheClient { get; set; }

        public MachineModelNoRepositoryForController(CommunicationClient client)
        {
            TheClient = client;
        }

        public MachineModelNoRepositoryForController(): this(CommunicationClient.Instance)
        {
            
        }

        [Obsolete("Disabled_A001", error: true)]
        public async Task<PatmarkMachineModel> Pull()
        {
            var maybeV = (await TheClient.RetrieveRomVersionForMachineModel());
            if (maybeV == null)
                return null;
            var romversion = (RomVersion)maybeV;
            var version = PatmarkVersion.Of(romversion.Major, romversion.Middle, romversion.Minor, romversion.Revision);
            return PatmarkMachineModel.FromVersion(version);
        }

        public Task<bool> Push(PatmarkMachineModel spec)
        {
            throw new NotSupportedException("Patmark の機種設定を書き換えることはできません");
        }

        private Task<T> SynchronusTask<T>(Func<T> func)
        {

            var tcs = new TaskCompletionSource<T>();
            try
            {
                tcs.SetResult(func());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}
