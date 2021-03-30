
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TokyoChokoku.Communication;

using Android.Util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Custom
{
	/// <summary>
	/// Loader State.
	/// </summary>
	public class LoaderInfo
	{
        static object StaticLock = new object();
		static Dictionary<string, LoaderInfo> LoaderMap = new Dictionary<string, LoaderInfo>();

        public static void SaveInfo(string key, LoaderInfo info) 
        {
            lock(StaticLock) {
                if (LoaderMap.ContainsKey(key))
                    LoaderMap.Remove(key);
                LoaderMap.Add(key, info);
            }
        }
        public static LoaderInfo RestoreInfo(string key) {
            lock(StaticLock) {
                var value = LoaderMap[key];
				LoaderMap.Remove(key);
				return value;
			}
        }
        public static void UnsafeClear()
        {
            lock(StaticLock) {
                LoaderMap.Clear();
            }
        }


		object TheLock = new object();
		volatile bool visible = false;
		event Action<LoaderInfo> Callback;

		public bool Visible
		{
			get => visible;
			set
			{
				lock (TheLock)
				{
					visible = value;
					Callback(this);
				}
			}

		}

		public void RemoveCallback()
		{
			Callback = null;
		}

		public void SetCallback(Action<LoaderInfo> callback, bool fireNow)
		{
			lock (TheLock)
			{
				Callback = callback;
				if (fireNow)
				{
					Callback(this);
				}
			}
		}
	}

    /// <summary>
    /// 読み込み中にオーバーレイ表示するビューです．
    /// 
    /// (注) このオブジェクトの ShowWithTask を呼び出してタスクを開始した後
    ///      タスクが終わる前にこのメソッドを呼び出した場合の動作は定義していません．
    /// 
    /// </summary>
    public class LoadingOverlayView : LinearLayout
    {
        class MyPercelable : Java.Lang.Object, IParcelable
        {
            public Bundle Bundle { get; } = new Bundle();

            public IParcelable BaseParcelable
            {
                get => (IParcelable)Bundle.GetParcelable("Base");
                set => Bundle.PutParcelable("Base", value);
            }

            public string LoaderInfoKey
            {
                get => Bundle.GetString("LoaderInfoKey");
                set
                {
                    Bundle.PutString("LoaderInfoKey", value);
                }
            }

            public int DescribeContents()
            {
                return 0;
            }

            public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
            {
                dest.WriteBundle(Bundle);
            }
        }

        public string LoaderInfoKey { get; private set; } = DateTime.Now.Ticks.ToString();

        LoaderInfo back_info = null;
        public LoaderInfo Info
        {
            get => back_info;
            set
            {
                var old = back_info;
                back_info = value;
                if (old != null)
                    old.RemoveCallback();
                value.SetCallback(ExecuteLoaderCallback, true);
            }
        }

        ProgressBar Pbar;

        public LoadingOverlayView(Context context) :
            base(context)
        {
            Initialize(context);
        }

        public LoadingOverlayView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context);
        }

        public LoadingOverlayView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        void Initialize(Context ctx)
        {
            Info = new LoaderInfo();
            LayoutInflater.From(ctx).Inflate(Resource.Layout.LoadingOverlay, this, true);

            Pbar = (ProgressBar)FindViewById(Resource.Id.progressBar);
        }

        protected override IParcelable OnSaveInstanceState()
        {
            var p = new MyPercelable();
            p.BaseParcelable = base.OnSaveInstanceState();
            p.LoaderInfoKey = LoaderInfoKey;

            if (!string.IsNullOrEmpty(LoaderInfoKey))
                LoaderInfo.SaveInfo(LoaderInfoKey, Info);

            return p;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (state is MyPercelable)
            {
                var p = (MyPercelable)state;
                base.OnRestoreInstanceState(p.BaseParcelable);
                this.LoaderInfoKey = p.LoaderInfoKey;

                if (!string.IsNullOrEmpty(LoaderInfoKey))
                {
                    Info = LoaderInfo.RestoreInfo(LoaderInfoKey) ?? new LoaderInfo();
                }
            }
        }

        void ExecuteLoaderCallback(LoaderInfo info)
        {
            if (info.Visible)
            {
                Visibility = ViewStates.Visible;
            }
            else
            {
                Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// 引数で渡したタスクが完了するまで、画面をグレーアウトします。
        /// </summary>
        /// <returns>The with task.</returns>
        /// <param name="func">Func.</param>
        //public async Task ShowWhileProcessing(Task child)
        //{
        //    Info.Visible = true;
        //    await FinishAfter(child);
        //}

        public async Task ShowWhileProcessing(Func<Task> child)
        {
            Application.SynchronizationContext.Post(_ =>
            {
                Info.Visible = true;
            }, null);
            await FinishAfter(child);
        }

        async Task FinishAfter(Func<Task> child)
        {
            try
            {
                await Task.Run(child);
            }
            finally
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    Info.Visible = false;
                }, null);
            }
        }
    }
}
