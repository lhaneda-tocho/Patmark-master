using System;
namespace TokyoChokoku
{
    public sealed class None{
        public static readonly None Instance = new None();


        public static R Run<R> (Func<R> f) {
            return f();
        }

        public static None Also (Action f) {
            f();
            return Instance;
        }

        None(){}
    };

    public static class FuncUtil
    {
        public static R Run<T, R> (this T it, Func<T, R> f) {
            return f(it);
        }

        public static T Also<T> (this T it, Action<T> f) {
            f(it);
            return it;
        }
    }
}
