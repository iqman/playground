namespace Cardgame.App.Games
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
