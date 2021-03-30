
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TokyoChokoku.Patmark.Settings;
using TokyoChokoku.Patmark.EmbossmentKit;

using PRanges = TokyoChokoku.Patmark.Settings.AppMarkingParameterRanges;


namespace TokyoChokoku.Patmark.Droid.Presenter
{
    [Activity(Label = "@string/defaultMarkingParameterActivity_name")]
    public class DefaultMarkingParameterActivity : Activity
    {
        AppSettingMarkingParameterDB AppSettingRepository => new AppSettingMarkingParameterDB();
        private MyMarkingParameterDB MarkingParameterDBFromUI => new MyMarkingParameterDB(this);

        Spinner SpinnerTextSizeSmall;
        Spinner SpinnerTextSizeMedium;
        Spinner SpinnerTextSizeLarge;
        Spinner SpinnerForceWeak;
        Spinner SpinnerForceMedium;
        Spinner SpinnerForceStrong;
        Spinner SpinnerQualityDot;
        Spinner SpinnerQualityMedium;
        Spinner SpinnerQualityLine;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SetContentView(Resource.Layout.DefaultMarkingParameter);

            FindViews();

            SetUpSpinners(AppSettingRepository.Baked());
        }

        void FindViews(){
            SpinnerTextSizeSmall = FindViewById(Resource.Id.spinner_text_size_small) as Spinner;
            SpinnerTextSizeMedium = FindViewById(Resource.Id.spinner_text_size_medium) as Spinner;
            SpinnerTextSizeLarge = FindViewById(Resource.Id.spinner_text_size_large) as Spinner;
            SpinnerForceWeak = FindViewById(Resource.Id.spinner_force_weak) as Spinner;
            SpinnerForceMedium = FindViewById(Resource.Id.spinner_force_medium) as Spinner;
            SpinnerForceStrong = FindViewById(Resource.Id.spinner_force_strong) as Spinner;
            SpinnerQualityDot = FindViewById(Resource.Id.spinner_quality_dot) as Spinner;
            SpinnerQualityMedium = FindViewById(Resource.Id.spinner_quality_medium) as Spinner;
            SpinnerQualityLine = FindViewById(Resource.Id.spinner_quality_line) as Spinner;
        }

        void SetUpSpinners(IPMMarkingParameterDB initialValues){
            MarkingParameterDBFromUI.Initialize(initialValues);
        }

        private IPMMarkingParameterDB GetParams()
        {
            return MarkingParameterDBFromUI.Baked();
        }

        void Commit(){
            var repo = AppSettingRepository;
            repo.Drain(GetParams());
            repo.Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.DefaultMarkingParameterMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                case Resource.Id.buttonSave:
                    Commit();
                    Finish();
                    return true;
                default:
                    return base.OnMenuItemSelected(featureId, item);
            }
        }




        class MyMarkingParameterDB : AbstractPMMarkingParameterDB
        {
            public override string DisplayName => $"{nameof(MyMarkingParameterDB)} in {nameof(DefaultMarkingParameterActivity)}";
            public override bool IsMutable => true;

            /// <summary>
            /// コントローラ
            /// </summary>
            private DefaultMarkingParameterActivity Controller { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MyMarkingParameterDB(DefaultMarkingParameterActivity controller)
            {
                Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            }


            private void SetupSpinner(Spinner spinner, Dictionary<string, PMForce> items, PMForce initialValue)
            {
                SetupSpinnerTypeRemoved(
                    spinner,
                    items.Select(it => it.Key),
                    initialValue.ToString()
                );
            }
            private void SetupSpinner(Spinner spinner, Dictionary<string, PMQuality> items, PMQuality initialValue)
            {
                SetupSpinnerTypeRemoved(
                    spinner,
                    items.Select(it => it.Key),
                    initialValue.ToString()
                );
            }
            private void SetupSpinner(Spinner spinner, Dictionary<string, PMTextSize> items, PMTextSize initialValue)
            {
                SetupSpinnerTypeRemoved(
                    spinner,
                    items.Select(it => it.Key),
                    initialValue.ToString()
                );
            }
            private void SetupSpinnerTypeRemoved(Spinner spinner, IEnumerable<string> items, string initialValue)
            {
                var itemList = items.ToList();
                spinner.Adapter = new ArrayAdapter<string>(Controller, Android.Resource.Layout.SimpleListItem1, itemList);
                spinner.SetSelection(itemList.IndexOf(initialValue));
            }

            public void Initialize(IPMMarkingParameterDB soruce)
            {
                foreach(var level in TextSizeLevels.AllItems)
                {
                    var ui    = GetTextSizeUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetTextSize(level);
                    SetupSpinner(ui, range, value);
                }
                foreach(var level in QualityLevels.AllItems)
                {
                    var ui    = GetQualityUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetQuality(level);
                    SetupSpinner(ui, range, value);
                }
                foreach(var level in ForceLevels.AllItems)
                {
                    var ui    = GetForceUI(level);
                    var range = PRanges.GetValueList(level);
                    var value = soruce.GetForce(level);
                    SetupSpinner(ui, range, value);
                }
            }

            public Spinner GetForceUI(ForceLevel key)
            {
                return key.Match(
                    weak    : _ => Controller.SpinnerForceWeak,
                    medium  : _ => Controller.SpinnerForceMedium,
                    strong  : _ => Controller.SpinnerForceStrong
                );
            }

            public Spinner GetQualityUI(QualityLevel key)
            {
                return key.Match(
                    dot     : _ => Controller.SpinnerQualityDot,
                    medium  : _ => Controller.SpinnerQualityMedium,
                    line    : _ => Controller.SpinnerQualityLine
                );
            }

            public Spinner GetTextSizeUI(TextSizeLevel key)
            {
                return key.Match(
                    small   : _ => Controller.SpinnerTextSizeSmall,
                    medium  : _ => Controller.SpinnerTextSizeMedium,
                    large   : _ => Controller.SpinnerTextSizeLarge
                );
            }

            public override PMForce GetForce(ForceLevel key)
            {
                var text = GetForceUI(key).SelectedItem.ToString();
                return PMForce.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetForce(key);
            }

            public override PMQuality GetQuality(QualityLevel key)
            {
                var text = GetQualityUI(key).SelectedItem.ToString();
                return PMQuality.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetQuality(key);
            }

            // 選択項目精度にパディングされる
            public override PMTextSize GetTextSize(TextSizeLevel key)
            {
                var text = GetTextSizeUI(key).SelectedItem.ToString();
                return PMTextSize.CreateFromDisplayTextOrNull(text) ?? FallbackDB.GetTextSize(key);
            }

            public override void SetForce(ForceLevel key, PMForce value)
            {
                var ui      = GetForceUI(key);
                var range   = PRanges.GetValueList(key);
                SetupSpinner(ui, range, value);
            }

            public override void SetQuality(QualityLevel key, PMQuality value)
            {
                var ui      = GetQualityUI(key);
                var range   = PRanges.GetValueList(key);
                SetupSpinner(ui, range, value);
            }

            public override void SetTextSize(TextSizeLevel key, PMTextSize value)
            {
                var ui      = GetTextSizeUI(key);
                var range   = PRanges.GetValueList(key);
                SetupSpinner(ui, range, value);
            }
        }
    }
}
