using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;

namespace Cardgame.App.GameLogic
{
    class SimpleGameState : IGameState
    {
        private readonly IDictionary<Card, PointF> cards = new Dictionary<Card, PointF>();
        private readonly IDictionary<string, PointF> slots = new Dictionary<string, PointF>();
        
        public event EventHandler StateUpdated;
        protected void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void PlaceCard(Card card, PointF position)
        {
            cards[card] = position;
            OnStateUpdated();
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
            OnStateUpdated();
        }

        public IDictionary<Card, PointF> GetCards()
        {
            return cards;
        }

        public IDictionary<string, PointF> GetSlots()
        {
            return slots;
        }

        public PointF GetCardPosition(Card card)
        {
            return cards.ContainsKey(card) ? cards[card] : PointF.Empty;
        }

        public PointF GetSlotPosition(string slotKey)
        {
            return slots.ContainsKey(slotKey) ? slots[slotKey] : PointF.Empty;
        }

        public void PlaceSlot(string key, PointF position)
        {
            slots.Add(key, position);
            OnStateUpdated();
        }
    }
}
