
namespace _10x10Solver.Bricks
{
    struct Brick_3X1 : IBrick
    {
        public byte[,] Fields
        {
            get
            {
                return new byte[,]
                           {
                               {1,1,1}
                           };
            }
        }

        public int Width { get { return 3; } }
        public int Height { get { return 1; } }

        public FieldValue FieldValue { get
        {
            return FieldValue.Green;
        }}
    }
}
