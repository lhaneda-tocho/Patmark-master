
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TokyoChokoku.Communication;

using TokyoChokoku.Patmark.MachineModel;

using TokyoChokoku.Patmark.Droid.Custom;
using TokyoChokoku.Patmark.Settings;
using TokyoChokoku.Patmark.Common;

namespace TokyoChokoku.Patmark.Droid.Presenter
{
    
    class SettingListViewTitle {
        public SettingListViewItem Item { get; }
        public string Kind { get; }

        public SettingListViewTitle(SettingListViewItem item, string kind) { Item = item; Kind = kind;}

        public override string ToString()
        {
            switch(Kind) {
                case "Title": return Item.Title;
                case "Message": return Item.Message;
                default: throw new ArgumentException();
            }
        }
    }


    class SettingListViewItem
    {
        /// <summary>
        /// リソースの記述内容から行の定義辞書を初期化する
        /// </summary>
        /// <returns>The resource string array.</returns>
        /// <param name="array">Array.</param>
        public static Dictionary<String, SettingListViewItem> FromResourceStringArray(string[] array)
        {
            var dict = new Dictionary<String, SettingListViewItem>();
            foreach (var c in array)
            {
                // リソースから得られた次の形式の文字列 ("Key|Value") を KeyとValueに分解
                var firstIndex = c.IndexOf('|');
                var key = c.Substring(0, firstIndex).Trim();
                var value = c.Substring(firstIndex + 1).Trim();
                var item = new SettingListViewItem(key, value);
                dict.Add(key, item);
            }
            return dict;
        }

        /// <summary>
        /// 行の定義辞書と行の順番を示すキーのリストから行のリストを取得します.
        /// </summary>
        /// <returns></returns>
        /// <param name="defs"></param>
        /// <param name="keyOrder"></param>
        /// <param name="keyOrder">リストの順番を決めるキーのリスト. このリストに含まれていないキーは無視されます.</param>
        public static List<SettingListViewItem> ApplyRowOrder(Dictionary<String, SettingListViewItem> defs, List<string> keyOrder)
        {
            return (
                from key in keyOrder
                select defs[key]
            ).ToList();
        }

        /// <summary>
        /// 行のリストと要素の割当先を表すキー，IDを指定して，SimpleAdapterを初期化します.
        /// </summary>
        /// <returns>The adapter of.</returns>
        /// <param name="list">List.</param>
        /// <param name="context">Context.</param>
        /// <param name="titleKey">Title key.</param>
        /// <param name="messageKey">Message key.</param>
        /// <param name="theme">Theme.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public static SimpleAdapter SimpleAdapterOf(List<SettingListViewItem> list, string titleKey, string messageKey, Context context, int theme, string[] from, int[] to)
        {
            var data = (
                from item in list
                select (IDictionary<string, object>) new JavaDictionary<string, object> { 
                    {titleKey  , item.ViewTitle()}, 
                    {messageKey, item.ViewMessage()}
            }).ToList();

            return new SimpleAdapter(context, data, theme, from, to);
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; } = "";


        public SettingListViewItem(string id, string name)
        {
            this.Title = name;
            this.Id = id;
        }

        public override string ToString()
        {
            return String.Format("{0} | {1} | {2}", Id, Title, Message);
        }

        public SettingListViewTitle ViewTitle() => new SettingListViewTitle(this, "Title");
        public SettingListViewTitle ViewMessage() => new SettingListViewTitle(this, "Message");
    }


    [Activity(Label = "@string/settingsActivity_name")]
    public class SettingsActivity : Activity
    {
        
        static readonly List<string> KeyOrder = new List<string> {
            "MarkingParameter",
            "AppVersion",
            "RomVersion",
            "MachineModel"
        };


        public AppSetting Setting { get; private set; }
        Dictionary<String, SettingListViewItem> ItemTable = new Dictionary<string, SettingListViewItem>();
        List<SettingListViewItem>               ItemList  = new List<SettingListViewItem>();


        ///// <summary>
        ///// リソースの文字配列からセルの定義を取得します.
        ///// </summary>
        ///// <param name="resid">Resid.</param>
        //Dictionary<String, MyListItemModel> LoadItemFromResourceID(int resid)
        //{
        //    var array = Resources.GetStringArray(Resource.Array.settings_menu);

        //    var dict = new Dictionary<String, MyListItemModel>();
        //    foreach(var c in array)
        //    {
        //        // リソースから得られた次の形式の文字列 ("Key|Value") を KeyとValueに分解
        //        var firstIndex = c.IndexOf('|');
        //        var key   = c.Substring(0, firstIndex).Trim();
        //        var value = c.Substring(firstIndex + 1).Trim();
        //        var item = new MyListItemModel(key, value);

        //        // 有効なKeyのみを登録
        //        if(AvailableIds.Contains(key))
        //            dict.Add(key, item);
        //    }

        //    return dict;
        //}

        ///// <summary>
        ///// セルの定義をリストにして返します.
        ///// </summary>
        ///// <returns>The item list.</returns>
        //List<IDictionary<string, object>> GetItems(Dictionary<String, MyListItemModel> table) {
        //    if (table == null)
        //        return new List<IDictionary<string, object>>();

        //    return (
        //        from entry in table
        //        select (IDictionary<string, object>) new JavaDictionary<string, object> {
        //                { Title, entry.Value }, {Message, entry.Value.ToMessage() } }
        //    ).ToList();                         
        //}


