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
        (Card card, PointF point) GetCardPositions();
    }
}
