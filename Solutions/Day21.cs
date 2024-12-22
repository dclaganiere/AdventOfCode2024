using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Point = (int r, int c);

namespace AdventOfCode2024.Solutions
{
    internal class Day21 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day21) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long value = long.Parse(line[..^1]);
                string output = BFS(line);

                sum += value * output.Length;
            }

            Console.WriteLine(sum);
        }

        private string BFS(string input)
        {
            List<List<char>> keypad = [
                ['7', '8', '9'],
                ['4', '5', '6'],
                ['1', '2', '3'],
                [' ', '0', 'A'],
            ];

            List<List<char>> controller = [
                [' ', '^', 'A'],
                ['<', 'v', '>'],
            ];

            string inputs = "A<^>v";
            HashSet<(Point kr, Point cr1, Point cr2, int idx)> visited = new();
            Queue<(Point kr, Point cr1, Point cr2, int idx, string input)> queue = new();
            queue.Enqueue(((3, 2), (0, 2), (0, 2), 0, string.Empty));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.kr, curr.cr1, curr.cr2, curr.idx)))
                {
                    continue;
                }

                if (curr.idx == input.Length)
                {
                    return curr.input;
                }

                if (curr.kr.r < 0 || curr.kr.r >= keypad.Count || curr.kr.c < 0 || curr.kr.c >= keypad[0].Count)
                {
                    continue;
                }

                if (curr.cr1.r < 0 || curr.cr1.r >= controller.Count || curr.cr1.c < 0 || curr.cr1.c >= controller[0].Count)
                {
                    continue;
                }

                if (curr.cr2.r < 0 || curr.cr2.r >= controller.Count || curr.cr2.c < 0 || curr.cr2.c >= controller[0].Count)
                {
                    continue;
                }

                if (keypad[curr.kr.r][curr.kr.c] == ' ')
                {
                    continue;
                }

                if (controller[curr.cr1.r][curr.cr1.c] == ' ')
                {
                    continue;
                }

                if (controller[curr.cr2.r][curr.cr2.c] == ' ')
                {
                    continue;
                }

                visited.Add((curr.kr, curr.cr1, curr.cr2, curr.idx));

                foreach (var c in inputs)
                {
                    var next_kr = curr.kr;
                    var next_cr1 = curr.cr1;
                    var next_cr2 = curr.cr2;
                    var next_idx = curr.idx;

                    if (c == 'A')
                    {
                        var cr2_c = controller[curr.cr2.r][curr.cr2.c];

                        if (cr2_c == 'A')
                        {
                            var cr1_c = controller[curr.cr1.r][curr.cr1.c];

                            if (cr1_c == 'A')
                            {
                                char button = keypad[curr.kr.r][curr.kr.c];
                                if (button != input[curr.idx])
                                {
                                    continue;
                                }
                                next_idx++;
                            }
                            else
                            {
                                var cr1_dir = directions[cr1_c];
                                next_kr = (next_kr.r + cr1_dir.r, next_kr.c + cr1_dir.c);
                            }
                        }
                        else
                        {
                            var cr2_dir = directions[cr2_c];
                            next_cr1 = (next_cr1.r + cr2_dir.r, next_cr1.c + cr2_dir.c);
                        }
                    }
                    else
                    {
                        var human_dir = directions[c];
                        next_cr2 = (next_cr2.r + human_dir.r, next_cr2.c + human_dir.c);
                    }

                    queue.Enqueue((next_kr, next_cr1, next_cr2, next_idx, curr.input + c));
                }
            }

            return string.Empty;
        }

        private static Dictionary<char, (int r, int c)> directions = new Dictionary<char, (int r, int c)>()
        {
            {'^', (-1, 0) },
            {'>', (0, 1) },
            {'v', (1, 0) },
            {'<', (0, -1) }
        };

        public static Dictionary<(char start, char end), string> HardcodeController()
        {
            Dictionary<(char start, char end), string> controller = new()
            {
                { ('>', '>'), string.Empty },
                { ('<', '<'), string.Empty },
                { ('v', 'v'), string.Empty },
                { ('^', '^'), string.Empty },
                { ('A', 'A'), string.Empty },

                { ('>', 'A'), "^" },
                { ('>', '^'), "<^" },
                { ('>', 'v'), "<" },
                { ('>', '<'), "<<" },

                { ('<', 'v'), ">" },
                { ('<', '>'), ">>" },
                { ('<', '^'), ">^" },
                { ('<', 'A'), ">>^" },

                { ('v', '<'), "<" },
                { ('v', '>'), ">" },
                { ('v', '^'), "^" },
                { ('v', 'A'), "^>" },

                { ('^', 'v'), "v" },
                { ('^', '<'), "v<" },
                { ('^', '>'), "v>" },
                { ('^', 'A'), ">" },

                { ('A', '^'), "<"},
                { ('A', '>'), "v" },
                { ('A', 'v'), "<v" },
                { ('A', '<'), "v<<" },

            };
            return controller;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long value = long.Parse(line[..^1]);
                string oneRobot = UndoLayer(UndoLayer(BFS(line)));
                long allRobots = Recurse(oneRobot, 25);

                sum += value * allRobots;
            }

            Console.WriteLine(sum);
        }

        public string UndoLayer(string input)
        {
            List<List<char>> controller = [
                [' ', '^', 'A'],
                ['<', 'v', '>'],
            ];

            int r = 0;
            int c = 2;

            StringBuilder sb = new();
            foreach (char i in input)
            {
                if (i == 'A')
                {
                    sb.Append(controller[r][c]);
                }
                else
                {
                    var d = directions[i];
                    r += d.r;
                    c += d.c;
                }
            }

            return sb.ToString();
        }

        Dictionary<(char start, char end), string> controller = HardcodeController();
        static Dictionary<(string s, int depth), long> dp = [];
        public long Recurse(string input, int depth)
        {
            if (depth == 0)
            {
                return input.Length;
            }

            if (!dp.ContainsKey((input, depth)))
            {
                long sum = 0;
                var sp = input.Split('A');

                foreach (string s in sp.SkipLast(1))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        sum++;
                        continue;
                    }

                    StringBuilder sb = new();
                    char prev = 'A';
                    for (int i = 0; i < s.Length; i++)
                    {
                        char start = prev;
                        char end = s[i];
                        sb.Append(controller[(start, end)]);
                        sb.Append('A');
                        prev = end;
                    }
                    sb.Append(controller[(prev, 'A')]);
                    sb.Append('A');

                    sum += Recurse(sb.ToString(), depth - 1);
                }

                dp[(input, depth)] = sum;
            }
            return dp[(input, depth)];
        }
    }
}
