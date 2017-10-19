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
        PointF GetSlotPosition(string slotKey);
        IDictionary<Card, PointF> GetCards();
        IDictionary<string, PointF> GetSlots();
        void PlaceSlot(string key, PointF position);
        void PlaceCard(Card card, PointF position);
        void RemoveCard(Card card);
        event EventHandler StateUpdated;
    }
}
