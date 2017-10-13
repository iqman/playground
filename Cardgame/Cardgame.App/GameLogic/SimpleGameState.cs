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
        public (Card card, PointF point) GetCardPositions()
        {
            return (Card.Clubs11, new PointF(100, 100));
        }
    }
}
