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
        private readonly IDictionary<Face, string> cardToFaceResourceStringMap = new Dictionary<Face, string> {

            { Face.Diamonds1, "Cardgame.Common.faces.1_of_diamonds.png"},
            { Face.Diamonds2, "Cardgame.Common.faces.2_of_diamonds.png"},
            { Face.Diamonds3, "Cardgame.Common.faces.3_of_diamonds.png"},
            { Face.Diamonds4, "Cardgame.Common.faces.4_of_diamonds.png"},
            { Face.Diamonds5, "Cardgame.Common.faces.5_of_diamonds.png"},
            { Face.Diamonds6, "Cardgame.Common.faces.6_of_diamonds.png"},
            { Face.Diamonds7, "Cardgame.Common.faces.7_of_diamonds.png"},
            { Face.Diamonds8, "Cardgame.Common.faces.8_of_diamonds.png"},
            { Face.Diamonds9, "Cardgame.Common.faces.9_of_diamonds.png"},
            { Face.Diamonds10, "Cardgame.Common.faces.10_of_diamonds.png"},
            { Face.Diamonds11, "Cardgame.Common.faces.11_of_diamonds.png"},
            { Face.Diamonds12, "Cardgame.Common.faces.12_of_diamonds.png"},
            { Face.Diamonds13, "Cardgame.Common.faces.13_of_diamonds.png"},
            { Face.Spades1, "Cardgame.Common.faces.1_of_spades.png"},
            { Face.Spades2, "Cardgame.Common.faces.2_of_spades.png"},
            { Face.Spades3, "Cardgame.Common.faces.3_of_spades.png"},
            { Face.Spades4, "Cardgame.Common.faces.4_of_spades.png"},
            { Face.Spades5, "Cardgame.Common.faces.5_of_spades.png"},
            { Face.Spades6, "Cardgame.Common.faces.6_of_spades.png"},
            { Face.Spades7, "Cardgame.Common.faces.7_of_spades.png"},
            { Face.Spades8, "Cardgame.Common.faces.8_of_spades.png"},
            { Face.Spades9, "Cardgame.Common.faces.9_of_spades.png"},
            { Face.Spades10, "Cardgame.Common.faces.10_of_spades.png"},
            { Face.Spades11, "Cardgame.Common.faces.11_of_spades.png"},
            { Face.Spades12, "Cardgame.Common.faces.12_of_spades.png"},
            { Face.Spades13, "Cardgame.Common.faces.13_of_spades.png"},
            { Face.Hearts1, "Cardgame.Common.faces.1_of_hearts.png"},
            { Face.Hearts2, "Cardgame.Common.faces.2_of_hearts.png"},
            { Face.Hearts3, "Cardgame.Common.faces.3_of_hearts.png"},
            { Face.Hearts4, "Cardgame.Common.faces.4_of_hearts.png"},
            { Face.Hearts5, "Cardgame.Common.faces.5_of_hearts.png"},
            { Face.Hearts6, "Cardgame.Common.faces.6_of_hearts.png"},
            { Face.Hearts7, "Cardgame.Common.faces.7_of_hearts.png"},
            { Face.Hearts8, "Cardgame.Common.faces.8_of_hearts.png"},
            { Face.Hearts9, "Cardgame.Common.faces.9_of_hearts.png"},
            { Face.Hearts10, "Cardgame.Common.faces.10_of_hearts.png"},
            { Face.Hearts11, "Cardgame.Common.faces.11_of_hearts.png"},
            { Face.Hearts12, "Cardgame.Common.faces.12_of_hearts.png"},
            { Face.Hearts13, "Cardgame.Common.faces.13_of_hearts.png"},
            { Face.Clubs1, "Cardgame.Common.faces.1_of_clubs.png"},
            { Face.Clubs2, "Cardgame.Common.faces.2_of_clubs.png"},
            { Face.Clubs3, "Cardgame.Common.faces.3_of_clubs.png"},
            { Face.Clubs4, "Cardgame.Common.faces.4_of_clubs.png"},
            { Face.Clubs5, "Cardgame.Common.faces.5_of_clubs.png"},
            { Face.Clubs6, "Cardgame.Common.faces.6_of_clubs.png"},
            { Face.Clubs7, "Cardgame.Common.faces.7_of_clubs.png"},
            { Face.Clubs8, "Cardgame.Common.faces.8_of_clubs.png"},
            { Face.Clubs9, "Cardgame.Common.faces.9_of_clubs.png"},
            { Face.Clubs10, "Cardgame.Common.faces.10_of_clubs.png"},
            { Face.Clubs11, "Cardgame.Common.faces.11_of_clubs.png"},
            { Face.Clubs12, "Cardgame.Common.faces.12_of_clubs.png"},
            { Face.Clubs13, "Cardgame.Common.faces.13_of_clubs.png"},
            { Face.Joker, "Cardgame.Common.faces.1_of_joker.png" }
        };

        public const int CardWidth = 222;
        public const int CardHeight = 323;

        private readonly IDictionary<Face, Image> cardToFaceImageMap = new Dictionary<Face, Image>();
        private readonly Image slotImage;
        private readonly Image backImage;

        public FaceCache()
        {
            var cards = Enum.GetNames(typeof(Face));
            foreach (var cardName in cards)
            {
                Face face = (Face)Enum.Parse(typeof(Face), cardName);
                cardToFaceImageMap[face] = CacheFaceBitmap(face);
            }
            slotImage = CacheBitmap("Cardgame.Common.faces.slot.png");
            backImage = CacheBitmap("Cardgame.Common.faces.back.png");
        }

        public Image CacheFaceBitmap(Face face)
        {
            return CacheBitmap(cardToFaceResourceStringMap[face]);
        }

        public Image CacheBitmap(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return new Bitmap(stream);
        }

        public Image GetFace(Face face)
        {
           // return CacheFaceBitmap(face);
            return cardToFaceImageMap[face];
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
