using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day18 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day18) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<(int r, int c)> data = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split(',');
                int r = int.Parse(sp[0]);
                int c = int.Parse(sp[1]);

                data.Add((r, c));
            }

            HashSet<(int r, int c)> walls = new();
            for (int i = -1; i <= 71; i++)
            {
                walls.Add((i, -1));
                walls.Add((i, 71));
                walls.Add((-1, i));
                walls.Add((71, i));
            }

            foreach(var d in data.Take(1024))
            {
                walls.Add(d);
            }
            sum = Navigate(walls);

            Console.WriteLine(sum);
        }

        private List<(int r, int c)> directions = new List<(int r, int c)>()
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        };

        public int Navigate(HashSet<(int r, int c)> walls)
        {
            HashSet<(int r, int c)> visited = new();

            Queue<(int r, int c, int d)> queue = new();
            queue.Enqueue((0, 0, 0));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (walls.Contains((curr.r, curr.c)))
                {
                    continue;
                }
                if (visited.Contains((curr.r, curr.c)))
                {
                    continue;
                }
                visited.Add((curr.r, curr.c));

                if (curr.r == 70 && curr.c == 70)
                {
                    return curr.d;
                }

                if (curr.r == 71 || curr.c == 71 || curr.r == -1 || curr.c == -1)
                {
                    continue;
                }

                foreach(var d in directions)
                {
                    queue.Enqueue((curr.r + d.r, curr.c + d.c, curr.d + 1));
                }
            }

            return -1;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<(int r, int c)> data = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split(',');
                int r = int.Parse(sp[0]);
                int c = int.Parse(sp[1]);

                data.Add((r, c));
            }

            HashSet<(int r, int c)> bounds = new();
            for (int i = -1; i <= 71; i++)
            {
                bounds.Add((i, -1));
                bounds.Add((i, 71));
                bounds.Add((-1, i));
                bounds.Add((71, i));
            }

            int min = 0;
            int max = data.Count;

            while (min < max)
            {
                int mid = (min + max) >> 1;
                HashSet<(int r, int c)> walls = new();
                walls.UnionWith(bounds);

                foreach (var d in data.Take(mid))
                {
                    walls.Add(d);
                }
                sum = Navigate(walls);

                if (sum == -1)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            var res = data[min - 1];
            Console.WriteLine($"{res.r},{res.c}");
        }
    }
}
