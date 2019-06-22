using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cardgame.App
{
    interface IMouseInputProxy
    {
        event EventHandler<MouseEventArgs> ViewportMouseUp;
        event EventHandler<MouseEventArgs> ViewportMouseDown;
        event EventHandler<MouseEventArgs> ViewportMouseMove;
        event EventHandler ViewportMouseLeave;
    }
}
