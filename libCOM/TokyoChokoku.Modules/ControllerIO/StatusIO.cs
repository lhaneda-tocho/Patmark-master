using System;
using System.IO;
using System.Threading.Tasks;
using TokyoChokoku.Communication;

namespace TokyoChokoku.ControllerIO
{
    public class StatusIO
    {
        public CommandExecutor Exec { get; }

        public StatusIO(CommandExecutor exec)
        {
            Exec = exec;
        }

        public StatusIO() : this(CommunicationClient.Instance.CommandExecutor) {
            
        }

        public async Task<T> ProtectOnMarking<T>(Func<Task<T>> task)
        {
            try
            {
                var sio = this;
                if (!await sio.IsMarkingNow())
                {
                    return await task();
                }
                else
                {
                    throw CommunicationProtectedException.WhileMarking();
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message ?? "unknown", ex);
            }
        }

        public async Task<bool> IsMarkingNow()
        {
            return MarkingState.Stop != await RetrieveMarkingState();
        }

        /// <summary>
        /// Retrieves the state of the marking.
        /// </summary>
        /// <returns>The marking state.</returns>
        /// <exception cref="System.IO.IOException">通信途中でエラーが発生した時</exception>
        public async Task<MarkingState> RetrieveMarkingState()
        {
            MBMarkingStatus        ms = await RetrieveMBMarkingStatus();
            MBMarkingPausingStatus ps = await RetrieveMBPausingStatus();

            if (ms != MBMarkingStatus.Stop)
                // Marking now (想定外の値も 打刻中の扱い
                return MarkingState.Marking;
            else {
                switch(ps) {
                    case MBMarkingPausingStatus.None:
                    case MBMarkingPausingStatus.Stop:
                        // finish
                        return MarkingState.Stop;
                    case MBMarkingPausingStatus.Pause:
                        // pause
                        return MarkingState.Pause;
                    case MBMarkingPausingStatus.Resume:
                    default:
                        // restart now (想定外の値も打刻中の扱い
                        return MarkingState.Marking;
                }
            }
        }


        /// <summary>
        /// Retrieve Retrieve alert from controller.
        /// </summary>
        /// <returns>The alert. null if a communication fail or not found any controllers.</returns>
        /// <exception cref="System.IO.IOException">通信途中でエラーが発生した時</exception>
        public async Task<MBAlert?> RetrieveAlert()
        {
            try
            {
                var resp = await Exec.ReadAlert();
                if (resp.IsOk)
                {
                    return resp.Value.ToMBAlert();
                }
                throw new IOException();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IOException("Error in RetrieveAlert", ex);
            }
        }

        /// <summary>
        /// Retrieve the marking result.
        /// </summary>
        /// <returns>The marking result. null if a communication fail or not found any controllers.</returns>
        /// <exception cref="System.IO.IOException">通信途中でエラーが発生した時</exception>
        async Task<MBMarkingResult> RetrieveMBMarkingResult()
        {
            try
            {
                var resp = await Exec.ReadMarkingResult();
                if (resp.IsOk)
                {
                    return resp.Value.ToMBMarkingResult();
                }
                throw new IOException();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IOException("Error in RetrieveMarkingResult", ex);
            }
        }

        /// <summary>
        /// 打刻中, Pause状態の取得
        /// </summary>
        /// <returns>The marking status. null if a communication fail or not found any controllers.</returns>
        /// <exception cref="System.IO.IOException">通信途中でエラーが発生した時</exception>
        async Task<MBMarkingStatus> RetrieveMBMarkingStatus()
        {
            try
            {
                var resp = await Exec.ReadMarkingStatus();
                if (resp.IsOk)
                {
                    return resp.Value.ToMBMarkingStatus();
                }
                throw new IOException();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IOException("Error in RetrieveMarkingStatus", ex);
            }
        }

        /// <summary>
        /// Retrieves the pausing status.
        /// </summary>
        /// <returns>The pausing status.</returns>
        /// <exception cref="System.IO.IOException">通信途中でエラーが発生した時</exception>
        async Task<MBMarkingPausingStatus> RetrieveMBPausingStatus()
        {
            try
            {
                var resp = await Exec.ReadMarkingPausingStatus();
                if(resp.IsOk) {
                    return resp.Value.ToMBMarkingPausingStatus();
                }
                throw new IOException("Response Error in RetrievePausingStatus");
            } 
            catch (IOException ex) 
            {
                Console.WriteLine(ex);
                throw ex;
            }
            catch(Exception ex) 
            {
                var nex = new IOException("Error in RetrievePausingStatus", ex);
                Console.WriteLine(nex);
                throw nex;
            }
        }
    }
}
