using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    interface IViewport
    {
        int Width { get; }
        int Height { get; }
        Graphics CreateGraphics();
        IntPtr Handle { get; }
        event EventHandler ViewportUpdated;
    }
}
