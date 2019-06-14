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
        private Image renderImage;
        private Brush cardBackgroundBrush;
        private Pen cardEdgePen;
        private Pen slotPen;
        const int CardCornerRadius = 10;
        const int CardStackVerticalSpacing = 40;

       // private (Face face, PointF p)? cardDragPositionOverride;

        public GameRenderer(IViewport viewport, IGameState gameState, FaceCache faceCache)
        {
            this.viewport = viewport;
            this.gameState = gameState;
            this.faceCache = faceCache;

            viewport.ViewportUpdated += Viewport_OnViewportUpdated;
            gameState.StateUpdated += GameState_StateUpdated;

            InitalizeRendering();

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
            var oldImage = renderImage;

            using (Graphics g = Graphics.FromImage(faceCache.GetSlot()))
            {
                // create the new bitmap using the resolution of one of the cards
                renderImage = new Bitmap(viewport.Width, viewport.Height, g);
            }

            viewport.SetImage(renderImage);

            oldImage?.Dispose();

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
            }

            viewport.Invalidate();
        }

        private void RenderSlots(Graphics g)
        {
            (Card draggedCard, PointF position)? foundDraggedCard = null;
            var slots = gameState.GetSlots();
            foreach (var slot in slots)
            {
                var position = Point.Round(slot.Position);
                var cardRect = CreateCardRect(position);
                g.DrawRoundedRectangle(slotPen, cardRect, CardCornerRadius);
                //  g.DrawImage(faceCache.GetSlot(), Point.Round(slot));

                var foundForCurrentSlot = RenderSlotCards(g, slot.Cards, position);
                foundDraggedCard = foundDraggedCard ?? foundForCurrentSlot;
            }

            if (foundDraggedCard != null)
            {
                RenderSingleCard(g, foundDraggedCard.Value.draggedCard, foundDraggedCard.Value.position);
            }
        }

        private (Card draggedCard, PointF position)? RenderSlotCards(Graphics g, IList<Card> cards, Point position)
        {
            (Card draggedCard, PointF position)? foundDraggedCard = null;

            foreach (var card in cards)
            {
                var thisCardPosition = position;
                //if (cardDragPositionOverride.HasValue && cardDragPositionOverride.Value.face == face)
                //{
                //    thisCardPosition = Point.Round(cardDragPositionOverride.Value.p);
                //    foundDraggedCard = (face, thisCardPosition);
                //}

                RenderSingleCard(g, card, thisCardPosition);

                position.Y += CardStackVerticalSpacing;
            }

            return foundDraggedCard;
        }

        private void RenderSingleCard(Graphics g, Card card, PointF position)
        {
            var cardRect = CreateCardRect(position);

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

            //if (cardDragPositionOverride.HasValue && cardDragPositionOverride.Value.face == face)
            //{
            //    p = cardDragPositionOverride.Value.p;
            //}

            return CreateCardRect(p);
        }

        public RectangleF GetSlotBounds(string slotKey)
        {
            var p = gameState.GetSlots().Single(s => s.Key == slotKey).Position;
            if (p == PointF.Empty)
            {
                return RectangleF.Empty;
            }

            return CreateCardRect(p);
        }

        //public void RenderCardDrag(Face face, PointF p)
        //{
        //    cardDragPositionOverride = (face, p);
        //    Render();
        //}

        //public void ClearCardDrag()
        //{
        //    cardDragPositionOverride = null;
        //    Render();
        //}

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
