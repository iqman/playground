using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _10x10Solver.Bricks;

namespace _10x10Solver
{
    class NextBricksSet
    {
        public const int NextBricksCount = 1;
        public IList<IBrick> NextBricks { get; private set; }

        public event Action BrickSetGenerated;

        public void OnBrickSetGenerated()
        {
            Action handler = BrickSetGenerated;
            if (handler != null) handler();
        }

        public NextBricksSet()
        {
            NextBricks = new List<IBrick>(Enumerable.Range(0, NextBricksCount).Select(x => (IBrick)null));
        }

        public void GenerateNextBricks()
        {
            for (int i = 0; i < NextBricksCount; i++)
            {
                NextBricks[i] = BrickFactory.CreateRandomBrick();
            }

            OnBrickSetGenerated();
        }
    }
}
