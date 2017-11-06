using Cardgame.App.Rendering;
using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class SimpleGameController : IGameController
    {
        private readonly IGameState gameState;
        private readonly IInteractor interactor;
        private readonly GameRenderer renderer;

        public event EventHandler<MeasurementCompleteEventArgs> MeasurementComplete;
        protected void OnMeasurementComplete(MeasurementCompleteEventArgs args)
        {
            MeasurementComplete?.Invoke(this, args);
        }

        public SimpleGameController(IGameState gameState, IInteractor interactor, GameRenderer renderer)
        {
            this.gameState = gameState;
            this.interactor = interactor;
            this.renderer = renderer;

            interactor.CardDragStopped += Interactor_CardDragStopped;
        }

        private void Interactor_CardDragStopped(object sender, CardDragStoppedEventArgs e)
        {
            if (e.TargetSlotKey != null)
            {
                gameState.RemoveCard(e.Card);
                gameState.PlaceCard(e.Card, e.TargetSlotKey);
            }
        }

        public void Start()
        {
            var sw = new Stopwatch();

            sw.Start();

            renderer.Suspend();
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap1), new PointF(30, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap2), new PointF(260, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap3), new PointF(490, 30));
            gameState.CreateSlot(GetSlotKey(SimpleSlot.Swap4), new PointF(720, 30));

            var slots = new List<SimpleSlot>
            {
                SimpleSlot.Swap1,
                SimpleSlot.Swap2,
                SimpleSlot.Swap3,
                SimpleSlot.Swap4
            };
            var slotIndex = 0;
            var allCards = GetAllCards(true);
            foreach (var card in allCards)
            {
                gameState.PlaceCard(card, GetSlotKey(slots[slotIndex++ % slots.Count]));
            }

            renderer.Resume();

            sw.Stop();

            OnMeasurementComplete(new MeasurementCompleteEventArgs(sw.ElapsedMilliseconds));
        }

        Random r = new Random();

        private Card RandomCard()
        {
            var possibleCardNames = Enum.GetNames(typeof(Card));
            var next = r.Next(possibleCardNames.Length);

            return (Card)Enum.Parse(typeof(Card), possibleCardNames[next]);
        }

        private string GetSlotKey(SimpleSlot slot)
        {
            return Enum.GetName(typeof(SimpleSlot), slot);
        }

        private SimpleSlot GetSlot(string key)
        {
            return (SimpleSlot)Enum.Parse(typeof(SimpleSlot), key);
        }


        private IEnumerable<Card> GetAllCards(bool randomize)
        {
            var allCards = Enum.GetNames(typeof(Card)).Select(cn => (Card)Enum.Parse(typeof(Card), cn)).ToArray();

            if (randomize)
            {
                DoInPlaceRandomization(allCards);
            }

            return allCards;
        }

        private void DoInPlaceRandomization<T>(IList<T> array)
        {
            var l = array.Count;

            var random = new Random();
            // While there remain elements to shuffle…
            while (l > 0)
            {
                // Pick a remaining element…
                var i = (int)(random.NextDouble() * l--);

                // And swap it with the current element.
                var t = array[l];
                array[l] = array[i];
                array[i] = t;
            }
        }
    }
}
