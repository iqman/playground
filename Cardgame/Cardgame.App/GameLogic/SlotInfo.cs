using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class SlotInfo
    {
        public SlotInfo(PointF position)
        {
            Position = position;
            Cards = new List<Card>();
        }

        public PointF Position { get; }
        public IList<Card> Cards { get; }
    }
}
