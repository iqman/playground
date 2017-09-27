using System;

namespace Slagmarken.Common
{
    public class HelloSayer
    {
        static string HelloText { get; } = "Hello World";

        public static string GiveMeAHello () => HelloText;
    }
}
