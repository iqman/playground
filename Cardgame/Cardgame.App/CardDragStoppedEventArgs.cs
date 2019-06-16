using Cardgame.Common;
using System.Collections.Generic;

namespace Cardgame.App
{
    class CardDragStoppedEventArgs
    {
        public CardDragStoppedEventArgs(string targetSlotKey)
        {
            TargetSlotKey = targetSlotKey;
        }

        public string TargetSlotKey { get; }
    }
}
