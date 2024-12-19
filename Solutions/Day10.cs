using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day10 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day10) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<int>> grid = [];
            List<(int r, int c)> trailheads = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var row = line.Select(c => (int)(c - '0')).ToList();

                row.Add(-1);
                row.Insert(0, -1);

                if (grid.Count == 0)
                {
                    grid.Add(Enumerable.Repeat(-1, row.Count).ToList());
                }

                grid.Add(row);
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] == 0)
                    {
                        trailheads.Add((grid.Count - 1, i));
                    }
                }
            }
            grid.Add(grid[0]);

            foreach (var (r, c) in trailheads)
            {
                sum += RateTrailhead(grid, r, c, true);
            }

            Console.WriteLine(sum);
        }

        private readonly List<(int r, int c)> directions =
        [
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        ];

        public int RateTrailhead(List<List<int>> grid, int startRow, int startCol, bool onlyUnique)
        {
            int reached = 0;
            HashSet<(int r, int c)> reachable = new();

            Stack<(int r, int c, int height)> stack = new();
            stack.Push((startRow, startCol, 0));

            while (stack.Count > 0)
            {
                var curr = stack.Pop();
                var actualHeight = grid[curr.r][curr.c];

                if (curr.height != actualHeight)
                {
                    continue;
                }

                if (curr.height == 9)
                {
                    reachable.Add((curr.r, curr.c));
                    reached++;
                    continue;
                }

                foreach (var d in directions)
                {
                    stack.Push((curr.r + d.r, curr.c + d.c, curr.height + 1));
                }
            }

            return onlyUnique ? reachable.Count : reached;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<int>> grid = [];
            List<(int r, int c)> trailheads = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var row = line.Select(c => (int)(c - '0')).ToList();

                row.Add(-1);
                row.Insert(0, -1);

                if (grid.Count == 0)
                {
                    grid.Add(Enumerable.Repeat(-1, row.Count).ToList());
                }

                grid.Add(row);
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] == 0)
                    {
                        trailheads.Add((grid.Count - 1, i));
                    }
                }
            }
            grid.Add(grid[0]);

            foreach (var (r, c) in trailheads)
            {
                sum += RateTrailhead(grid, r, c, false);
            }

            Console.WriteLine(sum);
        }
    }
}
