using System;
namespace TokyoChokoku.Patmark.StorageUtil
{
    public static class StorageProvider
	{
        public static bool AcceptReinject { get; set; } = false;


        static Holder<FieldStorage> fieldStorage = new Holder<FieldStorage>();
        public static FieldStorage FieldStorage {
            get => fieldStorage.Instance;
            set => Inject(value, into: fieldStorage);
        }


        static void Inject<T>(T value, Holder<T> into) where T:class
        {
            if (AcceptReinject)
                into.Reinject(value);
            else
                into.Instance = value;
        }

        private class Holder<T> where T: class
		{
			static readonly Object theLock = new object();

            volatile T _Instance = null;
            public T Instance {
                get {
					if (_Instance == null)
						throw new NotSupportedException();
					else
						return _Instance;
                }
                set {
                    lock (theLock)
					{
						if (value == null)
							throw new NullReferenceException("Not allowed to inject null.");
						if (_Instance != null)
							throw new InvalidOperationException("Not allowed re-inject.");
						else
							_Instance = value;
					}
                }
            }

            public void Reinject(T value)
            {
				lock (theLock)
				{
					if (value == null)
						throw new NullReferenceException("Not allowed to inject null.");
                    _Instance = value;
				}
            }
        }
    }
}
