using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8408();
            Welcome3723();
            Console.ReadKey();
        }
        static partial void Welcome3723();
        private static void Welcome8408()
        {
            Console.Write("Enter your name: ");
            string username = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", username);
        }
    }
}
