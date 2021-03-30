using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
namespace TokyoChokoku.ControllerIO
{
    public class TextValidator
    {
        public struct Result
        {
            public bool   IsSuccess;
            public char   ErrorChar;
            public int    Index;
        }

        CommunicationClient TheClient;

        public TextValidator(CommunicationClient theClient)
        {
            TheClient = theClient ?? CommunicationClient.Instance;
        }
        public TextValidator() : this(null)
        { }


        async Task<bool> IsProcessing()
        {
            var exe = TheClient.CommandExecutor;
            if(TheClient.Ready)
            {
                var res = await exe.ReadTextValidationStart();
                if (res.IsOk)
                {
                    return res.Value != 0;
                }
            }
            throw new System.IO.IOException();
        }


        // throws IOException
        async Task<ushort> GetError()
        {
            var exe = TheClient.CommandExecutor;
            if(TheClient.Ready)
            {
                var res = await exe.ReadTextValidationError();
                if (res.IsOk)
                {
                    return (ushort)res.Value;
                }
            }
            throw new System.IO.IOException();
        }

        /// <summary>
        /// Validate the specified text.
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="text">Text.</param>
        /// <exception cref="IOException">通信中にエラーが発生した時.</exception>
        public async Task<Result> Validate(string text)
        {
            try
            {
                int index = 0;
                var exe = TheClient.CommandExecutor;
                var texts = SplitWithCount(text, 9);

                foreach (var part in texts)
                {
                    if (!TheClient.Ready)
                        throw new System.IO.IOException();

                    var res = await exe.StartTextValidation(part);
                    if (!res.IsOk)
                        throw new System.IO.IOException();

                    // 終了待機
                    // 500ミリ秒経過しても処理が終わらない場合は、フォント確認機能が未実装であるとみなし、OKを返します。
                    var count = 0;
                    while (await IsProcessing())
                    {
                        if (count++ >= 5)
                        {
                            Log.Warn("フォント確認機能が未実装であるとみなし、確認処理を中断しました。");
                            return GetResultSuccess();
                        }
                        await Task.Delay(100);
                    }

                    // エラー取得
                    var error = await GetError();
                    if (error != 0)
                    {
                        Result result;
                        var ti = index + error - 1;
                        var c = text[ti];
                        result.IsSuccess = false;
                        result.ErrorChar = c;
                        result.Index = ti;
                        return result;
                    }
                    index += 9;
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine("Error in TextValidator.Validate");
                Console.Error.WriteLine(ex);
                throw new System.IO.IOException("Error in TextValidator.Validate", ex);
            }

            return GetResultSuccess();
        }

        static List<string> SplitWithCount(string text, int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("the size argument must be positive, musn't be zero.");

            var charTotal = text.Length;
            var partCount = text.Length / size + (text.Length % size == 0 ? 0 : 1);
            var list = new List<string>(partCount);
            for (int i = 0; i < partCount; i++)
            {
                int s = i * size;
                int r = charTotal - s;
                string sub = text.Substring(s, Math.Min(size, r));
                list.Add(sub);
            }
            return list;
        }

        static private Result GetResultSuccess(){
            Result result;
            result.IsSuccess = true;
            result.ErrorChar = '\0';
            result.Index = 0;
            return result;
        }
    }
}
