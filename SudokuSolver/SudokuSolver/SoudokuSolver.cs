using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SoudokuSolver
    {
        private readonly Board board;

        public event Action<string> StatusUpdate;
        protected virtual void OnStatusUpdate(string status)
        {
            StatusUpdate?.Invoke(status);
        }

        public SoudokuSolver(Board board)
        {
            this.board = board;
        }

        public void SolveStep(int number)
        {
            var solverBoard = board;//.Clone();


            ExcludeCells(solverBoard, number);

            var cell = FindCertainPlacementCell(solverBoard);

            if (cell != null)
            {
                cell.Highlight = true;
                cell.Value = number;

                ExcludeCells(solverBoard, number);

                OnStatusUpdate($"{number}: Found");
            }
            else
            {
                OnStatusUpdate($"{number}: Not found");
            }

            FindFiftyFifties(solverBoard, number);
        }

        public void FindAllFiftyFifties(Board b)
        {
            b.ClearFiftyFifties();

            for (int i = 1; i < Board.BoardSize+1; i++)
            {
                b.ClearExclusions();
                ExcludeCells(b, i);
                FindFiftyFifties(b, i);
            }
        }

        private void FindFiftyFifties(Board b, int number)
        {
            var freeRowCells = FindNFreeCells(2, b.GetRow);

            if (freeRowCells.Length > 0)
            {
                if (!freeRowCells.All(c => c.FiftyFifties.Contains(number)))
                {
                    foreach (SudokuCell cell in freeRowCells)
                    {
                        cell.FiftyFifties.Add(number);
                    }
                    OnStatusUpdate($"{number}: Found row 50/50");
                }
            }

            var freeColumnCells = FindNFreeCells(2, b.GetColumn);

            if (freeColumnCells.Length > 0)
            {
                if (!freeColumnCells.All(c => c.FiftyFifties.Contains(number)))
                {
                    foreach (SudokuCell t in freeColumnCells)
                    {
                        t.FiftyFifties.Add(number);
                    }
                    OnStatusUpdate($"{number}: Found column 50/50");
                }
            }
        }

        private SudokuCell FindCertainPlacementCell(Board solverBoard)
        {
            return FindSingleFreeCell(solverBoard.GetRow) ??
                   FindSingleFreeCell(solverBoard.GetColumn) ??
                   FindSingleFreeCell(solverBoard.GetGroup);
        }

        private SudokuCell FindSingleFreeCell(Func<int, SudokuCell[]> func)
        {
            return FindNFreeCells(1, func).SingleOrDefault();
        }

        private SudokuCell[] FindNFreeCells(int cellsToBeFree, Func<int, SudokuCell[]> func)
        {
            for (int i = 0; i < Board.BoardSize; i++)
            {
                var set = func(i);

                var nonExcluded = set.Where(c => !c.Excluded).ToArray();
                if (nonExcluded.Length == cellsToBeFree)
                {
                    return nonExcluded;
                }
            }

            return new SudokuCell[0];
        }

        private void ExcludeCells(Board b, int number)
        {
            for (int i = 0; i < Board.BoardSize*Board.BoardSize; i++)
            {
                var cell = b.CellAt(i);

                if (cell.Value != SudokuCell.EmptyValue)
                {
                    cell.Excluded = true;
                }
            }

            var indicesToExclude = new List<int>();

            for (int i = 0; i < Board.BoardSize; i++)
            {
                var column = b.GetColumn(i);
                if (column.Any(c => c.Value == number))
                {
                    indicesToExclude = indicesToExclude.Union(b.IndicesForColumn(i)).ToList();
                }
                foreach (var cell in FiftyFiftyPairsToExclude(column, number))
                {
               //     OnStatusUpdate("Excluded based on 50/50 via column");
                    cell.Excluded = true;
                }

                var row = b.GetRow(i);
                if (row.Any(c => c.Value == number))
                {
                    indicesToExclude = indicesToExclude.Union(b.IndicesForRow(i)).ToList();
                }
                foreach (var cell in FiftyFiftyPairsToExclude(row, number))
                {
                    cell.Excluded = true;
             //       OnStatusUpdate("Excluded based on 50/50 via row");
                }

                var group = b.GetGroup(i);
                if (group.Any(c => c.Value == number))
                {
                    indicesToExclude = indicesToExclude.Union(b.IndicesForGroup(i)).ToList();
                }
            }

            foreach (var i in indicesToExclude)
            {
                b.CellAt(i).Excluded = true;
            }
        }

        IEnumerable<SudokuCell> FiftyFiftyPairsToExclude(SudokuCell[] set, int number)
        {
            var additionalExclusions = new HashSet<SudokuCell>();

            int[] except = {number};
            for (int i = 0; i < set.Length; i++)
            {
                var firstFifties = set[i].FiftyFifties.Except(except).ToArray();

                if (firstFifties.Length < 2)
                {
                    continue;
                }

                for (int o = i+1; o < set.Length; o++)
                {
                    var secondFifties = set[o].FiftyFifties.Except(except).ToArray();

                    if (secondFifties.Length < 2)
                    {
                        continue;
                    }

                    if (firstFifties.Intersect(secondFifties).Count() == 2)
                    {
                        additionalExclusions.Add(set[i]);
                        additionalExclusions.Add(set[o]);
                    }
                }
            }

            return additionalExclusions;
        }

        delegate SudokuCell[] MyFunc(int number);

        public bool IsBoardValid()
        {
            var bc = board.Clone();

            bool allValid = true;

            for (int number = 1; number <= Board.BoardSize; number++)
            {
                ExcludeCells(bc, number);

                if (!AllValid(number, bc.GetRow))
                {
                    OnStatusUpdate($"Found invalid row for {number}");
                    allValid = false;
                }
                if (!AllValid(number, bc.GetColumn))
                {
                    OnStatusUpdate($"Found invalid column for {number}");
                    allValid = false;
                }

                if (!AllValid(number, bc.GetGroup))
                {
                    OnStatusUpdate($"Found invalid group for {number}");
                    allValid = false;
                }

                bc.ClearExclusions();
            }

            if (allValid)
            {
                OnStatusUpdate("Board is valid");
            }

            return allValid;
        }

        private bool AllValid(int number, MyFunc func)
        {
            for (int i = 0; i < Board.BoardSize; i++)
            {
                var set = func(i);

                if (set.Count(c => c.Value == number) == 1)
                {
                    continue;
                }

                if (set.Count(c => c.Value == number) > 1 ||
                    set.Count(c => c.Value == SudokuCell.EmptyValue) == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}