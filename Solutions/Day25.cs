using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day25 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day25) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<int>> keys = [];
            List<List<int>> locks = [];
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (line.StartsWith('.'))
                {
                    List<int> key = Enumerable.Repeat(-1, line.Length).ToList();
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine() ?? string.Empty;

                        if (string.IsNullOrEmpty(line))
                        {
                            break;
                        }

                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == '#')
                            {
                                key[i]++;
                            }
                        }
                    }

                    keys.Add(key);
                }
                else
                {
                    List<int> lck = Enumerable.Repeat(0, line.Length).ToList();
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine() ?? string.Empty;

                        if (string.IsNullOrEmpty(line))
                        {
                            break;
                        }

                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == '#')
                            {
                                lck[i]++;
                            }
                        }
                    }

                    locks.Add(lck);
                }
            }

            int max = Math.Max(locks.Max(x => x.Max()), keys.Max(x => x.Max()));

            for (int i = 0; i <  keys.Count; i++)
            {
                for (int j = 0; j < locks.Count; j++)
                {
                    if (locks[j].Zip(keys[i]).All(x => x.First + x.Second <= max))
                    {
                        sum++;
                    }
                }
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
