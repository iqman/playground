﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cardgame.App
{
    interface IInteractor
    {
        event EventHandler<CardDragStartedEventArgs> CardDragStarted;
        event EventHandler<CardDragStoppedEventArgs> CardDragStopped;
     //   event EventHandler<DragUpdatedEventArgs> DragUpdated;
    }
}
