using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;
using Ninject.Infrastructure.Language;

namespace Cardgame.App.Games.Klondike
{
    class Klondike : IGame
    {
        private const string Stack1 = "Stack1";
        private const string Stack2 = "Stack2";

        private const string Goal1 = "Goal1";
        private const string Goal2 = "Goal2";
        private const string Goal3 = "Goal3";
        private const string Goal4 = "Goal4";

        private const string Spot1 = "Spot1";
        private const string Spot2 = "Spot2";
        private const string Spot3 = "Spot3";
        private const string Spot4 = "Spot4";
        private const string Spot5 = "Spot5";
        private const string Spot6 = "Spot6";
        private const string Spot7 = "Spot7";

        // private readonly string[] allSlots = { Stack1, Stack2, Goal1, Goal2, Goal3, Goal4, Spot1, Spot2, Spot3, Spot4, Spot5, Spot6, Spot7 };
        private readonly string[] allSpotIds = { Spot1, Spot2, Spot3, Spot4, Spot5, Spot6, Spot7 };

        private readonly IGameState gameState;
        private readonly IInteractor interactor;
        private readonly ICardShuffler cardShuffler;
        private readonly ISharedGameLogic sharedGameLogic;
        private string dragSourceSlotKey;

        public Klondike(IGameState gameState, IInteractor interactor, ICardShuffler cardShuffler, ISharedGameLogic sharedGameLogic)
        {
            this.gameState = gameState;
            this.interactor = interactor;
            this.cardShuffler = cardShuffler;
            this.sharedGameLogic = sharedGameLogic;

            interactor.CardDragStarted += Interactor_CardDragStarted;
            interactor.CardDragStopped += Interactor_CardDragStopped;
            interactor.CardClicked += Interactor_CardClicked;
        }

        private void Interactor_CardClicked(object sender, CardClickedEventArgs e)
        {
            var isSpotSlot = allSpotIds.Contains(e.SourceSlotId);
            var cards = gameState.GetCards(e.SourceSlotId);

            if (isSpotSlot && cards.Last() == e.Card && e.Card.Side == Side.Back)
            {
                e.Card.Side = Side.Front;
                gameState.UpdateCard(e.Card);
            }
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
            return card.Side == Side.Front;
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
            gameState.InitializeBoard(new BoardConfiguration(7, 2));

            gameState.CreateSlot(new Slot(Stack1, 0, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Stack2, 1, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Goal1, 3, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Goal2, 4, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Goal3, 5, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Goal4, 6, 0, SlotStackingMode.TopCardVisible));
            gameState.CreateSlot(new Slot(Spot1, 0, 1));
            gameState.CreateSlot(new Slot(Spot2, 1, 1));
            gameState.CreateSlot(new Slot(Spot3, 2, 1));
            gameState.CreateSlot(new Slot(Spot4, 3, 1));
            gameState.CreateSlot(new Slot(Spot5, 4, 1));
            gameState.CreateSlot(new Slot(Spot6, 5, 1));
            gameState.CreateSlot(new Slot(Spot7, 6, 1));

            var deck = cardShuffler.GenerateDeck(true).ToList();
            deck.Map(c => c.Side = Side.Back);

            int spotNumber = 1;

            foreach (var spotId in allSpotIds)
            {
                var cardsForThisSpot = deck.Take(spotNumber).ToList();
                deck.RemoveRange(0, spotNumber);
                cardsForThisSpot.Last().Side = Side.Front;
                gameState.MoveCardsToSlot(cardsForThisSpot, spotId);
                spotNumber++;
            }

            gameState.MoveCardsToSlot(deck, Stack1);
        }
    }
}
