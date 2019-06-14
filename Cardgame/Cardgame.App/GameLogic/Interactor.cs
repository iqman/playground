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
        private const string DragSlotKey = "InteractionDragSlot";

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

            this.gameState.CreateSlot(DragSlotKey, PointF.Empty);
        }

        private void MouseInputProxy_ViewportMouseLeave(object sender, EventArgs e)
        {
            if (IsDragging())
            {
                RevertDrag();
            }
        }

        private void MouseInputProxy_ViewportMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(IsDragging())
            {
                var cardTopLeft = new PointF(e.Location.X - cardDragInfo.Offset.X, e.Location.Y - cardDragInfo.Offset.Y);
                //  renderer.RenderCardDrag(CardBeingDragged.Value.face, cardTopLeft);
                gameState.MoveSlot(DragSlotKey, cardTopLeft);
            }
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            StopCardDrag(e.Location);
        }

        private void StopCardDrag(Point location)
        {
            if (IsDragging())
            {
                var cardTopLeft = new PointF(location.X - cardDragInfo.Offset.X, location.Y - cardDragInfo.Offset.Y);
                var draggedCardCenter = renderer.GetCardCenterFromCardTopLeft(cardTopLeft);
                var targetSlotKey = GetTargetSlotKey(draggedCardCenter);

                if (targetSlotKey != null)
                {
                    var args = new CardDragStoppedEventArgs(cardDragInfo.Cards, targetSlotKey);
                    OnCardDragStopped(args);

                    if (args.DragAccepted)
                    {
                        StopDrag();
                    }
                    else
                    {
                        RevertDrag();
                    }
                }
                else
                {
                    RevertDrag();
                }
            }
        }

        private void MouseInputProxy_ViewportMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            StartDrag(e.Location);
        }

        private void RevertDrag()
        {
            MoveCardBeingDraggedAndCardsOnTopOfIt(DragSlotKey, cardDragInfo.SourceSlotKey);
            StopDrag();
        }

        private void StopDrag()
        {
            cardDragInfo = null;
        }

        private bool IsDragging()
        {
            return cardDragInfo != null;
        }

        private void MoveCardBeingDraggedAndCardsOnTopOfIt(string fromSlot, string toSlot)
        {
            var cards = gameState.GetCards(fromSlot);

            var dragBottomCard = cardDragInfo.Cards.First();
            var foundCard = false;
            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card == dragBottomCard || foundCard)
                {
                    foundCard = true;
                    gameState.RemoveCard(card);
                    gameState.PlaceCard(toSlot, card);
                //    i--;
                }
            }
        }

        private void StartDrag(PointF position)
        {
            var slots = gameState.GetSlots();

            var foundCard = false;
            var cardsBeingDragged = new List<Card>();

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
                        foundCard = true;
                        cardDragInfo = new CardDragInfo
                        {
                            Offset = new PointF(position.X - bounds.Location.X, position.Y - bounds.Location.Y),
                            SourceSlotKey = slot.Key
                        };
                    }

                    if (foundCard)
                    {
                        cardsBeingDragged.Add(card);
                    }
                }
                if (foundCard)
                {
                    break;
                }
            }

            if (foundCard)
            {
                cardDragInfo.Cards = cardsBeingDragged;

                MoveCardBeingDraggedAndCardsOnTopOfIt(cardDragInfo.SourceSlotKey, DragSlotKey);
                OnCardDragStarted(new CardDragStartedEventArgs(cardDragInfo.Cards, cardDragInfo.SourceSlotKey));
            }
        }

        private string GetTargetSlotKey(PointF position)
        {
            var slots = gameState.GetSlots().Where(s => s.Key != DragSlotKey);

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

        private class CardDragInfo
        {
            internal PointF Offset { get; set; }

            public IList<Card> Cards { get; set; }
            public string SourceSlotKey { get; set; }
        }
    }
}
