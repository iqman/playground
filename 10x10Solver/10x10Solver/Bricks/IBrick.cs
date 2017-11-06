namespace _10x10Solver.Bricks
{
    interface IBrick
    {
        byte[,] Fields { get; }
        int Width { get; }
        int Height { get; }
        FieldValue FieldValue { get; }
    }
}
