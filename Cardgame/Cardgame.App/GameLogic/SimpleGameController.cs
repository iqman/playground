using Cardgame.App.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class SimpleGameController : IGameController
    {
        private readonly GameRenderer renderer;

        public SimpleGameController(GameRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Start()
        {
            renderer.Render();
        }
    }
}
