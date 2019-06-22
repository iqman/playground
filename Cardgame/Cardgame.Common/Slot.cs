using System.Collections.Generic;

namespace Cardgame.Common
{
    public class Slot
    {
        public Slot(string key, int column, int row) : this(key, column, row,
            SlotStackingMode.AllCardsVisible)
        {
        }

        public Slot(string key, int column, int row, SlotStackingMode stackingMode)
        {
            Key = key;
            Column = column;
            Row = row;
            StackingMode = stackingMode;
            Cards = new List<Card>();
        }

        public string Key { get; }
        public int Column { get; }
        public int Row { get; }
        public SlotStackingMode StackingMode { get; }
        public IList<Card> Cards { get; }
    }
}
