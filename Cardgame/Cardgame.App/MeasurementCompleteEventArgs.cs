using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    class MeasurementCompleteEventArgs : EventArgs
    {
        public long Result { get; }

        public MeasurementCompleteEventArgs(long result)
        {
            Result = result;
        }
    }
}
