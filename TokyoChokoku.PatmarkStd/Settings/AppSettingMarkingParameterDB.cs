using System;
using TokyoChokoku.Patmark.EmbossmentKit;

namespace TokyoChokoku.Patmark.Settings
{
    /// <summary>
    /// グローバル変数に保存されている打刻設定となります。
    /// この DB は可変です。
    /// </summary>
    public class AppSettingMarkingParameterDB: IPMMutableMarkingParameterDB
    {
        /// <summary>
        /// 標準アプリ設定を使用してインスタンス化する。
        /// </summary>
        public static AppSettingMarkingParameterDB CreateDefault()
            => new AppSettingMarkingParameterDB();


        /// <summary>
        /// コンストラクタインジェクションによってインスタンス化する。
        /// </summary>
        /// <exception cref="ArgumentNullException">kvs が null の場合</exception>
        public static AppSettingMarkingParameterDB CreateWithInject(IKeyValueStore kvs, string rootKey = null, string displayName = null)
        {
            _ = kvs ?? throw new ArgumentNullException(nameof(kvs));
            return new AppSettingMarkingParameterDB(kvs, rootKey, displayName);
        }

        // ====

        public string DisplayName { get; }
        public bool IsMutable => true;

        /// <summary>
        /// アプリに保存・リストアの際に使用する文字列 (Injectable)
        /// </summary>
        string RootKey { get; }

        /// <summary>
        /// アプリ設定 (Injectable)
        /// </summary>
        IKeyValueStore Kvs;

        /// <summary>
        /// フォールバック設定
        /// </summary>
        private IPMMarkingParameterDB FallbackDB = new FallbackMarkingParameterDB();

        /// <summary>
        /// 標準アプリ設定を使用してインスタンス化する。
        /// アプリ設定オブジェクトをコンストラクタインジェクションすることでも インスタンス化可能です。
        /// </summary>
        /// <param name="kvs">アプリ設定の Key Value Store. Default: null</param>
        public AppSettingMarkingParameterDB(IKeyValueStore kvs = null, string rootKey = null, string displayName = null)
        {
            Kvs = kvs ?? KeyValueStoreFactoryHolder.Instance.Create();
            RootKey = rootKey ?? "DefaultMarkingParameter";
            DisplayName = displayName ?? nameof(AppSettingMarkingParameterDB);
        }

        /// <summary>
        /// 設定されたテキストサイズを取得します。未設定の場合は、デフォルト値にフォールバックされます。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public PMTextSize GetTextSize(TextSizeLevel key)
        {
            var storeKey = string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key);
            return PMTextSize.CreateOrDefaultWithFloat(
                Kvs.GetFloat(
                    storeKey,
                    FallbackDB.GetTextSize(key).ToFloat()
                )
            );
        }

        /// <summary>
        /// 設定された打刻力を取得します。未設定の場合は、デフォルト値にフォールバックされます。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public PMForce GetForce(ForceLevel key)
        {
            var storeKey = string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key);
            return PMForce.CreateOrDefault(
                Kvs.GetInt(
                    storeKey,
                    FallbackDB.GetForce(key).ToInt()
                )
            );
        }

        /// <summary>
        /// 設定された品質値を取得します。未設定の場合は、デフォルト値にフォールバックされます。
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public PMQuality GetQuality(QualityLevel key)
        {
            var storeKey = string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key);
            return PMQuality.CreateOrDefault(
                Kvs.GetInt(
                    storeKey,
                    FallbackDB.GetQuality(key).ToInt()
                )
            );
        }


        public void SetTextSize(TextSizeLevel key, PMTextSize val)
        {
            Kvs.Set(string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key), val.ToFloat());
        }

        public void SetForce(ForceLevel key, PMForce val)
        {
            Kvs.Set(string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key), val.ToInt());
        }

        public void SetQuality(QualityLevel key, PMQuality val)
        {
            Kvs.Set(string.Format("{0}_{1}_{2}", RootKey, key.GetType(), key), val.ToInt());
        }

        public IPMMarkingParameterDB Baked()
        {
            return this.StandardImmutableCopy();
        }

        public void Drain(IPMMarkingParameterDB src)
        {
            this.StandardDrain(src);
        }

        public void Commit(){
            Kvs.Commit();
        }

        [Obsolete("Baked に名前が変わりました")]
        public IPMMarkingParameterDB GetAll()
        {
            return Baked();
        }
    }
}
