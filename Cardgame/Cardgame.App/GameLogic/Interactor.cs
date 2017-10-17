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
                var correctedPosition = new PointF(e.Location.X - CardBeingDragged.Value.offset.X, e.Location.Y - CardBeingDragged.Value.offset.Y);
                renderer.RenderCardDrag(CardBeingDragged.Value.card, correctedPosition);
            }
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            CardBeingDragged = null;
        }

        private void MouseInputProxy_ViewportMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            CardBeingDragged = GetClickedCard(e.Location);
        }

        private (Card card, PointF offset)? GetClickedCard(PointF position)
        {
            var cards = gameState.GetCards();

            foreach (var card in cards)
            {
                var bounds = renderer.GetCardBounds(card.Key);
                if (bounds.Contains(position))
                {
                    var offset = new PointF(position.X-bounds.Location.X, position.Y -bounds.Location.Y);
                    return (card.Key, offset);
                }
            }

            return null;
        }
    }
}
