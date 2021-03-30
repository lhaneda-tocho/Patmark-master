using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using TokyoChokoku.Patmark.Settings;
using PRanges=TokyoChokoku.Patmark.Settings.AppMarkingParameterRanges;
using System.Linq;

using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.iOS.Presenter.Settings
{
    public partial class DefaultMarkingParameterController : UIViewController
    {
        public DefaultMarkingParameterController(IntPtr handle) : base(handle)
        {
        }

        private UITextField editingView = null;
        private IPMMarkingParameterDB AppSettingRepository => new AppSettingMarkingParameterDB();
        private MyMarkingParameterDB MarkingParameterDBFromUI => new MyMarkingParameterDB(this);

        private Dictionary<UIButton, UITextField> TextAndButtonCombinations
        {
            get
            {
                return new Dictionary<UIButton, UITextField>()
                {
                    {ButtonSizeSmall, InputSizeSmall},
                    {ButtonSizeMedium, InputSizeMedium},
                    {ButtonSizeBig, InputSizeBig},
                    {ButtonForceWeak, InputForceWeak},
                    {ButtonForceMedium, InputForceMedium},
                    {ButtonForceStrong, InputForceStrong},
                    {ButtonQualityDot, InputQualityDot},
                    {ButtonQualityMedium, InputQualityMedium},
                    {ButtonQualityLine, InputQualityLine},
                };
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "ctrl-settings-default-marking-parameter.title".Localize();

            MarkingParameterDBFromUI.Initialize(AppSettingRepository.Baked());

            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender, args) =>
                {
                    var repo = new AppSettingMarkingParameterDB();
                    repo.Drain(GetParams());
                    repo.Commit();
                    NavigationController.PopViewController(true);
                })
            , true);

            PickerBox.Hidden = true;

            foreach (KeyValuePair<UIButton, UITextField> item in TextAndButtonCombinations)
            {
                item.Key.TouchUpInside += (sender, e) =>
                {
                    item.Value.BecomeFirstResponder();
                };
            }

            ButtonPickerOk.TouchUpInside += (sender, e) =>
                {
                    PickerBox.Hidden = true;
                };
        }

        protected void SetupPicker(UITextField input, List<string> targets){
            input.EditingDidBegin += (sender, e) => {
                if (editingView != input)
                {
                    editingView = input;
                    input.EndEditing(true);

                    var model = new PickerDataModel(targets);
                    model.ValueChanged += (_sender, _e) => {
                        if (model.SelectedItem != null)
                        {
                            input.Text = model.SelectedItem;
                        }
                    };

                    Picker.Model = model;

                    var initalValue = (from v in targets where v == input.Text select v).FirstOrDefault();
                    int initialIndex = targets.IndexOf(input.Text);
                    Picker.Select(initialIndex >= 0 ? initialIndex : 0, 0, true);

                    Picker.ReloadAllComponents();
                    PickerBox.Hidden = false;
                }
                else
                {
                    editingView = null;
                    PickerBox.Hidden = true;
                }
            };
        }


        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                foreach (var view in RootView.Subviews)
                {
                    if (view.IsFirstResponder)
                    {
                        view.ResignFirstResponder();
                        PickerBox.Hidden = true;
                    }
                }
            }
        }

        private IPMMarkingParameterDB GetParams()
        {
            return MarkingParameterDBFromUI.Baked();
        }



        class MyMarkingParameterDB : AbstractPMMarkingParameterDB
        {
            public override string DisplayName => $"{nameof(MyMarkingParameterDB)} in {nameof(DefaultMarkingParameterController)}";
            public override bool IsMutable => true;

            /// <summary>
            /// コントローラ
            /// </summary>
            private DefaultMarkingParameterController Controller { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MyMarkingParameterDB(DefaultMarkingParameterController controller)
            {
                Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            }

            /// <summary>
            /// このDBをUIごと初期化します。
            /// </summary>
            /// <param name="soruce"></param>\
            public void Initialize(IPMMarkingParameterDB soruce)
            {
                foreach(var level in TextSizeLevels.AllItems)
                {
                    var ui    = GetTextSizeUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetTextSize(level);
                    Controller.SetupPicker(
                        ui,
                        range.Select(it => it.Key).ToList()
                    );
                    ui.Text = value.ToString();
                }
                foreach(var level in QualityLevels.AllItems)
                {
                    var ui    = GetQualityUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetQuality(level);
                    Controller.SetupPicker(
                        ui,
                        range.Select(it => it.Key).ToList()
                    );
                    ui.Text = value.ToString();
                }
                foreach(var level in ForceLevels.AllItems)
                {
                    var ui    = GetForceUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetForce(level);
                    Controller.SetupPicker(
                        ui,
                        range.Select(it => it.Key).ToList()
                    );
                    ui.Text = value.ToString();
                }
            }


            public UITextField GetForceUI(ForceLevel key)
            {
                return key.Match(
                    weak: _ => Controller.InputForceWeak,
                    medium: _ => Controller.InputForceMedium,
                    strong: _ => Controller.InputForceStrong
                );
            }

            public UITextField GetQualityUI(QualityLevel key)
            {
                return key.Match(
                    dot: _ => Controller.InputQualityDot,
                    medium: _ => Controller.InputQualityMedium,
                    line: _ => Controller.InputQualityLine
                );
            }

            public UITextField GetTextSizeUI(TextSizeLevel key)
            {
                return key.Match(
                    small: _ => Controller.InputSizeSmall,
                    medium: _ => Controller.InputSizeMedium,
                    large: _ => Controller.InputSizeBig
                );
            }

            public override PMForce GetForce(ForceLevel key)
            {
                var text = GetForceUI(key).Text;
                return PMForce.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetForce(key);
            }

            public override PMQuality GetQuality(QualityLevel key)
            {
                var text = GetQualityUI(key).Text;
                return PMQuality.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetQuality(key);
            }

            // 選択項目精度にパディングされる
            public override PMTextSize GetTextSize(TextSizeLevel key)
            {
                var text = GetTextSizeUI(key).Text;
                return PMTextSize.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetTextSize(key);
            }

            public override void SetForce(ForceLevel key, PMForce value)
            {
                GetForceUI(key).Text = value.ToString();
            }

            public override void SetQuality(QualityLevel key, PMQuality value)
            {
                GetQualityUI(key).Text = value.ToString();
            }

            public override void SetTextSize(TextSizeLevel key, PMTextSize value)
            {
                GetTextSizeUI(key).Text = value.ToString();
            }
        }

        public class PickerDataModel : UIPickerViewModel
        {
            // プロパティの変更を検知するプロパティ
            public event EventHandler<EventArgs> ValueChanged;
            // Pickerに表示するデータを格納するフィールド
            List<string> items = new List<string>();

            private int SelectedIndex = 0;

            public PickerDataModel(List<string> items)
            {
                this.items = items;
            }

            // 選択された値を取得するメソッド
            public string SelectedItem
            {
                get
                {
                    if (items.GetRange(SelectedIndex, 1).Count > 0)
                    {
                        return items[SelectedIndex];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            // カラム数
            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }
            // 行数
            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return items.Count;
            }
            // 文字列取得
            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return items[(int)row];
            }
            // 選択されたときの挙動
            public override void Selected(UIPickerView pickerView, nint row, nint component)
            {
                SelectedIndex = (int)row;
                if (ValueChanged != null)
                {
                    ValueChanged(this, new EventArgs());
                }
            }
        }

    }
}