using System;
using System.Collections.Generic;
using System.Linq;
using Cardgame.Common;

namespace Cardgame.App.GameLogic
{
    public class CardShuffler : ICardShuffler
    {
        private readonly Random r = new Random();

        public Card GenerateCard()
        {
            var possibleCardNames = Enum.GetNames(typeof(Face));
            var next = r.Next(possibleCardNames.Length);

            var randomFace = (Face)Enum.Parse(typeof(Face), possibleCardNames[next]);

            return new Card(GenerateCardId(), randomFace, Side.Front);
        }

        public IEnumerable<Card> GenerateDeck(bool randomize, int jokerCount = 0)
        {
            var allFaces = Enum.GetNames(typeof(Face)).Select(cn => (Face)Enum.Parse(typeof(Face), cn)).ToArray();

            var deck = allFaces.Select(f => new Card(GenerateCardId(), f, Side.Front)).ToList();

            if (randomize)
            {
                DoInPlaceRandomization(deck);
            }

            return deck;
        }

        private string GenerateCardId()
        {
            return Guid.NewGuid().ToString();
        }

        private void DoInPlaceRandomization<T>(IList<T> array)
        {
            var l = array.Count;

            var random = new Random();
            // While there remain elements to shuffle…
            while (l > 0)
            {
                // Pick a remaining element…
                var i = (int)(random.NextDouble() * l--);

                // And swap it with the current element.
                var t = array[l];
                array[l] = array[i];
                array[i] = t;
            }
        }
    }
}
