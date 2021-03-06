﻿using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Cardgame.App.Games;
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
        private float cardHeight;
        private float cardWidth;
        const int CardCornerRadius = 10;
        const int CardStackVerticalSpacing = 40;
        private const int SlotSpacing = 10;

        public GameRenderer(IViewport viewport, IGameState gameState, FaceCache faceCache)
        {
            this.viewport = viewport;
            this.gameState = gameState;
            this.faceCache = faceCache;

            viewport.ViewportUpdated += Viewport_OnViewportUpdated;
            gameState.StateUpdated += GameState_StateUpdated;
            gameState.BoardConfigurationUpdated += GameState_BoardConfigurationUpdated;

            InitalizeRendering();

            context = BufferedGraphicsManager.Current;
            RecreateRenderTarget();
        }

        private void GameState_BoardConfigurationUpdated(object sender, BoardConfigurationArgs e)
        {
            CalculateCardSize(e.Config);
        }

        private void CalculateCardSize(BoardConfiguration config)
        {
            var maxWidthOfSlotAndSpacing = ((float)viewport.Width) / config.SlotColumns;
            var maxWidth = maxWidthOfSlotAndSpacing - SlotSpacing;

            var maxHeightOfSlotAndSpacing = (float) (viewport.Height) / config.SlotRows;
            var maxHeight = maxHeightOfSlotAndSpacing - SlotSpacing;

            var widthRatio = maxWidth / FaceCache.CardWidth;
            var heightRatio = maxHeight / FaceCache.CardHeight;

            if (widthRatio < heightRatio)
            {
                cardHeight = widthRatio * FaceCache.CardHeight;
                cardWidth = widthRatio * FaceCache.CardWidth;
            }
            else
            {
                cardHeight = heightRatio * FaceCache.CardHeight;
                cardWidth = heightRatio * FaceCache.CardWidth;
            }
        }

        private void GameState_StateUpdated(object sender, EventArgs e)
        {
            Render();
        }

        private void InitalizeRendering()
        {
            cardBackgroundBrush = Brushes.White;
            cardEdgePen = Pens.Black;
            slotPen = new Pen(Color.Black, 2);
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

            if (gameState.IsInitialized)
            {
                CalculateCardSize(gameState.BoardConfiguration);
                Render();
            }
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
                    RenderSlot(g, gameState.CardsBeingDragged, dragPosition, SlotStackingMode.AllCardsVisible);
                }
            }

            grafx.Render(Graphics.FromHwnd(viewport.Handle));
        }

        private void RenderSlots(Graphics g)
        {
            var slots = gameState.GetSlots();
            foreach (var slot in slots)
            {
                var cardRect = CreateCardRect(CalculcateSlotPosition(slot));
                g.DrawRoundedRectangle(slotPen, cardRect, CardCornerRadius);
                RenderSlot(g, slot.Cards, CalculcateSlotPosition(slot), slot.StackingMode);
            }
        }

        private void RenderSlot(Graphics g, IList<Card> cards, PointF position, SlotStackingMode stackingMode)
        {
            if (!cards.Any())
            {
                return;
            }

            var thisCardPosition = position;

            var cardsToRender = cards;

            if (stackingMode == SlotStackingMode.TopCardVisible)
            {
               cardsToRender = new List<Card> {cardsToRender.Last()};
            }

            foreach (var card in cardsToRender)
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
            if (card.Side == Side.Front)
            {
                g.DrawImage(faceCache.GetFace(card.Face), cardRect);
            }
            else
            {
                g.DrawImage(faceCache.GetBack(), cardRect);
            }
            g.DrawRoundedRectangle(cardEdgePen, cardRect, CardCornerRadius);
        }

        private PointF CalculcateSlotPosition(Slot slot)
        {
            return new PointF(SlotSpacing/2.0f + (cardWidth + SlotSpacing) * slot.Column,
                SlotSpacing / 2.0f + (cardHeight + SlotSpacing) * slot.Row);
        }

        private RectangleF CreateCardRect(PointF p)
        {
            return new RectangleF(p.X, p.Y, cardWidth, cardHeight);
        }

        public PointF GetCardCenterFromCardTopLeft(PointF cardTopLeft)
        {
            return new PointF(cardTopLeft.X + cardWidth / 2, cardTopLeft.Y + cardHeight / 2);
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

            var slotPosition = CalculcateSlotPosition(slotContainingCard);

            var cardVisualOffsetInSlot = slotContainingCard.StackingMode == SlotStackingMode.AllCardsVisible
                ? CardStackVerticalSpacing * slotContainingCard.Cards.IndexOf(card)
                : 0;

            var p = new PointF(slotPosition.X, slotPosition.Y + cardVisualOffsetInSlot);

            return CreateCardRect(p);
        }

        public RectangleF GetSlotBounds(string slotKey, int cardsCount)
        {
            var slot = gameState.GetSlots().Single(s => s.Key == slotKey);
            var p = CalculcateSlotPosition(slot);
            if (p == PointF.Empty)
            {
                return RectangleF.Empty;
            }

            var baseRect = CreateCardRect(p);
            if (slot.StackingMode == SlotStackingMode.AllCardsVisible)
            {
                baseRect.Height += cardsCount * CardStackVerticalSpacing;
            }

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
