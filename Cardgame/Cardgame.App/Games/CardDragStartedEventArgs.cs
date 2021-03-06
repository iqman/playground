﻿using Cardgame.Common;

namespace Cardgame.App.Games
{
    class CardDragStartedEventArgs
    {
        public CardDragStartedEventArgs(Card card, string sourceSlotKey)
        {
            Card = card;
            SourceSlotKey = sourceSlotKey;
        }

        public Card Card { get; set; }
        public string SourceSlotKey { get; set; }

        public bool IsLegal { get; set; }
    }
}
