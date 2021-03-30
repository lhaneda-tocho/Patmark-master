using System;
namespace TokyoChokoku.Patmark
{
    public interface IKeyValueStoreFactory
    {
        IKeyValueStore Create();
    }
}
