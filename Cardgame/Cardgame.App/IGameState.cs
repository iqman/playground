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
        IDictionary<Card, PointF> GetCards();
        void PlaceCard(Card card, PointF position);
        void RemoveCard(Card card);
        event EventHandler StateUpdated;
    }
}
