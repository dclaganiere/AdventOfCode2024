using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day2 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day2) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var list = line.Split(' ').Select(i => int.Parse(i)).ToList();

                bool isInc = false;
                bool isSafe = true;
                for (int i = 1; i < list.Count; i++)
                {
                    if (i == 1)
                    {
                        isInc = list[i] > list[i - 1];
                    }

                    if (isInc != (list[i] > list[i - 1]))
                    {
                        isSafe = false;
                        break;
                    }

                    if (Math.Abs(list[i] - list[i - 1]) > 3 || list[i] == list[i - 1])
                    {
                        isSafe = false;
                        break;
                    }
                }

                if (isSafe)
                {
                    sum++;
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
                var list = line.Split(' ').Select(i => int.Parse(i)).ToList();

                bool isSafe = testList(list);

                if (!isSafe)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var newList = new List<int>(list);

                        newList.RemoveAt(i);
                        bool isSafe2 = testList(newList);

                        if (isSafe2)
                        {
                            isSafe = true;
                            break;
                        }
                    }
                }

                if (isSafe)
                {
                    sum++;
                }
            }

            Console.WriteLine(sum);

        }

        private static bool testList(List<int> list)
        {
            bool isInc = false;
            bool isSafe = true;
            for (int i = 1; i < list.Count; i++)
            {
                if (i == 1)
                {
                    isInc = list[i] > list[i - 1];
                }

                if (isInc != (list[i] > list[i - 1]))
                {
                    isSafe = false;
                    break;
                }

                if (Math.Abs(list[i] - list[i - 1]) > 3 || list[i] == list[i - 1])
                {
                    isSafe = false;
                    break;
                }
            }

            return isSafe;
        }
    }
}
