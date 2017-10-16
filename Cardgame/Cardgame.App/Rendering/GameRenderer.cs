using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common.Rendering;
using System.Drawing.Imaging;

namespace Cardgame.App.Rendering
{
    class GameRenderer
    {
        private readonly IViewport viewport;
        private readonly IGameState gameState;
        private readonly FaceCache faceCache;
        private Image renderImage;
        private Brush cardEdgeBrush;
        private Brush cardBackgroundBrush;
        private Pen cardEdgePen;
        const int CardCornerRadius = 10;

        public GameRenderer(IViewport viewport, IGameState gameState, FaceCache faceCache)
        {
            this.viewport = viewport;
            this.gameState = gameState;
            this.faceCache = faceCache;

            viewport.ViewportUpdated += Viewport_OnViewportUpdated;
            gameState.StateUpdated += GameState_StateUpdated;

            InitalizeRendering();

            RecreateRenderTarget(viewport.Width, viewport.Height);
        }

        private void GameState_StateUpdated(object sender, EventArgs e)
        {
            Render();
        }

        private void InitalizeRendering()
        {
            cardEdgeBrush = Brushes.Black;
            cardBackgroundBrush = Brushes.White;
            cardEdgePen = Pens.Black;
        }

        private void Viewport_OnViewportUpdated(object sender, ViewportUpdatedEventArgs e)
        {
            RecreateRenderTarget(e.Width, e.Height);
        }

        private void RecreateRenderTarget(int width, int height)
        {
            var oldImage = renderImage;

            using (Graphics g = Graphics.FromImage(faceCache.GetFace(Card.Clubs1)))
            {
                // create the new bitmap using the resolution of one of the cards
                renderImage = new Bitmap(viewport.Width, viewport.Height, g);
            }

            this.viewport.SetImage(renderImage);

            if (oldImage != null)
            {
                oldImage.Dispose();
            }

            Render();
        }

        public void Render()
        {
            var placedCards = gameState.GetCards();

            using (var g = Graphics.FromImage(renderImage))
            {
                foreach (var placedCard in placedCards)
                {
                    var card = placedCard.Key;
                    var position = Point.Round(placedCard.Value);

                    var cardRect = CreateCardRect(position);

                    g.FillRoundedRectangle(cardBackgroundBrush, cardRect, CardCornerRadius);
                    g.DrawImageUnscaled(faceCache.GetFace(card), position);
                    g.DrawRoundedRectangle(cardEdgePen, cardRect, CardCornerRadius);
                }
            }

            viewport.Invalidate();
        }

        private RectangleF CreateCardRect(PointF p)
        {
            return new RectangleF(p.X, p.Y, FaceCache.CardWidth, FaceCache.CardHeight);
        }
    }
}
