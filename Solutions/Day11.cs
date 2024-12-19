using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day11 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day11) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;
            List<long> input = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                input = line.Split(" ").Select(s => long.Parse(s)).ToList();
            }

            foreach (long num in input)
            {
                sum += Blink(num, 25);
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;
            List<long> input = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                input = line.Split(" ").Select(s => long.Parse(s)).ToList();
            }

            foreach( long num in input)
            {
                sum += Blink(num, 75);
            }

            Console.WriteLine(sum);
        }

        Dictionary<(long number, int depth), long> visited = [];
        public long Blink(long number, int depth)
        {
            if (depth == 0)
            {
                return 1;
            }

            if (!visited.ContainsKey((number, depth)))
            {
                long res;
                if (number == 0)
                {
                    res = Blink(1, depth - 1);
                }
                else
                {
                    int digits = (int)Math.Floor(Math.Log(number, 10)) + 1;

                    if (digits % 2 == 0)
                    {
                        var right = number % (int)Math.Pow(10, digits / 2);
                        var left = (number - right) / (int)Math.Pow(10, digits / 2);
                        res = Blink(left, depth - 1) + Blink(right, depth - 1);
                    }
                    else
                    {
                        res = Blink(number * 2024, depth - 1);
                    }
                }

                visited[(number, depth)] = res;
            }

            return visited[(number, depth)];
        }
    }
}
