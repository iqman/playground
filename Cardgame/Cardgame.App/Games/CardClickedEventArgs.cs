using Cardgame.Common;

namespace Cardgame.App.Games
{
    class CardClickedEventArgs
    {
        public Card Card { get; }
        public string SourceSlotId { get; }

        public CardClickedEventArgs(Card card, string sourceSlotId)
        {
            Card = card;
            SourceSlotId = sourceSlotId;
        }
    }
}