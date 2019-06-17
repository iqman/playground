using System;

namespace Cardgame.App.Games
{
    interface IInteractor
    {
        event EventHandler<CardDragStartedEventArgs> CardDragStarted;
        event EventHandler<CardDragStoppedEventArgs> CardDragStopped;
    }
}
