using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
{
    internal class Day15 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day15) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<string> grid = new List<string>();

            HashSet<(int r, int c)> boxes = new();
            HashSet<(int r,int c)> walls = new();
            int r = 0, c = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                for (int i = 0; i < line.Length; i++)
                {
                    switch(line[i])
                    {
                        case 'O':
                            boxes.Add((grid.Count, i));
                            break;
                        case '#':
                            walls.Add((grid.Count, i));
                            break;
                        case '@':
                            r = grid.Count;
                            c = i;
                            break;
                        default:
                            break;
                    }
                }
                grid.Add(line);
            }

            string input = string.Empty;


            while (!reader.EndOfStream)
            {
                input += reader.ReadLine() ?? string.Empty;
            }

            MoveRobot(boxes, walls, input, r, c);

            sum = boxes.Sum(b => 100 * b.r + b.c);

            Console.WriteLine(sum);
        }


        private Dictionary<char, (int r, int c)> directions = new Dictionary<char, (int r, int c)>()
        {
            {'^', (-1, 0) },
            {'>', (0, 1) },
            {'v', (1, 0) },
            {'<', (0, -1) }
        };

        public void MoveRobot(HashSet<(int r, int c)> boxes, HashSet<(int r, int c)> walls, string input, int r, int c)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var move = directions[input[i]];

                (int r, int c) next = (r + move.r, c + move.c);

                if (walls.Contains(next))
                {
                    continue;
                }

                if (boxes.Contains(next))
                {
                    if (!MoveBox(boxes, walls, next.r, next.c, input[i]))
                    {
                        continue;
                    }
                }

                r = next.r;
                c = next.c;
            }
        }

        public bool MoveBox(HashSet<(int r, int c)> boxes, HashSet<(int r, int c)> walls, int r, int c, char dir)
        {
            var move = directions[dir];

            (int r, int c) next = (r + move.r, c + move.c);

            if (walls.Contains(next))
            {
                return false;
            }

            if (boxes.Contains(next))
            {
                if (!MoveBox(boxes, walls, next.r, next.c, dir))
                {
                    return false;
                }
            }

            boxes.Remove((r, c));
            boxes.Add(next);
            return true;
        }

        public int MaxRow = 0;
        public int MaxCol = 0;

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            List<string> grid = new List<string>();

            HashSet<(int r, int c)> lboxes = new();
            HashSet<(int r, int c)> rboxes = new();
            HashSet<(int r, int c)> walls = new();
            int r = 0, c = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'O':
                            sb.Append("[]");
                            break;
                        case '#':
                            sb.Append("##");
                            break;
                        case '@':
                            sb.Append("@.");
                            break;
                        case '.':
                            sb.Append("..");
                            break;
                        default:
                            break;
                    }
                }

                line = sb.ToString();


                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case '[':
                            lboxes.Add((grid.Count, i));
                            break;
                        case ']':
                            rboxes.Add((grid.Count, i));
                            break;
                        case '#':
                            walls.Add((grid.Count, i));
                            break;
                        case '@':
                            r = grid.Count;
                            c = i;
                            break;
                        default:
                            break;
                    }
                }

                grid.Add(line);
            }

            MaxRow = grid.Count;
            MaxCol = grid[0].Length;

            string input = string.Empty;


            while (!reader.EndOfStream)
            {
                input += reader.ReadLine() ?? string.Empty;
            }

            MoveRobot2(lboxes, rboxes, walls, input, r, c);

            sum = lboxes.Sum(b => 100 * b.r + b.c);

            Console.WriteLine(sum);
        }

        public void MoveRobot2(HashSet<(int r, int c)> lboxes, HashSet<(int r, int c)> rboxes, HashSet<(int r, int c)> walls, string input, int r, int c)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var move = directions[input[i]];

                (int r, int c) next = (r + move.r, c + move.c);

                if (walls.Contains(next))
                {
                    continue;
                }

                if (lboxes.Contains(next))
                {
                    if (!CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c, input[i]))
                    {
                        continue;
                    }
                    MoveBoxL(lboxes, rboxes, walls, next.r, next.c, input[i]);
                }

                if (rboxes.Contains(next))
                {
                    if (!CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, input[i]))
                    {
                        continue;
                    }
                    MoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, input[i]);
                }

                r = next.r;
                c = next.c;
            }
        }

        public bool MoveBoxL(HashSet<(int r, int c)> lboxes, HashSet<(int r, int c)> rboxes, HashSet<(int r, int c)> walls, int r, int c, char dir)
        {
            var move = directions[dir];
            (int r, int c) next = (r + move.r, c + move.c);
            (int r, int c) next_r = (r + move.r, c + move.c + 1);

            if (walls.Contains(next))
            {
                return false;
            }

            switch (dir)
            {
                case '>':
                    if (lboxes.Contains(next_r))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next_r.r, next_r.c, dir);
                        break;
                    }
                    break;
                case '<':
                    if (rboxes.Contains(next))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, dir);
                    }
                    break;
                case '^':
                    if (lboxes.Contains(next))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    else if (rboxes.Contains(next))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, dir);
                    }

                    if (lboxes.Contains(next_r))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next_r.r, next_r.c, dir);
                    }
                    break;
                case 'v':
                    if (lboxes.Contains(next))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    else if (rboxes.Contains(next))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, dir);
                    }

                    if (lboxes.Contains(next_r))
                    {
                        MoveBoxL(lboxes, rboxes, walls, next_r.r, next_r.c, dir);
                    }
                    break;
                default:
                    break;
            }

            lboxes.Remove((r, c));
            lboxes.Add(next);
            rboxes.Remove((r, c + 1));
            rboxes.Add((next.r, next.c + 1));
            return true;
        }

        public bool CanMoveBoxL(HashSet<(int r, int c)> lboxes, HashSet<(int r, int c)> rboxes, HashSet<(int r, int c)> walls, int r, int c, char dir)
        {
            var move = directions[dir];
            (int r, int c) next = (r + move.r, c + move.c);

            if (walls.Contains(next))
            {
                return false;
            }

            switch (dir)
            {
                case '>':
                    return CanMoveBoxR(lboxes, rboxes, walls, next.r, next.c, dir);
                case '<':
                    if (rboxes.Contains(next))
                    {
                        return CanMoveBoxR(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    return true;
                case '^':
                    (int r, int c) next_r = (next.r, next.c + 1);

                    if (walls.Contains(next_r))
                    {
                        return false;
                    }

                    if (lboxes.Contains(next))
                    {
                        return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    if (rboxes.Contains(next))
                    {
                        if (lboxes.Contains(next_r))
                        {
                            if (!CanMoveBoxL(lboxes, rboxes, walls, next_r.r, next_r.c , dir))
                            {
                                return false;
                            }
                        }

                        return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, dir);
                    }

                    if (lboxes.Contains(next_r))
                    {
                        if (!CanMoveBoxL(lboxes, rboxes, walls, next_r.r, next_r.c, dir))
                        {
                            return false;
                        }
                    }

                    return true;
                case 'v':
                    (int r, int c) next_r2 = (next.r, next.c + 1);

                    if (walls.Contains(next_r2))
                    {
                        return false;
                    }

                    if (lboxes.Contains(next))
                    {
                        return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    if (rboxes.Contains(next))
                    {
                        if (lboxes.Contains(next_r2))
                        {
                            if (!CanMoveBoxL(lboxes, rboxes, walls, next_r2.r, next_r2.c, dir))
                            {
                                return false;
                            }
                        }

                        return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c - 1, dir);
                    }

                    if (lboxes.Contains(next_r2))
                    {
                        if (!CanMoveBoxL(lboxes, rboxes, walls, next_r2.r, next_r2.c, dir))
                        {
                            return false;
                        }
                    }

                    return true;
                default:
                    break;
            }

            return true;
        }

        public bool CanMoveBoxR(HashSet<(int r, int c)> lboxes, HashSet<(int r, int c)> rboxes, HashSet<(int r, int c)> walls, int r, int c, char dir)
        {
            var move = directions[dir];
            (int r, int c) next = (r + move.r, c + move.c);

            if (walls.Contains(next))
            {
                return false;
            }

            switch (dir)
            {
                case '<':
                    return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                case '>':
                    if (lboxes.Contains(next))
                    {
                        return CanMoveBoxL(lboxes, rboxes, walls, next.r, next.c, dir);
                    }
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        public void PrintGrid(HashSet<(int r, int c)> lboxes, HashSet<(int r, int c)> rboxes, HashSet<(int r, int c)> walls, int r, int c)
        {
            for (int i = 0; i < MaxRow; i++)
            {
                for (int j = 0; j < MaxCol; j++)
                {
                    if (r == i && c == j)
                    {
                        Console.Write('@');
                    }
                    else if (lboxes.Contains((i, j)))
                    {
                        Console.Write('[');
                    }
                    else if (rboxes.Contains((i, j)))
                    {
                        Console.Write(']');
                    }
                    else if (walls.Contains((i, j)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
