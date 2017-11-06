using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10x10Solver.Solvers
{
    interface ISolver
    {
        void Solve(IDictionary<ScoreComponent, float> scoreWeigths);
        void Solve();
    }
}
