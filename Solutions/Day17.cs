using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Solutions
{
    internal class Day17 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day17) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);

            Dictionary<string, ulong> registers = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var parse = Regex.Match(line, "Register (\\w+): (\\d+)");

                var r = parse.Groups[1].Value;
                var v = ulong.Parse(parse.Groups[2].Value);
                registers[r] = v;
            }

            string programString = reader.ReadLine() ?? string.Empty;

            var match = Regex.Match(programString, "Program: (.*)");
            var ops = match.Groups[1].Value.Split(',').Select(c => (ulong)(c[0] - '0')).ToList();

            var res = RunProgram(registers, ops);

            res.Reverse();


            if (res.Count > 0)
            {
                string output = string.Join(",", res);
                Console.WriteLine(output);
            }
        }

        private List<int> RunProgram(Dictionary<string, ulong> registers, List<ulong> opCodes)
        {
            List<int> res = [];
            for (int idx = 0; idx < opCodes.Count; idx += 2)
            {
                var op = opCodes[idx];
                ulong operand = opCodes[idx + 1];
                ulong operandValue = operand;

                switch (operand)
                {
                    case 4:
                        operandValue = registers["A"];
                        break;
                    case 5:
                        operandValue = registers["B"];
                        break;
                    case 6:
                        operandValue = registers["C"];
                        break;
                    default:
                        break;
                }

                switch (op)
                {
                    case 0:
                        var adv = registers["A"];
                        adv >>= (int)operandValue;
                        registers["A"] = adv;
                        break;

                    case 1:
                        var bxl = registers["B"];
                        bxl ^= operand;
                        registers["B"] = bxl;
                        break;

                    case 2:
                        var bst = operandValue % 8;
                        registers["B"] = bst;
                        break;

                    case 3:
                        if (registers["A"] != 0)
                        {
                            idx = (int)operand - 2;
                        }
                        break;

                    case 4:
                        var bxc = registers["B"] ^ registers["C"];
                        registers["B"] = bxc;
                        break;

                    case 5:
                        var out_ = operandValue % 8;
                        res.Insert(0, (int)out_);
                        break;

                    case 6:
                        var bdv = registers["A"];
                        bdv >>= (int)operandValue;
                        registers["B"] = bdv;
                        break;

                    case 7:
                        var cdv = registers["A"];
                        cdv >>= (int)operandValue;
                        registers["C"] = cdv;
                        break;

                    default:
                        break;
                }
            }

            return res;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);

            Dictionary<string, ulong> registers = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var parse = Regex.Match(line, "Register (\\w+): (\\d+)");

                var r = parse.Groups[1].Value;
                var v = ulong.Parse(parse.Groups[2].Value);
                registers[r] = v;
            }

            string programString = reader.ReadLine() ?? string.Empty;

            var match = Regex.Match(programString, "Program: (.*)");
            var ops = match.Groups[1].Value.Split(',').Select(c => (ulong)(c[0] - '0')).ToList();

            var rops = ops.Select(o => (int)o).ToList();
            rops.Reverse();

            var res = FindFrom(registers, ops, rops, 0, 1);

            if (res.Item1)
            {
                Console.WriteLine(res.Item2);
            }
        }

        private (bool, ulong) FindFrom(Dictionary<string, ulong> registers, List<ulong> opCodes, List<int> rops, ulong code, int digits)
        {
            if (digits > opCodes.Count)
            {
                return (true, code >> 3);
            }

            for (ulong i = 0; i < 8; i++)
            {
                ulong mid = code + i;
                registers["A"] = mid;

                var rops2 = RunProgram(registers, opCodes);

                bool found = true;
                foreach ((var a, var b) in rops.Zip(rops2))
                {
                    if (a != b)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    var res = FindFrom(registers, opCodes, rops, mid << 3, digits + 1);

                    if (res.Item1)
                    {
                        return res;
                    }
                }
            }

            return (false, 0);
        }
    }
}
