using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day12 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day12) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<char>> grid = new();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (grid.Count == 0)
                {
                    grid.Add(Enumerable.Repeat('.', line.Length + 2).ToList());
                }
                grid.Add([.. $".{line}."]);
            }
            grid.Add(grid[0]);

            HashSet<(int r, int c)> visited = new();

            for (int r = 1; r < grid.Count - 1; r++)
            {
                for (int c = 1; c < grid[r].Count - 1; c++)
                {
                    if (!visited.Contains((r, c)))
                    {
                        var res = FloodFill(grid, r, c);
                        visited.UnionWith(res.visited);
                        sum += res.perimeter * res.area;
                    }
                }
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

        private (HashSet<(int r, int c)> visited, int perimeter, int area) FloodFill(List<List<char>> grid, int r, int c)
        {
            HashSet<(int r, int c)> visited = new();

            char plant = grid[r][c];

            Stack<(int r, int c)> stack = new();
            stack.Push((r, c));
            while (stack.Count > 0)
            {
                var curr = stack.Pop();

                if (visited.Contains(curr) || grid[curr.r][curr.c] != plant)
                {
                    continue;
                }

                visited.Add(curr);

                var neighbors = directions.Select(d => (d.r + curr.r, d.c + curr.c));
                foreach (var n in neighbors)
                {
                    stack.Push(n);
                }
            }

            int area = visited.Count;
            int perimeter = 4 * area;

            foreach(var v in visited)
            {
                var neighbors = directions.Select(d => (d.r + v.r, d.c + v.c));
                perimeter -= neighbors.Count(visited.Contains);
            }

            return (visited, perimeter, area);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<char>> grid = new();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (grid.Count == 0)
                {
                    grid.Add(Enumerable.Repeat('.', line.Length + 2).ToList());
                }
                grid.Add([.. $".{line}."]);
            }
            grid.Add(grid[0]);

            HashSet<(int r, int c)> visited = new();

            for (int r = 1; r < grid.Count - 1; r++)
            {
                for (int c = 1; c < grid[r].Count - 1; c++)
                {
                    if (!visited.Contains((r, c)))
                    {
                        var res = FloodFill2(grid, r, c);
                        visited.UnionWith(res.visited);
                        sum += res.sides * res.area;
                    }
                }
            }

            Console.WriteLine(sum);
        }

        private (HashSet<(int r, int c)> visited, int sides, int area) FloodFill2(List<List<char>> grid, int r, int c)
        {
            HashSet<(int r, int c)> visited = new();

            char plant = grid[r][c];

            Stack<(int r, int c)> stack = new();
            stack.Push((r, c));
            while (stack.Count > 0)
            {
                var curr = stack.Pop();

                if (visited.Contains(curr) || grid[curr.r][curr.c] != plant)
                {
                    continue;
                }

                visited.Add(curr);

                var neighbors = directions.Select(d => (d.r + curr.r, d.c + curr.c));
                foreach (var n in neighbors)
                {
                    stack.Push(n);
                }
            }

            int area = visited.Count;
            int sides = 0;

            Dictionary<int, SortedSet<int>> up = new();
            Dictionary<int, SortedSet<int>> down = new();
            Dictionary<int, SortedSet<int>> left = new();
            Dictionary<int, SortedSet<int>> right = new();

            foreach (var row in visited.Select(v => v.r))
            {
                if (!up.ContainsKey(row))
                {
                    up.Add(row, new SortedSet<int>());
                }
                if (!up.ContainsKey(row + 1))
                {
                    up.Add(row + 1, new SortedSet<int>());
                }
                if (!down.ContainsKey(row))
                {
                    down.Add(row, new SortedSet<int>());
                }
                if (!down.ContainsKey(row + 1))
                {
                    down.Add(row + 1, new SortedSet<int>());
                }
            }
            foreach (var col in visited.Select(v => v.c))
            {
                if (!left.ContainsKey(col))
                {
                    left.Add(col, new SortedSet<int>());
                }
                if (!left.ContainsKey(col + 1))
                {
                    left.Add(col + 1, new SortedSet<int>());
                }
                if (!right.ContainsKey(col))
                {
                    right.Add(col, new SortedSet<int>());
                }
                if (!right.ContainsKey(col + 1))
                {
                    right.Add(col + 1, new SortedSet<int>());
                }
            }

            foreach (var v in visited)
            {
                if (down[v.r].Contains(v.c))
                {
                    down[v.r].Remove(v.c);
                }
                else
                {
                    up[v.r].Add(v.c);
                }

                if (up[v.r + 1].Contains(v.c))
                {
                    up[v.r + 1].Remove(v.c);
                }
                else
                {
                    down[v.r + 1].Add(v.c);
                }

                if (right[v.c].Contains(v.r))
                {
                    right[v.c].Remove(v.r);
                }
                else
                {
                    left[v.c].Add(v.r);
                }

                if (left[v.c + 1].Contains(v.r))
                {
                    left[v.c + 1].Remove(v.r);
                }
                else
                {
                    right[v.c + 1].Add(v.r);
                }
            }

            foreach (var h in up.Where(kv => kv.Value.Count > 0))
            {
                int curr = h.Value.First();
                foreach (var v in h.Value.Skip(1))
                {
                    if (v - curr > 1)
                    {
                        sides++;
                    }
                    curr = v;
                }
                sides++;
            }

            foreach (var h in down.Where(kv => kv.Value.Count > 0))
            {
                int curr = h.Value.First();
                foreach (var v in h.Value.Skip(1))
                {
                    if (v - curr > 1)
                    {
                        sides++;
                    }
                    curr = v;
                }
                sides++;
            }

            foreach (var v in left.Where(kv => kv.Value.Count > 0))
            {
                int curr = v.Value.First();
                foreach (var h in v.Value.Skip(1))
                {
                    if (h - curr > 1)
                    {
                        sides++;
                    }
                    curr = h;
                }
                sides++;
            }

            foreach (var v in right.Where(kv => kv.Value.Count > 0))
            {
                int curr = v.Value.First();
                foreach (var h in v.Value.Skip(1))
                {
                    if (h - curr > 1)
                    {
                        sides++;
                    }
                    curr = h;
                }
                sides++;
            }

            return (visited, sides, area);
        }
    }
}
