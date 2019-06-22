using System;
using System.Collections.Generic;
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
        private readonly ISharedGameLogic sharedGameLogic;
        private string dragSourceSlotKey;

        public Simple(IGameState gameState, IInteractor interactor, ICardShuffler shuffler, GameRenderer renderer, ISharedGameLogic sharedGameLogic)
        {
            this.gameState = gameState;
            this.interactor = interactor;
            this.shuffler = shuffler;
            this.renderer = renderer;
            this.sharedGameLogic = sharedGameLogic;

            interactor.CardDragStarted += Interactor_CardDragStarted;
            interactor.CardDragStopped += Interactor_CardDragStopped;
        }

        private void Interactor_CardDragStarted(object sender, CardDragStartedEventArgs e)
        {
            if (IsDragLegal(e.Card, e.SourceSlotKey))
            {
                e.IsLegal = true;
                dragSourceSlotKey = e.SourceSlotKey;
                sharedGameLogic.DragCardAndFollowers(e.Card, e.SourceSlotKey);
            }
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
            gameState.InitializeBoard(new BoardConfiguration(4, 1));

            renderer.Suspend();
            gameState.CreateSlot(new Slot(GetSlotKey(SimpleSlot.Swap1), 0, 0));
            gameState.CreateSlot(new Slot(GetSlotKey(SimpleSlot.Swap2), 1, 0));
            gameState.CreateSlot(new Slot(GetSlotKey(SimpleSlot.Swap3), 2, 0));
            gameState.CreateSlot(new Slot(GetSlotKey(SimpleSlot.Swap4), 3, 0));

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
