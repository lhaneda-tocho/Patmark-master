using System.Threading.Tasks;
using Foundation;
using System;
using UIKit;
using Functional.Maybe;

using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public partial class SerialSettingsEditorController : UIViewController
    {
        KeyboardManager KeyboardManager { get; set;}

        public SerialSettingsEditorController(IntPtr handle) : base(handle)
        {
        }

        public ViewModel Model { get; set; }

        public class ViewModel
        {
            public int                  SerialNo    { get; set; }
            public SerialStores         Stores      { get; set; }
            public event Action<string> OnInsert;
            public event Action<string> OnReplaceSerial;

            public void Purge ()
            {
                OnInsert   = null;
                OnReplaceSerial = null;
            }

            public void ExecInsert (string tag)
            {
                OnInsert (tag);
            }
        }


        SerialSettingsMaxValueConnection            MaxValueConnection;
        SerialSettingsMinValueConnection            MinValueConnection;
        SerialSettingsRepeatingCountConnection      RepeatingCountConnection;
        SerialSettingsClearingTimeHHConnection      ClearingTimeHHConnection;
        SerialSettingsClearingTimeMMConnection      ClearingTimeMMConnection;
        SerialSettingsCounterCurrentValueConnection CurrentValueConnection;


        override
        public void ViewDidLoad()
        {
            // ナビゲーションバーを無視してスクロールビューのサイズを決定します。
            AutomaticallyAdjustsScrollViewInsets = false;

            KeyboardManager = new KeyboardManager (ScrollView);

            // UITextFieldViewの初期化処理
            TextFieldInputAccessoryView.PressDoneButton += () => {
                // Doneボタンが押された時
                ScrollView.FindFirstResponder ().Do (view => {
                    view.ResignFirstResponder ();
                });
            };

            var textFields = UITextFieldExt.arrayOf (
                ClearingTimeHHTextField,
                ClearingTimeMMTextField,
                CurrentValueTextField,
                MaxValueTextField,
                MinValueTextField,
                RepeatingCountTextField
            );

            // すべてのテキストフィールドに完了ボタンを．(テンキーにEnterキーがないため)
            textFields.AddInputAccessoryView (TextFieldInputAccessoryView);

            // 繰り返し値の妥当性チェック機能
            RepeatingCountTextField.AddEndEditing ((t) => {
                // 1以上の値を許す．それ以外は デフォルト値
                var text = t.Text;
                int value;
                if (!int.TryParse (text, out value)) {
                    // 数値に変換できなかった場合
                    value = 1;
                }
                if (value < 1) {
                    // 1未満の時
                    value = 1;
                }
                t.Text = value.ToString ();
            });

            // フォーマット
            {
                var selector = FormatSelector;

                for (var i = 0; i < selector.NumberOfSegments; i++)
                {
                    selector.SetTitle(
                        NSBundle.MainBundle.LocalizedString(
                            string.Format("ctrl-serial-editor-formats-segment-{0}.title", i + 1),
                            ""
                        ),
                        i
                    );
                }

                selector.SelectedSegment = (int)Model.Stores.FormatStore.Content;
            }

            // リセット条件

            {
                var selector = CounterClearingConditionSelector;

                for (var i = 0; i < selector.NumberOfSegments; i++)
                {
                    selector.SetTitle(
                        NSBundle.MainBundle.LocalizedString(
                            string.Format("ctrl-serial-editor-counter-clearing-conditions-segment-{0}.title", i + 1),
                            ""
                        ),
                        i
                    );
                }

                selector.SelectedSegment = (int)Model.Stores.ClearingConditionStore.Content;
            }

            MaxValueConnection = new SerialSettingsMaxValueConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            MaxValueTextField.Text = MaxValueConnection.Content.ToString();


            MinValueConnection = new SerialSettingsMinValueConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            MinValueTextField.Text = MinValueConnection.Content.ToString();


            RepeatingCountConnection = new SerialSettingsRepeatingCountConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            RepeatingCountConnection.Content = 1;
            RepeatingCountTextField.Text = RepeatingCountConnection.Content.ToString();

            ClearingTimeHHConnection = new SerialSettingsClearingTimeHHConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            ClearingTimeHHTextField.Text = ClearingTimeHHConnection.Content.ToString();


            ClearingTimeMMConnection = new SerialSettingsClearingTimeMMConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            ClearingTimeMMTextField.Text = ClearingTimeMMConnection.Content.ToString();


            CurrentValueConnection = new SerialSettingsCounterCurrentValueConnection(
                Model.Stores,
                (result, sender) => { },
                (result, sender) => { }
            );
            CurrentValueTextField.Text = CurrentValueConnection.Content.ToString();

        }

        void Commit ()
        {
            Model.Stores.FormatStore           .SetIfValid ((short)FormatSelector                  .SelectedSegment);
            Model.Stores.ClearingConditionStore.SetIfValid ((short)CounterClearingConditionSelector.SelectedSegment);

            MaxValueConnection      .TrySet (MaxValueTextField      .Text);
            MinValueConnection      .TrySet (MinValueTextField      .Text);
            RepeatingCountConnection.TrySet (RepeatingCountTextField.Text);
            ClearingTimeHHConnection.TrySet (ClearingTimeHHTextField.Text);
            ClearingTimeMMConnection.TrySet (ClearingTimeMMTextField.Text);
            CurrentValueConnection  .TrySet (CurrentValueTextField  .Text);
        }


        override
        public void ViewWillAppear (bool animated)
        {
            KeyboardManager.StartObserve ();
        }

        override
        public void ViewDidDisappear (bool animated)
        {
            KeyboardManager.StopObserve ();
        }

        override
        public void ViewWillUnload ()
        {
            KeyboardManager.StopObserve ();
        }

        partial void Insert_TouchUpInside (BorderButton sender)
        {
            // 確定
            Commit ();

            // シリアル設定を保存します。
            Task.Run (async () => {
                await SerialSettingsManager.Instance.Save (null);
            });

            string tag;
            int n = Model.SerialNo;
            int v = CurrentValueConnection.Content;

            tag = "@S[" + v + '-' + n + "]";

            Model.ExecInsert (tag);
            DismissViewController (true, null);
        }

        partial void CancelButton_Activated (UIBarButtonItem sender)
        {
            DismissViewController (true, null);
        }
    }
}