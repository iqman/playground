using SudokuSolver.Ocr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        private ImageLoader il;

        public Form1()
        {
            board = new Board();
            InitializeComponent();

            il = new ImageLoader(pictureBoxImage.Size);

            il.ImageAnnotated += Il_ImageAnnotated;
            il.ImageLoaded += Il_ImageLoaded;
            il.AnnotationUpdate += Il_AnnotationUpdate;
            il.ImageContentParsed += Il_ImageContentParsed;

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

        private void Il_ImageContentParsed(int[] content)
        {
            board.Init(content);
            br.RenderBoard();
        }

        private void Il_AnnotationUpdate(string text)
        {
            this.InvokeIfRequired(() =>
            {
                textBoxAnnotationDetails.Text += Environment.NewLine + text;
            });
        }

        private void Il_ImageLoaded()
        {
            this.InvokeIfRequired(() =>
            {
                pictureBoxImage.Image = il.LoadedImage;
            });
        }

        private void Il_ImageAnnotated()
        {
            this.InvokeIfRequired(() =>
            {
                pictureBoxImage.Invalidate();
                this.Refresh();
            });
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
                if (type == StatusType.AutoSolving || type == StatusType.Performance || checkBoxAnimateAutoSolve.Checked)
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

        private void buttonPerfTest_Click(object sender, EventArgs e)
        {
            checkBoxAnimateAutoSolve.Checked = false;
            solver.PerfTest();
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            il.LoadImage();
        }

        private void buttonProcessImage_Click(object sender, EventArgs e)
        {
            il.ProcessImage();
        }

        private void buttonProcess2_Click(object sender, EventArgs e)
        {
            il.ProcessImage2();
        }

        private void buttonProcess3_Click(object sender, EventArgs e)
        {
            il.FindEdge();
        }

        private void buttonProcess4_Click(object sender, EventArgs e)
        {
            il.ProcessImage4();
        }

        private void buttonDenoise_Click(object sender, EventArgs e)
        {
            il.Denoise();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            il.HighlightEdges();
        }

        private void buttonCloseGaps_Click(object sender, EventArgs e)
        {
            il.CloseGaps();
        }

        private void buttonTransform_Click(object sender, EventArgs e)
        {
            il.Transform();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            il.Reload();
        }

        private void buttonLoadClipboard_Click(object sender, EventArgs e)
        {
            var img = GetImageFromClipboard();
            if (img != null)
            {
                il.SetImage(img);
            }
        }

        private System.Drawing.Image GetImageFromClipboard()
        {
            if (Clipboard.GetDataObject() == null) return null;
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Dib))
            {
                var dib = ((System.IO.MemoryStream)Clipboard.GetData(DataFormats.Dib)).ToArray();
                var width = BitConverter.ToInt32(dib, 4);
                var height = BitConverter.ToInt32(dib, 8);
                var bpp = BitConverter.ToInt16(dib, 14);
                if (bpp == 32)
                {
                    var gch = GCHandle.Alloc(dib, GCHandleType.Pinned);
                    Bitmap bmp = null;
                    try
                    {
                        var ptr = new IntPtr((long)gch.AddrOfPinnedObject() + 52);
                        bmp = new Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ptr);
                        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        return new Bitmap(bmp);
                    }
                    finally
                    {
                        gch.Free();
                        if (bmp != null) bmp.Dispose();
                    }
                }
            }
            return Clipboard.ContainsImage() ? Clipboard.GetImage() : null;
        }

        private void buttonShowCells_Click(object sender, EventArgs e)
        {
            il.DrawCells();
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
