using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App
{
    class ViewportUpdatedEventArgs : EventArgs
    {
        public int Width { get; }
        public int Height { get; }

        public ViewportUpdatedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
