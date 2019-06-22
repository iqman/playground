using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;

namespace Cardgame.App.Games
{
    class SharedGameLogic : ISharedGameLogic
    {
        private readonly IGameState gameState;

        public SharedGameLogic(IGameState gameState)
        {
            this.gameState = gameState;
        }
        public void DragCardAndFollowers(Card cardInitiallyDragged, string sourceSlotKey)
        {
            var allDraggedCards = DetermineAllDraggedCards(cardInitiallyDragged, sourceSlotKey);
            gameState.MoveToDragSlot(allDraggedCards);
        }

        private IList<Card> DetermineAllDraggedCards(Card cardInitiallyDragged, string sourceSlotKey)
        {
            IList<Card> allCardsDragged = new List<Card>();
            var addRemainingSlot = false;
            var cards = gameState.GetCards(sourceSlotKey);

            foreach (var card in cards)
            {
                if (addRemainingSlot || card == cardInitiallyDragged)
                {
                    allCardsDragged.Add(card);
                    addRemainingSlot = true;
                }
            }
            return allCardsDragged;
        }
    }
}
