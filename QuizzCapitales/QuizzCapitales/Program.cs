using System.Globalization;
using System.Text;
using System;

namespace QuizzCapitales
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Quizz.Jouer();
        }
    }
}