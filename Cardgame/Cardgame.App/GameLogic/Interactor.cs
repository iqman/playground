using Cardgame.App.Rendering;
using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Cardgame.App.GameLogic
{
    class Interactor : IInteractor
    {
        private readonly IMouseInputProxy mouseInputProxy;
        private readonly IGameState gameState;
        private readonly GameRenderer renderer;

        private CardDragInfo cardDragInfo;

        public event EventHandler<CardDragStartedEventArgs> CardDragStarted;
        protected void OnCardDragStarted(CardDragStartedEventArgs args)
        {
            CardDragStarted?.Invoke(this, args);
        }

        public event EventHandler<CardDragStoppedEventArgs> CardDragStopped;
        protected void OnCardDragStopped(CardDragStoppedEventArgs args)
        {
            CardDragStopped?.Invoke(this, args);
        }

        public Interactor(IMouseInputProxy mouseInputProxy, IGameState gameState, GameRenderer renderer)
        {
            this.mouseInputProxy = mouseInputProxy;
            this.gameState = gameState;
            this.renderer = renderer;
            this.mouseInputProxy.ViewportMouseDown += MouseInputProxy_ViewportMouseDown;
            this.mouseInputProxy.ViewportMouseUp += MouseInputProxy_ViewportMouseUp;
            this.mouseInputProxy.ViewportMouseMove += MouseInputProxy_ViewportMouseMove;
            this.mouseInputProxy.ViewportMouseLeave += MouseInputProxy_ViewportMouseLeave;
        }

        private void MouseInputProxy_ViewportMouseLeave(object sender, EventArgs e)
        {
            if (IsDragging())
            {
                StopDrag(null);
            }
        }

        private void MouseInputProxy_ViewportMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(IsDragging())
            {
                UpdateDragPosition(e.Location);
            }
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            StopCardDrag(e.Location);
        }

        private void UpdateDragPosition(Point p)
        {
            if (IsDragging())
            {
                var cardTopLeft = new PointF(p.X - cardDragInfo.Offset.X, p.Y - cardDragInfo.Offset.Y);
                renderer.RenderDrag(cardTopLeft);
            }
        }

        private void StopCardDrag(Point location)
        {
            if (IsDragging())
            {
                var cardTopLeft = new PointF(location.X - cardDragInfo.Offset.X, location.Y - cardDragInfo.Offset.Y);
                var draggedCardCenter = renderer.GetCardCenterFromCardTopLeft(cardTopLeft);
                var targetSlotKey = GetTargetSlotKey(draggedCardCenter);

                StopDrag(targetSlotKey);
            }
        }

        private void MouseInputProxy_ViewportMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            StartDrag(e.Location);
            UpdateDragPosition(e.Location);
        }

        private void StopDrag(string targetSlotKey)
        {
            OnCardDragStopped(new CardDragStoppedEventArgs(targetSlotKey));
            cardDragInfo = null;
        }

        private bool IsDragging()
        {
            return cardDragInfo != null;
        }

        private void StartDrag(PointF position)
        {
            var slots = gameState.GetSlots();

            foreach (var slot in slots)
            {
                // reverse to check last cards (the ones on the top) before the ones below
                // as the rendering may draw them partially on top of each other
                var cards = slot.Cards.Reverse();

                foreach (var card in cards)
                {
                    var bounds = renderer.GetCardBounds(card);
                    if (bounds.Contains(position))
                    {
                        var args = new CardDragStartedEventArgs(card, slot.Key);
                        OnCardDragStarted(args);

                        if (args.IsLegal)
                        {
                            cardDragInfo = new CardDragInfo
                            {
                                Offset = new PointF(position.X - bounds.Location.X, position.Y - bounds.Location.Y)
                            };
                        }

                        return;
                    }
                }
            }
        }

        private string GetTargetSlotKey(PointF position)
        {
            var slots = gameState.GetSlots();

            foreach (var slot in slots)
            {
                var bounds = renderer.GetSlotBounds(slot.Key, slot.Cards.Count);
                if (bounds.Contains(position))
                {
                    return slot.Key;
                }
            }

            return null;
        }

        private class CardDragInfo
        {
            internal PointF Offset { get; set; }
        }
    }
}
