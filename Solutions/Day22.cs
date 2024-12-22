using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day22 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day22) + ".txt";
        const int Modulo = 16777216;
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long initial = long.Parse(line);
                long num = long.Parse(line);
                
                for (int i = 0; i < 2000; i++)
                {
                    num = MixAndPrune(num, 6, true);
                    num = MixAndPrune(num, 5, false);
                    num = MixAndPrune(num, 11, true);
                }
                sum += num;
            }

            Console.WriteLine(sum);
        }

        public long MixAndPrune(long num, int by, bool isMul)
        {
            long x;
            if (isMul)
            {
                x = num << by;
            }
            else
            {
                x = num >> by;
            }
            num ^= x;
            num %= Modulo;
            return num;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);

            Dictionary<(int a, int b, int c, int d), int> bananas = [];
            HashSet<(int a, int b, int c, int d)> visited = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long initial = long.Parse(line);
                long num = long.Parse(line);

                List<int> prevs = [0, 0, 0, 0];
                visited.Clear();
                for (int i = 0; i < 2000; i++)
                {
                    int lastPrice = (int)(num % 10);
                    num = MixAndPrune(num, 6, true);
                    num = MixAndPrune(num, 5, false);
                    num = MixAndPrune(num, 11, true);

                    for (int j = 0; j < 3; j++)
                    {
                        prevs[j] = prevs[j + 1];
                    }
                    int price = (int)(num % 10);
                    prevs[3] = price - lastPrice;

                    if (i >= 3)
                    {
                        var key = (prevs[0], prevs[1], prevs[2], prevs[3]);

                        if (!visited.Contains(key))
                        {
                            if (bananas.ContainsKey(key))
                            {
                                bananas[key] += price;
                            }
                            else
                            {
                                bananas[key] = price;
                            }
                            visited.Add(key);
                        }
                    }
                }
            }

            var res = bananas.OrderByDescending(kv => kv.Value).First();

            Console.WriteLine(res.Value);
        }
    }
}
