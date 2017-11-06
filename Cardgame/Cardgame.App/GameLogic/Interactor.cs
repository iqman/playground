using Cardgame.App.Rendering;
using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class Interactor : IInteractor
    {
        private readonly IMouseInputProxy mouseInputProxy;
        private readonly IGameState gameState;
        private readonly GameRenderer renderer;

        public (Card card, PointF offset)? CardBeingDragged { get; set; }

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
            mouseInputProxy.ViewportMouseDown += MouseInputProxy_ViewportMouseDown;
            mouseInputProxy.ViewportMouseUp += MouseInputProxy_ViewportMouseUp;
            mouseInputProxy.ViewportMouseMove += MouseInputProxy_ViewportMouseMove;
            mouseInputProxy.ViewportMouseLeave += MouseInputProxy_ViewportMouseLeave;
        }

        private void MouseInputProxy_ViewportMouseLeave(object sender, EventArgs e)
        {
            CardBeingDragged = null;
        }

        private void MouseInputProxy_ViewportMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(CardBeingDragged != null)
            {
                var cardTopLeft = new PointF(e.Location.X - CardBeingDragged.Value.offset.X, e.Location.Y - CardBeingDragged.Value.offset.Y);
                renderer.RenderCardDrag(CardBeingDragged.Value.card, cardTopLeft);
            }
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (CardBeingDragged != null)
            {
                var cardTopLeft = new PointF(e.Location.X - CardBeingDragged.Value.offset.X, e.Location.Y - CardBeingDragged.Value.offset.Y);
                var draggedCardCenter = renderer.OffsetToCardCenter(cardTopLeft);
                var targetSlotKey = GetTargetSlotKey(draggedCardCenter);
                OnCardDragStopped(new CardDragStoppedEventArgs(CardBeingDragged.Value.card, targetSlotKey));
            }
            CardBeingDragged = null;
            renderer.ClearCardDrag();
        }

        private void MouseInputProxy_ViewportMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            CardBeingDragged = GetClickedCard(e.Location);
            if (CardBeingDragged != null)
            {
                OnCardDragStarted(new CardDragStartedEventArgs(CardBeingDragged.Value.card));
            }
        }

        private (Card card, PointF offset)? GetClickedCard(PointF position)
        {
            var slots = gameState.GetAllSlots();

            foreach (var slot in slots)
            {
                // reverse to check last cards the ones on the top) before the ones below
                // as the rendering may draw them partially on top of each other
                var cards = slot.Value.Cards.Reverse();

                foreach (var card in cards)
                {
                    var bounds = renderer.GetCardBounds(card);
                    if (bounds.Contains(position))
                    {
                        var offset = new PointF(position.X - bounds.Location.X, position.Y - bounds.Location.Y);
                        return (card, offset);
                    }
                }
            }

            return null;
        }

        private string GetTargetSlotKey(PointF position)
        {
            var slots = gameState.GetAllSlots();

            foreach (var slot in slots)
            {
                var bounds = renderer.GetSlotBounds(slot.Key);
                if (bounds.Contains(position))
                {
                    return slot.Key;
                }
            }

            return null;
        }
    }
}
