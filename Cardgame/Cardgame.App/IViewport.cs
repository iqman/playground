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
        void SetImage(Image image);
        void Invalidate();
        event EventHandler ViewportUpdated;
    }
}
