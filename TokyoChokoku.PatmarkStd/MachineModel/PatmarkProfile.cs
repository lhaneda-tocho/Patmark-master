using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
namespace TokyoChokoku.Patmark.MachineModel
{
    using Common;

    /// <summary>
    /// プロファイルのプロパティに設定するデータです。
    /// ToMutable, ToImmutable におけるコピー対象になります。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PatmarkProfilePublish: System.Attribute
    {
    }

    /// <summary>
    /// PatmarkProfile のファクトリインタフェースです。
    /// IPatmarkProfile は これを実装します。
    /// </summary>
    public interface IPatmarkProfileFactory
    {
        /// <summary>
        /// This method create immutable <c>IPatmarkProfile</c>.
        /// </summary>
        PatmarkProfile ToImmutable();

        /// <summary>
        /// This method create mutable <c>IPatmarkProfile</c>.
        /// </summary>
        MutablePatmarkProfile ToMutable();
    }

    /// <summary>
    /// Patmark 実機の機能を記録したクラスです。
    /// 機種の名前、値の定義域等を保存するために使用します。
    ///
    /// インスタンス化する際は、MutablePatmarkProfile のコンストラクタを呼び出してください。
    /// 書き換え不可能なオブジェクトが必要であれば、 ToImmutable メソッドを実行して、
    /// PatmarkProfile オブジェクトを取得します。
    ///
    /// 
    /// プロファイルに関わるプロパティは、下記の条件を満たすよう宣言されます。
    ///
    /// 1. 全て nullable である
    /// 2. null のプロパティは、Undefined なプロパティとして扱う。　(Undefined = 未設定 としております)
    /// 3. not null のプロパティは、代入済みのプロパティとして扱う。
    ///
    /// nullable なプロパティは全て [PatmarkProfilePublish] アトリビュートが設定されております。
    /// また、 nullable であることを表すため、 OrNull サフィックスが最後につきます。
    ///
    /// ex) NameOrNull
    ///
    /// Undefined なプロパティにアクセスした際に InvalidOperationException をスローして欲しいことがあります。
    /// その場合は、 OrNull サフィックスなしのメソッドにアクセスしてください。
    ///
    /// ex) Name()
    /// 
    /// </summary>
    public interface IPatmarkProfile: IPatmarkProfileFactory
    {
        /// <summary>
        /// このインタフェースに登録されているプロパティリストを返します。
        /// </summary>
        /// <returns>プロパティリスト</returns>
        public static IList<PropertyInfo> GetPropertyInfo()
        {
            var plist = typeof(IPatmarkProfile).GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
            .Where(it =>
            {
                var isDefined = Attribute.IsDefined(it, typeof(PatmarkProfilePublish));
                return isDefined;
            })
            .ToList();
            return plist;
        }

        /// <summary>
        /// プロパティをコピーします。
        /// </summary>
        /// <param name="from">コピー元</param>
        /// <param name="to">コピー先</param>
        internal static void CopyFromTo(IPatmarkProfile from, IPatmarkProfile to)
        {
            to.MaxTextSizeOrNull = from.MaxTextSizeOrNull;
            to.NameOrNull = from.NameOrNull;

            //// プロパティの複製を行う
            //var plist = GetPropertyInfo();
            //foreach (var p in plist)
            //{
            //    var value = p.GetValue(from);
            //    p.SetValue(to, value);
            //}
        }

        // ===============

        /// <summary>
        /// 最大テキストサイズです。未設定の場合は null が返されます。
        /// </summary>
        [PatmarkProfilePublish]
        public decimal? MaxTextSizeOrNull { get; set; }

        /// <summary>
        /// 機種タイトル(英語名)です。未設定の場合は null が返されます。
        /// </summary>
        [PatmarkProfilePublish]
        public string NameOrNull { get; set; }
    }

    /// <summary>
    /// PatmarkProfile 拡張クラスです。 インタフェースに直接定義できなかったメソッドをこちらに定義します。
    /// </summary>
    public static class PatmarkProfileExt
    {
        /// <summary>
        /// 最大テキストサイズです。未設定 (null) の場合は 未設定の場合は InvalidOperationException がスローされます。
        /// </summary>
        public static decimal MaxTextSize(this IPatmarkProfile self)
            => self.MaxTextSizeOrNull ?? throw new InvalidOperationException();

        /// <summary>
        /// 最大テキストサイズです。未設定の場合は InvalidOperationException がスローされます。
        /// </summary>
        public static string Name(this IPatmarkProfile self)
            => self.NameOrNull ?? throw new InvalidOperationException();

