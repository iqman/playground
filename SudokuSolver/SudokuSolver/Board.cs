using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Board
    {
        public const int BoardSize = 9;

        private readonly SudokuCell[] cells;

        private readonly int[] game = VeryHard;

        // https://cracking-the-cryptic.web.app/sudoku/RRf6bgb9GG
        private static readonly int[] RRf6bgb9GG =
        {
            6, 0, 9, 1, 0, 2, 0, 8, 0,
            0, 0, 0, 0, 0, 0, 4, 0, 0,
            5, 0, 2, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 2, 0, 3, 0, 4,
            1, 0, 0, 0, 0, 5, 0, 0, 0,
            0, 2, 0, 0, 0, 0, 5, 0, 6,
            0, 0, 0, 8, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 9,
            8, 0, 5, 9, 0, 7, 0, 4, 0
        };

        // https://sudoku.game/ easy
        private static readonly int[] Easy =
        {
            0, 0, 0, 0, 0, 0, 4, 0, 2,
            0, 9, 4, 5, 0, 2, 0, 0, 0,
            2, 0, 8, 0, 0, 0, 0, 1, 0,
            0, 5, 3, 0, 4, 0, 0, 0, 7,
            0, 8, 0, 1, 2, 5, 0, 6, 0,
            1, 0, 0, 0, 6, 0, 9, 5, 0,
            0, 7, 0, 0, 0, 0, 2, 0, 1,
            0, 0, 0, 4, 0, 6, 8, 3, 0,
            4, 0, 9, 0, 0, 0, 0, 0, 0
        };

        // https://sudoku.game/ medium
        private static readonly int[] Medium =
        {
            0, 0, 0, 2, 0, 0, 0, 6, 4,
            0, 6, 5, 0, 0, 3, 2, 0, 0,
            7, 1, 2, 4, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 5, 1,
            0, 8, 4, 0, 0, 0, 6, 7, 0,
            2, 5, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 7, 3, 4, 9,
            0, 0, 1, 9, 0, 0, 7, 8, 0,
            5, 9, 0, 0, 0, 4, 0, 0, 0
        };

        // https://sudoku.game/ Hard
        private static readonly int[] Hard =
        {
            0, 0, 5, 0, 6, 0, 0, 8, 0,
            1, 4, 0, 0, 0, 0, 0, 0, 0,
            0, 6, 9, 1, 0, 0, 0, 2, 0,
            9, 0, 0, 0, 4, 0, 6, 0, 0,
            0, 3, 0, 2, 8, 0, 0, 5, 0,
            4, 0, 0, 0, 3, 6, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 0, 0, 5, 0, 2,
            2, 9, 0, 0, 0, 0, 3, 6, 0
        };

        // https://sudoku.game/ Very Hard
        private static readonly int[] VeryHard =
        {
            0, 5, 0, 0, 3, 2, 6, 0, 8,
            0, 0, 0, 1, 0, 5, 7, 3, 0,
            0, 0, 0, 0, 0, 6, 0, 0, 2,
            0, 0, 0, 0, 5, 0, 0, 8, 7,
            0, 0, 7, 0, 1, 0, 2, 0, 0,
            3, 6, 0, 0, 8, 0, 0, 0, 0,
            4, 0, 0, 7, 0, 0, 0, 0, 0,
            0, 7, 6, 3, 0, 4, 0, 0, 0,
            8, 0, 9, 5, 2, 0, 0, 7, 0
        };


        public Board()
        {
            cells = new SudokuCell[BoardSize*BoardSize];

            for (int i = 0; i < BoardSize*BoardSize; i++)
            {
                cells[i] = new SudokuCell();
            }
        }

        public Board Clone()
        {
            var b = new Board();
            for (int i = 0; i < cells.Length; i++)
            {
                b.cells[i] = cells[i].Clone();
            }

            return b;
        }

        public void Restore(Board b)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = b.cells[i];
            }
        }

        public void Init()
        {
            for (int i = 0; i < BoardSize * BoardSize; i++)
            {
                cells[i] = new SudokuCell {Value = game[i]};
            }
        }

        public SudokuCell CellAt(int index)
        {
            return cells[index];
        }


        public SudokuCell GetCell(int x, int y)
        {
            return cells[x+y* BoardSize];
        }

        public SudokuCell[] GetGroup(int groupNumber)
        {
            var indices = IndicesForGroup(groupNumber);
            var res = new List<SudokuCell>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (indices.Contains(i))
                {
                    res.Add(cells[i]);
                }
            }
            return res.ToArray();
        }

        public SudokuCell[] GetRow(int rowNumber)
        {
            var indices = IndicesForRow(rowNumber);
            var res = new List<SudokuCell>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (indices.Contains(i))
                {
                    res.Add(cells[i]);
                }
            }
            return res.ToArray();
        }

        public SudokuCell[] GetColumn(int columnNumber)
        {
            var indices = IndicesForColumn(columnNumber);
            var res = new List<SudokuCell>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (indices.Contains(i))
                {
                    res.Add(cells[i]);
                }
            }
            return res.ToArray();
        }

        public int[] IndicesForRow(int rowNumber)
        {
            var indices = new List<int>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (i >= BoardSize * rowNumber && i < BoardSize * (rowNumber + 1))
                {
                    indices.Add(i);
                }
            }
            return indices.ToArray();
        }

        public int[] IndicesForColumn(int columnNumber)
        {
            var indices = new List<int>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (i % BoardSize == columnNumber)
                {
                    indices.Add(i);
                }
            }
            return indices.ToArray();
        }

        public int[] IndicesForGroup(int groupNumber)
        {
            if (groupNumber == 3 || groupNumber == 4 || groupNumber == 5)
            {
                groupNumber += 6;
            }

            if (groupNumber == 6 || groupNumber == 7 || groupNumber == 8)
            {
                groupNumber += 12;
            }

            var indices = new List<int>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (i >= (0 + groupNumber) * 3 && i < (0 + groupNumber) * 3 + 3 ||
                    i >= (3 + groupNumber) * 3 && i < (3 + groupNumber) * 3 + 3 ||
                    i >= (6 + groupNumber) * 3 && i < (6 + groupNumber) * 3 + 3)
                {
                    indices.Add(i);
                }
            }
            return indices.ToArray();
        }

        public void ClearExclusions()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].Excluded = false;
            }
        }

        public void ClearFiftyFifties()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].FiftyFifties.Clear();;
            }
        }

        public void ClearFiftyFiftiesForNumber(int number)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].FiftyFifties.Remove(number);
            }
        }
    }
}
