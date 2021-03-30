using System;
using GlobalToast;

namespace TokyoChokoku.Patmark.iOS.Presenter
{
    public static class MyToast
    {
        public const double Long  = 3.5;
        public const double Short = 2.5;

        /// <summary>
        /// トーストを経由してユーザにメッセージを伝えます。
        /// メッセージはローカライズされて表示されます。
        /// </summary>
        /// <param name="text">表示するメッセージ。メッセージはローカライズされて表示されます。</param>
        /// <param name="duration">表示時間の設定です。</param>
        /// <param name="fargs">引数を指定すると、 <c>text</c> をローカライズした物を、string.Format でフォーマットされます。</param>
        public static void ShowMessage(string text, double duration = Short, params object[] fargs)
        {
            ShowMessageUnsafe(text, localize: true, duration: duration, fargs: fargs);
        }

        /// <summary>
        /// トーストを経由して文字列を表示します。
        /// メッセージはローカライズされませんので、デバッグログの表示に利用できます。
        /// UI の機能と1つとして利用する場合はこのメソッドではなく、 <c>ShowMessage</c> メソッドを使うべきです。
        /// </summary>
        /// <param name="text">表示するメッセージ。メッセージはローカライズされて表示されます。</param>
        /// <param name="duration">表示時間の設定です。</param>
        /// <param name="fargs">引数を指定すると、 <c>text</c> の内容が string.Format でフォーマットされます。</param>
        [Obsolete("トーストメッセージはローカライズされるべきです。 (デバッグ用途で使う場合は無視して構いません)", error: false)]
        public static void ShowMessageNotLocalized(string text, double duration = Short, params object[] fargs)
        {
            ShowMessageUnsafe(text, localize: false, duration: duration, fargs: fargs);
        }

        private static void ShowMessageUnsafe(string text, bool localize, double duration, params object[] fargs)
        {
            if (localize)
                text = text.Localize();
            if (fargs == null)
                fargs = new object[0];
            if (fargs.Length > 0)
                text = string.Format(text, fargs);
            Toast.MakeToast(text)
                .SetDuration(duration*1000.0)
                .Show();
        }
    }
}
