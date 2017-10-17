using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.Common
{
    public class FaceCache
    {
        private IDictionary<Card, string> cardToFaceResourceStringMap = new Dictionary<Card, string>() {

            { Card.Diamonds1, "Cardgame.Common.faces.1_of_diamonds.png"},
            { Card.Diamonds2, "Cardgame.Common.faces.2_of_diamonds.png"},
            { Card.Diamonds3, "Cardgame.Common.faces.3_of_diamonds.png"},
            { Card.Diamonds4, "Cardgame.Common.faces.4_of_diamonds.png"},
            { Card.Diamonds5, "Cardgame.Common.faces.5_of_diamonds.png"},
            { Card.Diamonds6, "Cardgame.Common.faces.6_of_diamonds.png"},
            { Card.Diamonds7, "Cardgame.Common.faces.7_of_diamonds.png"},
            { Card.Diamonds8, "Cardgame.Common.faces.8_of_diamonds.png"},
            { Card.Diamonds9, "Cardgame.Common.faces.9_of_diamonds.png"},
            { Card.Diamonds10, "Cardgame.Common.faces.10_of_diamonds.png"},
            { Card.Diamonds11, "Cardgame.Common.faces.11_of_diamonds.png"},
            { Card.Diamonds12, "Cardgame.Common.faces.12_of_diamonds.png"},
            { Card.Diamonds13, "Cardgame.Common.faces.13_of_diamonds.png"},
            { Card.Spades1, "Cardgame.Common.faces.1_of_spades.png"},
            { Card.Spades2, "Cardgame.Common.faces.2_of_spades.png"},
            { Card.Spades3, "Cardgame.Common.faces.3_of_spades.png"},
            { Card.Spades4, "Cardgame.Common.faces.4_of_spades.png"},
            { Card.Spades5, "Cardgame.Common.faces.5_of_spades.png"},
            { Card.Spades6, "Cardgame.Common.faces.6_of_spades.png"},
            { Card.Spades7, "Cardgame.Common.faces.7_of_spades.png"},
            { Card.Spades8, "Cardgame.Common.faces.8_of_spades.png"},
            { Card.Spades9, "Cardgame.Common.faces.9_of_spades.png"},
            { Card.Spades10, "Cardgame.Common.faces.10_of_spades.png"},
            { Card.Spades11, "Cardgame.Common.faces.11_of_spades.png"},
            { Card.Spades12, "Cardgame.Common.faces.12_of_spades.png"},
            { Card.Spades13, "Cardgame.Common.faces.13_of_spades.png"},
            { Card.Hearts1, "Cardgame.Common.faces.1_of_hearts.png"},
            { Card.Hearts2, "Cardgame.Common.faces.2_of_hearts.png"},
            { Card.Hearts3, "Cardgame.Common.faces.3_of_hearts.png"},
            { Card.Hearts4, "Cardgame.Common.faces.4_of_hearts.png"},
            { Card.Hearts5, "Cardgame.Common.faces.5_of_hearts.png"},
            { Card.Hearts6, "Cardgame.Common.faces.6_of_hearts.png"},
            { Card.Hearts7, "Cardgame.Common.faces.7_of_hearts.png"},
            { Card.Hearts8, "Cardgame.Common.faces.8_of_hearts.png"},
            { Card.Hearts9, "Cardgame.Common.faces.9_of_hearts.png"},
            { Card.Hearts10, "Cardgame.Common.faces.10_of_hearts.png"},
            { Card.Hearts11, "Cardgame.Common.faces.11_of_hearts.png"},
            { Card.Hearts12, "Cardgame.Common.faces.12_of_hearts.png"},
            { Card.Hearts13, "Cardgame.Common.faces.13_of_hearts.png"},
            { Card.Clubs1, "Cardgame.Common.faces.1_of_clubs.png"},
            { Card.Clubs2, "Cardgame.Common.faces.2_of_clubs.png"},
            { Card.Clubs3, "Cardgame.Common.faces.3_of_clubs.png"},
            { Card.Clubs4, "Cardgame.Common.faces.4_of_clubs.png"},
            { Card.Clubs5, "Cardgame.Common.faces.5_of_clubs.png"},
            { Card.Clubs6, "Cardgame.Common.faces.6_of_clubs.png"},
            { Card.Clubs7, "Cardgame.Common.faces.7_of_clubs.png"},
            { Card.Clubs8, "Cardgame.Common.faces.8_of_clubs.png"},
            { Card.Clubs9, "Cardgame.Common.faces.9_of_clubs.png"},
            { Card.Clubs10, "Cardgame.Common.faces.10_of_clubs.png"},
            { Card.Clubs11, "Cardgame.Common.faces.11_of_clubs.png"},
            { Card.Clubs12, "Cardgame.Common.faces.12_of_clubs.png"},
            { Card.Clubs13, "Cardgame.Common.faces.13_of_clubs.png"},
            { Card.Joker, "Cardgame.Common.faces.1_of_joker.png" }
        };

        public const int CardWidth = 222;
        public const int CardHeight = 323;

        private IDictionary<Card, Image> cardToFaceImageMap = new Dictionary<Card, Image>();
        private Image slotImage;
        private Image backImage;

        public FaceCache()
        {
            var cards = Enum.GetNames(typeof(Card));
            foreach (var cardName in cards)
            {
                Card card = (Card)Enum.Parse(typeof(Card), cardName);
                cardToFaceImageMap[card] = CacheFaceBitmap(card);
            }
            slotImage = CacheBitmap("Cardgame.Common.faces.slot.png");
            backImage = CacheBitmap("Cardgame.Common.faces.back.png");
        }

        public Image CacheFaceBitmap(Card card)
        {
            return CacheBitmap(cardToFaceResourceStringMap[card]);
        }

        public Image CacheBitmap(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return new Bitmap(stream);
        }

        public Image GetFace(Card card)
        {
           // return CacheFaceBitmap(card);
            return cardToFaceImageMap[card];
        }

        public Image GetSlot()
        {
            return slotImage;
        }

        public Image GetBack()
        {
            return backImage;
        }
    }
}
