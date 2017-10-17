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
        private bool suspended = false;
        private readonly IList<PointF> slots = new List<PointF>();
        
        public event EventHandler StateUpdated;
        protected void OnStateUpdated()
        {
            if (!suspended)
            {
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
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

        public IList<PointF> GetSlots()
        {
            return slots;
        }

        public PointF GetCardPosition(Card card)
        {
            return cards.ContainsKey(card) ? cards[card] : PointF.Empty;
        }

        public void PlaceSlot(PointF position)
        {
            slots.Add(position);
            OnStateUpdated();
        }

        public void Suspend()
        {
            suspended = true;
        }

        public void Resume()
        {
            suspended = false;
            OnStateUpdated();
        }
    }
}
