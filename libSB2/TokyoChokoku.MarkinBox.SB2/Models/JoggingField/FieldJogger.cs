using System;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.MarkinBox.Sketchbook.Fields;
using TokyoChokoku.MarkinBox.Sketchbook.Parameters;
using TokyoChokoku.MarkinBox.Sketchbook.Communication;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    /// <summary>
    /// Field jogger.
    /// </summary>
    public class FieldJogger
    {
        private IField<IConstantParameter> Field { get; set; }
        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.MarkinBox.Sketchbook.FieldJogger"/> class.
        /// </summary>
        /// <param name="field">Field.</param>
        public FieldJogger(IField<IConstantParameter> field)
        {
            this.Field = field;
        }

        /// <summary>
        /// Requests to jog.
        /// </summary>
        /// <returns><c>true</c>, if to jog was requested, <c>false</c> otherwise.</returns>
        public void RequestToJog()
        {
            lock (cts)
            {
                Log.Debug(new String[] {
                    "ジョグ機能の起動を試みます。",
                    "選択したフィールドのジョグ状態:" + Field.Parameter.Jogging,
                    "コントローラとの接続状態:" + CommunicationClientManager.Instance.IsOnline()
                });
                if (!cts.IsCancellationRequested) {
                    cts.Cancel ();
                }
                cts = new CancellationTokenSource ();

                var state = CommunicationClientManager.Instance.GetCurrentState ();
                if (Field.Parameter.Jogging && state.Ready ()) {
                    Task.Delay (50, cts.Token).ContinueWith (async _ => {
                        await Jog ();
                    });
                }
            }
        }

        /// <summary>
        /// Jog this instance.
        /// </summary>
        /// <returns>The jog.</returns>
        private async Task<Nil> Jog()
        {
            Log.Debug("ジョグ機能を起動します。");

            var model = MachineModelNoManager.Get();
            var stdSpan = model.StandardHeadMovingSpan();
            var stdPulse = model.StandardHeadMovingPulse();

            float rateX = (float)(Field.Parameter.X) / stdSpan.X;
            float rateY = (float)(Field.Parameter.Y) / stdSpan.Y;

            rateX = Math.Max(0.0f, Math.Min(1.0f, rateX));
            rateY = Math.Max(0.0f, Math.Min(1.0f, rateY));

            var pulseX = rateX * stdPulse.X;
            var pulseY = rateY * stdPulse.Y;

            // X方向に端から端まで1秒で到達する速度
            float speed = Consts.MB2_STD_CLOCK / stdPulse.X;

            Log.Debug(String.Format("PulseX:{0}, PulseY:{1}, Speed:{2}", pulseX, pulseY, speed));

            await CommandExecuter.SetOptionsOfPinMoving((short)pulseX, (short)pulseY, (short)speed);
            await CommandExecuter.StartToMovePin();

            return null;
        }
    }
}
