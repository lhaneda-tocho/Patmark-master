using System;
using System.Collections.Generic;

namespace TokyoChokoku.Communication
{
    public sealed class Response : AbstractResponse
    {
        public Response (ReadResponse raw) : base(raw)
        {
        }
    }
}

