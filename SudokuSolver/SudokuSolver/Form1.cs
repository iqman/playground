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
        private SoudokuSolver solver;

        private int stepsSinceLastProgress;
        private bool abortAutoSolving;

        public Form1()
        {
            board = new Board();
            InitializeComponent();


            br = new BoardRenderer(board, pictureBoxBoard.Size);
            pictureBoxBoard.Image = br.RenderingImage;

            br.RenderingComplete += Br_RenderingComplete;
            br.RenderBoard();

            solver = new SoudokuSolver(board);

            solver.StatusUpdate += Solver_StatusUpdate;
            solver.ProgressUpdate += Solver_ProgressUpdate;
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

        private void Solver_ProgressUpdate()
        {
            stepsSinceLastProgress = 0;
        }

        private void Solver_StatusUpdate(string status)
        {
            this.InvokeIfRequired(() =>
            {
                listBoxStatus.Items.Add(status);
                listBoxStatus.TopIndex = listBoxStatus.Items.Count - 1;
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
            abortAutoSolving = false;

            Task.Run(() =>
            {
                int number = 1;
                int guesses = 0;
                int firstLevelGuesses = 0;

                while (firstLevelGuesses < 10)
                {
                    while (guesses < 10)
                    {
                        while (stepsSinceLastProgress < 10)
                        {
                            if (!solver.IsBoardValid())
                            {
                                solver.RevertToPreviousBoardSnapshot();
                                break;
                            }

                            board.ClearExclusions();
                            solver.SolveStep(number);

                            number = number++ % Board.BoardSize + 1;
                            stepsSinceLastProgress++;

                            br.RenderBoard();

                            if (abortAutoSolving)
                            {
                                Solver_StatusUpdate("Aborted autosolving");
                                return;
                            }
                        }

                        if (solver.IsDone())
                        {
                            Solver_StatusUpdate("Autosolving complete");
                            return;
                        }

                        number = number++ % Board.BoardSize + 1;

                        if (solver.CurrentGuessDepth == 0)
                        {
                            firstLevelGuesses++;
                        }
                        solver.MakeGuess(number);

                        guesses++;
                    }

                    while (solver.CurrentGuessDepth > 0)
                    {
                        solver.RevertToPreviousBoardSnapshot();
                    }
                    guesses = 0;
                }

                Solver_StatusUpdate("Failed autosolving");
            });
        }

        private void buttonAbortAutoSolve_Click(object sender, EventArgs e)
        {
            abortAutoSolving = true;
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
