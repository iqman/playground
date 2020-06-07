using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SoudokuSolver
    {
        private readonly Board board;

        private readonly Stack<SnapShotContainer> boardSnapshots;
        private int[] wrongGuessCounterAtCurrentLevel;

        public event Action<string> StatusUpdate;
        protected virtual void OnStatusUpdate(string status)
        {
            StatusUpdate?.Invoke(status);
        }

        public event Action ProgressUpdate;
        protected virtual void OnProgressUpdate()
        {
            ProgressUpdate?.Invoke();
        }

        public event Action<int> GuessMade;
        protected virtual void OnGuessMade(int number)
        {
            GuessMade?.Invoke(number);
        }

        public event Action GuessReverted;
        protected virtual void OnGuessReverted()
        {
            GuessReverted?.Invoke();
        }

        public SoudokuSolver(Board board)
        {
            this.board = board;
            boardSnapshots = new Stack<SnapShotContainer>();
            wrongGuessCounterAtCurrentLevel = new int[Board.BoardSize];
        }

        public void SolveStep(int number)
        {
            if (IsDone())
            {
                return;
            }

            ExcludeCells(board, number);

            var cell = FindCertainPlacementCell(board);

            if (cell != null)
            {
                cell.Highlight = true;
                cell.Value = number;

                board.ClearFiftyFiftiesForNumber(number);
                ExcludeCells(board, number);

                OnStatusUpdate($"{number}: Found");

                OnProgressUpdate();
            }
            else
            {
                OnStatusUpdate($"{number}: Not found");
            }

            FindFiftyFifties(board, number);
        }

        public void UpdateAllFiftyFifties(Board b)
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

                    OnProgressUpdate();
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

                    OnProgressUpdate();
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
            return FindNFreeCells(1, func).FirstOrDefault();
        }

        private SudokuCell[] FindNFreeCells(int cellsToBeFree, Func<int, SudokuCell[]> func)
        {
            var freeCells = new List<SudokuCell>();
            for (int i = 0; i < Board.BoardSize; i++)
            {
                var set = func(i);

                var nonExcluded = set.Where(c => !c.Excluded).ToArray();
                if (nonExcluded.Length == cellsToBeFree)
                {
                    freeCells.AddRange(nonExcluded);
                }
            }

            return freeCells.ToArray();
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
                //    OnStatusUpdate("Excluded based on 50/50 via column");
                //    cell.Excluded = true;
                //    cell.FiftyFiftyExclusion = true;
                }

                var row = b.GetRow(i);
                if (row.Any(c => c.Value == number))
                {
                    indicesToExclude = indicesToExclude.Union(b.IndicesForRow(i)).ToList();
                }
                foreach (var cell in FiftyFiftyPairsToExclude(row, number))
                {
                //    cell.Excluded = true;
                //    cell.FiftyFiftyExclusion = true;
                    //   OnStatusUpdate("Excluded based on 50/50 via row");
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

            bc.ClearExclusions();

            bool allValid = true;

            for (int number = 1; number <= Board.BoardSize; number++)
            {
                ExcludeCells(bc, number);

                if (!AllValid(number, bc.GetRow))
                {
                    OnStatusUpdate($"{number}: Found invalid row");
                    allValid = false;
                }
                if (!AllValid(number, bc.GetColumn))
                {
                    OnStatusUpdate($"{number}: Found invalid column");
                    allValid = false;
                }

                if (!AllValid(number, bc.GetGroup))
                {
                    OnStatusUpdate($"{number}: Found invalid group");
                    allValid = false;
                }

                bc.ClearExclusions();
            }

            if (allValid)
            {
                OnStatusUpdate("Board is valid");
            }
            else if (boardSnapshots.Count == 0)
            {
                OnStatusUpdate("ERROR");
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
                    set.Where(c => c.Excluded == false).Count(c => c.Value == SudokuCell.EmptyValue) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsDone()
        {
            var done = Enumerable.Range(0, Board.BoardSize * Board.BoardSize)
                       .All(i => board.CellAt(i).Value != SudokuCell.EmptyValue)
                   && IsBoardValid();

            if (done)
            {
                OnStatusUpdate("Sudoku has been solved");
            }

            return done;
        }

        public int CurrentGuessDepth => boardSnapshots.Count;

        public void MakeGuess(int number)
        {
            var snapshot = board.Clone();

            UpdateAllFiftyFifties(board);

            var guessesToSkipForNumber = wrongGuessCounterAtCurrentLevel[number-1];

            for (int i = 0; i < Board.BoardSize*Board.BoardSize; i++)
            {
                var cell = board.CellAt(i);
                if (cell.FiftyFifties.Contains(number))
                {
                    if (guessesToSkipForNumber > 0)
                    {
                        guessesToSkipForNumber--;
                        continue;
                    }

                    cell.Value = number;
                    cell.Guessed = true;
                    OnStatusUpdate($"{number} Guessed");

                    boardSnapshots.Push(new SnapShotContainer(snapshot, wrongGuessCounterAtCurrentLevel.Select(n => n).ToArray(), number));

               //     Array.Clear(wrongGuessCounterAtCurrentLevel, 0, Board.BoardSize);

                    OnGuessMade(number);

                    OnProgressUpdate();

                    return;
                }
            }

            OnStatusUpdate($"{number} Found no 50/50 to guess from");
        }

        public void RevertToPreviousBoardSnapshot()
        {
            if (boardSnapshots.Any())
            {
                var snapShot = boardSnapshots.Pop();
                board.Restore(snapShot.Snapshot);

                wrongGuessCounterAtCurrentLevel[snapShot.GuessedNumber-1]++;
                if (boardSnapshots.Any())
                {
                    var temp = boardSnapshots.Pop();
                    temp.WrongGuessCounter[snapShot.GuessedNumber-1]++;
                    boardSnapshots.Push(temp);
                }

                OnGuessReverted();
            }
        }
    }

    public class SnapShotContainer
    {
        public Board Snapshot { get; }
        public int[] WrongGuessCounter { get; set; }
        public int GuessedNumber { get; set; }

        public SnapShotContainer(Board snapshot, int[] wrongGuessCounter, int guessedNumber)
        {
            Snapshot = snapshot;
            WrongGuessCounter = wrongGuessCounter;
            GuessedNumber = guessedNumber;
        }
    }
}