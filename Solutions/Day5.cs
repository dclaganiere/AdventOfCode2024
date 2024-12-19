using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day5 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day5) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            Dictionary<int, List<int>> rules = new();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var sp = line.Split('|');
                var l = int.Parse(sp[0]);
                var r = int.Parse(sp[1]);

                if (!rules.ContainsKey(l))
                {
                    rules.Add(l, new List<int>());
                }
                rules[l].Add(r);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var list = line.Split(",").Select(l => int.Parse(l)).ToList();

                var dict = new Dictionary<int, int>();
                for (int i = 0; i < list.Count; i++)
                {
                    dict[list[i]] = i;
                }

                bool isValid = true;
                foreach(int i in list)
                {
                    if (rules.TryGetValue(i, out List<int>? value))
                    {
                        foreach (int j in value)
                        {
                            if (dict.ContainsKey(j))
                            {
                                if (dict[j] < dict[i])
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (isValid)
                {
                    sum += list[list.Count / 2];
                }
            }


            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            Dictionary<int, HashSet<int>> rules = new();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var sp = line.Split('|');
                var l = int.Parse(sp[0]);
                var r = int.Parse(sp[1]);

                if (!rules.ContainsKey(l))
                {
                    rules.Add(l, new HashSet<int>());
                }
                rules[l].Add(r);
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var list = line.Split(",").Select(l => int.Parse(l)).ToList();

                var dict = new Dictionary<int, int>();
                for (int i = 0; i < list.Count; i++)
                {
                    dict[list[i]] = i;
                }

                bool isValid = true;
                foreach (int i in list)
                {
                    if (rules.TryGetValue(i, out HashSet<int>? value))
                    {
                        foreach (int j in value)
                        {
                            if (dict.ContainsKey(j))
                            {
                                if (dict[j] < dict[i])
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!isValid)
                {
                    var newOrder = new List<int>();
                    var found = new HashSet<int>();
                    var notFound = new HashSet<int>(list);

                    while (found.Count < list.Count)
                    {
                        int toAdd = -1;
                        foreach (var x in notFound)
                        {
                            bool canAdd = true;
                            foreach (var y in notFound.Where(i => i != x))
                            {
                                if (rules.TryGetValue(y, out HashSet<int>? value) && value.Contains(x))
                                {
                                    canAdd = false;
                                    break;
                                }
                            }

                            if (canAdd)
                            {
                                toAdd = x;
                                break;
                            }
                        }

                        if (toAdd > 0)
                        {
                            newOrder.Add(toAdd);
                            found.Add(toAdd);
                            notFound.Remove(toAdd);
                        }
                    }

                    sum += newOrder[list.Count / 2];
                }
            }


            Console.WriteLine(sum);
        }
    }
}
