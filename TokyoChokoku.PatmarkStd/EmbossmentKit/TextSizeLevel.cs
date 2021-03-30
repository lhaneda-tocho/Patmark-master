using System;
using System.Collections.Generic;
using TokyoChokoku.Patmark.TextData;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// This enumeration is represents the specification of large, medium and small text size for patmark.
    /// </summary>
    public enum TextSizeLevel
    {
        Small, Medium, Large
    }

    /// <summary>
    /// This class is represents the specification of large, medium and small text size for patmark,
    /// and this class could materialize these specification as the display value.
    /// </summary>
    public class MaterializableTextSizeLevel
    {
        /// <summary>
        /// This method instantiates this class with the valid level and the nonnull materialization delegate.
        /// </summary>
        /// <param name="level">The text size level specification.</param>
        /// <param name="materializer">The nonnull materialization delegate.</param>
        /// <param name="materializerName">The nonnull materializer name. (for debug and inspection)</param>
        /// <exception cref="ArgumentNullException">
        /// if the <c>materializer</c> is null.
        /// </exception>
        public static MaterializableTextSizeLevel Create(TextSizeLevel level, Func<TextSizeLevel, PMTextSize> materializer, string materializerName)
        {
            return new MaterializableTextSizeLevel(level, materializer, materializerName);
        }

        // ====

        /// <summary>
        /// Level Specification.
        /// </summary>
        public TextSizeLevel Level { get; }

        /// <summary>
        /// Level Materializer
        /// </summary>
        /// <param name="level"></param>
        /// <param name="materializer"></param>
        public Func<TextSizeLevel, PMTextSize> Materializer { get; }

        public string MaterializerName { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <see cref="Create"/>
        private MaterializableTextSizeLevel(TextSizeLevel level, Func<TextSizeLevel, PMTextSize> materializer, string materializerName)
        {
            Level = level;
            Materializer = materializer ?? throw new ArgumentNullException(nameof(materializer));
            MaterializerName = materializerName ?? throw new ArgumentNullException(nameof(materializerName));
        }

        /// <summary>
        /// This method converts this instance to TextSizeLevel.
        /// </summary>
        /// <returns>level value</returns>
        public TextSizeLevel ToLevel() => Level;

        /// <summary>
        /// This method retrieve the value corresponded with my <c>Level</c>.
        /// </summary>
        /// <returns></returns>
        public PMTextSize ToMaterialized() => Materializer(Level);


        /// <summary>
        /// Convert to EmbossmentTextSize
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        public EmbossmentTextSize ToEmbossmentTextSize()
        {
            return ToMaterialized().ToEmbossmentTextSize();
        }

        /// <summary>
        /// Convert to ITextSize
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        public ITextSize ToPresentationTextSize()
        {
            return ToMaterialized().ToPresentationTextSize();
        }

        /// <summary>
        /// This methods converts this instance to string.
        /// </summary>
        /// <returns>level name</returns>
        public override string ToString()
        {
            return Level.ToString();
        }

        /// <summary>
        /// This methods returns a info text for debugging.
        /// </summary>
        /// <returns>debug information</returns>
        public string ToStringForDebug()
        {
            return $"({Level}, {MaterializerName}: (TextSizeLevel)->PMTextSize)";
        }
    }

    /// <summary>
    /// This class is assistant of enum <c>TextSizeLevel</c>.
    /// </summary>
    public static class TextSizeLevels
    {

        /// <summary>
        /// All enum value of TextSizeLevel
        /// </summary>
        public static IList<TextSizeLevel> AllItems { get; } = Common.EnumUtil.GetValues <TextSizeLevel>();

        /// <summary>
        /// This function could be used as substitute of <c>switch statement</c>.
        /// </summary>
        /// <typeparam name="R">prograble return value type</typeparam>
        /// <param name="it">value equivalent to <c>switch</c> argument</param>
        /// <param name="small">This delegate will be called if <c>it</c> is small.</param>
        /// <param name="medium">This delegate will be called if <c>it</c> is medium.</param>
        /// <param name="large">This delegate will be called if <c>it</c> is large.</param>
        /// <returns>This methods returns the value which produced by any delegate.</returns>
        public static R Match<R>(this TextSizeLevel it,
                                      Func<TextSizeLevel, R> small  = null,
                                      Func<TextSizeLevel, R> medium = null,
                                      Func<TextSizeLevel, R> large  = null)
        {
            switch(it)
			{
                case TextSizeLevel.Small : return InvokeNullable(small , it);
				case TextSizeLevel.Medium: return InvokeNullable(medium, it);
				case TextSizeLevel.Large : return InvokeNullable(large , it);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// This method returns the name of <c>it</c>.
        /// </summary>
        /// <param name="it">text size spec.</param>
        /// <returns>this level name</returns>
        public static string GetName(this TextSizeLevel it) {
            return Enum.GetName(typeof(TextSizeLevel), it);
        }

        /// <summary>
        /// This method returns the index corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>this level index</returns>
        public static int GetIndex(this TextSizeLevel v)
        {
            switch (v)
            {
                case TextSizeLevel.Small:
                    return 0;
                case TextSizeLevel.Medium:
                    return 1;
                case TextSizeLevel.Large:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException("TextSizeLevel のケース設定に誤りがあります。");
            }
        }

        /// <summary>
        /// This method returns the default text size corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>default value for this level.</returns>
        public static PMTextSize DefaultValueFixed(this TextSizeLevel v)
        {
            switch (v)
            {
                case TextSizeLevel.Small:
                    return PMTextSize.Create(2.0m);
                case TextSizeLevel.Medium:
                    return PMTextSize.Create(5.0m);
                case TextSizeLevel.Large:
                    return PMTextSize.Create(10.0m);
                default:
                    throw new ArgumentOutOfRangeException("TextSizeLevel のケース設定に誤りがあります。");
            }
        }

        // ====


        [Obsolete("代替メソッドなし", error: true)]
        public static EmbossmentTextSize ToEmbossmentTextSize(this TextSizeLevel it)
        {
            var isize = ToTextSize(it);
            return EmbossmentTextSize.InitWithHeightmm(isize.Heightmm);
        }

        [Obsolete("代替メソッドなし", error: true)]
        public static ITextSize ToTextSize(this TextSizeLevel it)
        {
            var value = DefaultParameterProvider.Instance.GetTextSize.Invoke(it);
            return value.ToPresentationTextSize();
        }

        private static R InvokeNullable<R>(Func<TextSizeLevel, R> it, TextSizeLevel arg)
        {
            if (it != null)
                return it(arg);
            else
                throw new NotSupportedException(arg.GetName());
        }

    }
}
