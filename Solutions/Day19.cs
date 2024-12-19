using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day19 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day19) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            string tLine = reader.ReadLine() ?? string.Empty;

            var sp = tLine.Split(", ");
            List<string> towels = [.. sp];

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                bool isPossible = CanMakePattern(towels, line, 0);

                sum += isPossible ? 1 : 0;

            }

            Console.WriteLine(sum);
        }

        public static bool CanMakePattern(List<string> towels, string pattern, int idx)
        {
            if (idx == pattern.Length)
            {
                return true;
            }

            foreach (string towel in towels)
            {
                if (pattern[idx..].StartsWith(towel))
                {
                    if (CanMakePattern(towels, pattern, idx + towel.Length))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            string tLine = reader.ReadLine() ?? string.Empty;

            var sp = tLine.Split(", ");
            List<string> towels = [.. sp];

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                sum += CalcWaysMakePattern(towels, line, 0);

            }

            Console.WriteLine(sum);
        }

        private readonly Dictionary<string, long> lookup = [];
        public long CalcWaysMakePattern(List<string> towels, string pattern, int idx)
        {
            if (idx == pattern.Length)
            {
                return 1;
            }

            string substring = pattern[idx..];
            if (!lookup.TryGetValue(substring, out long value))
            {
                long ways = 0;
                foreach (string towel in towels)
                {
                    if (substring.StartsWith(towel))
                    {
                        ways += CalcWaysMakePattern(towels, pattern, idx + towel.Length);
                    }
                }

                value = ways;
                lookup[substring] = value;
            }

            return value;
        }
    }
}
