using System;
using System.Collections.Generic;

namespace Cardgame.Common
{
    public class Card : IEquatable<Card>
    {
        public Card(string id, Face face, Side side)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Face = face;
            Side = side;
        }

        public Face Face { get; set; }
        public Side Side { get; }
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        public bool Equals(Card other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Card card1, Card card2)
        {
            return EqualityComparer<Card>.Default.Equals(card1, card2);
        }

        public static bool operator !=(Card card1, Card card2)
        {
            return !(card1 == card2);
        }
    }
}
