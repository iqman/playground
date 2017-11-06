using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _10x10Solver.Bricks;

namespace _10x10Solver.Solvers
{
    class Solver : ISolver
    {
        private readonly Board board;
        private readonly NextBricksSet nextBricksSet;

        private const int ScoreScale = 1000;

        private readonly IDictionary<ScoreComponent, float> maxScores = new Dictionary<ScoreComponent, float>
        {
            // perfect score is empty board where all fields have 4 free neigbours (exept board edges)
            {ScoreComponent.Cohesion, Board.BoardSize * Board.BoardSize * 4 - Board.BoardSize * 4},

            // perfect score is difficult to determine, but holes should really be kept down
            {ScoreComponent.Holes, Board.BoardSize},

            // perfect score is empty board
            {ScoreComponent.BoardFill, Board.BoardSize * Board.BoardSize},

            // perfect score is difficult to determine, but holes should really be kept down
            {ScoreComponent.Islands, Board.BoardSize},

            // perfect score is that all rows and columns are given 1 point for being "good"
            {ScoreComponent.RowColumnBuilding, Board.BoardSize * 2},
        };

        public Solver(Board board, NextBricksSet nextBricksSet)
        {
            this.board = board;
            this.nextBricksSet = nextBricksSet;
        }

        public void Solve()
        {
            IDictionary<ScoreComponent, float> scoreWeigths = new Dictionary<ScoreComponent, float>
            {
                {ScoreComponent.Cohesion, .7f},
                {ScoreComponent.Holes, .6f},
                {ScoreComponent.BoardFill, .8f},
                {ScoreComponent.Islands, .6f},
                {ScoreComponent.RowColumnBuilding, .0f},
            };

            Solve(scoreWeigths);
        }

        public void Solve(IDictionary<ScoreComponent, float> scoreWeigths)
        {
            IBrick brick = nextBricksSet.NextBricks.First();

            if (brick != null)
            {
                var p = GetOptimalBrickPosition(brick, scoreWeigths);
                board.PutBrick(brick, p);
            }
        }

        private Point GetOptimalBrickPosition(IBrick brick, IDictionary<ScoreComponent, float> scoreWeigths)
        {
            IDictionary<Point, float> placements = new Dictionary<Point, float>();

            for (int x = 0; x < Board.BoardSize; x++)
            {
                for (int y = 0; y < Board.BoardSize; y++)
                {
                    var p = new Point(x, y);
                    if (board.IsPositionValid(brick, p))
                    {
                        placements[p] = EvaluateBrickPlacement(brick, p, scoreWeigths);
                    }
                }
            }

            var orderedLocations = placements.OrderByDescending(p => p.Value).ToArray();
            return orderedLocations.First().Key;
        }

        private float EvaluateBrickPlacement(IBrick brick, Point p, IDictionary<ScoreComponent, float> scoreWeigths)
        {
            try
            {
                board.StartSimulation();

                board.PutBrick(brick, p);

                var scores = new Dictionary<ScoreComponent, float>();

                //scores[ScoreComponent.Cohesion] = CalculateCohesion();
                //scores[ScoreComponent.BoardFill] = CalculateBoardFill();
                //var holesAndIslandsScore = CalculateHolesAndIslands();
                //scores[ScoreComponent.Holes] = holesAndIslandsScore.HolesScore;
                //scores[ScoreComponent.Islands] = holesAndIslandsScore.IslandsScore;
                scores[ScoreComponent.RowColumnBuilding] = CalculateRowColumnsBuilding();

                return WeighAndSumScores(scores, scoreWeigths);
            }
            finally
            {
                board.ClearSimulation();
            }
        }

        private float WeighAndSumScores(IDictionary<ScoreComponent, float> scores, IDictionary<ScoreComponent, float> scoreWeigths)
        {
            float sum = 0;
            foreach (var s in scores)
            {
                sum += s.Value / maxScores[s.Key] * ScoreScale * scoreWeigths[s.Key];
            }

            return sum;
        }

