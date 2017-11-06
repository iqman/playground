using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _10x10Solver.Bricks;

namespace _10x10Solver
{
    class BoardRenderer
    {
        private const float Gbw = 2;
        private const int NextBricksAreaHeight = 120;
        private const int NextBrickSetMargin = 10;

        private readonly Board board;
        private readonly NextBricksSet nextBrickSet;
        private Size viewportSize;
        private SizeF cellSize;
        private SizeF boardSize;
        private readonly IList<RectangleF> nextBrickAreas;
        public Image RenderingImage { get; set; }

        public event Action RenderingComplete;

        public void OnRenderingComplete()
        {
            Action handler = RenderingComplete;
            if (handler != null) handler();
        }

        public BoardRenderer(Board board, NextBricksSet nextBrickSet, Size viewportSize)
        {
            this.board = board;
            this.nextBrickSet = nextBrickSet;
            this.viewportSize = viewportSize;

            nextBrickAreas = new List<RectangleF>(Enumerable.Range(0, NextBricksSet.NextBricksCount).Select(x => RectangleF.Empty));
            nextBrickSet.BrickSetGenerated += RenderBoard;

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
                                  this.viewportSize.Height - NextBrickSetMargin - NextBricksAreaHeight);
            cellSize = new SizeF((boardSize.Width - Gbw) / Board.BoardSize,
                                 (boardSize.Height - Gbw) / Board.BoardSize);

            float nextBrickWidth = (boardSize.Width - NextBrickSetMargin * (NextBricksSet.NextBricksCount + 1)) / NextBricksSet.NextBricksCount;
            const float nextBrickHeight = NextBricksAreaHeight - NextBrickSetMargin;
            if (nextBrickWidth > nextBrickHeight)
            {
                nextBrickWidth = nextBrickHeight;
            }
            for (int i = 0; i < NextBricksSet.NextBricksCount; i++)
            {
                nextBrickAreas[i] = new RectangleF(
                    NextBrickSetMargin*(i+1) + i*nextBrickWidth,
                    boardSize.Height + NextBrickSetMargin,
                    nextBrickWidth,
                    nextBrickHeight
                    );
            }
        }

        public void RenderBoard()
        {
            using (Graphics g = Graphics.FromImage(RenderingImage))
            {
                DrawBoard(g, board, cellSize, new PointF(0,0), new Size(Board.BoardSize, Board.BoardSize));
                DrawNextBricks(g);
            }

            OnRenderingComplete();
        }

        private void DrawBoard(Graphics g, Board b, SizeF drawCellSize, PointF boardTopLeft, Size boardCellCount)
        {
            // draw grid
            Pen gridPen = new Pen(Color.Gray, Gbw);
            float gbw2 = Gbw / 2;
            for (int y = 0; y < boardCellCount.Width + 1; y++)
            {
                g.DrawLine(gridPen,
                    boardTopLeft.X + drawCellSize.Width * y + gbw2,
                    boardTopLeft.Y,
                    boardTopLeft.X + drawCellSize.Width * y + gbw2,
                    boardTopLeft.Y + drawCellSize.Height * boardCellCount.Height + Gbw);
            }
            for (int x = 0; x < boardCellCount.Height + 1; x++)
            {
                g.DrawLine(gridPen,
                    boardTopLeft.X,
                    boardTopLeft.Y + drawCellSize.Height * x + gbw2,
                    boardTopLeft.X + drawCellSize.Width * boardCellCount.Width + Gbw,
                    boardTopLeft.Y + drawCellSize.Height * x + gbw2);
            }

            // draw board cells
            for (int x = 0; x < boardCellCount.Width; x++)
            {
                for (int y = 0; y < boardCellCount.Height; y++)
                {
                    RectangleF cellInterior = new RectangleF(
                    drawCellSize.Width * x + Gbw + boardTopLeft.X,
                    drawCellSize.Height * y + Gbw + boardTopLeft.Y,
                    drawCellSize.Width - Gbw,
                    drawCellSize.Height - Gbw);

                    var color = FieldValueToColor(b.Fields[x, y]);
                    g.FillRectangle(new SolidBrush(color), cellInterior);
                }
            }
        }

        private void DrawNextBricks(Graphics g)
        {
            for (int i = 0; i < nextBrickAreas.Count; i++)
            {
                g.FillRectangle(new SolidBrush(SystemColors.Control), nextBrickAreas[i]);
                g.DrawLines(Pens.Black, new[]
                                                {
                                                    new PointF(nextBrickAreas[i].Left, nextBrickAreas[i].Top),
                                                    new PointF(nextBrickAreas[i].Right, nextBrickAreas[i].Top),
                                                    new PointF(nextBrickAreas[i].Right, nextBrickAreas[i].Bottom),
                                                    new PointF(nextBrickAreas[i].Left, nextBrickAreas[i].Bottom),
                                                    new PointF(nextBrickAreas[i].Left, nextBrickAreas[i].Top),
                                                });
                var brick = nextBrickSet.NextBricks[i];
                if (brick != null)
                {
                    var area = nextBrickAreas[i];

                    var nextBrickCellSize = new SizeF((area.Width - Gbw) / BrickFactory.MaxBrickLength,
                             (area.Height - Gbw) / BrickFactory.MaxBrickLength);

                    var boardTopLeft = area.Location;
                    boardTopLeft.Y += (nextBrickCellSize.Height) * (BrickFactory.MaxBrickLength - brick.Height) / 2f;
                    boardTopLeft.X += (nextBrickCellSize.Width) * (BrickFactory.MaxBrickLength - brick.Width) / 2f;

                    var b = new Board();
                    b.PutBrick(brick, new Point(0, 0));
                    DrawBoard(g, b, nextBrickCellSize, boardTopLeft, new Size(brick.Width, brick.Height));
                }
            }
        }

        public bool IsInsideBoard(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < boardSize.Width && point.Y < boardSize.Height;
        }

        public bool IsInsideNextBrickArea(Point p)
        {
            return nextBrickAreas.Any(a => a.Contains(p));
        }

        public int NextBrickAreaIndex(Point p)
        {
            for (int i = 0; i < nextBrickAreas.Count; i++)
            {
                if (nextBrickAreas[i].Contains(p))
                {
                    return i;
                }
            }
            throw new Exception("Not inside any next brick area");
        }

        public void StartDraggingNextBrick(int index)
        {
            
        }

        public Point ToBoardCoordinates(Point point)
        {
            return new Point(point.X/(int) cellSize.Width, point.Y/(int) cellSize.Height);
        }

        private Color FieldValueToColor(FieldValue fieldValue)
        {
            switch (fieldValue)
            {
                case FieldValue.Free:
                    return Color.Black;
                case FieldValue.Red:
                    return Color.Red;
                case FieldValue.Blue:
                    return Color.Blue;
                case FieldValue.Green:
                    return Color.Green;
                case FieldValue.Brown:
                    return Color.Brown;
                case FieldValue.Purple:
                    return Color.Purple;
                case FieldValue.Teal:
                    return Color.Teal;
                case FieldValue.Yellow:
                    return Color.Yellow;
                case FieldValue.LightBlue:
                    return Color.LightBlue;
                case FieldValue.Orange:
                    return Color.Orange;
                case FieldValue.LightGreen:
                    return Color.LightGreen;
                case FieldValue.IndianRed:
                    return Color.IndianRed;
                default:
                    throw new ArgumentOutOfRangeException("fieldValue");
            }
        }
    }
}
