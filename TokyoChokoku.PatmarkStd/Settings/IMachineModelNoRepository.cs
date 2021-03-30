using System;
using TokyoChokoku.Patmark.MachineModel;
using System.Threading.Tasks;
namespace TokyoChokoku.Patmark.Settings
{
    /// <summary>
    /// MachineModelNo インスタンスの入出力を行うクラスです.
    /// </summary>
    public interface IMachineModelNoRepository
    {
        /// <summary>
        /// MachineModelSpecインスタンスを取得します．
        /// </summary>
        /// <returns>値を読み込めた場合はインスタンスが返されます. 失敗した場合は null が返されます.</returns>
        Task<PatmarkMachineModel> Pull();

        /// <summary>
        /// MachineModelSpecインスタンスのデータを保存します.
        /// </summary>
        /// <returns>The push.</returns>
        /// <param name="spec">Spec.</param>
        /// <returns>保存に成功した場合は true を返し，失敗した場合は false を返す.</returns>
        /// <exception cref="NotSupportedException">リポジトリがPushに対応していない場合</exception>
        Task<bool> Push(PatmarkMachineModel spec);
    }


    public static class MachineModelNoRepositoryExt
    {
        /// <summary>
        /// Pullタスクが終わるまで待ち，その結果を返すメソッドです.
        /// </summary>
        /// <returns>The pull.</returns>
        /// <param name="repository">Repository.</param>
        /// <exception cref="TaskCanceledException">タスクがキャンセルされた場合</exception>
        public static PatmarkMachineModel BlockingPull(this IMachineModelNoRepository repository)
        {
            var task = repository.Pull();
            task.Wait();
            if (task.IsCompleted)
                return task.Result;
            if (task.IsFaulted)
                throw task.Exception;
            throw new TaskCanceledException();
        }


        /// <summary>
        /// Pushタスクが終わるまで待ち，その結果を返すメソッドです.
        /// </summary>
        /// <returns>The push.</returns>
        /// <param name="repository">Repository.</param>
        /// <exception cref="TaskCanceledException">タスクがキャンセルされた場合</exception>
        /// <exception cref="NotSupportedException">リポジトリがPushに対応していない場合</exception>
        public static bool BlockingPush(this IMachineModelNoRepository repository, PatmarkMachineModel spec)
        {
            var task = repository.Push(spec);
            task.Wait();
            if (task.IsCompleted)
                return task.Result;
            if (task.IsFaulted)
                throw task.Exception;
            throw new TaskCanceledException();
        }
    }
}
