using System;
namespace TokyoChokoku.Patmark
{
    public static class KeyValueStoreFactoryHolder
    {
        public static IKeyValueStoreFactory Instance { get; private set; }

        public static void Inject(IKeyValueStoreFactory impl){
            Instance = impl;
        }
    }
}
