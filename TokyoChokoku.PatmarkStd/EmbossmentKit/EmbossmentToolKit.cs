using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.Communication;
using TokyoChokoku.ControllerIO;
using TokyoChokoku.Patmark.Settings;
using TokyoChokoku.Patmark.Common;
using TokyoChokoku.Patmark.MachineModel;
using TokyoChokoku.Patmark.RuleKit;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// Patmark 打刻機 API
    /// </summary>
    public class EmbossmentToolKit
    {

        /// <summary>
        /// EmbossmentToolKit が提供する IValidationResourceProvider の実装です。
        /// </summary>
        private class ValidationResourceProviderImpl : IValidationResourceProvider
        {
            EmbossmentToolKit Parent { get; }

            public CommunicationClient CommunicationClient
                => Parent.TheClient;

            public CommandExecutor CommandExecutor
                => Parent.Exec;

            public FileIO FileIO
                => Parent.TheFileIO;

            public StatusIO StatusIO
                => Parent.TheStatusIO;

            public CombinedMachineModelNoRepository MachineModelNoRepository
                => Parent.MachineModelNoRepository;

            public PatmarkMachineModel CurrentMachineModel
                => MachineModelNoRepository.CurrentOnPreference();


            public Action CallbackOnEmpty { get; }
            public Action<TextError> CallbackOnTextError { get; }
            public Action CallbackOnOffline { get; }
            public Action CallbackOnMismatchMachineModel { get; }
            public Action<PatmarkMachineModel> CallbackOnTextSizeTooLarge { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="parent">EmbossmentToolKit クラス</param>
            /// <param name="callbackOnEmpty">コールバック</param>
            /// <param name="callbackOnTextError">コールバック</param>
            /// <param name="callbackOnOffline">コールバック</param>
            /// <param name="callbackOnMismatchMachineModel">コールバック</param>
            /// <param name="callbackOnTextSizeTooLarge">コールバック</param>
            public ValidationResourceProviderImpl(
                EmbossmentToolKit parent,
                Action callbackOnEmpty,
                Action<TextError> callbackOnTextError,
                Action callbackOnOffline,
                Action callbackOnMismatchMachineModel,
                Action<PatmarkMachineModel> callbackOnTextSizeTooLarge
            )
            {
                Parent = parent ?? throw new ArgumentNullException(nameof(parent));
                CallbackOnEmpty = callbackOnEmpty ?? throw new ArgumentNullException(nameof(callbackOnEmpty));
                CallbackOnTextError = callbackOnTextError ?? throw new ArgumentNullException(nameof(callbackOnTextError));
                CallbackOnOffline = callbackOnOffline ?? throw new ArgumentNullException(nameof(callbackOnOffline));
                CallbackOnMismatchMachineModel = callbackOnMismatchMachineModel ?? throw new ArgumentNullException(nameof(callbackOnMismatchMachineModel));
                CallbackOnTextSizeTooLarge = callbackOnTextSizeTooLarge ?? throw new ArgumentNullException(nameof(callbackOnTextSizeTooLarge));
            }

            /// <inheritdoc/>
            public async Task<IUnstable<CommunicationResult<bool>>> CurrentMachineModelIsEqualsToRemote()
            {
                return await MachineModelNoRepository.IsControllerModelMatchedWith();
            }
        }

        #region Global

        static readonly object TheLock = new object();

        static EmbossmentToolKit instance { get; set; } = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EmbossmentToolKit Instance { 
            get {
                lock (TheLock)
                {
                    if (instance == null)
                        throw new InvalidOperationException("The Global Tool Kit is not initialized");
                    return instance;
                }
            }
            private set {
                lock (TheLock)
                {
                    instance = value;
                }
            }
        }


        /// <summary>
        /// This method removes the globally accessible API instance.
        /// 
        /// Thread Safety : Anti Thread
        /// 
        /// 同時にこのメソッドが呼ばれた場合，Instanceプロパティの状態は不定になります. \n
        /// 初期化の順番とInstanceプロパティへの反映の順番は保証されないためです。
        /// 
        /// </summary>
        public static void DeleteGlobal() {
            // Instance = null;
        }


        /// <summary>
        /// This method creates a globally accessible API instance.
        /// 
        /// Thread Safety : Conditional Thread Safe
        /// 
        /// 複数のスレッドから同時にこのメソッドが呼ばれた場合、下記のように振る舞います。
        ///
        /// 
        /// a. 初期化前に呼び出された場合は 1度だけ初期化されます. 返り値は初期化結果です.(どのスレッドが初期化するかは保証されない) \n
        /// b. すでに初期化済みの場合は何もせず，初期化済の値が返されます
        /// 
        /// ただし、DeleteGlobal と同時に呼ばれた場合は Instanceプロパティの状態が不定になります.
        /// 
        /// </summary>
        /// <param name="repository">
        /// 機種設定リポジトリです。打刻データ送信時にここに指定したリポジトリを使用して機種設定を送信します。
        /// <c>null</c> を指定した場合はこのメソッド内で生成します。
        /// </param>
        /// <returns>初期化済みの API インスタンス</returns>
        /// <exception cref="CommunicationClient.NotConnectedException">when the Communication client not initialized.</exception>
        public static EmbossmentToolKit InitGlobalIfNeeded(CombinedMachineModelNoRepository repository = null)
        {
            lock(TheLock)
            {
                var client = CommunicationClient.Instance;
                if (client == null)
                    throw new CommunicationClient.NotConnectedException();
                if(instance == null)
                {
                    instance = new EmbossmentToolKit(client, repository ?? new CombinedMachineModelNoRepository());
                }

                return Instance;
            }
        }


        //==================================//
        #endregion

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public CommunicationClient TheClient   { get; }
        public CommandExecutor     Exec        { get; }
        public FileIO              TheFileIO   { get; }
        public StatusIO            TheStatusIO { get; }
        public CombinedMachineModelNoRepository MachineModelNoRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.PatmarkStd.EmbossmentKit.EmbossmentToolKit"/> class.
        /// </summary>
        /// <param name="client">Client.</param>
        public EmbossmentToolKit(CommunicationClient client, CombinedMachineModelNoRepository machineModelNoRepository)
        {
            MachineModelNoRepository = machineModelNoRepository ?? throw new ArgumentNullException(nameof(machineModelNoRepository));
            TheClient = client ?? throw new ArgumentNullException(nameof(client));
            Exec = client.CommandExecutor;
            TheFileIO = FileIO.Create(client);
            TheStatusIO = TheFileIO.StatusIO;
        }




        #region private services 

        static IList<HorizontalText> ConvertForPatmark(EmbossmentData data, PatmarkMachineModel machineModel)
        {
            var param = ApplyEmbossmentData(data, machineModel, null);
            return new List<HorizontalText>(1).Also((it) =>
            {
                it.Add(HorizontalText.Constant.Create(param));
            });
        }

        #endregion

        /// <summary>
        /// EmbossmentData オブジェクトと、MBData の初期値を指定して MBData に変換します。
        /// </summary>
        /// <param name="data">変換したい EmbossmentData オブジェクト。</param>
        /// <param name="machineModel">機種設定を指定します。ファイル保存向けにデータを生成する場合は null を指定してください。</param>
        /// <param name="src">MBData の初期値。null を指定することも可能.</param>
        /// <returns>初期値 <c>src</c> に EmbossmentData の内容を書き込んだ新しい MBData</returns>
        public static MBData ApplyEmbossmentData(EmbossmentData data, PatmarkMachineModel machineModel, MBData src)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            var param = src != null ? MutableHorizontalTextParameter.Create(src) : MutableHorizontalTextParameter.Create();

            var pdb = data.MarkingParameterDB;
            var pmtextsize = pdb.GetTextSize(data.Mode.TextSize);
            var pmforce    = pdb.GetForce(data.Mode.Force);
            var pmquality  = pdb.GetQuality(data.Mode.Quality);

            var ts        = pmtextsize.ToEmbossmentTextSize();
            var power     = pmforce.ToBinary();
            var quality   = pmquality.ToBinary();


            machineModel = (machineModel ?? Instance.MachineModelNoRepository.CurrentOnPreference()) ?? PatmarkMachineModel.Default;
            var machineModelHeight = machineModel.Height;

            param.BasePoint = (byte)Structure.BasePoint.LB;
            param.X = 0;
            param.Y = machineModelHeight/2.0m + (decimal)(ts.Heightmm * 0.5);

            param.Height = (decimal)ts.Heightmm;
            param.Aspect = (decimal)ts.Aspect * 100m;
            param.Pitch = (decimal)ts.Stridemm;
            param.Power = power;
            param.Speed = quality;

            param.Text = data.Text.Text;

            var sendable = param.ToSerializable();


            Log.Info("====== 打刻データ情報 ==================");
            Log.Info($"PMFORCE   = {pmforce  } --> {power  } --> Binary.Power = {sendable.Power}");
            Log.Info($"PMQUALITY = {pmquality} --> {quality} --> Binary.Speed = {sendable.Speed}");
            Log.Info($"=====================================");

            return sendable;
        }

        /// <summary>
        /// 打刻ファイルを転送します。 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        async Task<IUnstable<MarkingResult>> SendMarkingData(IList<MBData> data, Action onSendFailure)
        {
            // ファイル送信
            var success = await TheFileIO.EmbossmentIO.SetupPermanentMarking(data.ToList());
            if (!success)
            {
                onSendFailure?.Invoke();
                return MarkingResult.Failure("Send failure").ToReturnable();
            }
            return MarkingResult.Success().ToReturnable();
        }

        /// <summary>
        /// クイック打刻を開始します
        /// </summary>
        /// <returns>The marking.</returns>
        /// <param name="data">Data.</param>
        public async Task<IUnstable<MarkingResult>> StartMarking(
            EmbossmentData data,
            Action                      onEmpty,
            Action<TextError>           onTextError,
            Action                      onOffline,
            Action                      onMismatchMachineModel,
            Action                      onSendFailure,
            Action<PatmarkMachineModel> onTextSizeTooLarge
        )
        {
            // リソースプロバイダを作成・設定
            var resourceProvider = new ValidationResourceProviderImpl(
                parent: this,
                callbackOnEmpty:                onEmpty,
                callbackOnOffline:              onOffline,
                callbackOnMismatchMachineModel: onMismatchMachineModel,
                callbackOnTextError:            onTextError,
                callbackOnTextSizeTooLarge:     onTextSizeTooLarge
            );

            // 適用するルールを指定
            var rules = new List<SendRule>
            {
                SendRule.OnlineRule,
                SendRule.TextSizeRule,
                SendRule.CharactorRule,
                SendRule.NotEmptyRule,
                SendRule.MachineModelRule,
            };

            // 検証対象となるデータをインスタンス化
            var target = new QuickModeData(
                data,
                serialized: () => ToSerializable(data, MachineModelNoRepository.CurrentOnPreference(), pause: true)
            );

            try
            {
                // バリデーションを行う
                foreach (var rule in rules)
                {
                    var validator = rule.CreateValidator(resourceProvider);
                    var r = await validator.ValidateQuickMode(target);
                    if(r.IsFailure)
                    {
                        Console.WriteLine($"[EmbossmentToolKit>QuickMode>{rule}]: {r.Description.Message}");
                        return r;
                    }
                }

                // バリデーションが済んだら, 送信を実行する
                var s = target.Serialized;
                var result = await SendMarkingData(s, onSendFailure);
                if (result.IsFailure)
                {
                    var message = result.ErrorDescription.Message;
                    Console.WriteLine($"[EmbossmentToolKit>QuickMode>Send]: {message}");
                }

                return result;
            }
            catch(System.IO.IOException ex) {
                Console.WriteLine(ex);
                throw;
            } catch (Exception ex) {
                var nex = new System.IO.IOException("Error in StartMarking", ex);
                Console.WriteLine(nex);
                throw nex;
            }
        }


        /// <summary>
        /// PCファイルの打刻を開始します
        /// </summary>
        /// <returns>The marking.</returns>
        /// <param name="data">Data.</param>
        public async Task<IUnstable<MarkingResult>> StartMarking(
            List<MBData> data,
            Action onEmpty,
            Action onOffline,
            Action onMismatchMachineModel,
            Action onSendFailure
        )
        {
            // リソースプロバイダを作成・設定
            var resourceProvider = new ValidationResourceProviderImpl(
                parent: this,
                callbackOnEmpty:                onEmpty,
                callbackOnOffline:              onOffline,
                callbackOnMismatchMachineModel: onMismatchMachineModel,
                callbackOnTextError:            (_)=> { /* 何もしない */ },
                callbackOnTextSizeTooLarge:     (_)=> { /* 何もしない */ }
            );

            // 検証対象となるデータをインスタンス化
            var target = new PCFileModeData(
                data,
                strict: () => StrictMBDataList(data)
            );

            // 適用するルールを指定
            var rules = new List<SendRule>
            {
                SendRule.OnlineRule,
                //SendRule.TextSizeRule,
                //SendRule.CharactorRule,
                SendRule.NotEmptyRule,
                SendRule.MachineModelRule,
            };

            try
            {
                // バリデーションを行う
                foreach (var rule in rules)
                {
                    var validator = rule.CreateValidator(resourceProvider);
                    var r = await validator.ValidatePCFileMode(target);
                    if (r.IsFailure)
                    {
                        Console.WriteLine($"[EmbossmentToolKit>PCFileMode>{rule}]: {r.Description.Message}");
                        return r;
                    }
                }

                // バリデーションが済んだら, 送信を実行する
                var result = await SendMarkingData(target.StrictData, onSendFailure);
                if (result.IsFailure)
                {
                    var message = result.ErrorDescription.Message;
                    Console.WriteLine($"[EmbossmentToolKit>PCFileMode>Send]: {message}");
                }

                return result;
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                var nex = new System.IO.IOException("Error in StartMarking", ex);
                Console.WriteLine(nex);
                throw nex;
            }
        }

        /// <summary>
        /// EmbossmentData を保存可能な形式に変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="machineModel">機種設定を指定します。
        /// <param name="pause"></param>
        /// <returns></returns>
        public static List<MBData> ToSerializable(EmbossmentData data, PatmarkMachineModel machineModel, bool pause = false)
        {
            var fields = ConvertForPatmark(data, machineModel);
            var mbdata = from field in fields
                         select field.ToSerializable();
            return mbdata.ToList();
        }

        /// <summary>
        /// 引数の打刻データを厳格なフォーマットに変換します。
        /// </summary>
        /// <param name="data">打刻データ/param>
        /// <returns>Strict フォーマットに準拠した打刻データ</returns>
        public static List<MBData> StrictMBDataList(List<MBData> data)
        {
            return EmptyFieldRemover.FilterNotEmpty(data).ToList();
        }

    }
}
