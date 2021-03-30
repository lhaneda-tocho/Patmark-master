using System;
using System.Linq;
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
using TokyoChokoku.MarkinBox.Sketchbook;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;

namespace TokyoChokoku.Patmark.RuleKit.Test
{
    [TestFixture()]
    public class NotEmptyRuleTest
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

            public PatmarkMachineModel CurrentMachineModel
                => NoResource<PatmarkMachineModel>();



            public Action CallbackOnEmpty { get; set; } = () => { };

            public Action<TextError> CallbackOnTextError
                => Fail;

            public Action CallbackOnOffline
                => Fail;

            public Action CallbackOnMismatchMachineModel
                => Fail;

            public Action<PatmarkMachineModel> CallbackOnTextSizeTooLarge
                => Fail;


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
        }


        private HorizontalText CreateHT(string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            var ht = HorizontalText.Mutable.Create();
            ht.Parameter.Text = text;
            return ht;
        }

        private QrCode CreateQR(string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            var qr = QrCode.Mutable.Create();
            qr.Parameter.Text = text;
            return qr;
        }

        private DataMatrix CreateDM(string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            var dm = DataMatrix.Mutable.Create();
            dm.Parameter.Text = text;
            return dm;
        }

        private Bypass CreateBypass()
        {
            return Bypass.Mutable.Create();
        }


        /// <summary>
        /// 正常と認識されるテキストを入れた時に、正常なデータとして扱われることを確認する。
        /// </summary>
        [Test()]
        public void QuickModeUT_Std()
        {
            void With(string input)
            {
                Console.WriteLine($"Input={input}");

                var rule = SendRule.NotEmptyRule;
                var rp = new MyResourceProvider();
                var mp = new PMMutableMarkingParameterDB();
                var validator = rule.CreateValidator(rp);

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(), input, mp
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(
                    data,
                    serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true)
                );

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnEmpty = () =>
                {
                    Assert.Fail($"Unexpected Callbacking");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            void WithEach(string input)
            {
                foreach(var c in input)
                {
                    With(c.ToString());
                }
            }

            WithEach("mew");
            WithEach("x");
            WithEach("あいうえお");
            WithEach("かきくけこ");
            WithEach("さしすせそ");
            WithEach("0123456789");
            WithEach("qwertyuiopasdfghjklzxcvbnm");
            WithEach("QWERTYUIOPASDFGHJKLZXCVBNM");
            WithEach("[];'./-=!@#$%^&*()~:\"'?><");
        }


        [Test()]
        public void QuickModeUT_Except()
        {
            void With(string input)
            {
                Console.WriteLine($"Input={input}");

                var rule = SendRule.NotEmptyRule;
                var rp = new MyResourceProvider();
                var mp = new PMMutableMarkingParameterDB();
                var validator = rule.CreateValidator(rp);

                EmbossmentData data = EmbossmentData.Create(
                    new EmbossmentMode(), input, mp
                );

                // 送信可能な形式にする
                QuickModeData qdata = new QuickModeData(
                    data,
                    serialized: () => EmbossmentToolKit.ToSerializable(data, null, pause: true)
                );

                // コールバックが呼ばれたら、成功とする
                bool isCallbacked = false;
                rp.CallbackOnEmpty = () =>
                {
                    isCallbacked = true;
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidateQuickMode(qdata);
                    Assert.IsTrue(result.IsFailure, $"Validation Should be Failure: {result.Description.Message}, {result.InnerException}");
                    Console.WriteLine($"Caused Error (15.5mm): {result.Description.Message}");
                });

                Assert.IsTrue(isCallbacked, $"Callback not invoked.");
            }

            void WithEach(string input)
            {
                foreach (var c in input)
                {
                    With(c.ToString());
                }
            }

            WithEach(" 　\t\f\r\n");
            With("");
        }



        [Test()]
        public void PCFileModeUT_Std()
        {
            void WithMBData(IList<MBData> data)
            {
                Console.WriteLine($"Input={data}");

                var rule = SendRule.NotEmptyRule;
                var rp = new MyResourceProvider();
                var mp = new PMMutableMarkingParameterDB();
                var validator = rule.CreateValidator(rp);

                // 送信可能な形式にする
                PCFileModeData qdata = new PCFileModeData(
                    data,
                    strict: () => EmbossmentToolKit.StrictMBDataList(data.ToList())
                );

                // コールバックが呼ばれたら、失敗とする
                rp.CallbackOnEmpty = () =>
                {
                    Assert.Fail($"Unexpected Callbacking");
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidatePCFileMode(qdata);
                    Assert.IsTrue(result.IsSuccess, $"Validation Error: {result.Description.Message}, {result.InnerException}");
                });
            }

            void WithHT(string input)
            {
                WithMBData(new List<MBData>
                {
                    CreateHT(input).ToSerializable()
                });
            }
            void WithDMQR(string input)
            {
                WithMBData(new List<MBData>
                {
                    CreateDM(input).ToSerializable()
                });
                WithMBData(new List<MBData>
                {
                    CreateQR(input).ToSerializable()
                });
            }

            void WithHTEach(string input)
            {
                foreach (var c in input)
                {
                    WithHT(input);
                }
            }

            void WithDMQREach(string input)
            {
                foreach (var c in input)
                {
                    WithDMQR(input);
                }
            }

            WithHTEach("mew");
            WithHTEach("x");
            WithHTEach("あいうえお");
            WithHTEach("かきくけこ");
            WithHTEach("さしすせそ");
            WithHTEach("0123456789");
            WithHTEach("qwertyuiopasdfghjklzxcvbnm");
            WithHTEach("QWERTYUIOPASDFGHJKLZXCVBNM");
            WithHTEach("[];'./-=!@#$%^&*()~:\"'?><");
            WithDMQREach(" 　\t\f\r\n");
        }


        [Test()]
        public void PCFileModeUT_Except()
        {
            void WithMBData(IList<MBData> data)
            {
                Console.WriteLine($"Input={data}");

                var rule = SendRule.NotEmptyRule;
                var rp = new MyResourceProvider();
                var mp = new PMMutableMarkingParameterDB();
                var validator = rule.CreateValidator(rp);

                // 送信可能な形式にする
                PCFileModeData qdata = new PCFileModeData(
                    data,
                    strict: () => EmbossmentToolKit.StrictMBDataList(data.ToList())
                );

                // コールバックが呼ばれたら、成功とする
                bool isCallbacked = false;
                rp.CallbackOnEmpty = () =>
                {
                    isCallbacked = true;
                };

                // 検証を行う
                SingleThreadWorker.RunBlocking(async () => {
                    var result = await validator.ValidatePCFileMode(qdata);
                    Assert.IsTrue(result.IsFailure, $"Validation Should be Failure: {result.Description.Message}, {result.InnerException}");
                    Console.WriteLine($"Caused Error (15.5mm): {result.Description.Message}");
                });

                Assert.IsTrue(isCallbacked, $"Callback not invoked.");
            }

            void WithHT(string input)
            {
                WithMBData(new List<MBData>
                {
                    CreateHT(input).ToSerializable()
                });
            }
            void WithDMQR(string input)
            {
                WithMBData(new List<MBData>
                {
                    CreateDM(input).ToSerializable()
                });
                WithMBData(new List<MBData>
                {
                    CreateQR(input).ToSerializable()
                });
            }

            void WithHTEach(string input)
            {
                foreach (var c in input)
                {
                    WithHT(input);
                }
            }

            void WithDMQREach(string input)
            {
                foreach (var c in input)
                {
                    WithDMQR(input);
                }
            }

            //void WithBypass()
            //{
            //    WithMBData(new List<MBData>
            //    {
            //        CreateBypass().ToSerializable()
            //    });
            //}

            //WithBypass();
            WithHT("");
            WithDMQR("");
            WithHTEach(" 　\t\f\r\n");
        }
    }
}
