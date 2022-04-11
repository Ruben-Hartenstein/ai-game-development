using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RelegatiaCCG.rccg.engine.exceptions
{
    class GameException : Exception
    {
        public GameException(string message) : base(message)
        {
        }
    }
}
