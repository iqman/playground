
namespace _10x10Solver.Bricks
{
    struct Brick_3X3 : IBrick
    {
        public byte[,] Fields
        {
            get
            {
                return new byte[,]
                           {
                               {1, 1, 1},
                               {1, 1, 1},
                               {1, 1, 1}
                           };
            }
        }

        public int Width { get { return 3; } }
        public int Height { get { return 3; } }

        public FieldValue FieldValue { get
        {
            return FieldValue.LightBlue;
        }}
    }
}
