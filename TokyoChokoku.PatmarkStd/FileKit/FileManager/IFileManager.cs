using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Monad;

namespace TokyoChokoku.Patmark
{

    public interface IFileManager
    {
        FileContext Loaded { get; }
    }
}

