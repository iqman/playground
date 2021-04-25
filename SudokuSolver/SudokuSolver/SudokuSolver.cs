#define OPTIMIZATION1
#define OPTIMIZATION2
#define OPTIMIZATION5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Narkhedegs.PerformanceMeasurement;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        private readonly Board board;

        private readonly Stack<SnapShotContainer> boardSnapshots;
        private readonly int[] wrongGuessCounterAtCurrentLevel;

        private int stepsSinceLastProgress;
        private bool abortAutoSolving;

        public event Action<StatusType, string> StatusUpdate;
        protected virtual void OnStatusUpdate(StatusType type, string status)
        {
            StatusUpdate?.Invoke(type, status);
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

        public SudokuSolver(Board board)
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

                OnStatusUpdate(StatusType.Info, $"{number}: Found");

                RegisterProgress();
            }
            else
            {
                OnStatusUpdate(StatusType.Info, $"{number}: Not found");
            }

            FindFiftyFifties(board, number);
        }

        public void UpdateAllFiftyFifties(Board b)
        {
            b.ClearFiftyFifties();

            for (int i = 1; i < Board.BoardSize + 1; i++)
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
                    OnStatusUpdate(StatusType.Info, $"{number}: Found row 50/50");

                    RegisterProgress();
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
                    OnStatusUpdate(StatusType.Info, $"{number}: Found column 50/50");

                    RegisterProgress();
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
#if OPTIMIZATION5
                var nonExcluded = set.Where(c => !c.Excluded);
#else
                var nonExcluded = set.Where(c => !c.Excluded).ToArray();
#endif
                if (nonExcluded.Count() == cellsToBeFree)
                {
                    freeCells.AddRange(nonExcluded);
                }
            }

            return freeCells.ToArray();
        }

        private void ExcludeCells(Board b, int number)
        {
            for (int i = 0; i < Board.BoardSize * Board.BoardSize; i++)
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
#if OPTIMIZATION2
                    foreach (var o in b.IndicesForColumn(i))
                    {
                        indicesToExclude.Add(o);
                    }
#else
                    indicesToExclude = indicesToExclude.Union(b.IndicesForColumn(i)).ToList();
#endif
                }

                var row = b.GetRow(i);
                if (row.Any(c => c.Value == number))
                {
#if OPTIMIZATION2
                    foreach (var o in b.IndicesForRow(i))
                    {
                        indicesToExclude.Add(o);
                    }
#else
                    indicesToExclude = indicesToExclude.Union(b.IndicesForRow(i)).ToList();
#endif
                }

                var group = b.GetGroup(i);
                if (group.Any(c => c.Value == number))
                {
#if OPTIMIZATION2
                    foreach (var o in b.IndicesForGroup(i))
                    {
                        indicesToExclude.Add(o);
                    }
#else
                    indicesToExclude = indicesToExclude.Union(b.IndicesForGroup(i)).ToList();
#endif
                }
            }

            foreach (var i in indicesToExclude)
            {
                b.CellAt(i).Excluded = true;
            }
        }

        public bool IsBoardValid()
        {
            var bc = board;

#if OPTIMIZATION1
#else
            bc.ClearExclusions();
#endif

            bool allValid = true;
#if OPTIMIZATION1
            for (int i = 0; i < Board.BoardSize; i++)
#else
            for (int i = 1; i <= Board.BoardSize; i++)
#endif
            {

#if OPTIMIZATION1
#else
                ExcludeCells(bc, i);
#endif

                if (!AllValid(i, bc.GetRow))
                {
                    OnStatusUpdate(StatusType.Validity, $"Row number {i} is invalid");
                    allValid = false;
                }
                if (!AllValid(i, bc.GetColumn))
                {
                    OnStatusUpdate(StatusType.Validity, $"Column number {i} is invali");
                    allValid = false;
                }

                if (!AllValid(i, bc.GetGroup))
                {
                    OnStatusUpdate(StatusType.Validity, $"Group number {i} is invali");
                    allValid = false;
                }
#if OPTIMIZATION1
#else
                bc.ClearExclusions();
#endif
            }

            if (allValid)
            {
                OnStatusUpdate(StatusType.Validity, "Board is valid");
            }
            else if (boardSnapshots.Count == 0)
            {
                OnStatusUpdate(StatusType.Validity, "Invalid starting position, or invalid placement of number");
            }

            return allValid;
        }

        private bool AllValid(int number, Func<int, SudokuCell[]> func)
        {
#if OPTIMIZATION1
            var nonEmptyCellValues = func(number).Select(c => c.Value).Where(v => v != SudokuCell.EmptyValue).ToList();
            return nonEmptyCellValues.Distinct().Count() == nonEmptyCellValues.Count;
#else
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
#endif
        }

        public bool IsDone()
        {
            var done = Enumerable.Range(0, Board.BoardSize * Board.BoardSize)
                       .All(i => board.CellAt(i).Value != SudokuCell.EmptyValue)
                   && IsBoardValid();

            if (done)
            {
                OnStatusUpdate(StatusType.Completion, "Sudoku has been solved");
            }

            return done;
        }

        public int CurrentGuessDepth => boardSnapshots.Count;

        public void MakeGuess(int number)
        {
            var snapshot = board.Clone();

            UpdateAllFiftyFifties(board);

            var guessesToSkipForNumber = wrongGuessCounterAtCurrentLevel[number - 1];

            for (int i = 0; i < Board.BoardSize * Board.BoardSize; i++)
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
                    OnStatusUpdate(StatusType.Info, $"{number} Guessed");

                    boardSnapshots.Push(new SnapShotContainer(snapshot, wrongGuessCounterAtCurrentLevel.Select(n => n).ToArray(), number));

                    OnGuessMade(number);

                    RegisterProgress();

                    return;
                }
            }

            OnStatusUpdate(StatusType.Info, $"{number} Found no 50/50 to guess from");
        }

        public void RevertToPreviousBoardSnapshot()
        {
            if (boardSnapshots.Any())
            {
                var snapShot = boardSnapshots.Pop();
                board.Restore(snapShot.Snapshot);

                wrongGuessCounterAtCurrentLevel[snapShot.GuessedNumber - 1]++;
                if (boardSnapshots.Any())
                {
                    var temp = boardSnapshots.Pop();
                    temp.WrongGuessCounter[snapShot.GuessedNumber - 1]++;
                    boardSnapshots.Push(temp);
                }

                OnGuessReverted();
            }
        }

        private void RegisterProgress()
        {
            stepsSinceLastProgress = 0;
        }

        public void StopAsyncAutoSolve()
        {
            abortAutoSolving = true;
        }

        public void StartAsyncAutoSolve()
        {
            abortAutoSolving = false;
            Task.Run(() => { AutoSolve(false); });
        }

        public void ResetSolving()
        {
            boardSnapshots.Clear();
            for (int i = 0; i < wrongGuessCounterAtCurrentLevel.Length; i++)
            {
                wrongGuessCounterAtCurrentLevel[i] = 0;
            }
            stepsSinceLastProgress = 0;
            abortAutoSolving = false;
        }

        public void PerfTest()
        {
            var chronometer = new Chronometer(new ChronometerOptions
            {
                NumberOfInterations = 15,
                Warmup = true,
                MeasureUsingProcessorTime = false,
                UseNormalizedMean = false,
                AllowMeasurementsUnderDebugMode = true
            });

            var executionTimeInMilliseconds = chronometer.Measure(() =>
            {
                board.Init();
                ResetSolving();
                AutoSolve(true);
            });
            OnStatusUpdate(StatusType.Performance, $"Time in ms: {executionTimeInMilliseconds}");
        }

        private void AutoSolve(bool performanceTest)
        {
            int number = 1;
            int guesses = 0;
            int firstLevelGuesses = 0;
            var startTime = DateTime.Now;

            while (firstLevelGuesses < 10)
            {
                while (guesses < 10)
                {
                    while (stepsSinceLastProgress < 10)
                    {
                        if (!IsBoardValid())
                        {
                            RevertToPreviousBoardSnapshot();
                            break;
                        }

                        board.ClearExclusions();
                        SolveStep(number);

                        number = number++ % Board.BoardSize + 1;
                        stepsSinceLastProgress++;

                        if (abortAutoSolving)
                        {
                            if (!performanceTest)
                            {
                                OnStatusUpdate(StatusType.AutoSolving, "Aborted autosolving");
                            }
                            return;
                        }
                    }

                    if (IsDone())
                    {
                        if (!performanceTest)
                        {
                            OnStatusUpdate(StatusType.AutoSolving, "Autosolving complete");
                        }

                        var endTime = DateTime.Now;
                        var s = (endTime - startTime).TotalMilliseconds;

                        if (!performanceTest)
                        {
                            OnStatusUpdate(StatusType.AutoSolving, $"Solve time: {s}");
                        }
                        return;
                    }

                    number = number++ % Board.BoardSize + 1;

                    if (CurrentGuessDepth == 0)
                    {
                        firstLevelGuesses++;
                    }
                    MakeGuess(number);

                    guesses++;
                }

                while (CurrentGuessDepth > 0)
                {
                    RevertToPreviousBoardSnapshot();
                }
                guesses = 0;
            }

            if (!performanceTest)
            {
                OnStatusUpdate(StatusType.AutoSolving, "Failed autosolving");
            }
        }
    }

    public enum StatusType
    {
        Info,
        Validity,
        AutoSolving,
        Completion,
        Performance
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