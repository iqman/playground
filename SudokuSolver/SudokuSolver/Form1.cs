using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        private readonly Board board;
        private BoardRenderer br;
        private SudokuSolver solver;

        public Form1()
        {
            board = new Board();
            InitializeComponent();


            br = new BoardRenderer(board, pictureBoxBoard.Size);
            pictureBoxBoard.Image = br.RenderingImage;

            br.RenderingComplete += Br_RenderingComplete;

            solver = new SudokuSolver(board);

            solver.StatusUpdate += Solver_StatusUpdate;
            solver.GuessMade += SolverGuessMade;
            solver.GuessReverted += Solver_GuessReverted;

            board.Init();
            br.RenderBoard();
        }

        private void Solver_GuessReverted()
        {
            this.InvokeIfRequired(() =>
            {
                listBoxGuessStack.Items.RemoveAt(listBoxGuessStack.Items.Count-1);
            });
        }

        private void SolverGuessMade(int number)
        {
            this.InvokeIfRequired(() =>
            {
                listBoxGuessStack.Items.Add(number);
                labelNumberOfGuesses.Text = (int.Parse(labelNumberOfGuesses.Text)+1).ToString();
            });
        }

        private void Br_RenderingComplete()
        {
            this.InvokeIfRequired(() =>
            {
                pictureBoxBoard.Invalidate();
            });
        }

        private void Solver_StatusUpdate(StatusType type, string status)
        {
            this.InvokeIfRequired(() =>
            {
                if (type == StatusType.AutoSolving || checkBoxAnimateAutoSolve.Checked)
                {
                    listBoxStatus.Items.Add(status);
                    listBoxStatus.TopIndex = listBoxStatus.Items.Count - 1;
                    br.RenderBoard();
                }
            });
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
            solver.UpdateAllFiftyFifties(board);
            br.RenderBoard();
        }

        private void buttonClearExclusion_Click(object sender, EventArgs e)
        {
            board.ClearExclusions();
            br.RenderBoard();
        }

        private void buttonGuess_Click(object sender, EventArgs e)
        {
            solver.MakeGuess(int.Parse(textBoxNumberToSolve.Text));
            br.RenderBoard();
        }

        private void buttonRevertToBeforeGuess_Click(object sender, EventArgs e)
        {
            solver.RevertToPreviousBoardSnapshot();
            br.RenderBoard();
        }

        private void buttonAutoSolve_Click(object sender, EventArgs e)
        {
            solver.StartAsyncAutoSolve();
        }

        private void buttonAbortAutoSolve_Click(object sender, EventArgs e)
        {
            solver.StopAsyncAutoSolve();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            board.Init();
            solver.ResetSolving();
            br.RenderBoard();
        }
    }

    public static class InvokeHelper
    {
        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                var args = new object[0];
                obj.Invoke(action, args);
            }
            else
            {
                action();
            }
        }
    }
}
