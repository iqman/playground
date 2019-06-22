using Cardgame.Common;

namespace Cardgame.App.Games
{
    interface ISharedGameLogic
    {
        void DragCardAndFollowers(Card cardInitiallyDragged, string sourceSlotKey);
    }
}