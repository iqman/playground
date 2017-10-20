using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;
using System.Drawing;
using Cardgame.App.GameLogic;

namespace Cardgame.App
{
    interface IGameState
    {
        SlotInfo GetSlot(string slotKey);
        IDictionary<string, SlotInfo> GetAllSlots();
        void CreateSlot(string key, PointF position);
        void PlaceCard(Card card, string slotKey);
        void RemoveCard(Card card);
        event EventHandler StateUpdated;
    }
}
