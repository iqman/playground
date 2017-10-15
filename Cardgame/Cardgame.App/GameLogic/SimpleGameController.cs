using Cardgame.App.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.GameLogic
{
    class SimpleGameController : IGameController
    {
        private readonly GameRenderer renderer;

        public event EventHandler<MeasurementCompleteEventArgs> MeasurementComplete;
        protected void OnMeasurementComplete(MeasurementCompleteEventArgs args)
        {
            MeasurementComplete?.Invoke(this, args);
        }

        public SimpleGameController(GameRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Start()
        {
            var sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < 100; i++)
            {
                renderer.Render();
            }

            sw.Stop();

            OnMeasurementComplete(new MeasurementCompleteEventArgs(sw.ElapsedMilliseconds));
        }
    }
}
