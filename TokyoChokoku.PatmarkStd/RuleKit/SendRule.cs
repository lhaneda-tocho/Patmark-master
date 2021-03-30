/// @file SendRule.cs
/// @brief クラス定義ファイル
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TokyoChokoku.Patmark.RuleKit
{
    /// <summary>
    /// 送信データが満たすべきルールを示します。
    /// </summary>
    public class SendRule : RuleEnumeration<SendRule>
    {
        /// <summary>
        /// <c>Size</c>以外の全ての要素を記録したリストです。このリストは不変です。
        /// </summary>
        public static IList<SendRule> ValueList { get; private set; } = new List<SendRule>();

        /// <summary>
        /// オンラインであるべきことを示すルールです。
        /// </summary>
        public static SendRule OnlineRule { get; }       = new SendRule(0, "OnlineRule",       OnlineValidator.Create);

        /// <summary>
        /// テキストサイズが有効範囲にあるべきであることを示すルールです。
        /// </summary>
        public static SendRule TextSizeRule { get; }     = new SendRule(1, "TextSizeRule",     TextSizeValidator.Create);

        /// <summary>
        /// 使用不可能な文字列が含まれてはならないことを示すルールです。
        /// </summary>
        public static SendRule CharactorRule { get; }    = new SendRule(2, "CharactorRule",    CharactorValidator.Create);

        /// <summary>
        /// 送信するデータは空であってはならないことを示すルールです。
        /// </summary>
        public static SendRule NotEmptyRule { get; }     = new SendRule(3, "NotEmptyRule",     NotEmptyValidator.Create);

        /// <summary>
        /// 機種設定のルールです
        /// </summary>
        public static SendRule MachineModelRule { get; } = new SendRule(4, "MachineModelRule", MachineModelValidator.Create);

        /// <summary>
        /// この列挙型の要素数です。 <c>Index</c> == ValueList.Count であることを保証します。
        /// </summary>
        public static SendRule Size { get; } = new SendRule(-1, "Size");


        // ====


        /// <summary>
        /// インスタンス化された順に採番される番号を示します。
        /// </summary>
        public override int Index { get; }

        private Func<IValidationResourceProvider, ISendDataValidator> ValidatorCreator { get; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="id">一意のID</param>
        /// <param name="name">名前</param>
        /// <param name="createValidator"></param>
        /// <param name="isSizeSpecification"></param>
        private SendRule(
            int    id,
            string name,
            Func<IValidationResourceProvider, ISendDataValidator> createValidator
        ) : base(id, name)
        {
            ValidatorCreator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            Index = ValueList.Count;
            ValueList.Add(this);
        }

        /// <summary>
        /// サイズオブジェクトをインスタンス化します。
        /// このコンストラクタの呼び出した後、このクラスのインスタンス化が不可能になります。
        /// </summary>
        private SendRule(int id, string name) : base(id, name)
        {
            // サイズ指定オブジェクト限定の操作
            ValidatorCreator = null;
            Index = ValueList.Count;
            ValueList = ValueList.ToImmutableList();
        }

        /// <summary>
        /// ルールに対応するバリデータを生成します.
        /// </summary>
        /// <param name="resources">リソースプロバイダ</param>
        /// <returns>バリデータ</returns>
        public ISendDataValidator CreateValidator(IValidationResourceProvider resources)
        {
            return ValidatorCreator(resources);
        }
    }
}