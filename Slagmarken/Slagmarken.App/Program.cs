using Slagmarken.Common;
using System;
using static System.Console;


namespace Slagmarken.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var hello = HelloSayer.GiveMeAHello();
            WriteLine(hello);
        }
    }
}
