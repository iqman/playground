using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cardgame.Common.Rendering;

namespace Cardgame.App.Rendering
{
    class GameRenderer
    {
        private readonly IViewport viewport;
        private readonly IGameState gameState;
        private readonly FaceCache faceCache;
        private bool suspended;
        private readonly BufferedGraphicsContext context;
        private BufferedGraphics grafx;
        private Brush cardBackgroundBrush;
        private Pen cardEdgePen;
        private Pen slotPen;
        const int CardCornerRadius = 10;
        const int CardStackVerticalSpacing = 40;

        public GameRenderer(IViewport viewport, IGameState gameState, FaceCache faceCache)
        {
            this.viewport = viewport;
            this.gameState = gameState;
            this.faceCache = faceCache;

            viewport.ViewportUpdated += Viewport_OnViewportUpdated;
            gameState.StateUpdated += GameState_StateUpdated;

            InitalizeRendering();

            context = BufferedGraphicsManager.Current;
            RecreateRenderTarget();
        }

        private void GameState_StateUpdated(object sender, EventArgs e)
        {
            Render();
        }

        private void InitalizeRendering()
        {
            cardBackgroundBrush = Brushes.White;
            cardEdgePen = Pens.Black;
            slotPen = new Pen(Color.Black, 3);
        }

        private void Viewport_OnViewportUpdated(object sender, EventArgs e)
        {
            RecreateRenderTarget();
        }

        private void RecreateRenderTarget()
        {
            context.MaximumBuffer = new Size(viewport.Width + 1, viewport.Height + 1);

            grafx?.Dispose();

            grafx = context.Allocate(viewport.CreateGraphics(),
                new Rectangle(0, 0, viewport.Width, viewport.Height));

            Render();
        }

        public void RenderDrag(PointF position)
        {
            RenderInternal(position);
        }

        public void Render()
        {
            RenderInternal(PointF.Empty);
        }
        private void RenderInternal(PointF dragPosition)
        { 
            if(suspended)
            {
                return;
            }

            var g = grafx.Graphics;
            {
                g.Clear(Color.LightGray);
                RenderSlots(g);

                if (!dragPosition.IsEmpty)
                {
                    RenderSlot(g, gameState.CardsBeingDragged, dragPosition);
                }
            }

            grafx.Render(Graphics.FromHwnd(viewport.Handle));
        }

        private void RenderSlots(Graphics g)
        {
            var slots = gameState.GetSlots();
            foreach (var slot in slots)
            {
                var cardRect = CreateCardRect(slot.Position);
                g.DrawRoundedRectangle(slotPen, cardRect, CardCornerRadius);
                RenderSlot(g, slot.Cards, slot.Position);
            }
        }

        private void RenderSlot(Graphics g, IList<Card> cards, PointF position)
        {
            var thisCardPosition = position;

            foreach (var card in cards)
            {
                RenderSingleCard(g, card, thisCardPosition);
                thisCardPosition.Y += CardStackVerticalSpacing;
            }
        }

        private void RenderSingleCard(Graphics g, Card card, PointF position)
        {
            var cardRect = CreateCardRect(position);

            // Card faces are transparent. TODO: make cards have white background to avoid extra draw call
            g.FillRoundedRectangle(cardBackgroundBrush, cardRect, CardCornerRadius);
            g.DrawImage(faceCache.GetFace(card.Face), position);
            g.DrawRoundedRectangle(cardEdgePen, cardRect, CardCornerRadius);
        }

        private RectangleF CreateCardRect(PointF p)
        {
            return new RectangleF(p.X, p.Y, FaceCache.CardWidth, FaceCache.CardHeight);
        }

        public PointF GetCardCenterFromCardTopLeft(PointF cardTopLeft)
        {
            return new PointF(cardTopLeft.X + FaceCache.CardWidth / 2, cardTopLeft.Y + FaceCache.CardHeight / 2);
        }

        public RectangleF GetCardBounds(Card card)
        {
            var slots = gameState.GetSlots();
            var slotsContainingCard = slots.Where(slot => slot.Cards.Contains(card));

            if (!slotsContainingCard.Any())
            {
                return RectangleF.Empty;
            }

            var slotContainingCard = slotsContainingCard.Single();

            var slotPosition = slotContainingCard.Position;

            var p = new PointF(slotPosition.X, slotPosition.Y + CardStackVerticalSpacing * slotContainingCard.Cards.IndexOf(card));

            return CreateCardRect(p);
        }

        public RectangleF GetSlotBounds(string slotKey, int cardsCount)
        {
            var p = gameState.GetSlots().Single(s => s.Key == slotKey).Position;
            if (p == PointF.Empty)
            {
                return RectangleF.Empty;
            }

            var baseRect = CreateCardRect(p);
            baseRect.Height += cardsCount * CardStackVerticalSpacing;

            return baseRect;
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
