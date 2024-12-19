using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{

    internal class Day9 : IDay
    {
        internal class FileSpace(int idx, long size)
        {
            public int Id { get; set; } = idx%2 == 0 ? idx/2 : -1;
            public long Size { get; set; } = size;

            public bool IsSpace { get; set; } = idx % 2 != 0;
            
            public override string? ToString()
            {
                return $"{this.Id}: {this.Size}";
            }
        }

        const string InputFile = "Input\\" + nameof(Day9) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;
            List<int> input = new();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                input = line.Select(c => (c - '0')).ToList();
            }

            sum = GetChecksum(input);

            Console.WriteLine(sum);
        }

        private static long GetChecksum(List<int> input)
        {
            long sum = 0;
            List<(int id, int size)> files = new();

            for (int i = 0; i * 2 < input.Count; i++)
            {
                int curr = input[i * 2];
                files.Add((i, curr));
            }

            files.Reverse();

            int slot = 0;
            int fileSize = files.Sum(f => f.size);
            int fileId = 0;
            int fileWritten = 0;
            for (int i = 0; i < input.Count && slot < fileSize; i++)
            {
                if (i % 2 == 0)
                {
                    int id = i / 2;
                    for (int j = 0; j < input[i] && slot < fileSize; j++)
                    {
                        sum += id * slot;
                        slot++;
                    }
                }
                else
                {
                    int data = 0;
                    while (data < input[i] && slot < fileSize)
                    {
                        var file = files[fileId];
                        sum += file.id * slot;
                        slot++;
                        fileWritten++;
                        data++;

                        if (fileWritten == file.size)
                        {
                            fileId++;
                            fileWritten = 0;
                        }
                    }
                }
            }

            return sum;
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            long sum = 0;
            List<FileSpace> input = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                input = line.Select((c, idx) => new FileSpace(idx, c - '0')).ToList();
            }

            for (int i = input.Count - 1; i >= 0; i--)
            {
                var fs = input[i];
                if (i % 2 == 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (j % 2 == 1 && input[j].Size >= fs.Size)
                        {
                            input[j].Size -= fs.Size;
                            input.Remove(fs);
                            input[i - 1].Size += fs.Size;
                            input.Insert(j, new FileSpace(-1, 0));
                            input.Insert(j + 1, fs);
                            i += 2;
                            break;
                        }
                    }
                }
            }

            long slot = 0;
            for (int i = 0; i < input.Count; i++)
            {
                var fs = input[i];
                if (fs.Id == -1)
                {
                    slot += fs.Size;
                }
                else
                {
                    for (long j = 0; j < fs.Size; j++)
                    {
                        sum += fs.Id * slot;
                        slot++;
                    }
                }
            }

            Console.WriteLine(sum);
        }
    }
}
