using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
{
    internal class Day16 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day16) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            List<List<char>> grid = new();
            int r = -1;
            int c = -1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                int start_c = line.IndexOf('S');
                if (start_c != -1)
                {
                    r = grid.Count;
                    c = start_c;
                }

                grid.Add(line.ToList());
            }

            sum = MazeSolve(grid, r, c);

            Console.WriteLine(sum);
        }

        private List<(int r, int c)> directions = new List<(int r, int c)>()
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        };

        public int MazeSolve(List<List<char>> grid, int r, int c)
        {
            PriorityQueue<(int r, int c, int d, int s), int> queue = new();
            HashSet<(int r, int c, int d)> visited = new();

            queue.Enqueue((r, c, 1, 0), 0);

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.r, curr.c, curr.d)))
                {
                    continue;
                }

                if (grid[curr.r][curr.c] == '#')
                {
                    continue;
                }

                if (grid[curr.r][curr.c] == 'E')
                {
                    return curr.s;
                }

                visited.Add((curr.r, curr.c, curr.d));

                var dir = directions[curr.d];
                var next_r = curr.r + dir.r;
                var next_c = curr.c + dir.c;
                var dir_l = (curr.d + 3) % 4;
                var dir_r = (curr.d + 1) % 4;


                queue.Enqueue((next_r, next_c, curr.d, curr.s + 1), curr.s + 1);
                queue.Enqueue((curr.r, curr.c, dir_l, curr.s + 1000), curr.s + 1000);
                queue.Enqueue((curr.r, curr.c, dir_r, curr.s + 1000), curr.s + 1000);
            }

            return -1;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            List<List<char>> grid = new();
            int r = -1;
            int c = -1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                int start_c = line.IndexOf('S');
                if (start_c != -1)
                {
                    r = grid.Count;
                    c = start_c;
                }

                grid.Add(line.ToList());
            }

            sum = MazeSolve2(grid, r, c);

            Console.WriteLine(sum);
        }

        public int MazeSolve2(List<List<char>> grid, int r, int c)
        {
            PriorityQueue<(int r, int c, int d, int s), int> queue = new();
            Dictionary<(int r, int c, int d), int> visited = new();

            queue.Enqueue((r, c, 1, 0), 0);

            int endR = -1;
            int endC = -1;
            int endD = -1;
            int bestScore = int.MaxValue;

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (curr.s > bestScore)
                {
                    continue;
                }

                if (visited.ContainsKey((curr.r, curr.c, curr.d)))
                {
                    continue;
                }

                if (grid[curr.r][curr.c] == '#')
                {
                    continue;
                }

                if (grid[curr.r][curr.c] == 'E')
                {
                    visited.Add((curr.r, curr.c, curr.d), curr.s);
                    bestScore = curr.s;
                    endR = curr.r;
                    endC = curr.c;
                    endD = curr.d;
                    continue;
                }

                visited.Add((curr.r, curr.c, curr.d), curr.s);

                var dir = directions[curr.d];
                var next_r = curr.r + dir.r;
                var next_c = curr.c + dir.c;
                var dir_l = (curr.d + 3) % 4;
                var dir_r = (curr.d + 1) % 4;

                queue.Enqueue((next_r, next_c, curr.d, curr.s + 1), curr.s + 1);
                queue.Enqueue((curr.r, curr.c, dir_l, curr.s + 1000), curr.s + 1000);
                queue.Enqueue((curr.r, curr.c, dir_r, curr.s + 1000), curr.s + 1000);
            }

            HashSet<(int r, int c, int d)> bestPaths = new();
            Stack<(int r, int c, int d, int s)> stack = new();

            stack.Push((endR, endC, endD, bestScore));

            while (stack.Count > 0)
            {
                var curr = stack.Pop();
                if (grid[curr.r][curr.c] == '#')
                {
                    continue;
                }

                if (!visited.ContainsKey((curr.r, curr.c, curr.d)))
                {
                    continue;
                }

                if (visited[(curr.r, curr.c, curr.d)] != curr.s)
                {
                    continue;
                }

                bestPaths.Add((curr.r, curr.c, curr.d));

                var dir = directions[curr.d];
                var next_r = curr.r - dir.r;
                var next_c = curr.c - dir.c;
                var dir_l = (curr.d + 3) % 4;
                var dir_r = (curr.d + 1) % 4;

                stack.Push((next_r, next_c, curr.d, curr.s - 1));
                stack.Push((curr.r, curr.c, dir_l, curr.s - 1000));
                stack.Push((curr.r, curr.c, dir_r, curr.s - 1000));
            }

            HashSet<(int r, int c)> bestNodes = new(bestPaths.Select(bp => (bp.r, bp.c)));

            return bestNodes.Count;
        }
    }
}
