using System;
using System.Drawing;
using System.Linq;

namespace SudokuSolver
{
    public class BoardRenderer
    {
        private const float Gbw = 1;

        private readonly Board board;

        private Size viewportSize;
        private SizeF cellSize;
        private SizeF boardSize;

        public Image RenderingImage { get; set; }

        public event Action RenderingComplete;
        public void OnRenderingComplete()
        {
            Action handler = RenderingComplete;
            handler?.Invoke();
        }

        public BoardRenderer(Board s, Size viewportSize)
        {
            board = s;
            this.viewportSize = viewportSize;

            CreateImage();
        }

        public void UpdateViewportSize(Size viewportSize)
        {
            this.viewportSize = viewportSize;
            CreateImage();
            RenderBoard();
        }

        private void CreateImage()
        {
            if (RenderingImage != null)
            {
                RenderingImage.Dispose();
                RenderingImage = null;
            }

            RenderingImage = new Bitmap(this.viewportSize.Width, this.viewportSize.Height);

            boardSize = new SizeF(this.viewportSize.Width,
                                  this.viewportSize.Height);
            cellSize = new SizeF((boardSize.Width - Gbw) / Board.BoardSize,
                                 (boardSize.Height - Gbw) / Board.BoardSize);
        }

        public void RenderBoard()
        {
            using (Graphics g = Graphics.FromImage(RenderingImage))
            {
                DrawGrid(g, board, cellSize, new PointF(0, 0), new Size(Board.BoardSize, Board.BoardSize));
                DrawCellValues(g, board, cellSize, new PointF(0, 0));
            }

            OnRenderingComplete();
        }

        private void DrawCellValues(Graphics graphics, Board board, SizeF drawCellSize, PointF boardTopLeft)
        {
            Font valueFont = new Font(FontFamily.GenericMonospace, 25, FontStyle.Bold);
            Font fiftyFiftyFint = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);

            for (int y = 0; y < Board.BoardSize; y++)
            {
                for (int x = 0; x < Board.BoardSize; x++)
                {
                    var cell = board.GetCell(x, y);

                    if (cell.Excluded)
                    {
                        RectangleF cellInterior = new RectangleF(
                            drawCellSize.Width * x + Gbw*3 + boardTopLeft.X,
                            drawCellSize.Height * y + Gbw*3 + boardTopLeft.Y,
                            drawCellSize.Width - Gbw*6,
                            drawCellSize.Height - Gbw*6);

                        graphics.FillRectangle(new SolidBrush(Color.LightCoral), cellInterior);
                    }

                    if (cell.Highlight)
                    {
                        RectangleF cellInterior = new RectangleF(
                            drawCellSize.Width * x + Gbw * 8 + boardTopLeft.X,
                            drawCellSize.Height * y + Gbw * 8 + boardTopLeft.Y,
                            drawCellSize.Width - Gbw * 16,
                            drawCellSize.Height - Gbw * 16);

                        graphics.FillRectangle(new SolidBrush(Color.LightBlue), cellInterior);
                    }

                    if (cell.FiftyFiftyExclusion)
                    {
                        RectangleF cellInterior = new RectangleF(
                            drawCellSize.Width * x + Gbw * 12 + boardTopLeft.X,
                            drawCellSize.Height * y + Gbw * 12 + boardTopLeft.Y,
                            drawCellSize.Width - Gbw * 24,
                            drawCellSize.Height - Gbw * 24);

                        graphics.FillRectangle(new SolidBrush(Color.DarkOrange), cellInterior);
                    }

                    if (cell.Guessed)
                    {
                        RectangleF cellInterior = new RectangleF(
                            drawCellSize.Width * x + Gbw * 16 + boardTopLeft.X,
                            drawCellSize.Height * y + Gbw * 16 + boardTopLeft.Y,
                            drawCellSize.Width - Gbw * 32,
                            drawCellSize.Height - Gbw * 32);

                        graphics.FillRectangle(new SolidBrush(Color.MediumPurple), cellInterior);
                    }

                    if (cell.Value != SudokuCell.EmptyValue)
                    {
                        var s = cell.Value.ToString();
                        var stringSize = graphics.MeasureString(s, valueFont);

                        PointF textPoint = new PointF(
                            boardTopLeft.X + drawCellSize.Width * x + drawCellSize.Width / 2 - stringSize.Width / 2,
                            boardTopLeft.Y + drawCellSize.Height * y + drawCellSize.Height / 2 - stringSize.Height / 2 +
                            Gbw * 3
                        );

                        graphics.DrawString(s, valueFont, Brushes.Black, textPoint);
                    }

                    if (cell.FiftyFifties.Any())
                    {
                        string fiftyFifties= string.Join(",", cell.FiftyFifties);

                        PointF textPoint = new PointF(
                            boardTopLeft.X + drawCellSize.Width * x,
                            boardTopLeft.Y + drawCellSize.Height * y
                        );

                        graphics.DrawString(fiftyFifties, fiftyFiftyFint, Brushes.Blue, textPoint);
                    }
                }
            }
        }

        private void DrawGrid(Graphics g, Board b, SizeF drawCellSize, PointF boardTopLeft, Size boardCellCount)
        {
            g.Clear(Color.Wheat);

            // draw grid
            Pen gridPen = new Pen(Color.Gray, Gbw);
            Pen thickGridPen = new Pen(Color.Gray, Gbw*4);
            float gbw2 = Gbw / 2;
            for (int y = 0; y < boardCellCount.Width + 1; y++)
            {
                g.DrawLine(y % 3 == 0 ? thickGridPen : gridPen,
                    boardTopLeft.X + drawCellSize.Width * y + gbw2,
                    boardTopLeft.Y,
                    boardTopLeft.X + drawCellSize.Width * y + gbw2,
                    boardTopLeft.Y + drawCellSize.Height * boardCellCount.Height + Gbw);
            }
            for (int x = 0; x < boardCellCount.Height + 1; x++)
            {
                g.DrawLine(x % 3 == 0 ? thickGridPen : gridPen,
                    boardTopLeft.X,
                    boardTopLeft.Y + drawCellSize.Height * x + gbw2,
                    boardTopLeft.X + drawCellSize.Width * boardCellCount.Width + Gbw,
                    boardTopLeft.Y + drawCellSize.Height * x + gbw2);
            }

            //// draw board cells
            //for (int x = 0; x < boardCellCount.Width; x++)
            //{
            //    for (int y = 0; y < boardCellCount.Height; y++)
            //    {
            //        RectangleF cellInterior = new RectangleF(
            //        drawCellSize.Width * x + Gbw + boardTopLeft.X,
            //        drawCellSize.Height * y + Gbw + boardTopLeft.Y,
            //        drawCellSize.Width - Gbw,
            //        drawCellSize.Height - Gbw);

            //        g.FillRectangle(new SolidBrush(Color.Wheat), cellInterior);
            //    }
            //}
        }
    }
}