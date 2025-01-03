﻿// See https://aka.ms/new-console-template for more information
using AdventOfCode2024.Solutions;
using System.Diagnostics;

IDay day = new Day25();

Stopwatch sw = Stopwatch.StartNew();
day.SolveA();
sw.Stop();
Console.WriteLine(sw.Elapsed.ToString());

sw = Stopwatch.StartNew();
day.SolveB();
sw.Stop();
Console.WriteLine(sw.Elapsed.ToString());
