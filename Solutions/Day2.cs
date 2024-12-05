using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
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

                //Console.WriteLine($"{(isSafe ? "":"un")}safe");

                if (isSafe)
                {
                    sum++;
                }
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            //using StreamReader reader = new(InputFile);
            //int sum = 0;
            //while (!reader.EndOfStream)
            //{
            //    string line = reader.ReadLine() ?? string.Empty;
            //    var list = line.Split(' ').Select(i => int.Parse(i)).ToList();

            //    bool isInc = false;
            //    bool isSafe = true;

            //    for (int i = 1; i < list.Count; i++)
            //    {
            //        int curr = i;
            //        int prev = i - 1;

            //        if (curr == 1)
            //        {
            //            isInc = list[curr] > list[prev];
            //        }

            //        if (isInc != (list[curr] > list[prev]))
            //        {
            //            isSafe = false;
            //            break;
            //        }

            //        int diff = Math.Abs(list[curr] - list[prev]);
            //        if (diff > 3 || diff == 0)
            //        {
            //            isSafe = false;
            //            break;
            //        }
            //    }

            //    if (!isSafe)
            //    {
            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            bool isSafe2 = true;
            //            for (int j = 1; j < list.Count; j++)
            //            {
            //                int curr = j;
            //                int prev = j - 1;

            //                if (i == 0 && j == 1)
            //                {
            //                    isInc = list[2] > list[1];
            //                    continue;
            //                }

            //                if (i == curr)
            //                {
            //                    curr++;
            //                    if (curr == list.Count)
            //                    {
            //                        break;
            //                    }
            //                }

            //                if (prev == i)
            //                {
            //                    prev--;
            //                }

            //                if (curr == 1)
            //                {
            //                    isInc = list[curr] > list[prev];
            //                }

            //                if (isInc != (list[curr] > list[prev]))
            //                {
            //                    isSafe2 = false;
            //                    break;
            //                }

            //                int diff = Math.Abs(list[curr] - list[prev]);
            //                if (diff > 3 || diff == 0)
            //                {
            //                    isSafe2 = false;
            //                    break;
            //                }
            //            }

            //            if (isSafe2)
            //            {
            //                Console.Write($"{i} - ");
            //                isSafe = true;
            //                break;
            //            }
            //        }
            //    }

            //    Console.WriteLine($"{(isSafe ? "" : "un")}safe");

            //    if (isSafe)
            //    {
            //        sum++;
            //    }
            //}

            //Console.WriteLine(sum);

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

                    bool isSafeBad = testList2(list);

                    if (isSafeBad != isSafe)
                    {
                        foreach (int i in list)
                        {
                            Console.Write($"{i} ");
                        }

                        Console.WriteLine($"Is {isSafe}");
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

        private static bool testList2(List<int> list)
        {
            bool isInc = false;
            for (int i = 0; i < list.Count; i++)
            {
                bool isSafe2 = true;
                for (int j = 1; j < list.Count; j++)
                {
                    int curr = j;
                    int prev = j - 1;

                    if (i == 0 && j == 1)
                    {
                        isInc = list[2] > list[1];
                        continue;
                    }

                    if (i == curr)
                    {
                        curr++;
                        if (curr == list.Count)
                        {
                            break;
                        }
                    }

                    if (prev == i)
                    {
                        prev--;
                    }

                    if (j == 1)
                    {
                        isInc = list[curr] > list[prev];
                    }

                    if (isInc != (list[curr] > list[prev]))
                    {
                        isSafe2 = false;
                        break;
                    }

                    int diff = Math.Abs(list[curr] - list[prev]);
                    if (diff > 3 || diff == 0)
                    {
                        isSafe2 = false;
                        break;
                    }
                }

                if (isSafe2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
