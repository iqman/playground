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
        private IDictionary<Card, PointF> cards = new Dictionary<Card, PointF>();

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
    }
}
