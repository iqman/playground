
namespace _10x10Solver.Bricks
{
    struct Brick_2Plus2D : IBrick
    {
        public byte[,] Fields
        {
            get
            {
                return new byte[,]
                           {
                               {1, 1},
                               {1, 0}
                           };
            }
        }

        public int Width { get { return 2; } }
        public int Height { get { return 2; } }

        public FieldValue FieldValue { get
        {
            return FieldValue.Orange;
        }}
    }
}
