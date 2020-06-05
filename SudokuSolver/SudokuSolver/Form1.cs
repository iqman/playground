using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        private readonly Board board;
        private BoardRenderer br;
        private SoudokuSolver solver;

        public Form1()
        {
            board = new Board();
            InitializeComponent();


            br = new BoardRenderer(board, pictureBoxBoard.Size);
            pictureBoxBoard.Image = br.RenderingImage;

            br.RenderingComplete += () => pictureBoxBoard.Invalidate();
            br.RenderBoard();

            solver = new SoudokuSolver(board);

            solver.StatusUpdate += Solver_StatusUpdate;

            board.Init();
            br.RenderBoard();
        }

        private void Solver_StatusUpdate(string status)
        {
            listBoxStatus.Items.Add(status);
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            board.ClearExclusions();

            var n = int.Parse(textBoxNumberToSolve.Text);

            solver.SolveStep(n);

            br.RenderBoard();

            textBoxNumberToSolve.Text = (n++ % Board.BoardSize+1).ToString();
        }

        private void buttonIsValid_Click(object sender, EventArgs e)
        {
            solver.IsBoardValid();
        }

        private void buttonFindFiftyFiefties_Click(object sender, EventArgs e)
        {
            solver.FindAllFiftyFifties(board);
            br.RenderBoard();
        }
    }
}
