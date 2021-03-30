using System;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.Settings
{



    /// <summary>
    /// ControllerとPreferenceのリポジトリを合わせたもの. このリポジトリは IMachineModelNoRepositoryを実装しません.
    /// </summary>
    public class CombinedMachineModelNoRepository
    {
        [Obsolete("Disabled_A001", error: true)]
        readonly IMachineModelNoRepository forController = new MachineModelNoRepositoryForController();
        readonly IMachineModelNoRepository forPreference = new MachineModelNoRepositoryOfKeyValueStore();


        /// <summary>
        /// コントローラから機種番号を取得し，それを返します. 
        /// </summary>
        [Obsolete("Disabled_A001", error: true)]
        public async Task<IUnstable<CommunicationResult<PatmarkMachineModel>>> RetrieveFromController()
        {
            try
            {
                var spec = await forController.Pull();
                if (ReferenceEquals(spec, null))
                {
                    var message = "Failure to Receive";
                    Console.Error.WriteLine(message);
                    return CommunicationResult
                        .Failure<PatmarkMachineModel>(message)
                        .ToReturnable();
                }
                return CommunicationResult
                    .Success<PatmarkMachineModel>(spec)
                    .ToReturnable();
            }
            catch (Exception ex)
            {
                var message = "Failure to Receive";
                Console.Error.WriteLine("Failure to Communication", ex);
                return CommunicationResult
                    .Failure<PatmarkMachineModel>(message)
                    .ToReturnable();
            }
        }

        /// <summary>
        /// コントローラ側のモデルが、引数に指定したモデルと一致する場合に <c>true</c> を返します。
        /// </summary>
        /// <param name="model">期待するモデル番号。<c>null</c> を指定した場合は プリファレンスから読み取った値を使用します。 (初期値 <c>null</c>)</param>
        /// <returns>
        /// 通信に成功し、モデルが一致する場合は CommunicationResult.Success(true)
        /// 通信に成功し、モデルが一致しない場合は CommunicationResult.Success(false)
        /// 通信に失敗した場合は CommunicationResult.Failure()
        /// </returns>
        [Obsolete("Disabled_A001 (with a dummy implementation)", error: false)]
        public async Task<IUnstable<CommunicationResult<bool>>> IsControllerModelMatchedWith(PatmarkMachineModel model = null)
        {
            // オリジナルの実装
            async Task<IUnstable<CommunicationResult<bool>>> Run()
            {
                // 引数の補完
                if (ReferenceEquals(model, null))
                    model = CurrentOnPreference();

                // 試しに受信する
                PatmarkMachineModel controllersSpec;
                try
                {
                    controllersSpec = await forController.Pull();
                    if (ReferenceEquals(controllersSpec, null))
                    {
                        var message = "Cause error on retrieving machine model.";
                        Console.Error.WriteLine(message);
                        return CommunicationResult.Failure<bool>(message).ToReturnable();
                    }
                }
                catch (Exception ex)
                {
                    var message = "Cause error on retrieving machine model.";
                    Console.Error.WriteLine(message, ex);
                    return CommunicationResult.Failure<bool>(message, ex).ToReturnable();
                }

                // 値が同じだった時は true
                if (model == controllersSpec)
                {
                    return CommunicationResult.Success<bool>(true).ToReturnable();
                }
                // そうでなければ false
                else
                {
                    return CommunicationResult.Success<bool>(false).ToReturnable();
                }
            }

            // ダミー実装
            Task<IUnstable<CommunicationResult<bool>>> Dummy()
            {
                var ans = CommunicationResult.Success<bool>(true).ToReturnable();
                return Task.FromResult(ans);
            }

            return await Dummy();
        }

        /// <summary>
        /// プリファレンスから現在の設定を読み込みます。設定が読み込めなかった場合は <c>PatmarkMachineModel.Default</c> が返されます。
        /// </summary>
        /// <returns>The machine model.</returns>
        public PatmarkMachineModel CurrentOnPreference()
        {
            PatmarkMachineModel current;
            try
            {
                current = forPreference.BlockingPull();
            }
            catch (Exception ex)
            {
                // 失敗したら一番最小の値を取得.
                Console.Error.WriteLine("Cause error on reading machine model from preference", ex);
                current = PatmarkMachineModel.Default;
            }
            return current;
        }

        /// <summary>
        /// プリファレンスに指定の機種番号を保存します.
        /// </summary>
        /// <returns>The hold.</returns>
        /// <param name="model">Model.</param>
        public bool HoldOnPreference(PatmarkMachineModel model)
        {
            return forPreference.BlockingPush(model);
        }
    }
}
