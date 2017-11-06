using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _10x10Solver.Solvers;

namespace _10x10Solver
{
    static class SolverFactory
    {
        public static ISolver CreateSolver(Board board, NextBricksSet nextBricksSet)
        {
            return new Solver(board, nextBricksSet);
        }
    }
}
