using System;
using System.Collections.Generic;
using Cardgame.Common;
using System.Drawing;

namespace Cardgame.App
{
    interface IGameState
    {
        IList<Card> GetCards(string slotKey);
        IList<Slot> GetSlots();
        void CreateSlot(string key, PointF position);
        void MoveSlot(string slotKey, PointF newPosition);
        void PlaceCard(string slotKey, Card card);
        void RemoveCard(Card card);
        void MoveCardsToSlot(IList<Card> cards, string slotKey);
        IList<Card> CardsBeingDragged { get; }

        event EventHandler StateUpdated;

        void MoveToDragSlot(IList<Card> cards);
        void MoveDraggedCardsToSlot(string slotKey);
    }
}
