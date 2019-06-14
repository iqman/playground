using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.Common
{
    public class Card : IEquatable<Card>
    {
        public Card(string id, Face face)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Face = face;
        }

        public Face Face { get; set; }
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
