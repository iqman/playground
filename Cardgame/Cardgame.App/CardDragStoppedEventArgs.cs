using Cardgame.Common;
using System.Collections.Generic;

namespace Cardgame.App
{
    class CardDragStoppedEventArgs
    {
        public CardDragStoppedEventArgs(IList<Card> cards, string targetSlotKey)
        {
            Cards = cards;
            TargetSlotKey = targetSlotKey;
        }

        public IList<Card> Cards { get; }
        public string TargetSlotKey { get; }
        public bool DragAccepted { get; set; }
    }
}
