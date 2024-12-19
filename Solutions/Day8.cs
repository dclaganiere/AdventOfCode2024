using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day8 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day8) + ".txt";
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

            Dictionary<char, List<(int r, int c)>> antennas = new();
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] != '.')
                    {
                        if (!antennas.ContainsKey(grid[i][j]))
                        {
                            antennas[grid[i][j]] = new();
                        }

                        antennas[grid[i][j]].Add((i, j));
                    }
                }
            }

            int maxRow = grid.Count;
            int maxCol = grid[0].Length;
            HashSet<(int r, int c)> found = new();

            foreach ((char c, var list) in antennas)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var first = list[i];
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        var second = list[j];
                        int dRow = first.r - second.r;
                        int dCol = first.c - second.c;

                        int anti1Row = first.r + dRow;
                        int anti1Col = first.c + dCol;
                        int anti2Row = second.r - dRow;
                        int anti2Col = second.c - dCol;

                        found.Add((anti1Row, anti1Col));
                        found.Add((anti2Row, anti2Col));
                    }
                }
            }

            sum = found.Count(f => f.r >= 0 && f.c >= 0 && f.r < maxRow && f.c < maxCol);

            Console.WriteLine(sum);
        }

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

            Dictionary<char, List<(int r, int c)>> antennas = new();
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] != '.')
                    {
                        if (!antennas.ContainsKey(grid[i][j]))
                        {
                            antennas[grid[i][j]] = new();
                        }

                        antennas[grid[i][j]].Add((i, j));
                    }
                }
            }

            int maxRow = grid.Count;
            int maxCol = grid[0].Length;
            HashSet<(int r, int c)> found = new();

            foreach ((char c, var list) in antennas)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var first = list[i];
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        var second = list[j];
                        int dRow = first.r - second.r;
                        int dCol = first.c - second.c;

                        found.Add(first);
                        found.Add(second);

                        int anti1Row = first.r + dRow;
                        int anti1Col = first.c + dCol;

                        while (anti1Row >= 0 && anti1Col >= 0 && anti1Row < maxRow && anti1Col < maxCol)
                        {
                            found.Add((anti1Row, anti1Col));
                            anti1Row += dRow;
                            anti1Col += dCol;
                        }

                        int anti2Row = second.r - dRow;
                        int anti2Col = second.c - dCol;

                        while (anti2Row >= 0 && anti2Col >= 0 && anti2Row < maxRow && anti2Col < maxCol)
                        {
                            found.Add((anti2Row, anti2Col));
                            anti2Row -= dRow;
                            anti2Col -= dCol;
                        }
                    }
                }
            }

            sum = found.Count();

            Console.WriteLine(sum);
        }
    }
}
