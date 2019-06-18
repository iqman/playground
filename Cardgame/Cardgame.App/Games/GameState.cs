using System;
using System.Collections.Generic;
using System.Linq;
using Cardgame.Common;

namespace Cardgame.App.Games
{
    class SimpleGameState : IGameState
    {
        private readonly IDictionary<string, Slot> slots = new Dictionary<string, Slot>();
        private bool multiCardOperationInProgress;
        public BoardConfiguration BoardConfiguration { get; private set; }
        public bool IsInitialized => BoardConfiguration != null;

        public IList<Card> CardsBeingDragged { get; } = new List<Card>();

        public event EventHandler StateUpdated;
        public event EventHandler<BoardConfigurationArgs> BoardConfigurationUpdated;

        protected void OnBoardConfigurationUpdated(BoardConfiguration config)
        {
            BoardConfigurationUpdated?.Invoke(this, new BoardConfigurationArgs(config));
        }

        protected void OnStateUpdated()
        {
            if (!multiCardOperationInProgress)
            {
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void InitializeBoard(BoardConfiguration config)
        {
            this.BoardConfiguration = config;
            OnBoardConfigurationUpdated(config);
        }

        public void PlaceCard(string slotKey, Card card)
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
            OnStateUpdated();
        }

        public IList<Slot> GetSlots()
        {
            return slots.Values.ToList();
        }

        public IList<Card> GetCards(string slotKey)
        {
            if (!slots.ContainsKey(slotKey))
            {
                throw new InvalidOperationException();
            }

            return slots[slotKey].Cards.ToList();
        }

        public void CreateSlot(string key, int column, int row)
        {
            slots.Add(key, new Slot(key, column, row));
            OnStateUpdated();
        }

        public void MoveCardsToSlot(IList<Card> cards, string slotKey)
        {
            using (MultiCardOperationScope())
            {
                foreach (var card in cards)
                {
                    RemoveCard(card);
                    PlaceCard(slotKey, card);
                }
            }
        }

        public void MoveToDragSlot(IList<Card> cards)
        {
            using (MultiCardOperationScope())
            {
                foreach (var card in cards)
                {
                    RemoveCard(card);
                    CardsBeingDragged.Add(card);
                }
                OnStateUpdated();
            }
        }

        public void MoveDraggedCardsToSlot(string slotKey)
        {
            using (MultiCardOperationScope())
            {
                foreach (var card in CardsBeingDragged)
                {
                    PlaceCard(slotKey, card);
                }
                CardsBeingDragged.Clear();
            }
        }

        public MultiCardOperation MultiCardOperationScope()
        {
            multiCardOperationInProgress = true;
            return new MultiCardOperation(this);
        }

        public void CompleteScope()
        {
            multiCardOperationInProgress = false;
            OnStateUpdated();
        }
    }
}
