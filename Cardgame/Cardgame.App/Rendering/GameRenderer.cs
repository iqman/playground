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
        private bool suspended = false;
        private Image renderImage;
        private Brush cardEdgeBrush;
        private Brush cardBackgroundBrush;
        private Pen cardEdgePen;
        private Pen slotPen;
        const int CardCornerRadius = 10;

        private (Card card, PointF p)? cardDragPositionOverride;

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
            slotPen = new Pen(Color.Black, 3);
        }

        private void Viewport_OnViewportUpdated(object sender, ViewportUpdatedEventArgs e)
        {
            RecreateRenderTarget(e.Width, e.Height);
        }

        private void RecreateRenderTarget(int width, int height)
        {
            var oldImage = renderImage;

            using (Graphics g = Graphics.FromImage(faceCache.GetSlot()))
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
            if(suspended)
            {
                return;
            }

            using (var g = Graphics.FromImage(renderImage))
            {
                g.Clear(Color.Transparent);
                RenderSlots(g);
                RenderCards(g);
            }

            viewport.Invalidate();
        }

        private void RenderSlots(Graphics g)
        {
            var slots = gameState.GetSlots().Values;
            foreach (var slot in slots)
            {
                var position = Point.Round(slot);
                var cardRect = CreateCardRect(position);
                g.DrawRoundedRectangle(slotPen, cardRect, CardCornerRadius);
              //  g.DrawImage(faceCache.GetSlot(), Point.Round(slot));
            }
        }

        private void RenderCards(Graphics g)
        {
            var placedCards = gameState.GetCards();

            foreach (var placedCard in placedCards)
            {
                var card = placedCard.Key;
                var position = Point.Round(placedCard.Value);

                if (cardDragPositionOverride.HasValue && cardDragPositionOverride.Value.card == card)
                {
                    position = Point.Round(cardDragPositionOverride.Value.p);
                }

                var cardRect = CreateCardRect(position);

                g.FillRoundedRectangle(cardBackgroundBrush, cardRect, CardCornerRadius);
                g.DrawImage(faceCache.GetFace(card), position);
                g.DrawRoundedRectangle(cardEdgePen, cardRect, CardCornerRadius);
            }
        }

        private RectangleF CreateCardRect(PointF p)
        {
            return new RectangleF(p.X, p.Y, FaceCache.CardWidth, FaceCache.CardHeight);
        }

        public RectangleF GetCardBounds(Card card)
        {
            var p = gameState.GetCardPosition(card);
            if (p == PointF.Empty)
            {
                return RectangleF.Empty;
            }

            if (cardDragPositionOverride.HasValue && cardDragPositionOverride.Value.card == card)
            {
                p = cardDragPositionOverride.Value.p;
            }

            return CreateCardRect(p);
        }

        public RectangleF GetSlotBounds(string slotKey)
        {
            var p = gameState.GetSlotPosition(slotKey);
            if (p == PointF.Empty)
            {
                return RectangleF.Empty;
            }

            return CreateCardRect(p);
        }

        public void RenderCardDrag(Card card, PointF p)
        {
            cardDragPositionOverride = (card, p);
            Render();
        }

        public void ClearCardDrag()
        {
            cardDragPositionOverride = null;
            Render();
        }

        public void Suspend()
        {
            suspended = true;
        }

        public void Resume()
        {
            suspended = false;
            Render();
        }
    }
}
