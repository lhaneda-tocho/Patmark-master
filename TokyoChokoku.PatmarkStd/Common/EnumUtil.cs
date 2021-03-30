using System;
using System.Collections.Generic;
using System.Linq;

namespace TokyoChokoku.Patmark.Common
{
    public static class EnumUtil
    {
        /// <summary>
        /// This method collect all values of T specified by type argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="immutable">See this return value specification. Default value is true.</param>
        /// <returns>
        /// The all enum value stored in a IList. This list is <c>immutable</c> if the immutable is true. otherwise, the list is mutable.
        /// </returns>
        public static IList<T> GetValues<T>(bool immutable=true)
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToList();
        }
    }
}
