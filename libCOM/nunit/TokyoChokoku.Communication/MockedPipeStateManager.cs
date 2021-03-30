using System;
using System.Threading.Tasks;
using TokyoChokoku.Communication;
namespace nunit.TokyoChokoku.Communication
{
    public class MockedPipeStateManager : PipeStateManager
    {
        /// <summary>
        /// CallStart(), CallTerminate()メソッドが 成功するか・失敗するかの設定.
        /// true の時は成功, falseの時は失敗です.
        /// </summary>
        public bool Successfully { get; set; } = true;

        /// <summary>
        /// CallStart()メソッドの失敗時に例外を投げるかどうかの設定.
        /// trueの場合に例外を投げます. falseの時はfalseを返します.
        /// CallTerminate()メソッドの挙動は変更できません. 失敗時は必ず例外を投げます.
        /// Successfullyがfalseの時に有効です.
        /// </summary>
        public bool ThrowOnFailureToStart { get; set; } = false;

        public MockedPipeStateManager(LineObserver lineObserver) : base(lineObserver)
        {
        }

        protected override Task<bool> CallStart()
        {
            
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (Successfully)
                tcs.SetResult(true);
            else 
            {
                if (ThrowOnFailureToStart)
                    tcs.SetException(new System.IO.IOException("[Mock] ThrowOnFailureToStart Enabled"));
                else
                    tcs.SetResult(false);
            }
            return tcs.Task;
        }

        protected override Task CallTerminate()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            if (Successfully)
                tcs.SetResult(null);
            else
                tcs.SetException(new System.IO.IOException("[Mock]"));
            return tcs.Task;
        }
    }
}
