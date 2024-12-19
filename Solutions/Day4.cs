using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day4 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day4) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<string> grid = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                grid.Add(line);
            }

            int height = grid.Count;
            int width = grid[0].Length;

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 'X')
                    {
                        sum += CountXmases(grid, i, j);
                    }
                }
            }

            Console.WriteLine(sum);
        }

        private List<List<(int x, int y)>> searches = new()
        {
            new() {(0,0), (1,0), (2,0), (3,0) },
            new() {(0,0), (-1,0), (-2,0), (-3,0) },
            new() {(0,0), (1,1), (2,2), (3,3) },
            new() {(0,0), (1,-1), (2,-2), (3,-3) },
            new() {(0,0), (-1,1), (-2,2), (-3,3) },
            new() {(0,0), (-1,-1), (-2,-2), (-3,-3) },
            new() {(0,0), (0,1), (0,2), (0,3) },
            new() {(0,0), (0,-1), (0,-2), (0,-3) }
        };

        public const string XMAS = "XMAS";


        public int CountXmases(List<string> grid, int i, int j)
        {
            int height = grid.Count;
            int width = grid[0].Length;
            int count = 0;

            foreach (var s in searches)
            {
                int minX = s.Min(o => o.x);
                int minY = s.Min(o => o.y);
                int maxX = s.Max(o => o.x);
                int maxY = s.Max(o => o.y);

                if (maxX + j >= width || minX + j < 0 || maxY + i >= height || minY + i < 0)
                {
                    continue;
                }

                bool isXmas = true;
                for (int p = 0; p < 4; p++)
                {
                    var k = s[p];
                    if (grid[i + k.y][j + k.x] != XMAS[p])
                    {
                        isXmas = false;
                        break;
                    }
                }

                if (isXmas)
                {
                    count++;
                }
            }

            return count;
        }

        public List<(int x, int y)> diag = new()
        {
            (1,1),
            (-1,1),
            (-1,-1),
            (1,-1)
        };


        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<string> grid = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                grid.Add(line);
            }

            int height = grid.Count;
            int width = grid[0].Length;

            for (int i = 1; i < grid.Count-1; i++)
            {
                for (int j = 1; j < grid[i].Length-1; j++)
                {
                    if (grid[i][j] == 'A')
                    {
                        int m = 0;
                        int s = 0;

                        foreach (var d in diag)
                        {
                            if (grid[i + d.y][j + d.x] == 'M')
                            {
                                m++;
                            }
                            if (grid[i + d.y][j + d.x] == 'S')
                            {
                                s++;
                            }
                        }

                        if (m == 2 && s == 2)
                        {
                            if (grid[i + 1][j+1] != grid[i - 1][j-1])
                            {
                                sum++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }
    }
}