        private float CalculateRowColumnsBuilding()
        {
            float rowColumnBuildingScore = maxScores[ScoreComponent.RowColumnBuilding];

            const int min = Board.BoardSize/5;
            const int max = Board.BoardSize - min;

            // examine columns
            for (int x = 0; x < Board.BoardSize; x++)
            {
                int free = 0;
                for (int y = 0; y < Board.BoardSize; y++)
                {
                    if (board.Fields[x,y] == FieldValue.Free)
                    {
                        free++;
                    }
                }
                if (free >= min && free <= max)
                {
                    rowColumnBuildingScore--;
                }
            }

            // examine rows
            for (int y = 0; y < Board.BoardSize; y++)
            {
                int free = 0;
                for (int x = 0; x < Board.BoardSize; x++)
                {
                    if (board.Fields[x, y] == FieldValue.Free)
                    {
                        free++;
                    }
                }
                if (free >= min && free <= max)
                {
                    rowColumnBuildingScore--;
                }
            }

            return rowColumnBuildingScore;
        }

        private HolesAndIslandsScores CalculateHolesAndIslands()
        {
            float holesScore = maxScores[ScoreComponent.Holes];
            float islandsScore = maxScores[ScoreComponent.Islands];

            var reachableHolePoints = new HashSet<Point>();
            var reachableIslandPoints = new HashSet<Point>();

            for (int x = 0; x < Board.BoardSize; x++)
            {
                for (int y = 0; y < Board.BoardSize; y++)
                {
                    var p = new Point(x, y);
                    if (board.Fields[x, y] == FieldValue.Free)
                    {
                        if (!reachableHolePoints.Contains(p))
                        {
                            reachableHolePoints.Add(p);
                            holesScore--;
                        }

                        FindReachableFreeFieldsRecursively(p, reachableHolePoints, ReachableFieldMode.Free);
                    }
                    else
                    {
                        if (!reachableIslandPoints.Contains(p))
                        {
                            reachableIslandPoints.Add(p);
                            islandsScore--;
                        }

                        FindReachableFreeFieldsRecursively(p, reachableIslandPoints, ReachableFieldMode.Occupied);
                    }
                }
            }

            return new HolesAndIslandsScores {HolesScore = holesScore, IslandsScore = islandsScore};
        }

        private IEnumerable<Point> FindReachableFreeFieldsRecursively(Point p, HashSet<Point> reachablePoints, ReachableFieldMode mode)
        {
            foreach (var neighbour in GetNeigbours(p.X, p.Y))
            {
                if (board.IsInsideBoard(neighbour) &&
                    !reachablePoints.Contains(neighbour) &&
                    (mode == ReachableFieldMode.Free
                        ? board.Fields[neighbour.X, neighbour.Y] == FieldValue.Free
                        : board.Fields[neighbour.X, neighbour.Y] != FieldValue.Free))
                {
                    reachablePoints.Add(neighbour);
                    reachablePoints.UnionWith(FindReachableFreeFieldsRecursively(neighbour, reachablePoints, mode));
                }
            }

            return reachablePoints;
        }

        private float CalculateBoardFill()
        {
            var score = maxScores[ScoreComponent.BoardFill];

            for (int x = 0; x < Board.BoardSize; x++)
            {
                for (int y = 0; y < Board.BoardSize; y++)
                {
                    score -= board.Fields[x, y] != FieldValue.Free ? 1 : 0;
                }
            }

            return score;
        }

        private float CalculateCohesion()
        {
            var score = maxScores[ScoreComponent.Cohesion];

            for (int x = 0; x < Board.BoardSize; x++)
            {
                for (int y = 0; y < Board.BoardSize; y++)
                {
                    if (board.Fields[x, y] == FieldValue.Free)
                    {
                        score -= GetNeigbours(x, y)
                            .Where(p => !board.IsInsideBoard(p) || board.Fields[p.X, p.Y] != FieldValue.Free)
                            .Select(p => 1).Sum();
                    }
                }
            }

            return score;
        }

        private IEnumerable<Point> GetNeigbours(int x, int y)
        {
            return new[]
            {
                new Point(x, y - 1),
                new Point(x + 1, y),
                new Point(x, y + 1),
                new Point(x - 1, y)
            };
        }
    }
}