        /// <summary>
        /// Undefined なプロパティが存在する場合に限り、 InvalidOperationException 例外をスローします。
        /// </summary>
        public static void ShouldHaveUndefinedProperty(this IPatmarkProfile self)
        {
            // プロパティの複製を行う
            var plist = IPatmarkProfile.GetPropertyInfo();
            foreach (var p in plist)
            {
                var value = p.GetValue(self);
                if (ReferenceEquals(value, null))
                {
                    throw new InvalidOperationException("[Fatal] Uninitialized property detected.");
                }
            }
        }

        /// <summary>
        /// 人が読める形式に変換します。
        /// </summary>
        /// <param name="self">receiver</param>
        /// <returns>人が読める形式</returns>
        public static string ToStringExt(this IPatmarkProfile self)
        {
            var sb = new System.Text.StringBuilder();
            var plist = IPatmarkProfile.GetPropertyInfo();

            var prefix = "{ ";
            foreach (var p in plist)
            {
                var value = p.GetValue(self);
                var text  = value?.ToString() ?? "<undefined>";
                sb.Append(prefix).Append("\"").Append(p.Name).Append("\":\"").Append(text).Append("\"");
                prefix = ", ";
            }
            sb.Append(" }");

            return sb.ToString();
        }

    }



    /// <summary>
    /// Immutable type of <c>IPatmarkProfile</c>
    /// </summary>
    public class PatmarkProfile : IPatmarkProfile
    {
        // 初期化終了時に true が設定される。
        private bool Initialized { get; set; } = false;


        // リフレクションで、メンバのコピーを行うため、 private な Setter を定義しておく必要がある。
        /// <inheritdoc cref="IPatmarkProfile.MaxTextSizeOrNull" />
        public decimal? MaxTextSizeOrNull { get; private set; }

        /// <inheritdoc cref="IPatmarkProfile.NameOrNull" />
        public string   NameOrNull { get; private set; }


        decimal? IPatmarkProfile.MaxTextSizeOrNull {
            get => MaxTextSizeOrNull;
            set
            {
                if (Initialized)
                    throw new InvalidOperationException("object is immutable");
                MaxTextSizeOrNull = value;
            }
        }

        string IPatmarkProfile.NameOrNull
        {
            get => NameOrNull;
            set
            {
                if (Initialized)
                    throw new InvalidOperationException("object is immutable");
                NameOrNull = value;
            }
        }


        /// <summary>
        /// This constructor initialize the instance by copying the specified object.
        /// </summary>
        /// <param name="original">copy source</param>
        public PatmarkProfile(
            IPatmarkProfile original
        )
        {
            // プロパティの複製を行う
            IPatmarkProfile.CopyFromTo(original, this);
            Initialized = true;
        }

        /// <inheritdoc cref="IPatmarkProfileFactory.ToImmutable()" />
        public PatmarkProfile ToImmutable()
        {
            return this;
        }

        /// <inheritdoc cref="IPatmarkProfileFactory.ToMutable()" />
        public MutablePatmarkProfile ToMutable()
        {
            return new MutablePatmarkProfile(this);
        }

        /// <inheritdoc cref="PatmarkProfileExt.ToStringExt(IPatmarkProfile)" />
        public override string ToString()
        {
            return this.ToStringExt();
        }
    }

    /// <summary>
    /// Mutable type of <c>IPatmarkProfile</c>
    /// </summary>
    public class MutablePatmarkProfile : IPatmarkProfile
    {
        /// <inheritdoc cref="IPatmarkProfile.MaxTextSizeOrNull" />
        public decimal? MaxTextSizeOrNull { get; set; }

        /// <inheritdoc cref="IPatmarkProfile.NameOrNull" />
        public string   NameOrNull        { get; set; }

        /// <summary>
        /// This constructor initialize the instance with copying the specified object.
        /// </summary>
        /// <param name="original">copy source</param>
        public MutablePatmarkProfile(
            IPatmarkProfile original
        )
        {
            // プロパティの複製を行う
            IPatmarkProfile.CopyFromTo(original, this);
        }

        /// <summary>
        /// This constructor initialize the instance with null.
        /// </summary>
        public MutablePatmarkProfile()
        {
            // プロパティの複製を行う
            MaxTextSizeOrNull = null;
        }

        /// <inheritdoc cref="IPatmarkProfileFactory.ToImmutable()" />
        public PatmarkProfile ToImmutable()
        {
            return new PatmarkProfile(this);
        }

        /// <inheritdoc cref="IPatmarkProfileFactory.ToMutable()" />
        public MutablePatmarkProfile ToMutable()
        {
            return new MutablePatmarkProfile(this);
        }

        /// <inheritdoc cref="PatmarkProfileExt.ToStringExt(IPatmarkProfile)" />
        public override string ToString()
        {
            return this.ToStringExt();
        }
    }

}
