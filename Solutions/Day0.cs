using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
{
    internal class Day0 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day0) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

            }

            Console.WriteLine(sum);
        }
    }
}
