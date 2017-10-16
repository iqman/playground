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
            
        }
    }
}
