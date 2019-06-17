using System.Collections.Generic;

namespace Cardgame.Common
{
    public interface ICardShuffler
    {
        IEnumerable<Card> GenerateDeck(bool randomize, int jokerCount = 0);
    }
}