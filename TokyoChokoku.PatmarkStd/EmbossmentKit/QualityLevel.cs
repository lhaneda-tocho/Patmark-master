using System;
using System.Collections.Generic;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// This enumeration is represents the specification of dot, medium, line quality for patmark.
    /// </summary>
    public enum QualityLevel
    {
        Dot, Medium, Line
    }

    /// <summary>
    /// This class is assistant of enum <c>QualityLevels</c>.
    /// </summary>
    public static class QualityLevels {
        /// <summary>
        /// All enum value of QualityLevel
        /// </summary>
        public static IList<QualityLevel> AllItems { get; } = Common.EnumUtil.GetValues<QualityLevel>();

        /// <summary>
        /// This function could be used as substitute of <c>switch statement</c>.
        /// </summary>
        /// <typeparam name="R">prograble return value type</typeparam>
        /// <param name="it">value equivalent to <c>switch</c> argument</param>
        /// <param name="small">This delegate will be called if <c>it</c> is small.</param>
        /// <param name="medium">This delegate will be called if <c>it</c> is medium.</param>
        /// <param name="large">This delegate will be called if <c>it</c> is large.</param>
        /// <returns>This methods returns the value which produced by any delegate.</returns>
        public static R Match<R>(this QualityLevel it,
                                      Func<QualityLevel, R> dot = null,
                                      Func<QualityLevel, R> medium = null,
                                      Func<QualityLevel, R> line = null)
        {
            switch (it)
            {
                case QualityLevel.Dot   : return InvokeNullable(dot   , it);
                case QualityLevel.Medium: return InvokeNullable(medium, it);
                case QualityLevel.Line  : return InvokeNullable(line  , it);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// This method returns the name of <c>it</c>.
        /// </summary>
        /// <param name="it">this level spec.</param>
        /// <returns>text size spec name.</returns>
        public static string GetName(this QualityLevel it) 
        {
            return Enum.GetName(typeof(QualityLevel), it);
        }

        /// <summary>
        /// This method returns the index corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>index</returns>
        public static int GetIndex(this QualityLevel v)
        {
            switch (v)
            {
                case QualityLevel.Dot:
                    return 0;
                case QualityLevel.Medium:
                    return 1;
                case QualityLevel.Line:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException("QualityLevel のケース設定に誤りがあります。");
            }
        }

        /// <summary>
        /// This method returns the default text size corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>default value for this level.</returns>
        public static PMQuality DefaultValueFixed(this QualityLevel v)
        {
            switch (v)
            {
                case QualityLevel.Dot:
                    return PMQuality.Create(2);
                case QualityLevel.Medium:
                    return PMQuality.Create(5);
                case QualityLevel.Line:
                    return PMQuality.Create(8);
                default:
                    throw new ArgumentOutOfRangeException("QualityLevel のケース設定に誤りがあります。");
            }
        }

        // ====

        private static R InvokeNullable<R>(Func<QualityLevel, R> it, QualityLevel arg)
        {
            if (it != null)
                return it(arg);
            else
                throw new NotSupportedException(arg.GetName());
        }


        [Obsolete("代替メソッドなし", error: true)]
        public static PMQuality ToQuality(this QualityLevel it)
        {
            var quality = DefaultParameterProvider.Instance.GetQuality.Invoke(it);
            return quality;
        }
    }
}
