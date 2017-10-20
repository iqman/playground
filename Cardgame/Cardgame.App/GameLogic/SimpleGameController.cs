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

            gameState.PlaceCard(Card.Diamonds11, GetSlotKey(SimpleSlot.Swap1));
            gameState.PlaceCard(Card.Diamonds7, GetSlotKey(SimpleSlot.Swap1));
            gameState.PlaceCard(Card.Diamonds12, GetSlotKey(SimpleSlot.Swap2));

            renderer.Resume();

            sw.Stop();

            OnMeasurementComplete(new MeasurementCompleteEventArgs(sw.ElapsedMilliseconds));
        }

        private PointF RandomPosition()
        {
            var r = new Random();

            return new PointF(r.Next(300), r.Next(300));
        }

        private Card RandomCard()
        {
            var possibleCardNames = Enum.GetNames(typeof(Card));
            var r = new Random();
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

    }
}
