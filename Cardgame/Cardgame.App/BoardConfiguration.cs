using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    class BoardConfiguration
    {
        public int SlotColumns { get; }
        public int SlotRows { get; }

        public BoardConfiguration(int slotColumns, int slotRows)
        {
            SlotColumns = slotColumns;
            SlotRows = slotRows;
        }
    }
}