        protected override void OnCreate(Bundle savedInstanceState)
        {
            // View init
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            // Get View
            var menu = FindViewById(Resource.Id.settings_menu) as ListView;

            // Model Init
            Setting = new AppSetting(this);

            // Load List
            ItemTable   = SettingListViewItem.FromResourceStringArray(Resources.GetStringArray(Resource.Array.settings_menu));
            ItemList    = SettingListViewItem.ApplyRowOrder(ItemTable, KeyOrder);
            var adapter = SettingListViewItem.SimpleAdapterOf(
                ItemList, "title", "mes",
                this, 
                Android.Resource.Layout.SimpleListItem2,
                new string[] { "title", "mes" }, 
                new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 }
            );
            menu.Adapter = adapter;

            // サブタイトル内容設定
            string name = Setting.CurrentMachineModelOnPreference().LocalizedName(Application.Context);

            // サブタイトル内容設定
            ItemTable["MachineModel"].Message = name;

            // クリックイベント処理の分配
            menu.ItemClick += (sender, e) => {
                var item = ItemList[e.Position];

                switch(item.Id){
                    case "MarkingParameter":
                        GotoDefaultMarkingParameter();
                        break;
                    case "AppVersion":
                        ShowVersion();
                        break;
                    case "RomVersion":
                        ShowRomVersion();
                        break;
                    case "MachineModel":
                        ShowModelNoSetting(menu, adapter);
                        break;
                    default:
                        break;
                }
            };
        }

        /// <summary>
        /// デフォルト打刻パラメータ設定画面へ遷移します。
        /// </summary>
        void GotoDefaultMarkingParameter(){
            StartActivity(new Intent(this, typeof(DefaultMarkingParameterActivity)).Also((intent) => {
                //intent.PutExtras(bundle);
            }));
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnMenuItemSelected(featureId, item);
            }
        }

        private void ShowVersion() {
            Toast.MakeText(this, Setting.ApplicationVersion, ToastLength.Long).Show();
        }

        void ShowRomVersion() {
            if (CommunicationClient.Instance.Ready)
            {
                ProgressDialogManager.ShowAndProcessOnBackground("ShowRomVersionProgress", async () =>
                {
                    var version = await Setting.ReadRomVersion();
                    await Task.Delay(500);
                    await PatmarkApplication.RunOnMain(() =>
                    {
                        Toast.MakeText(this, version, ToastLength.Long).Show();
                    });
                });
            }
            else
            {
                Toast.MakeText(this, GetString(Resource.String.toast_communication_required), ToastLength.Long).Show();
            }
        }



        /// <summary>
        /// モデル番号の設定のためのアクションシート
        /// </summary>
        void ShowModelNoSetting(
            ListView view,
            SimpleAdapter adapter
        )
        {
            // 選択画面の状態を変更する処理
            void DidHoldOnPreference(PatmarkMachineModel model)
            {
                // 名前取得
                string name = Setting.CurrentMachineModelOnPreference().LocalizedName(Application.Context);

                // サブタイトル内容設定
                ItemTable["MachineModel"].Message = name;
                view.Adapter = adapter;
            }

            ShowModelNoSettingStatic(this, Setting, DidHoldOnPreference);
        }


        /// <summary>
        /// モデル番号の設定のためのアクションシートを表示します。
        /// SettingsActivity で表示されるものと同じビューを別のアクティビティで使用できるよう公開しております。
        ///
        /// このメソッドでは、 Activity の画面の更新処理が除去されております。
        /// 呼び出し側は、didHoldOnPreference 引数に、画面更新処理 Action を指定する必要があります。
        /// </summary>
        internal static void ShowModelNoSettingStatic(
            Activity        self,
            AppSetting      appSetting,
            Action<PatmarkMachineModel> didHoldOnPreference
        )
        {
            var dialog = new Alert.MachineModelSelectionDialogFragment(self);
            dialog.OnClickRow += (spec) =>
            {
                // モデルの変更を受けた場合、それをプリファレンスに保存する
                appSetting.HoldOnPreference(spec);
                // 設定ができたことを通知
                didHoldOnPreference?.Invoke(spec);
                // コントローラ側の設定確認
                _ = PatmarkApplication.CommunicateOnReady(
                    terminator: true,
                    onOffline: () => Task.CompletedTask,
                    block: async () =>
                    {
                        var checkResult = await appSetting.IsControllerModelMatchedWith();
                        await checkResult.Aggregate<CommunicationResult<bool>, Task>(
                            onFailure: (_) =>
                            {
                                return PatmarkApplication.RunOnMain(() =>
                                {
                                    int id = Resource.String.toast_appsetting_controller_fail_to_check;
                                    Toast.MakeText(Application.Context, id, ToastLength.Long).Show();
                                });
                            },
                            onSuccess: (result) =>
                            {
                                if (!result.Value)
                                {
                                    return PatmarkApplication.RunOnMain(() =>
                                    {
                                        int id = Resource.String.toast_appsetting_controller_different;
                                        Toast.MakeText(Application.Context, id, ToastLength.Long).Show();
                                    });
                                }
                                else
                                {
                                    return Task.CompletedTask;
                                }
                            }
                        );
                    }
                );
            };
            dialog.ShowOn(self, "ActivityDialog.ModelSelection");
        }

    }
}
