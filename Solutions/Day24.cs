using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day24 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day24) + "fix.txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            Dictionary<string, bool> wires = [];
            Dictionary<string, List<int>> related = [];
            List<(string x, string y, string res, string op)> operations = [];
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var sp = line.Split(": ");
                var wire = sp[0];
                bool value = sp[1] == "1";
                wires.Add(wire, value);

                if (!related.ContainsKey(wire))
                {
                    related[wire] = [];
                }
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                var match = Regex.Match(line, "(\\S+) (\\S+) (\\S+) -> (\\S+)");
                if (match.Success)
                {
                    var x = match.Groups[1].Value;
                    var op = match.Groups[2].Value;
                    var y = match.Groups[3].Value;
                    var res = match.Groups[4].Value;

                    operations.Add((x, y, res, op));

                    if (!related.ContainsKey(x))
                    {
                        related[x] = [];
                    }
                    if (!related.ContainsKey(y))
                    {
                        related[y] = [];
                    }
                    if (!related.ContainsKey(res))
                    {
                        related[res] = [];
                    }

                    related[x].Add(operations.Count - 1);
                    related[y].Add(operations.Count - 1);
                    related[res].Add(operations.Count - 1);
                }
            }

            HashSet<int> evaluated = [];

            Queue<int> queue = [];
            foreach ( var w in wires.Keys )
            {
                foreach (var i in related[w])
                {
                    queue.Enqueue(i);
                }
            }

            while (queue.Any())
            {
                var idx = queue.Dequeue();

                if (evaluated.Contains(idx))
                {
                    continue;
                }

                var op = operations[idx];

                if (wires.TryGetValue(op.x, out bool xv) && wires.TryGetValue(op.y, out bool yv))
                {
                    bool res;
                    switch (op.op)
                    {
                        case "AND":
                            res = xv && yv;
                            break;
                        case "OR":
                            res = xv || yv;
                            break;
                        case "XOR":
                            res = xv ^ yv;
                            break;
                        default:
                            throw new Exception("Huh");
                    }

                    wires[op.res] = res;

                    foreach(var r in related[op.res])
                    {
                        queue.Enqueue(r);
                    }
                    evaluated.Add(idx);
                }
            }

            ulong xval = 0;
            for (int i = 44; i >= 0; i--)
            {
                string key = $"x{i:00}";

                ulong val = wires[key] ? 1ul : 0ul;

                xval <<= 1;
                xval |= val;
            }


            ulong yval = 0;
            for (int i = 44; i >= 0; i--)
            {
                string key = $"y{i:00}";

                ulong val = wires[key] ? 1ul : 0ul;

                yval <<= 1;
                yval |= val;
            }

            int maxZ = -1;
            for (int i = 0; i < 64; i++)
            {
                string key = $"z{i:00}";

                if (!wires.ContainsKey(key))
                {
                    maxZ = i - 1;
                    break;
                }
            }

            ulong ans = 0;
            for( int i = maxZ; i >= 0; i--)
            {
                string key = $"z{i:00}";

                ulong val = wires[key] ? 1ul : 0ul;

                ans <<= 1;
                ans |= val;
            }


            Console.WriteLine(ans);
            Console.WriteLine($"{xval} + {yval} == {xval + yval}");
        }

        private readonly List<(int r, int c)> directions =
        [
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1)
        ];

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;
            Dictionary<string, bool> wires = [];
            Dictionary<string, List<int>> related = [];
            List<(string x, string y, string res, string op)> operations = [];
            Dictionary<string, int> requiredOps = [];
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var sp = line.Split(": ");
                var wire = sp[0];
                bool value = sp[1] == "1";
                wires.Add(wire, value);

                if (!related.ContainsKey(wire))
                {
                    related[wire] = [];
                }
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                var match = Regex.Match(line, "(\\S+) (\\S+) (\\S+) -> (\\S+)");
                if (match.Success)
                {
                    var x = match.Groups[1].Value;
                    var op = match.Groups[2].Value;
                    var y = match.Groups[3].Value;
                    var res = match.Groups[4].Value;

                    operations.Add((x, y, res, op));

                    if (!related.ContainsKey(x))
                    {
                        related[x] = [];
                    }
                    if (!related.ContainsKey(y))
                    {
                        related[y] = [];
                    }
                    if (!related.ContainsKey(res))
                    {
                        related[res] = [];
                    }

                    related[x].Add(operations.Count - 1);
                    related[y].Add(operations.Count - 1);
                    related[res].Add(operations.Count - 1);
                    requiredOps[res] = operations.Count - 1;
                }
            }

            List<string> halfAdd1_xor = [];
            List<string> halfAdd1_and = [];
            List<string> halfAdd2_xor = [];
            List<string> halfAdd2_and = [];
            List<string> fullAddCarry = [];
            int maxZ = -1;
            for (int i = 0; i < 64; i++)
            {
                string key = $"z{i:00}";

                if (!requiredOps.ContainsKey(key))
                {
                    maxZ = i - 1;
                    break;
                }

                halfAdd1_xor.Add(string.Empty);
                halfAdd1_and.Add(string.Empty);
                halfAdd2_xor.Add(string.Empty);
                halfAdd2_and.Add(string.Empty);
                fullAddCarry.Add(string.Empty);
            }

            for (int i = 1; i <= maxZ; i++)
            {
                string key = $"z{i:00}";
                var op = operations[requiredOps[key]];
                string c_prev;

                if (i == 45)
                {
                    c_prev = key;
                }
                else
                {

                    if (op.op != "XOR")
                    {
                        throw new Exception($"XOR not used for evaluating {key}: {op}");
                    }

                    var m = op.x;
                    c_prev = op.y;

                    var m_op = operations[requiredOps[m]];
                    if (m_op.op != "XOR")
                    {
                        (c_prev, m) = (m, c_prev);
                        m_op = operations[requiredOps[m]];
                    }

                    halfAdd2_xor[i] = key;
                    halfAdd1_xor[i] = m;

                    var x = m_op.x;
                    var y = m_op.y;

                    if (!x.StartsWith(x))
                    {
                        (y, x) = (x, y);
                        if (!x.StartsWith(x))
                        {
                            throw new Exception("Huh");
                        }
                    }

                    if (!y.StartsWith(y))
                    {
                        throw new Exception("Huh");
                    }
                }


                fullAddCarry[i - 1] = c_prev;

                var c_op = operations[requiredOps[c_prev]];
                var cn = c_op.x;
                var cl = c_op.y;

                if (i == 1)
                {
                    if (c_op.op != "AND")
                    {
                        throw new Exception("Huh");
                    }
                    halfAdd1_xor[i - 1] = "z00";
                    halfAdd1_and[i - 1] = c_prev;
                    continue;
                }

                if (c_op.op != "OR")
                {
                    throw new Exception("Huh");
                }

                var l_op = operations[requiredOps[cl]];

                if (!l_op.x.StartsWith('x') && !l_op.y.StartsWith('x'))
                {
                    (cn, cl) = (cl, cn);
                    l_op = operations[requiredOps[cl]];
                }

                if (l_op.op != "AND")
                {
                    throw new Exception("Huh");
                }

                var lx = l_op.x;
                var ly = l_op.y;

                if (!lx.StartsWith('x'))
                {
                    (ly, lx) = (lx, ly);
                }

                if (!lx.StartsWith('x'))
                {
                    throw new Exception("Huh");
                }
                if (!ly.StartsWith('y'))
                {
                    throw new Exception("Huh");
                }

                var n_op = operations[requiredOps[cn]];

                if (n_op.op != "AND")
                {
                    throw new Exception("Huh");
                }

                var nx = n_op.x;
                var ny = n_op.y;

                if (nx != halfAdd1_xor[i - 1])
                {
                    (ny, nx) = (nx, ny);
                }
                if (nx != halfAdd1_xor[i - 1])
                {
                    throw new Exception("Huh");
                }
                
                if (i == 1)
                {
                    fullAddCarry[i - 2] = ny;
                }
                else if (ny != fullAddCarry[i - 2])
                {
                    throw new Exception("Huh");
                }

                halfAdd1_and[i - 1] = cl;
                halfAdd2_and[i - 1] = cn;
            }

            Console.WriteLine($"00 - HA1: ({halfAdd1_xor[0]}, {halfAdd1_and[0]}) Carry: {fullAddCarry[0]}");

            for (int i = 1; i < 45; i++)
            {
                Console.WriteLine($"{i:00} - HA1: ({halfAdd1_xor[i]}, {halfAdd1_and[i]}) HA2: ({halfAdd2_xor[i]}, {halfAdd2_and[i]}) Carry: {fullAddCarry[i]}");
            }

            Console.WriteLine("Carry: z45");

            Console.WriteLine(sum);
        }
    }
}
