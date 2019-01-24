
namespace Test
{
    using System;


    partial class Program
    {

        static Program()
        {
            Console.BufferHeight = short.MaxValue - 1;
        }

        private static void Main(string[] args)
        {
            Run(args);
            Console.ReadKey();
        }

        static partial void Run(string[] args);
    }
}
