using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.Rendering
{
    class GameRenderer
    {
        private readonly IRenderTargetHolder renderTargetHolder;
        private readonly IGameState gameState;
        private readonly FaceCache faceCache;
        private Image renderTarget;

        public GameRenderer(IRenderTargetHolder renderTargetHolder, IGameState gameState, FaceCache faceCache)
        {
            this.renderTargetHolder = renderTargetHolder;
            this.gameState = gameState;
            this.faceCache = faceCache;

            this.renderTarget = new Bitmap(200, 200);

            this.renderTargetHolder.Target = renderTarget;
        }

        public void Render()
        {
            var a = gameState.GetCardPositions();

            using (var g = Graphics.FromImage(renderTargetHolder.Target))
            {
                g.DrawImage(faceCache.GetFace(a.card), a.point);
            }
             
        }
    }
}
