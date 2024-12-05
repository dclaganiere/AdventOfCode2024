﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
{
    internal class Day1 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day1) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split("   ");
                left.Add(int.Parse(sp[0]));
                right.Add(int.Parse(sp[1]));
            }

            left.Sort();
            right.Sort();

            foreach ((int l, int r) in left.Zip(right))
            {
                sum += Math.Abs(l-r);
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split("   ");
                left.Add(int.Parse(sp[0]));
                right.Add(int.Parse(sp[1]));
            }

            left.Sort();
            right.Sort();

            var dist = right.Distinct().ToList();
            var dict = new Dictionary<int, int>();
            foreach (var n in dist) 
            {
                dict[n] = right.Count(x => x==n);
            }

            foreach ((int l, int r) in left.Zip(right))
            {
                if (dict.ContainsKey(l))
                {
                    sum += l * dict[l];
                }
            }

            Console.WriteLine(sum);
        }
    }
}
