using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day20 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day20) + ".txt";
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

            int normalSteps = MazeSolve(grid, r, c);
            var dist = CalcDistToEnd(grid);
            sum = CheatMazeSolve(grid, dist, r, c, normalSteps, 2);
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
            Queue<(int r, int c, int s)> queue = new();
            HashSet<(int r, int c)> visited = new();

            queue.Enqueue((r, c, 0));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.r, curr.c)))
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

                visited.Add((curr.r, curr.c));

                foreach (var d in directions)
                {
                    queue.Enqueue((curr.r + d.r, curr.c + d.c, curr.s + 1));
                }
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

            int normalSteps = MazeSolve(grid, r, c);

            var dist = CalcDistToEnd(grid);
            sum = CheatMazeSolve(grid, dist, r, c, normalSteps, 20);

            Console.WriteLine(sum);
        }

        public Dictionary<(int r, int c), int> CalcDistToEnd(List<List<char>> grid)
        {
            Dictionary<(int r, int c), int> dist = [];

            for (int i = 1; i < grid.Count - 1; i++)
            {
                for (int j = 1; j < grid[i].Count - 1; j++)
                {
                    if (grid[i][j] != '#')
                    {
                        int curr = MazeSolve(grid, i, j);
                        dist.Add((i, j), curr);
                    }
                }
            }

            return dist;
        }

        public int CheatMazeSolve(List<List<char>> grid, Dictionary<(int r, int c), int> dist, int r, int c, int normal, int cheatTime)
        {
            int negativeCheatTime = -1 * cheatTime;
            int better = 0;
            HashSet<(int r, int c)> visited = [];
            Queue<(int r, int c, int s)> queue = [];
            queue.Enqueue((r, c, 0));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (normal - curr.s < 100)
                {
                    continue;
                }

                if (visited.Contains((curr.r, curr.c)))
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

                visited.Add((curr.r, curr.c));

                for (int i = negativeCheatTime; i <= cheatTime; i++)
                {
                    for (int j = negativeCheatTime; j <= cheatTime; j++)
                    {
                        if (Math.Abs(i) + Math.Abs(j) > cheatTime)
                        {
                            continue;
                        }
                        if (dist.ContainsKey((curr.r + i, curr.c + j)))
                        {
                            int score = dist[(curr.r + i, curr.c + j)] + curr.s + Math.Abs(i) + Math.Abs(j);

                            if (normal - score >= 100)
                            {
                                better++;
                            }
                        }
                    }
                }

                foreach (var d in directions)
                {
                    queue.Enqueue((curr.r + d.r, curr.c + d.c, curr.s + 1));
                }
            }

            return better;
        }
    }
}
