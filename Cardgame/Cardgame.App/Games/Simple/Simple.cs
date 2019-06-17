using System;
using System.Collections.Generic;
using System.Drawing;
using Cardgame.App.Rendering;
using Cardgame.Common;

namespace Cardgame.App.Games.Simple
{
    class Simple : IGame
    {
        private readonly IGameState gameState;
        private readonly IInteractor interactor;
        private readonly ICardShuffler shuffler;
        private readonly GameRenderer renderer;
        private string dragSourceSlotKey;

        public Simple(IGameState gameState, IInteractor interactor, ICardShuffler shuffler, GameRenderer renderer)
        {
            this.gameState = gameState;
            this.interactor = interactor;
            this.shuffler = shuffler;
            this.renderer = renderer;

            interactor.CardDragStarted += Interactor_CardDragStarted;
            interactor.CardDragStopped += Interactor_CardDragStopped;
        }

        private void Interactor_CardDragStarted(object sender, CardDragStartedEventArgs e)
        {
            if (IsDragLegal(e.Card, e.SourceSlotKey))
            {
                e.IsLegal = true;
                dragSourceSlotKey = e.SourceSlotKey;
                DragCardAndFollowers(e.Card, e.SourceSlotKey);
            }
        }

        private void DragCardAndFollowers(Card cardInitiallyDragged, string sourceSlotKey)
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

        private bool IsDragLegal(Card card, string sourceSlotKey)
        {
            return true;
        }

        private void Interactor_CardDragStopped(object sender, CardDragStoppedEventArgs e)
        {
            if (e.TargetSlotKey == null)
            {
                gameState.MoveDraggedCardsToSlot(dragSourceSlotKey);
            }
            else
            {
                gameState.MoveDraggedCardsToSlot(e.TargetSlotKey);
            }

            dragSourceSlotKey = null;
        }

        public void Start()
        {
            renderer.Suspend();
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap1), new PointF(30, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap2), new PointF(260, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap3), new PointF(490, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap4), new PointF(720, 30));

            var slots = new List<SimpleSlot>
            {
                SimpleSlot.Swap1,
                SimpleSlot.Swap2,
                SimpleSlot.Swap3,
                SimpleSlot.Swap4
            };
            var slotIndex = 0;
            var deck = shuffler.GenerateDeck(true);
            foreach (var card in deck)
            {
                gameState.PlaceCard(GetSlotKey(slots[slotIndex++ % slots.Count]), card);
            }

            renderer.Resume();
        }

        private string GetSlotKey(SimpleSlot slot)
        {
            return Enum.GetName(typeof(SimpleSlot), slot);
        }
    }
}
