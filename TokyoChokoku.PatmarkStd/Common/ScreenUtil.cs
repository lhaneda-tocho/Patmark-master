using System;
namespace TokyoChokoku.Patmark.Common
{
    public abstract class ScreenUtil
    {
        #region Injection
        private static readonly Object theLock = new object();
        public static ScreenUtil Instance { get; private set; } = null;

		/// <summary>
		/// 指定したオブジェクトをInjectします．
		/// 2回目はInvalidOperationExceptionとなります．
		/// </summary>
		public static void Inject(ScreenUtil impl)
        {
            if (impl == null)
                throw new NullReferenceException("Not allowed null.");
            lock (theLock)
            {
                if (Instance == null)
                    Instance = impl;
                else
                    throw new InvalidOperationException("Not allowed re-inject.");
            }
        }


		/// <summary>
		/// 指定したオブジェクトをInjectします．
		/// 2回目を呼び出しても何も起きません．
		/// </summary>
		public static void InjectNeeded(ScreenUtil impl)
		{
			if (impl == null)
				throw new NullReferenceException("Not allowed null.");
            if (Instance == null)
				Inject(impl);
		}
        #endregion

        public abstract DPI  getDPI();
        public virtual  DPMM getDPMM() {
            var dpi = getDPI().value;
            var dpmm = dpi / 25.4;
            return new DPMM(dpmm);
        }
	}

	public class DPI
	{
		public double value { get; }
		public DPI(double value) { this.value = value; }
	}


	public class DPMM
	{
		public double value { get; }
		public DPMM(double value) { this.value = value; }
	}
}
