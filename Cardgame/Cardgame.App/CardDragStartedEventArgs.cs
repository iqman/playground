using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    class CardDragStartedEventArgs
    {
        public CardDragStartedEventArgs(Card card)
        {
            Card = card;
        }

        public Card Card { get; }
    }
}
