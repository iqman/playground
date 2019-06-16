using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cardgame.Common;

namespace Cardgame.App.GameLogic
{
    class SimpleGameState : IGameState
    {
        private readonly IDictionary<string, Slot> slots = new Dictionary<string, Slot>();

        public IList<Card> CardsBeingDragged { get; } = new List<Card>();

        public event EventHandler StateUpdated;
        protected void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void MoveSlot(string slotKey, PointF newPosition)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            slots[slotKey].Position = newPosition;
            OnStateUpdated();
        }

        public void PlaceCard(string slotKey, Card card)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            slots[slotKey].Cards.Add(card);
            OnStateUpdated();
        }

        public void RemoveCard(Card card)
        {
            var slotsContaining = slots.Where(slot => slot.Value.Cards.Contains(card));

            foreach (var slot in slotsContaining)
            {
                slot.Value.Cards.Remove(card);
            }
            OnStateUpdated();
        }

        public IList<Slot> GetSlots()
        {
            return slots.Values.ToList();
        }

        public IList<Card> GetCards(string slotKey)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            return slots[slotKey].Cards.ToList();
        }
        
        public void CreateSlot(string key, PointF position)
        {
            slots.Add(key, new Slot(key, position));
            OnStateUpdated();
        }

        public void MoveCardsToSlot(IList<Card> cards, string slotKey)
        {
            foreach (var card in cards)
            {
                RemoveCard(card);
                PlaceCard(slotKey, card);
            }
        }

        public void MoveToDragSlot(IList<Card> cards)
        {
            foreach (var card in cards)
            {
                RemoveCard(card);
                CardsBeingDragged.Add(card);
            }
            OnStateUpdated();
        }

        public void MoveDraggedCardsToSlot(string slotKey)
        {
            foreach (var card in CardsBeingDragged)
            {
                PlaceCard(slotKey, card);
            }
            CardsBeingDragged.Clear();
        }
    }
}
