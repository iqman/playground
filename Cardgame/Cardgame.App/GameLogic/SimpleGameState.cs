using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardgame.Common;

namespace Cardgame.App.GameLogic
{
    class SimpleGameState : IGameState
    {
        private readonly IDictionary<string, SlotInfo> slots = new Dictionary<string, SlotInfo>();
        
        public event EventHandler StateUpdated;
        protected void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void PlaceCard(Card card, string slotKey)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            slots[slotKey].Cards.Add(card);
            OnStateUpdated();
        }

        public void RemoveCard(Card card)
        {
            var slotsContaining = slots.Where(slot => slot.Value.Cards.Contains(card));

            foreach (var slot in slotsContaining)
            {
                slot.Value.Cards.Remove(card);
            }
        }

        public IDictionary<string, SlotInfo> GetAllSlots()
        {
            return slots;
        }

        public SlotInfo GetSlot(string slotKey)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            return slots[slotKey];
        }

        public void CreateSlot(string key, PointF position)
        {
            slots.Add(key, new SlotInfo(position));
            OnStateUpdated();
        }
    }
}
