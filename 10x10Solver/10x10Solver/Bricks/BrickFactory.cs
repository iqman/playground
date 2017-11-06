using System;
using System.Linq;
using System.Collections.Generic;

namespace _10x10Solver.Bricks
{
    static class BrickFactory
    {
        private static readonly IList<IBrick> Bricks;

        static BrickFactory()
        {
            var brickTypes = typeof (BrickFactory).Assembly.GetTypes().Where(t => typeof (IBrick).IsAssignableFrom(t) && !t.IsAbstract);
            Bricks = brickTypes.Select(t => (IBrick)(Activator.CreateInstance(t))).ToList();
        }

        public static int MaxBrickLength
        {
            get { return Bricks.Select(b => Math.Max(b.Width, b.Height)).Max(); }
        }

        public static IBrick CreateRandomBrick()
        {
            return Bricks[new Random().Next(Bricks.Count)];
        }
    }
}
