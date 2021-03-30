using System;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.MachineModel;
namespace TokyoChokoku.Patmark.Settings
{
    /// <summary>
    /// key value storeによるIMachineModelNoRepositoryの実装.
    /// このクラスのPullとPushは必ず成功するように組まれています.
    /// </summary>
    public class MachineModelNoRepositoryOfKeyValueStore: IMachineModelNoRepository
    {
        public static PatmarkMachineModel CurrentSpec {
            get {
                var store = new MachineModelNoRepositoryOfKeyValueStore();
                PatmarkMachineModel spec;
                try {
                    spec = store.PullDirect();
                } catch (Exception) {
                    spec = PatmarkMachineModel.Patmark1515;
                }
                return spec;
            }
        }

        public const  string Key = "DefaultMachineModelNo";
        public static readonly PatmarkMachineModel Default = PatmarkMachineModel.Patmark1515;
        public readonly IKeyValueStore kvs;

        /// <summary>
        /// IKeyValueStoreを指定して初期化します.
        /// </summary>
        /// <param name="kvs">Kvs.</param>
        public MachineModelNoRepositoryOfKeyValueStore(IKeyValueStore kvs) {
            this.kvs = kvs;
        }

        /// <summary>
        /// KeyValueStoreFactoryHolderからIKeyValueStoreを取得して初期化します.
        /// <see cref="T:TokyoChokoku.Patmark.Settings.MachineModelNoRepositoryWithKeyValuStore"/> class.
        /// </summary>
        public MachineModelNoRepositoryOfKeyValueStore() : this(KeyValueStoreFactoryHolder.Instance.Create()) {
            
        }
            
        /// <summary>
        /// MachineModelSpecインスタンスを取得します．
        /// </summary>
        /// <returns>The pull.</returns>
        public PatmarkMachineModel PullDirect()
        {
            var name = kvs.GetString(Key, Default.Name);
            var spec = PatmarkMachineModel.SpecList.Find((it) => it.Name==name);
            return spec;
        }

        /// <summary>
        /// MachineModelSpecインスタンスのデータを保存します.
        /// </summary>
        /// <returns>The push.</returns>
        /// <param name="spec">Spec.</param>
        public void PushDirect(PatmarkMachineModel spec)
        {
            var name = spec.Name;
            kvs.Set(Key, spec.Name);
            kvs.Commit();
        }

        public Task<PatmarkMachineModel> Pull()
        {
            return SynchronusTask(() =>
            {
                return PullDirect();
            });
        }

        public Task<bool> Push(PatmarkMachineModel spec)
        {
            return SynchronusTask(() =>
            {
                PushDirect(spec);
                return true;
            });
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
