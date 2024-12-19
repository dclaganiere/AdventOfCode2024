using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal partial class Day14 : IDay
    {
        public record Robot(int Px, int Py, int Vx, int Vy);
        const string InputFile = "Input\\" + nameof(Day14) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            int maxX = 101;
            int maxY = 103;

            int midX = (maxX - 1) / 2;
            int midY = (maxY - 1) / 2;

            List<Robot> robots = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var re = RobotRegex().Match(line);
                if (re.Success)
                {
                    int px = int.Parse(re.Groups[1].Value);
                    int py = int.Parse(re.Groups[2].Value);
                    int vx = int.Parse(re.Groups[3].Value);
                    int vy = int.Parse(re.Groups[4].Value);
                    robots.Add(new Robot(px, py, vx, vy));
                }
            }

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < robots.Count; j++)
                {
                    var r = robots[j];
                    int px = (r.Px + r.Vx + maxX) % maxX;
                    int py = (r.Py + r.Vy + maxY) % maxY;
                    robots[j] = new(px, py, r.Vx, r.Vy);
                }
            }

            int q1 = robots.Count(r => r.Px < midX && r.Py < midY);
            int q2 = robots.Count(r => r.Px < midX && r.Py > midY);
            int q3 = robots.Count(r => r.Px > midX && r.Py < midY);
            int q4 = robots.Count(r => r.Px > midX && r.Py > midY);
            sum = q1 * q2 * q3 * q4;
            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);

            int maxX = 101;
            int maxY = 103;

            int midX = (maxX - 1) / 2;
            int midY = (maxY - 1) / 2;

            List<Robot> robots = [];
            List<Robot> robots2 = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var re = RobotRegex().Match(line);
                if (re.Success)
                {
                    int px = int.Parse(re.Groups[1].Value);
                    int py = int.Parse(re.Groups[2].Value);
                    int vx = int.Parse(re.Groups[3].Value);
                    int vy = int.Parse(re.Groups[4].Value);
                    robots.Add(new Robot(px, py, vx, vy));
                    robots2.Add(new Robot(px, py, vx, vy));
                }
            }

            int seconds = 0;
            while (seconds < 10403)
            {
                seconds++;
                HashSet<(int x, int y)> points = [];
                bool isOverlap = false;
                for (int j = 0; j < robots.Count; j++)
                {
                    var r = robots[j];
                    int px = (r.Px + r.Vx + maxX) % maxX;
                    int py = (r.Py + r.Vy + maxY) % maxY;
                    robots[j] = new(px, py, r.Vx, r.Vy);

                    if (points.Contains((px, py)))
                    {
                        isOverlap = true;
                    }

                    points.Add((px, py));
                }


                if (!isOverlap)
                {
                    Console.WriteLine(seconds);
                    for (int y = 0; y < maxY; y++)
                    {
                        for (int x = 0; x < maxX; x++)
                        {
                            bool isP = points.Contains((x, y));
                            Console.Write(isP ? "*" : " ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        [GeneratedRegex("p=(-?\\d+),(-?\\d+) v=(-?\\d+),(-?\\d+)")]
        private static partial Regex RobotRegex();
    }
}
