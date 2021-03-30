using System;
namespace TokyoChokoku.Patmark.Common
{
    public static class Checks
    {
        static void error(String message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void RequiredNotNull(object value, Func<String> message)
        {
            if (!ReferenceEquals(value, null))
                return;
            var m = message?.Invoke() ?? "null not accepted";
            error(m);
            throw new ArgumentNullException(m);
        }

        public static void RequiredNotNull(object value, string message = null)
        {
            if (!ReferenceEquals(value, null))
                return;
            var m = message ?? "null not accepted";
            error(m);
            throw new ArgumentNullException(m);
        }

        public static void Required(bool condition, Func<string> message)
        {
            if (condition)
                return;
            var m = message?.Invoke() ?? "argument condition not accuired";
            error(m);
            throw new ArgumentException(m);
        }

        public static void Required(bool condition, string message = null)
        {
            if (condition)
                return;
            var m = message ?? "argument condition not accuired";
            error(m);
            throw new ArgumentException(m);
        }

        public static void Expect(bool condition, Func<string> message)
        {
            if (condition)
                return;
            var m = message?.Invoke() ?? "unexpected state";
            error(m);
            throw new InvalidOperationException(m);
        }

        public static void Expect(bool condition, string message = null)
        {
            if (condition)
                return;
            var m = message ?? "unexpected state";
            error(m);
            throw new InvalidOperationException(m);
        }

        
    }
}
