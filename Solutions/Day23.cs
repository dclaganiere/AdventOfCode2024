using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Solutions
{
    internal class Day23 : IDay
    {
        const string InputFile = "Input\\" + nameof(Day23) + ".txt";
        public void SolveA()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            HashSet<string> computers = [];
            Dictionary<string, HashSet<string>> connections = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split('-');
                var pc1 = sp[0];
                var pc2 = sp[1];

                computers.Add(pc1);
                computers.Add(pc2);

                if (!connections.ContainsKey(pc1))
                {
                    connections.Add(pc1, []);
                }
                if (!connections.ContainsKey(pc2))
                {
                    connections.Add(pc2, []);
                }

                connections[pc1].Add(pc2);
                connections[pc2].Add(pc1);
            }

            List<string> pcList = computers.ToList();

            for (int i = 0; i < pcList.Count; i++)
            {
                var pc1 = pcList[i];
                for (int j = i; j < pcList.Count; j++)
                {
                    var pc2 = pcList[j];
                    for (int k = j; k < pcList.Count; k++)
                    {
                        var pc3 = pcList[k];

                        if (!pc1.StartsWith("t") && !pc2.StartsWith("t") && ! pc3.StartsWith("t"))
                        {
                            continue;
                        }

                        if (connections[pc1].Contains(pc2) && connections[pc1].Contains(pc3)
                            && connections[pc2].Contains(pc1) && connections[pc2].Contains(pc3)
                            && connections[pc3].Contains(pc1) && connections[pc3].Contains(pc2))
                        {
                            sum++;
                        }

                    }

                }
            }

            Console.WriteLine(sum);
        }

        public void SolveB()
        {
            using StreamReader reader = new(InputFile);
            int sum = 0;

            HashSet<string> computers = [];
            Dictionary<string, HashSet<string>> connections = [];

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine() ?? string.Empty;
                var sp = line.Split('-');
                var pc1 = sp[0];
                var pc2 = sp[1];

                computers.Add(pc1);
                computers.Add(pc2);

                if (!connections.ContainsKey(pc1))
                {
                    connections.Add(pc1, []);
                }
                if (!connections.ContainsKey(pc2))
                {
                    connections.Add(pc2, []);
                }

                connections[pc1].Add(pc2);
                connections[pc2].Add(pc1);
            }

            List<string> pcList = FindLAN(connections, computers);
            pcList.Sort();
            string ans = string.Join(",", pcList);
            Console.WriteLine(ans);
        }

        public List<string> FindLAN(Dictionary<string, HashSet<string>> connections, HashSet<string> computers)
        {
            HashSet<string> visited = [];

            while (visited.Count < computers.Count)
            {
                var start = computers.Except(visited).First();

                Dictionary<string, int> counts = [];
                foreach (var pc in connections[start])
                {
                    counts[pc] = connections[pc].Intersect(connections[start]).Count();
                }

                if (counts.Where(x => x.Value > 0).Count(x => x.Value == 11) < 11)
                {
                    visited.Add(start);
                }
                else
                {
                    List<string> res = connections[start].ToList();
                    res.Remove(counts.First(x => x.Value == 0).Key);
                    res.Add(start);
                    return res;
                }
            }

            return [];
        }
    }
}
