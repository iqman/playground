using System.Collections.Generic;
using System.Drawing;

namespace Cardgame.Common
{
    public class Slot
    {
        public Slot(string key, PointF position)
        {
            Key = key;
            Position = position;
            Cards = new List<Card>();
        }

        public string Key { get; }
        public PointF Position { get; set; }
        public IList<Card> Cards { get; }
    }
}
