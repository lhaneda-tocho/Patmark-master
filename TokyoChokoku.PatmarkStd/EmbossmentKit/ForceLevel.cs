using System;
using System.Collections.Generic;

namespace TokyoChokoku.Patmark.EmbossmentKit
{
    /// <summary>
    /// This enumeration is represents the specification of weak, medium, strong force for patmark.
    /// </summary>
    public enum ForceLevel
    {
        Weak, Medium, Strong
    }


    /// <summary>
    /// This class is assistant of enum <c>ForceLevels</c>.
    /// </summary>
    public static class ForceLevels
    {

        /// <summary>
        /// All enum value of ForceLevel
        /// </summary>
        public static IList<ForceLevel> AllItems { get; } = Common.EnumUtil.GetValues<ForceLevel>();

        /// <summary>
        /// This function could be used as substitute of <c>switch statement</c>.
        /// </summary>
        /// <typeparam name="R">prograble return value type</typeparam>
        /// <param name="it">value equivalent to <c>switch</c> argument</param>
        /// <param name="small">This delegate will be called if <c>it</c> is small.</param>
        /// <param name="medium">This delegate will be called if <c>it</c> is medium.</param>
        /// <param name="large">This delegate will be called if <c>it</c> is large.</param>
        /// <returns>This methods returns the value which produced by any delegate.</returns>
        public static R Match<R>(this ForceLevel it,
                                      Func<ForceLevel, R> weak = null,
                                      Func<ForceLevel, R> medium = null,
                                      Func<ForceLevel, R> strong = null)
        {
            switch (it)
            {
                case ForceLevel.Weak  : return InvokeNullable(weak  , it);
                case ForceLevel.Medium: return InvokeNullable(medium, it);
                case ForceLevel.Strong: return InvokeNullable(strong, it);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// This method returns the name of <c>it</c>.
        /// </summary>
        /// <param name="it">text size spec.</param>
        /// <returns>this level name</returns>
        public static string GetName(this ForceLevel it)
        {
            return Enum.GetName(typeof(ForceLevel), it);
        }

        /// <summary>
        /// This method returns the index corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>this level index</returns>
        public static int GetIndex(this ForceLevel v)
        {
            switch (v)
            {
                case ForceLevel.Weak:
                    return 0;
                case ForceLevel.Medium:
                    return 1;
                case ForceLevel.Strong:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException("ForceLevel のケース設定に誤りがあります。");
            }
        }

        /// <summary>
        /// This method returns the default text size corresponding <c>v</c>.
        /// </summary>
        /// <param name="v">this level spec.</param>
        /// <returns>default value for this level.</returns>
        public static PMForce DefaultValueFixed(this ForceLevel v)
        {
            switch (v)
            {
                case ForceLevel.Weak:
                    return PMForce.Create(1);
                case ForceLevel.Medium:
                    return PMForce.Create(4);
                case ForceLevel.Strong:
                    return PMForce.Create(8);
                default:
                    throw new ArgumentOutOfRangeException("ForceLevel のケース設定に誤りがあります。");
            }
        }

        // ====

        [Obsolete("代替メソッドなし", error: true)]
        public static PMForce ToForce(this ForceLevel it)
        {
            var force = DefaultParameterProvider.Instance.GetForce.Invoke(it);
            return force;
        }

        private static R InvokeNullable<R>(Func<ForceLevel, R> it, ForceLevel arg)
        {
            if (it != null)
                return it(arg);
            else
                throw new NotSupportedException(arg.GetName());
        }
    }
}
