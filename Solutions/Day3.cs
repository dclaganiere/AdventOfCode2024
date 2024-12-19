using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal partial class Day3 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day3) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var matches = Pattern().Matches(line);

                foreach(Match m in matches)
                {
                    var l = m.Groups[1].Value;
                    var r = m.Groups[2].Value;
                    sum += int.Parse(l) * int.Parse(r);
                }
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            bool shouldDo = true;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                var do_idxes = Dos().Matches(line);
                var dont_idxes = Donts().Matches(line);

                var pairs = new List<(int start, bool isDo)>();

                foreach (Match m in do_idxes)
                {
                    pairs.Add((m.Index, true));
                }
                foreach (Match m in dont_idxes)
                {
                    pairs.Add((m.Index, false));
                }

                pairs = pairs.OrderBy(x => x.start).ToList();

                List<string> run = new List<string>();
                int prev = 0;
                foreach(var p in pairs)
                {
                    int curr = p.start;

                    if (shouldDo)
                    {
                        run.Add(line[prev..curr]);
                    }

                    shouldDo = p.isDo;

                    if (shouldDo)
                    {
                        prev = curr + 4;
                    }
                    else
                    {
                        prev = curr + 7;
                    }
                }

                if (shouldDo)
                {
                    run.Add(line[prev..]);
                }

                foreach(var x in run)
                {
                    var matches = Pattern().Matches(x);
                    foreach (Match m in matches)
                    {
                        var l = m.Groups[1].Value;
                        var r = m.Groups[2].Value;
                        sum += int.Parse(l) * int.Parse(r);
                    }
                }

            }

            Console.WriteLine(sum);
        }

        [GeneratedRegex("mul\\((\\d+),(\\d+)\\)")]
        private static partial Regex Pattern();

        [GeneratedRegex("do\\(\\)")]
        private static partial Regex Dos();

        [GeneratedRegex("don't\\(\\)")]
        private static partial Regex Donts();
    }
}
