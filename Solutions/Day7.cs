using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day7 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day7) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            List<(long value, List<long> list)> input = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                var sp = line.Split(": ");
                var value = long.Parse(sp[0]);
                var list = sp[1].Split(' ').Select(x => long.Parse(x)).ToList();
                input.Add((value, list));
            }

            foreach ((long value, List<long> list) in input)
            {
                if (IsPossible(value, list, "+*"))
                {
                    sum += value;
                }
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            List<(long value, List<long> list)> input = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                var sp = line.Split(": ");
                var value = long.Parse(sp[0]);
                var list = sp[1].Split(' ').Select(x => long.Parse(x)).ToList();
                input.Add((value, list));
            }

            foreach ((long value, List<long> list) in input)
            {
                if (IsPossible(value, list, "+*|"))
                {
                    sum += value;
                }
            }

            Console.WriteLine(sum);
        }

        private bool IsPossible(long value, List<long> list, string operations)
        {
            Stack<(long value, int pos, char op)> st = new();
            foreach (char c in operations)
            {
                st.Push((list[0], 1, c));
            }

            while (st.Count > 0)
            {
                (long curr, int pos, char op) = st.Pop();

                if (pos == list.Count)
                {
                    if (value == curr)
                    {
                        return true;
                    }
                    continue;
                }

                if (value < curr)
                {
                    continue;
                }

                switch (op)
                {
                    case '+':
                        long next = curr + list[pos];
                        foreach (char c in operations)
                        {
                            st.Push((next, pos + 1, c));
                        }
                        break;
                    case '*':
                        long next2 = curr * list[pos];
                        foreach (char c in operations)
                        {
                            st.Push((next2, pos + 1, c));
                        }
                        break;
                    case '|':
                        int digits = (int)Math.Floor(Math.Log(list[pos], 10)) + 1;
                        long next3 = curr * (int)(Math.Pow(10, digits)) + list[pos];
                        foreach (char c in operations)
                        {
                            st.Push((next3, pos + 1, c));
                        }
                        break;
                    default:
                        break;
                }
            }

            return false;
        }
    }
}
