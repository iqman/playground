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

        public event EventHandler<MeasurementCompleteEventArgs> MeasurementComplete;
        protected void OnMeasurementComplete(MeasurementCompleteEventArgs args)
        {
            MeasurementComplete?.Invoke(this, args);
        }

        public SimpleGameController(IGameState gameState)
        {
            this.gameState = gameState;
        }

        public void Start()
        {
            var sw = new Stopwatch();

            sw.Start();

            gameState.PlaceCard(Card.Diamonds11, new PointF(200, 200));

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
    }
}
