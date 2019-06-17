using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.Games
{
    class MultiCardOperation : IDisposable
    {
        private readonly IGameState gameState;

        public MultiCardOperation(IGameState gameState)
        {
            this.gameState = gameState;
        }

        public void Dispose()
        {
            gameState.CompleteScope();
        }
    }
}
