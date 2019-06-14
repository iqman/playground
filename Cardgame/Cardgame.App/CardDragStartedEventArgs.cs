using Cardgame.Common;
using System.Collections.Generic;

namespace Cardgame.App
{
    class CardDragStartedEventArgs
    {
        public CardDragStartedEventArgs(IList<Card> cards, string sourceSlotKey)
        {
            Cards = cards;
            SourceSlotKey = sourceSlotKey;
        }

        public IList<Card> Cards { get; set; }
        public string SourceSlotKey { get; set; }
    }
}
