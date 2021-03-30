using System;
using System.Linq;
using System.Collections.Immutable;
using System.Collections.Generic;
namespace TokyoChokoku.Patmark.MachineModel
{
    /// <summary>
    /// Patmarkの機種番号にあたる機能. Patmarkではバーションで判断する.
    /// </summary>
    public class PatmarkMachineModel : TokyoChokoku.MachineModel.MachineModelSpec
    {
        /// <summary>
        /// Patmark Mini / Patmark1515
        /// </summary>
        public static PatmarkMachineModel Patmark1515 { get; } = new PatmarkMachineModel("1515", 15, 15, PatmarkVersion.Of(4, 0), new MutablePatmarkProfile
        {
            NameOrNull = "Patmark Mini"
        });

        /// <summary>
        /// Patmark / Patmark3315
        /// </summary>
        public static PatmarkMachineModel Patmark3315 { get; } = new PatmarkMachineModel("3315", 33, 15, PatmarkVersion.Of(4, 10), new MutablePatmarkProfile
        {
            NameOrNull = "Patmark"
        });

        /// <summary>
        /// Patmark Plus / Patmark8020
        /// </summary>
        public static PatmarkMachineModel Patmark8020 { get; } = new PatmarkMachineModel("8020", 80, 20, PatmarkVersion.Of(5,  0), new MutablePatmarkProfile
        {
            NameOrNull = "Patmark Plus"
        });

        /// <summary>
        /// デフォルト値
        /// </summary>
        public static PatmarkMachineModel Default => Patmark1515;

        /// <summary>
        /// 先頭に行くほど新しい機種が並ぶように整列されたリストです。
        /// </summary>
        public static IList<PatmarkMachineModel> SpecListFromLatest { get; } = new List<PatmarkMachineModel> {
            Patmark8020,
            Patmark3315,
            Patmark1515
        }.ToImmutableList();

        /// <summary>
        /// 機種リストです。GUI などに表示する場合は、こちらのリストを使います。
        /// </summary>
        public static List<PatmarkMachineModel> SpecList => SpecListFromLatest.ToList();


        /// <summary>
        /// バージョン番号から該当する機種番号インスタンスを返します.
        /// 最小値は 4.0.0ですが，それ未満のバージョンを指定した場合は一番古い機種番号を返します.
        /// </summary>
        /// <returns>The model from version.</returns>
        /// <exception cref="NullReferenceException">versionがnullの場合</exception>
        [Obsolete("Disabled_A001", error: true)]
        public static PatmarkMachineModel FromVersion(PatmarkVersion version)
        {
            if (version == null)
                throw new NullReferenceException("\"version\" is must not be null.");
            foreach (var model in SpecListFromLatest)
            {
                // 機種Xのバージョン < 実機のバージョン を満たす、最も新しいバージョンの 機種X を選択する
                if (model.SupportVersion <= version) 
                    return model;
            }
            // 該当なし ... とりあえず一番古いバージョンを返す.
            return Patmark1515;
        }


        // ================

        /// <summary>
        /// 対応するバージョン(の開始)を表します.
        /// </summary>
        /// <value>The support version.</value>
        public PatmarkVersion SupportVersion { get; }

        /// <summary>
        /// 機種ごとに依存する設定のプロファイルです。
        /// </summary>
        public PatmarkProfile Profile { get; }

        /// <summary>
        /// Enum コンストラクタ
        /// </summary>
        /// <param name="name">機種名</param>
        /// <param name="width">打刻範囲幅 [mm]</param>
        /// <param name="height">打刻範囲高さ [mm]</param>
        /// <param name="support">サポートするバージョン</param>
        /// <param name="profile">プロフィール (対応する機能や定義領を決定する)</param>
        private PatmarkMachineModel(string name, ushort width, ushort height, PatmarkVersion support, MutablePatmarkProfile profile): base(name, "Patmark", width, height)
        {
            SupportVersion = support ?? throw new NullReferenceException("\"support\" must not be null.");
            profile.MaxTextSizeOrNull = profile.MaxTextSizeOrNull ?? (decimal?)height;
            Profile = profile.ToImmutable();
            // 未定義の意プロパティが存在する場合は、例外がスローされる。
            Profile.ShouldHaveUndefinedProperty();
        }

        /// <summary>
        /// デバッグ用の文字列を出力します。
        /// </summary>
        /// <returns>このクラスの内容をミトが見やすい形式に変換したもの。</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}x{2}(Name={3},SupportVersion=\"{4} ~\")", Target, Width, Height, Name, SupportVersion);
        }

        /// <summary>
        /// ローカライズ用の文字列を返します.
        /// Patmark:33x15 の形式で返されるので,これで条件分岐してローカライズを行います.
        /// </summary>
        /// <returns>The identifier.</returns>
        public string LocalizationTag => String.Format("{0}:{1}x{2}", Target, Width, Height);


    }
}
