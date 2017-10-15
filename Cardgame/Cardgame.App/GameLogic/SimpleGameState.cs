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
            return (RandomCard(), RandomPosition());
            //return (Card.Clubs11, new PointF(100, 100));
        }

        private PointF RandomPosition()
        {
            var r = new Random();

            return new PointF(r.Next(200), r.Next(200));
        }

        private Card RandomCard()
        {
            var possibleCardNames = Enum.GetNames(typeof(Card));
            var r = new Random();
            var next = r.Next(possibleCardNames.Length);

            return (Card)Enum.Parse(typeof(Card), possibleCardNames[next]);
        }
    }
}
