using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuCell
    {
        public const int EmptyValue = 0;

        public int Value { get; set; }
        public bool Excluded { get; set; }
        public bool Highlight { get; set; }
        public HashSet<int> FiftyFifties { get; set; }

        public SudokuCell()
        {
            FiftyFifties = new HashSet<int>();
        }

        private SudokuCell(int value, bool excluded, bool highlight, HashSet<int> fiftyFifties)
        {
            Value = value;
            Excluded = excluded;
            Highlight = highlight;
            FiftyFifties = fiftyFifties;
        }

        public SudokuCell Clone()
        {
            return new SudokuCell(Value, Excluded, Highlight, FiftyFifties);
        }
    }
}
