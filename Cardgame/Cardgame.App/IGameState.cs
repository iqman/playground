using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;
using System.Drawing;

namespace Cardgame.App
{
    interface IGameState
    {
        PointF GetCardPosition(Card card);
        IDictionary<Card, PointF> GetCards();
        IList<PointF> GetSlots();
        void PlaceSlot(PointF position);
        void PlaceCard(Card card, PointF position);
        void RemoveCard(Card card);
        event EventHandler StateUpdated;
        void Suspend();
        void Resume();
    }
}
