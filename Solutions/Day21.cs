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

            //Dictionary<(char start, char end), string> keypad = CalculateKeypad();
            //Dictionary<(char start, char end), string> controller = CalculateController();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long value = long.Parse(line[..^1]);
                string output = SolveStuff(line);

                Console.WriteLine($"{line} = {output.Length}: {output}");
                sum += value * output.Length;
            }

            Console.WriteLine(sum);
        }

        private string SolveStuff(string input)
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

        public static Dictionary<(char start, char end), string> CalculateKeypad()
        {
            List<List<char>> original = [
                ['7', '8', '9'],
                ['4', '5', '6'],
                ['1', '2', '3'],
                [' ', '0', 'A'],
            ];
            //string inputs = "0123456789A";
            //Dictionary<char, (int r, int c)> lookup = [];
            //Dictionary<(char start, char end), string> keypad = [];

            //for (int r = 0; r < original.Count; r++)
            //{
            //    for (int c = 0; c < original[r].Count; c++)
            //    {
            //        lookup[original[r][c]] = (r, c);
            //    }
            //}

            //HashSet<char> bottom = new HashSet<char>() { '0', 'A' };
            //HashSet<char> left = new() { '1', '4', '7' };

            //for (int i = 0; i < inputs.Length; i++)
            //{
            //    for (int j = 0; j < inputs.Length; j++)
            //    {
            //        if (i == j)
            //        {
            //            keypad[(inputs[i], inputs[j])] = string.Empty;
            //            continue;
            //        }

            //        var start = lookup[inputs[i]];
            //        var end = lookup[inputs[j]];

            //        StringBuilder sb = new();
            //        char lr = end.c > start.c ? '>' : '<';
            //        char ud = end.r > start.r ? 'v' : '^';

            //        if (start.r == end.r)
            //        {
            //            sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
            //            keypad[(inputs[i], inputs[j])] = sb.ToString();
            //            continue;
            //        }

            //        if (bottom.Contains(inputs[i]) && left.Contains(inputs[j]))
            //        {
            //            sb.Append(new string(Enumerable.Repeat(ud, Math.Abs(end.r - start.r)).ToArray()));
            //            sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
            //        }
            //        else
            //        {
            //            sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
            //            sb.Append(new string(Enumerable.Repeat(ud, Math.Abs(end.r - start.r)).ToArray()));
            //        }

            //        keypad[(inputs[i], inputs[j])] = sb.ToString();
            //    }
            //}

            //return keypad;

            Dictionary<(char start, char end), string> keypad = [];
            Queue<(char start, bool turn, int r, int c, string input)> queue = [];

            for (int r = 0; r < original.Count; r++)
            {
                for (int c = 0; c < original[r].Count; c++)
                {
                    queue.Enqueue((original[r][c], false, r, c, string.Empty));
                }
            }

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (curr.r < 0 || curr.r >= original.Count || curr.c < 0 || curr.c >= original[0].Count)
                {
                    continue;
                }

                char end = original[curr.r][curr.c];

                if (curr.start == ' ' || end == ' ')
                {
                    continue;
                }

                if (keypad.ContainsKey((curr.start, end)))
                {
                    continue;
                }

                keypad.Add((curr.start, end), curr.input);

                if (string.IsNullOrEmpty(curr.input))
                {
                    foreach ((char c, var d) in directions)
                    {
                        queue.Enqueue((curr.start, false, curr.r + d.r, curr.c + d.c, curr.input + c));
                    }
                }
                else if (curr.turn)
                {
                    var turn = directions[curr.input[^1]];
                    queue.Enqueue((curr.start, curr.turn, curr.r + turn.r, curr.c + turn.c, curr.input + curr.input[^1]));
                }
                else
                {
                    var first = directions[curr.input[0]];
                    queue.Enqueue((curr.start, curr.turn, curr.r + first.r, curr.c + first.c, curr.input + curr.input[0]));

                    if (curr.input[0] == '^' || curr.input[0] == 'v')
                    {
                        var left = directions['<'];
                        var right = directions['>'];
                        queue.Enqueue((curr.start, true, curr.r + left.r, curr.c + left.c, curr.input + '<'));
                        queue.Enqueue((curr.start, true, curr.r + right.r, curr.c + right.c, curr.input + '>'));
                    }
                    else
                    {
                        var up = directions['^'];
                        var down = directions['v'];
                        queue.Enqueue((curr.start, true, curr.r + up.r, curr.c + up.c, curr.input + '^'));
                        queue.Enqueue((curr.start, true, curr.r + down.r, curr.c + down.c, curr.input + 'v'));
                    }
                }
            }

            return keypad;
        }

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
                { ('^', '>'), "v>" }, // Assumption
                { ('^', 'A'), ">" },

                { ('A', '^'), "<"},
                { ('A', '>'), "v" },
                { ('A', 'v'), "<v" },
                { ('A', '<'), "v<<" },

            };
            return controller;
        }

        public static Dictionary<(char start, char end), string> CalculateController()
        {
            List<List<char>> original = [
                [' ', '^', 'A'],
                ['<', 'v', '>'],
            ];
            string inputs = "<>^vA";
            Dictionary<char, (int r, int c)> lookup = [];
            Dictionary<(char start, char end), string> controller = [];

            for (int r = 0; r < original.Count; r++)
            {
                for (int c = 0; c < original[r].Count; c++)
                {
                    lookup[original[r][c]] = (r, c);
                }
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (i == j)
                    {
                        controller[(inputs[i], inputs[j])] = string.Empty;
                        continue;
                    }

                    var start = lookup[inputs[i]];
                    var end = lookup[inputs[j]];

                    StringBuilder sb = new();
                    char lr = end.c > start.c ? '>' : '<';
                    char ud = end.r > start.r ? 'v' : '^';

                    if (start.r == end.r)
                    {
                        sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
                        controller[(inputs[i], inputs[j])] = sb.ToString();
                        continue;
                    }

                    if (lr == '<' && ud == 'v')
                    {
                        sb.Append(ud);
                        sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
                    }
                    else
                    {
                        sb.Append(new string(Enumerable.Repeat(lr, Math.Abs(end.c - start.c)).ToArray()));
                        sb.Append(ud);
                    }

                    controller[(inputs[i], inputs[j])] = sb.ToString();
                }
            }

            return controller;
        }

        public string MapInputs(Dictionary<(char start, char end), string> keypad, Dictionary<(char start, char end), string> controller, string input)
        {
            StringBuilder stringBuilder = new();
            char prev = 'A';
            for (int i = 0; i < input.Length; i++)
            {
                char start = prev;
                char end = input[i];
                stringBuilder.Append(keypad[(start, end)]);
                stringBuilder.Append('A');
                prev = end;
            }

            string input2 = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int k = 0; k < 2; k++)
            {
                prev = 'A';
                for (int i = 0; i < input2.Length; i++)
                {
                    char start = prev;
                    char end = input2[i];
                    stringBuilder.Append(controller[(start, end)]);
                    stringBuilder.Append('A');
                    prev = end;
                }

                input2 = stringBuilder.ToString();
                stringBuilder.Clear();
            }

            return input2;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;
            long sum2 = 0;

            //Dictionary<(char start, char end), string> keypad = CalculateKeypad();
            //Dictionary<(char start, char end), string> controller = CalculateController();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                long value = long.Parse(line[..^1]);
                string oneRobot = UndoLayer(UndoLayer(SolveStuff(line)));
                long allRobots = Recurse(oneRobot, 25);

                sum += value * allRobots;
            }

            Console.WriteLine(sum);
            Console.WriteLine(sum2);
            //using StreamReader reader = new(InputFile);
            //long sum = 0;

            //int fails = 0;
            //for (int i = 0; i < 1000; i++)
            //{
            //    string input = $"{i:000}A";
            //    string correct = SolveStuff3(input);

            //    string basic = UndoLayer(UndoLayer(UndoLayer(UndoLayer(correct))));
            //    string actual = Recurse2(basic, 4);

            //    if (correct.Length != actual.Length)
            //    {
            //        fails++;
            //        Console.WriteLine($"{input}: Expected {correct.Length}, got {actual.Length}");
            //        Console.WriteLine(correct);
            //        Console.WriteLine(actual);
            //        Console.WriteLine(UndoLayer(correct));
            //        Console.WriteLine(UndoLayer(actual));
            //        Console.WriteLine(UndoLayer(UndoLayer(correct)));
            //        Console.WriteLine(UndoLayer(UndoLayer(actual)));
            //        Console.ReadLine();
            //    }
            //}
            //Console.WriteLine(fails);


            //while (!reader.EndOfStream)
            //{
            //    string line = reader.ReadLine() ?? string.Empty;

            //    long value = long.Parse(line[..^1]);
            //    string output = SolveStuff2(line);
            //    //Console.WriteLine($"{line} = {output.Length}: {output}");

            //    string output2 = UndoLayer(output);

            //    long len = Recurse(output2, 23);
            //    sum += value * len;
            //    //Console.WriteLine();
            //    Console.WriteLine(len);
            //}

            //Console.WriteLine(sum);
        }


        private string SolveStuff2(string input)
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
            HashSet<(Point kr, Point cr1, Point cr2, Point cr3, int idx)> visited = new();
            Queue<(Point kr, Point cr1, Point cr2, Point cr3, int idx, string input)> queue = new();
            queue.Enqueue(((3, 2), (0, 2), (0, 2), (0, 2), 0, string.Empty));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.kr, curr.cr1, curr.cr2, curr.cr3, curr.idx)))
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

                if (curr.cr3.r < 0 || curr.cr3.r >= controller.Count || curr.cr3.c < 0 || curr.cr3.c >= controller[0].Count)
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

                if (controller[curr.cr3.r][curr.cr3.c] == ' ')
                {
                    continue;
                }

                visited.Add((curr.kr, curr.cr1, curr.cr2, curr.cr3, curr.idx));

                foreach (var c in inputs)
                {
                    var next_kr = curr.kr;
                    var next_cr1 = curr.cr1;
                    var next_cr2 = curr.cr2;
                    var next_cr3 = curr.cr3;
                    var next_idx = curr.idx;

                    if (c == 'A')
                    {
                        var cr3_c = controller[curr.cr3.r][curr.cr3.c];

                        if (cr3_c == 'A')
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
                            var cr3_dir = directions[cr3_c];
                            next_cr2 = (next_cr2.r + cr3_dir.r, next_cr2.c + cr3_dir.c);
                        }
                    }
                    else
                    {
                        var human_dir = directions[c];
                        next_cr3 = (next_cr3.r + human_dir.r, next_cr3.c + human_dir.c);
                    }

                    queue.Enqueue((next_kr, next_cr1, next_cr2, next_cr3, next_idx, curr.input + c));
                }
            }

            return string.Empty;
        }

        private string SolveStuff3(string input)
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
            HashSet<(Point kr, Point cr1, Point cr2, Point cr3, Point cr4, int idx)> visited = new();
            Queue<(Point kr, Point cr1, Point cr2, Point cr3, Point cr4, int idx, string input)> queue = new();
            queue.Enqueue(((3, 2), (0, 2), (0, 2), (0, 2), (0, 2), 0, string.Empty));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.kr, curr.cr1, curr.cr2, curr.cr3, curr.cr4, curr.idx)))
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

                if (curr.cr3.r < 0 || curr.cr3.r >= controller.Count || curr.cr3.c < 0 || curr.cr3.c >= controller[0].Count)
                {
                    continue;
                }

                if (curr.cr4.r < 0 || curr.cr4.r >= controller.Count || curr.cr4.c < 0 || curr.cr4.c >= controller[0].Count)
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

                if (controller[curr.cr3.r][curr.cr3.c] == ' ')
                {
                    continue;
                }

                if (controller[curr.cr4.r][curr.cr4.c] == ' ')
                {
                    continue;
                }

                visited.Add((curr.kr, curr.cr1, curr.cr2, curr.cr3, curr.cr4, curr.idx));

                foreach (var c in inputs)
                {
                    var next_kr = curr.kr;
                    var next_cr1 = curr.cr1;
                    var next_cr2 = curr.cr2;
                    var next_cr3 = curr.cr3;
                    var next_cr4 = curr.cr4;
                    var next_idx = curr.idx;

                    if (c == 'A')
                    {
                        var cr4_c = controller[curr.cr4.r][curr.cr4.c];

                        if (cr4_c == 'A')
                        {
                            var cr3_c = controller[curr.cr3.r][curr.cr3.c];

                            if (cr3_c == 'A')
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
                                var cr3_dir = directions[cr3_c];
                                next_cr2 = (next_cr2.r + cr3_dir.r, next_cr2.c + cr3_dir.c);
                            }
                        }
                        else
                        {
                            var cr4_dir = directions[cr4_c];
                            next_cr3 = (next_cr3.r + cr4_dir.r, next_cr3.c + cr4_dir.c);
                        }


                    }
                    else
                    {
                        var human_dir = directions[c];
                        next_cr4 = (next_cr4.r + human_dir.r, next_cr4.c + human_dir.c);
                    }

                    queue.Enqueue((next_kr, next_cr1, next_cr2, next_cr3, next_cr4, next_idx, curr.input + c));
                }
            }

            return string.Empty;
        }

        private string SolveController(string input)
        {

            List<List<char>> controller = [
                [' ', '^', 'A'],
                ['<', 'v', '>'],
            ];

            string inputs = "A<^>v";
            HashSet<(Point cr, Point cr1, Point cr2, Point cr3, Point cr4, int idx)> visited = new();
            Queue<(Point cr, Point cr1, Point cr2, Point cr3, Point cr4, int idx, string input)> queue = new();
            queue.Enqueue(((0, 2), (0, 2), (0, 2), (0, 2), (0, 2), 0, string.Empty));

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (visited.Contains((curr.cr, curr.cr1, curr.cr2, curr.cr3, curr.cr4, curr.idx)))
                {
                    continue;
                }

                if (curr.idx == input.Length)
                {
                    return curr.input;
                }

                if (curr.cr.r < 0 || curr.cr.r >= controller.Count || curr.cr.c < 0 || curr.cr.c >= controller[0].Count)
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

                if (curr.cr3.r < 0 || curr.cr3.r >= controller.Count || curr.cr3.c < 0 || curr.cr3.c >= controller[0].Count)
                {
                    continue;
                }

                if (curr.cr4.r < 0 || curr.cr4.r >= controller.Count || curr.cr4.c < 0 || curr.cr4.c >= controller[0].Count)
                {
                    continue;
                }

                if (controller[curr.cr.r][curr.cr.c] == ' ')
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

                if (controller[curr.cr3.r][curr.cr3.c] == ' ')
                {
                    continue;
                }

                if (controller[curr.cr4.r][curr.cr4.c] == ' ')
                {
                    continue;
                }

                visited.Add((curr.cr, curr.cr1, curr.cr2, curr.cr3, curr.cr4, curr.idx));

                foreach (var c in inputs)
                {
                    var next_cr = curr.cr;
                    var next_cr1 = curr.cr1;
                    var next_cr2 = curr.cr2;
                    var next_cr3 = curr.cr3;
                    var next_cr4 = curr.cr4;
                    var next_idx = curr.idx;

                    if (c == 'A')
                    {
                        var cr4_c = controller[curr.cr4.r][curr.cr4.c];

                        if (cr4_c == 'A')
                        {
                            var cr3_c = controller[curr.cr3.r][curr.cr3.c];

                            if (cr3_c == 'A')
                            {
                                var cr2_c = controller[curr.cr2.r][curr.cr2.c];

                                if (cr2_c == 'A')
                                {
                                    var cr1_c = controller[curr.cr1.r][curr.cr1.c];

                                    if (cr1_c == 'A')
                                    {
                                        char button = controller[curr.cr.r][curr.cr.c];
                                        if (button != input[curr.idx])
                                        {
                                            continue;
                                        }
                                        next_idx++;
                                    }
                                    else
                                    {
                                        var cr1_dir = directions[cr1_c];
                                        next_cr = (next_cr.r + cr1_dir.r, next_cr.c + cr1_dir.c);
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
                                var cr3_dir = directions[cr3_c];
                                next_cr2 = (next_cr2.r + cr3_dir.r, next_cr2.c + cr3_dir.c);
                            }
                        }
                        else
                        {
                            var cr4_dir = directions[cr4_c];
                            next_cr3 = (next_cr3.r + cr4_dir.r, next_cr3.c + cr4_dir.c);
                        }
                    }
                    else
                    {
                        var human_dir = directions[c];
                        next_cr4 = (next_cr4.r + human_dir.r, next_cr4.c + human_dir.c);
                    }

                    queue.Enqueue((next_cr, next_cr1, next_cr2, next_cr3, next_cr4, next_idx, curr.input + c));
                }
            }

            return string.Empty;
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

        public static Dictionary<(char start, char end), long> SetupUsage()
        {
            Dictionary<(char start, char end), long> res = [];
            string inputs = "<>^vA";

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    res[(inputs[i], inputs[j])] = 0;
                }
            }

            return res;
        }

        public string LookupCR(char start, char end, int depth)
        {
            usage[(start, end)]++;

            if (start == 'v' && end == 'A' && depth % 2 == 1)
            {
                return "^>";
            }

            return controller[(start, end)];
        }

        Dictionary<(char start, char end), long> usage = SetupUsage();
        Dictionary<(char start, char end), string> controller = HardcodeController();
        static Dictionary<(string s, int depth), long> dp = [];
        static Dictionary<(string s, int depth), long> dpOld = [];
        static Dictionary<(string s, int depth), string> doNotUse = [];
        public long Recurse(string input, int depth)
        {
            if (depth == 0)
            {
                //Console.Write(input);
                return input.Length;
            }

            if (!dpOld.ContainsKey((input, depth)))
            {
                long sum = 0;
                var sp = input.Split('A');

                foreach (string s in sp.SkipLast(1))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        sum++;
                        //Console.Write('A');
                        continue;
                    }

                    StringBuilder sb = new();
                    char prev = 'A';
                    for (int i = 0; i < s.Length; i++)
                    {
                        char start = prev;
                        char end = s[i];
                        sb.Append(LookupCR(start, end, depth));
                        sb.Append('A');
                        prev = end;
                    }
                    sb.Append(LookupCR(prev, 'A', depth));
                    sb.Append('A');

                    sum += Recurse(sb.ToString(), depth - 1);
                }

                dpOld[(input, depth)] = sum;
            }
            return dpOld[(input, depth)];
        }

        public string Recurse2(string input, int depth)
        {
            if (depth == 0)
            {
                return input;
            }

            if (!doNotUse.ContainsKey((input, depth)))
            {
                var sp = input.Split('A');

                StringBuilder sb = new();
                foreach (string s in sp.SkipLast(1))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        sb.Append('A');
                        continue;
                    }

                    StringBuilder sb2 = new();
                    char prev = 'A';
                    for (int i = 0; i < s.Length; i++)
                    {
                        char start = prev;
                        char end = s[i];
                        sb2.Append(LookupCR(start, end, depth));
                        sb2.Append('A');
                        prev = end;
                    }
                    sb2.Append(LookupCR(prev, 'A', depth));
                    sb2.Append('A');

                    sb.Append(Recurse2(sb2.ToString(), depth - 1));
                }

                doNotUse[(input, depth)] = sb.ToString();
            }
            return doNotUse[(input, depth)];
        }

        public long Recurse3(string input, int depth)
        {
            if (depth == 0)
            {
                //Console.Write(input);
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
                        //Console.Write('A');
                        continue;
                    }

                    string test = SolveController(s+'A');
                    string new_s = UndoLayer(UndoLayer(UndoLayer(UndoLayer(test))));
                    sum += Recurse3(new_s, depth - 1);
                }

                dp[(input, depth)] = sum;
            }
            return dp[(input, depth)];
        }
    }
}
