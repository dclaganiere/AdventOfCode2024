using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day6 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day6) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<string> grid = new();
            int startRow = -1;
            int startCol = -1;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (grid.Count == 0)
                {
                    grid.Add(new string(Enumerable.Repeat('o', line.Length + 2).ToArray()));
                }
                grid.Add($"o{line}o");

                int idx = line.IndexOf('^');
                if (idx != -1)
                {
                    startRow = grid.Count - 1;
                    startCol = idx + 1;
                }
            }

            grid.Add(grid[0]);
            sum = NavigateMaze(grid, startRow, startCol);

            Console.WriteLine(sum);
        }

        private List<(int r, int c)> directions = new List<(int r, int c)>()
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        };

        private int NavigateMaze(List<string> grid, int startRow, int startCol)
        {
            HashSet<(int r, int c, int dir)> visitedTurns = new();
            HashSet<(int r, int c)> visited = new();

            int r = startRow;
            int c = startCol;
            int dir = 0;

            while (!visitedTurns.Contains((r, c, dir)))
            {
                visited.Add((r, c));

                var n = directions[dir];

                if (grid[r + n.r][c + n.c] == '#')
                {
                    visitedTurns.Add((r, c, dir));
                    dir = (dir + 1) % 4;
                }
                else if (grid[r + n.r][c + n.c] == 'o')
                {
                    break;
                }
                else
                {
                    r += n.r;
                    c += n.c;
                }
            }

            return visited.Count;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<List<char>> grid = new();
            int startRow = -1;
            int startCol = -1;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (grid.Count == 0)
                {
                    grid.Add(Enumerable.Repeat('o', line.Length + 2).ToList());
                }
                grid.Add($"o{line}o".ToList());

                int idx = line.IndexOf('^');
                if (idx != -1)
                {
                    startRow = grid.Count - 1;
                    startCol = idx + 1;
                }
            }

            grid.Add(grid[0]);

            MarkupMaze(grid, startRow, startCol);

            for (int i = 1;  i < grid.Count - 1; i++)
            {
                for (int j = 1; j < grid[i].Count - 1; j++)
                {
                    if (grid[i][j] == 'X')
                    {
                        grid[i][j] = '#';
                        if (IsLoop(grid, startRow, startCol))
                        {
                            sum++;
                        }
                        grid[i][j] = 'X';
                    }
                }
            }

            Console.WriteLine(sum);
        }

        private int MarkupMaze(List<List<char>> grid, int startRow, int startCol)
        {
            HashSet<(int r, int c, int dir)> visitedTurns = new();
            HashSet<(int r, int c)> visited = new();

            int r = startRow;
            int c = startCol;
            int dir = 0;

            while (!visitedTurns.Contains((r, c, dir)))
            {
                visited.Add((r, c));

                grid[r][c] = 'X';

                var n = directions[dir];

                if (grid[r + n.r][c + n.c] == '#')
                {
                    visitedTurns.Add((r, c, dir));
                    dir = (dir + 1) % 4;
                }
                else if (grid[r + n.r][c + n.c] == 'o')
                {
                    break;
                }
                else
                {
                    r += n.r;
                    c += n.c;
                }
            }

            return visited.Count;
        }

        private bool IsLoop(List<List<char>> grid, int startRow, int startCol)
        {
            HashSet<(int r, int c, int dir)> visitedTurns = new();

            int r = startRow;
            int c = startCol;
            int dir = 0;

            while (!visitedTurns.Contains((r, c, dir)))
            {
                var n = directions[dir];

                if (grid[r + n.r][c + n.c] == '#')
                {
                    visitedTurns.Add((r, c, dir));
                    dir = (dir + 1) % 4;
                }
                else if (grid[r + n.r][c + n.c] == 'o')
                {
                    return false;
                }
                else
                {
                    r += n.r;
                    c += n.c;
                }
            }

            return true;
        }
    }
}
