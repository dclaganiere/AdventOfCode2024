using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day13 : IDay
    {
        public record Point(long X, long Y);
        const string InputFile = "Input\\" + nameof(Day13) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            while (!reader.EndOfStream)
            {
                string line_a = reader.ReadLine() ?? string.Empty;
                string line_b = reader.ReadLine() ?? string.Empty;
                string line_prize = reader.ReadLine() ?? string.Empty;
                reader.ReadLine();

                var regex_a = Regex.Match(line_a, "Button A: X\\+(\\d+), Y\\+(\\d+)");
                var regex_b = Regex.Match(line_b, "Button B: X\\+(\\d+), Y\\+(\\d+)");
                var regex_prize = Regex.Match(line_prize, "Prize: X=(\\d+), Y=(\\d+)");

                Point buttonA = new(int.Parse(regex_a.Groups[1].Value), int.Parse(regex_a.Groups[2].Value));
                Point buttonB = new(int.Parse(regex_b.Groups[1].Value), int.Parse(regex_b.Groups[2].Value));
                Point prize = new(int.Parse(regex_prize.Groups[1].Value), int.Parse(regex_prize.Groups[2].Value));

                long payzx = buttonA.Y * prize.X;
                long paxzy = buttonA.X * prize.Y;
                long paypbx = buttonA.Y * buttonB.X;
                long paxpby = buttonA.X * buttonB.Y;

                long b = (payzx - paxzy) / (paypbx - paxpby);
                long a = (prize.X - (buttonB.X * b)) / buttonA.X;

                long validX = (a * buttonA.X) + (b * buttonB.X);
                long validY = (a * buttonA.Y) + (b * buttonB.Y);

                if (validX == prize.X && validY == prize.Y)
                {
                    sum += (a * 3) + b;
                }

            }

            Console.WriteLine(sum);
        }

        public int WinPrize(Point buttonA, Point buttonB, Point prize, bool limitPresses = true)
        {
            PriorityQueue<(Point loc, int pressesA, int pressesB), int> queue = new();
            HashSet<(long x, long y)> visited = new();

            queue.Enqueue((new Point(0, 0), 0, 0), 0);

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.loc.X, curr.loc.Y)))
                {
                    continue;
                }

                if (limitPresses && (curr.pressesA > 100 || curr.pressesB > 100))
                {
                    continue;
                }

                if (curr.loc.X == prize.X && curr.loc.Y == prize.Y)
                {
                    return (curr.pressesA * 3) + curr.pressesB;
                }

                if (curr.loc.X > prize.X || curr.loc.Y > prize.Y)
                {
                    continue;
                }

                visited.Add((curr.loc.X, curr.loc.Y));

                queue.Enqueue((new Point(curr.loc.X + buttonA.X, curr.loc.Y + buttonA.Y), curr.pressesA + 1, curr.pressesB), ((curr.pressesA + 1) * 3) + curr.pressesB);
                queue.Enqueue((new Point(curr.loc.X + buttonB.X, curr.loc.Y + buttonB.Y), curr.pressesA, curr.pressesB + 1), (curr.pressesA * 3) + curr.pressesB + 1);
            }

            return 0;
        }

        public const long offset = 10_000_000_000_000;

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            while (!reader.EndOfStream)
            {
                string line_a = reader.ReadLine() ?? string.Empty;
                string line_b = reader.ReadLine() ?? string.Empty;
                string line_prize = reader.ReadLine() ?? string.Empty;
                reader.ReadLine();

                var regex_a = Regex.Match(line_a, "Button A: X\\+(\\d+), Y\\+(\\d+)");
                var regex_b = Regex.Match(line_b, "Button B: X\\+(\\d+), Y\\+(\\d+)");
                var regex_prize = Regex.Match(line_prize, "Prize: X=(\\d+), Y=(\\d+)");

                Point buttonA = new(int.Parse(regex_a.Groups[1].Value), int.Parse(regex_a.Groups[2].Value));
                Point buttonB = new(int.Parse(regex_b.Groups[1].Value), int.Parse(regex_b.Groups[2].Value));
                Point prize = new(int.Parse(regex_prize.Groups[1].Value), int.Parse(regex_prize.Groups[2].Value));
                prize = new(prize.X + offset, prize.Y + offset);

                long payzx = buttonA.Y * prize.X;
                long paxzy = buttonA.X * prize.Y;
                long paypbx = buttonA.Y * buttonB.X;
                long paxpby = buttonA.X * buttonB.Y;

                long b = (payzx - paxzy) / (paypbx - paxpby);
                long a = (prize.X - (buttonB.X * b)) / buttonA.X;

                long validX = (a * buttonA.X) + (b * buttonB.X);
                long validY = (a * buttonA.Y) + (b * buttonB.Y);

                if (validX == prize.X && validY == prize.Y)
                {
                    sum += (a * 3) + b;
                }
            }

            Console.WriteLine(sum);
        }
    }
}
