using System.Collections.Generic;

namespace Cardgame.Common
{
    public class Slot
    {
        public Slot(string key, int column, int row)
        {
            Key = key;
            Column = column;
            Row = row;
            Cards = new List<Card>();
        }

        public string Key { get; }
        public int Column { get; }
        public int Row { get; }
        public IList<Card> Cards { get; }
    }
}
