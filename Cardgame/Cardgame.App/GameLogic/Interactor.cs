using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class Interactor : IInteractor
    {
        private readonly IMouseInputProxy mouseInputProxy;

        public Interactor(IMouseInputProxy mouseInputProxy)
        {
            this.mouseInputProxy = mouseInputProxy;
            mouseInputProxy.ViewportMouseDown += MouseInputProxy_ViewportMouseDown;
            mouseInputProxy.ViewportMouseUp += MouseInputProxy_ViewportMouseUp;
            mouseInputProxy.ViewportMouseMove += MouseInputProxy_ViewportMouseMove;
            mouseInputProxy.ViewportMouseLeave += MouseInputProxy_ViewportMouseLeave;
        }

        private void MouseInputProxy_ViewportMouseLeave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseInputProxy_ViewportMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseInputProxy_ViewportMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseInputProxy_ViewportMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
