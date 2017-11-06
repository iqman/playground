using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _10x10Solver.Bricks;

namespace _10x10Solver
{
    class Board
    {
        public const int BoardSize = 10;
        public FieldValue[,] Fields { get; private set; }
        public int Score { get; set; }

        private bool isSimulating;
        private readonly FieldValue[,] fieldsBackup;

        public Board()
        {
            Fields = new FieldValue[BoardSize, BoardSize];
            fieldsBackup = new FieldValue[BoardSize, BoardSize];
        }

        public void PutBrick(IBrick brick, Point point)
        {
            CheckPosition(brick, point);
            WriteBrick(brick, point, brick.FieldValue);
            ClearFullRowsAndColumns();
        }

        private void ClearFullRowsAndColumns()
        {
            var range = Enumerable.Range(0, BoardSize).ToArray();
            
            for (int x = 0; x < BoardSize; x++)
            {
                if (range.All(y => Fields[x, y] != FieldValue.Free))
                {
                    for (int y = 0; y < BoardSize; y++)
                    {
                        Fields[x, y] = FieldValue.ToBeCleared;
                    }
                }
            }

            for (int y = 0; y < BoardSize; y++)
            {
                if (range.All(x => Fields[x, y] != FieldValue.Free))
                {
                    for (int x = 0; x < BoardSize; x++)
                    {
                        Fields[x, y] = FieldValue.ToBeCleared;
                    }
                }
            }

            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (Fields[x, y] == FieldValue.ToBeCleared)
                    {
                        Fields[x, y] = FieldValue.Free;
                        if (!isSimulating)
                        {
                            Score++;
                        }
                    }
                }
            }
        }

        public void StartSimulation()
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    fieldsBackup[x, y] = Fields[x, y];
                }
            }
            isSimulating = true;
        }

        public void ClearSimulation()
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    Fields[x, y] = fieldsBackup[x, y];
                }
            }
            isSimulating = false;
        }

        private void CheckPosition(IBrick brick, Point point)
        {
            if (!IsPositionValid(brick, point))
            {
                throw new Exception("Brick position is invalid");
            }
        }

        public bool IsInsideBoard(Point point)
        {
            return point.X >= 0 && point.X < BoardSize &&
                    point.Y >= 0 && point.Y < BoardSize;
        }

        public bool IsPositionValid(IBrick brick, Point point)
        {
            if (point.X < 0 || point.X + brick.Width > BoardSize ||
                point.Y < 0 || point.Y + brick.Height > BoardSize)
            {
                return false;
            }

            for (int x = 0; x < brick.Width; x++)
            {
                for (int y = 0; y < brick.Height; y++)
                {
                    if (brick.Fields[y,x] == 1 && Fields[point.X + x, point.Y + y] != FieldValue.Free)
                    {
                        return false; 
                    }
                }
            }

            return true;
        }

        private void WriteBrick(IBrick brick, Point point, FieldValue fieldValue)
        {
            for (int x = 0; x < brick.Width; x++)
            {
                for (int y = 0; y < brick.Height; y++)
                {
                    if (brick.Fields[y,x] == 1)
                    {
                        Fields[point.X + x, point.Y + y] = fieldValue;
                        if (!isSimulating)
                        {
                            Score++;
                        }
                    }
                }
            }
        }

        public void Seed()
        {
            PutBrick(new Brick_2X2(), new Point(0,0));
            PutBrick(new Brick_5X1(), new Point(2,2));
        }

        public void Clear()
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    Fields[x, y] = FieldValue.Free;
                }
            }
            Score = 0;
        }
    }
}
