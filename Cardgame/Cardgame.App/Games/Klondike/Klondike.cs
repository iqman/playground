using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

       // private readonly string[] AllSlots = { Stack, Goal1, Goal2, Goal3, Goal4, Spot1, Spot2, Spot3, Spot4, Spot5, Spot6, Spot7 };

        private readonly IGameState gameState;
        private readonly IInteractor interactor;

        public Klondike(IGameState gameState, IInteractor interactor)
        {
            this.gameState = gameState;
            this.interactor = interactor;

            interactor.CardDragStarted += Interactor_CardDragStarted;
            interactor.CardDragStopped += Interactor_CardDragStopped;
        }

        private void Interactor_CardDragStarted(object sender, CardDragStartedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Interactor_CardDragStopped(object sender, CardDragStoppedEventArgs e)
        {

        }

        public void Start()
        {
            gameState.InitializeBoard(new BoardConfiguration(7, 2));

            gameState.CreateSlot(Stack1, 0, 0);
            gameState.CreateSlot(Stack2, 1, 0);
            gameState.CreateSlot(Goal1, 3, 0);
            gameState.CreateSlot(Goal2, 4, 0);
            gameState.CreateSlot(Goal3, 5, 0);
            gameState.CreateSlot(Goal4, 6, 0);
            gameState.CreateSlot(Spot1, 0, 1);
            gameState.CreateSlot(Spot2, 1, 1);
            gameState.CreateSlot(Spot3, 2, 1);
            gameState.CreateSlot(Spot4, 3, 1);
            gameState.CreateSlot(Spot5, 4, 1);
            gameState.CreateSlot(Spot6, 5, 1);
            gameState.CreateSlot(Spot7, 6, 1);
        }
    }
}
