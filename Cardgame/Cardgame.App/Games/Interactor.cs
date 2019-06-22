using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Cardgame.App.Rendering;
using Cardgame.Common;

namespace Cardgame.App.Games
{
    class Interactor : IInteractor
    {
        private readonly IMouseInputProxy mouseInputProxy;
        private readonly IGameState gameState;
        private readonly GameRenderer renderer;

        private CardDragInfo cardDragInfo = new CardDragInfo();
        private Point dragThreshold;

        const int SM_CXDRAG = 68;
        const int SM_CYDRAG = 69;
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int index);

        public event EventHandler<CardClickedEventArgs> CardClicked;
        protected void OnCardClicked(CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, e);
        }

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

            dragThreshold = new Point(GetSystemMetrics(SM_CXDRAG), GetSystemMetrics(SM_CYDRAG));
        }

        private void MouseClickRegistered(Point p)
        {
            StopDrag(null);

            Card foundCard = null;
            Slot foundSlot = null;

            if (DetermineCardAtPosition(p, ref foundCard, ref foundSlot))
            {
                OnCardClicked(new CardClickedEventArgs(foundCard, foundSlot.Key));
            }
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
            if (cardDragInfo.IsMouseDownState && !IsDragging() &&
                !IsWithinDragThreshold(cardDragInfo.MouseDownPosition, e.Location))
            {
                StartDrag(e.Location);
            }

            if(IsDragging())
            {
                UpdateDragPosition(e.Location);
            }
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsWithinDragThreshold(cardDragInfo.MouseDownPosition, e.Location))
            {
                MouseClickRegistered(e.Location);
            }
            else
            {
                StopCardDrag(e.Location);
            }
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
            cardDragInfo.MouseDownPosition = e.Location;
            cardDragInfo.IsMouseDownState = true;
            //StartDrag(e.Location);
            //UpdateDragPosition(e.Location);
        }

        private void StopDrag(string targetSlotKey)
        {
            OnCardDragStopped(new CardDragStoppedEventArgs(targetSlotKey));
            cardDragInfo.IsDragging = false;
            cardDragInfo.IsMouseDownState = false;
        }

        private bool IsDragging()
        {
            return cardDragInfo.IsDragging;
        }

        private void StartDrag(PointF position)
        {
            Card foundCard = null;
            Slot foundSlot = null;

            if (DetermineCardAtPosition(position, ref foundCard, ref foundSlot))
            {
                var bounds = renderer.GetCardBounds(foundCard);

                var args = new CardDragStartedEventArgs(foundCard, foundSlot.Key);
                OnCardDragStarted(args);

                if (args.IsLegal)
                {
                    cardDragInfo.Offset = new PointF(position.X - bounds.Location.X, position.Y - bounds.Location.Y);
                    cardDragInfo.IsDragging = true;
                }
                else
                {
                    StopDrag(null);
                }
            }
        }

        private bool DetermineCardAtPosition(PointF position, ref Card foundCard, ref Slot foundSlot)
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
                        foundCard = card;
                        foundSlot = slot;
                        return true;
                    }
                }
            }

            return false;
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

        private bool IsWithinDragThreshold(Point a, Point b)
        {
            return a.X > b.X - dragThreshold.X && a.X < b.X + dragThreshold.X &&
                   a.Y > b.Y - dragThreshold.Y && a.Y < b.Y + dragThreshold.Y;
        }

        private class CardDragInfo
        {
            public Point MouseDownPosition { get; set; }
            public PointF Offset { get; set; }
            public bool IsDragging { get; set; }
            public bool IsMouseDownState { get; set; }
        }
    }
}
