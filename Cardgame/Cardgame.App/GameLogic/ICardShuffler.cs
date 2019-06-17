using System.Collections.Generic;
using Cardgame.Common;

namespace Cardgame.App.GameLogic
{
    public interface ICardShuffler
    {
        IEnumerable<Card> GenerateDeck(bool randomize, int jokerCount = 0);
    }
}