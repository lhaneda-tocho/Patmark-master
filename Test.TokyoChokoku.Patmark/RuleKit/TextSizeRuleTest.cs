using System;
using System.Collections.Generic;
using TokyoChokoku.Patmark.MachineModel;
using NUnit.Framework;
using TokyoChokoku.Communication;
using TokyoChokoku.ControllerIO;
using TokyoChokoku.Patmark.Settings;
using TokyoChokoku.Patmark.EmbossmentKit;
using System.Threading.Tasks;
using TokyoChokoku.Patmark.Common;

using TokyoChokoku.WorkerThread;

namespace TokyoChokoku.Patmark.RuleKit.Test
{
    [TestFixture()]
    public class TextSizeRuleTest
    {
        class MyResourceProvider : IValidationResourceProvider
        {
            public CommunicationClient CommunicationClient
                => NoResource<CommunicationClient>();

            public CommandExecutor CommandExecutor
                => NoResource<CommandExecutor>();

            public FileIO FileIO
                => NoResource<FileIO>();

            public StatusIO StatusIO
                => NoResource<StatusIO>();

            public PatmarkMachineModel CurrentMachineModel { get; }



            public Action CallbackOnEmpty
                => Fail;

            public Action<TextError> CallbackOnTextError
                => Fail;

            public Action CallbackOnOffline
                => Fail;

            public Action CallbackOnMismatchMachineModel
                => Fail;

            public Action<PatmarkMachineModel> CallbackOnTextSizeTooLarge { get; set; } = _ => { };


            T NoResource<T>()
            {
                Assert.Fail("No Resource was provided.");
                throw new InvalidOperationException();
            }


            void Fail()
            {
                Assert.Fail();
            }

            void Fail<T>(T any)
            {
                Assert.Fail();
            }

            public async Task<IUnstable<CommunicationResult<bool>>> CurrentMachineModelIsEqualsToRemote()
            {
                return NoResource<IUnstable<CommunicationResult<bool>>>();
            }



            public MyResourceProvider(PatmarkMachineModel currentInput)
            {
                CurrentMachineModel = currentInput ?? throw new ArgumentNullException(nameof(currentInput));
            }
        }



        [Test()]
        public void QuickModeTestForPatmark1515()
        {
            var rule = SendRule.TextSizeRule;
            var rp = new MyResourceProvider(PatmarkMachineModel.Patmark1515);
            var validator = rule.CreateValidator(rp);


            {
                // 正常系: テキストサイズが 0.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Small, PMTextSize.Create(0.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Small, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 正常系: テキストサイズが 15mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 例外系: テキストサイズが 15.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、テスト成功とする。
                bool isCallbacked = false;
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    isCallbacked = true;
                };

                // 検証を行う. ここの検証は失敗するはず.
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsFailure, $"Validation Should be Failure: {result.Description.Message}, {result.InnerException}");
                    Console.WriteLine($"Caused Error (15.5mm): {result.Description.Message}");
                });

                Assert.IsTrue(isCallbacked, $"Callback not invoked.");
            }
        }

        [Test()]
        public void QuickModeTestForPatmark3315()
        {
            var rule = SendRule.TextSizeRule;
            var rp = new MyResourceProvider(PatmarkMachineModel.Patmark3315);
            var validator = rule.CreateValidator(rp);


            {
                // 正常系: テキストサイズが 0.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Small, PMTextSize.Create(0.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Small, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 正常系: テキストサイズが 15mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 例外系: テキストサイズが 15.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、テスト成功とする。
                bool isCallbacked = false;
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    isCallbacked = true;
                };

                // 検証を行う. ここの検証は失敗するはず.
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsFailure, $"Validation Should be Failure: {result.Description.Message}, {result.InnerException}");
                    Console.WriteLine($"Caused Error (15.5mm): {result.Description.Message}");
                });

                Assert.IsTrue(isCallbacked, $"Callback not invoked.");
            }
        }

        [Test()]
        public void QuickModeTestForPatmark8020()
        {
            var rule = SendRule.TextSizeRule;
            var rp = new MyResourceProvider(PatmarkMachineModel.Patmark8020);
            var validator = rule.CreateValidator(rp);


            {
                // 正常系: テキストサイズが 0.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Small, PMTextSize.Create(0.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Small, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 正常系: テキストサイズが 15mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            {
                // 例外系: テキストサイズが 15.5mm である時の挙動
                var markingParams = new PMMutableMarkingParameterDB();
                markingParams.SetTextSize(TextSizeLevel.Large, PMTextSize.Create(15.5m));

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(TextSizeLevel.Large, ForceLevel.Strong, QualityLevel.Line),
                    "neko",
                    markingParams
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(data, serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true));

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnTextSizeTooLarge = it =>
                {
                    Assert.Fail($"arg == {it}");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }
        }
    }
}
