using System;

namespace Cardgame.App.Games
{
    interface IInteractor
    {
        event EventHandler<CardClickedEventArgs> CardClicked; 
        event EventHandler<CardDragStartedEventArgs> CardDragStarted;
        event EventHandler<CardDragStoppedEventArgs> CardDragStopped;
    }
}
