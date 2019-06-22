using System;
using System.Collections.Generic;
using Cardgame.Common;

namespace Cardgame.App.Games
{
    interface IGameState
    {
        IList<Card> GetCards(string slotKey);
        IList<Slot> GetSlots();
        void CreateSlot(Slot slot);
        void PlaceCard(string slotKey, Card card);
        void RemoveCard(Card card);
        void MoveCardsToSlot(IEnumerable<Card> cards, string slotKey);
        IList<Card> CardsBeingDragged { get; }
        BoardConfiguration BoardConfiguration { get; }
        bool IsInitialized { get; }

        void InitializeBoard(BoardConfiguration config);

        void UpdateCard(Card card);

        event EventHandler StateUpdated;
        event EventHandler<BoardConfigurationArgs> BoardConfigurationUpdated;

        void MoveToDragSlot(IEnumerable<Card> cards);
        void MoveDraggedCardsToSlot(string slotKey);

        MultiCardOperation MultiCardOperationScope();
        void CompleteScope();
    }
}
