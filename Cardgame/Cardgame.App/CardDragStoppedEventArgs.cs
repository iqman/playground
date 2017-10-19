using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    class CardDragStoppedEventArgs
    {
        public CardDragStoppedEventArgs(Card card, string targetSlotKey)
        {
            Card = card;
            TargetSlotKey = targetSlotKey;
        }

        public Card Card { get; }
        public string TargetSlotKey { get; }
    }
}
