using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace AdventofCode2023 {
	internal static class Day8 {
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<string, (string left, string right)> map = new Dictionary<string, (string left, string right)>();
			string instructions = null;
			foreach (string line in lines)
			{
				if (instructions == null)
				{
					instructions = line;
					continue;
				}
				if(string.IsNullOrEmpty(line)) continue;
				string[] parts = line.Split('=');
				string h = parts[0].Replace(" ", "");
				string[] dir = parts[1].Split(',');
				string l = dir[0].Replace("(","").Replace(" ", "");
				string r = dir[1].Replace(")", "").Replace(" ", "");
				map.Add(h, (l,r));
			}
			result = FindPath(map,instructions);
			return result;
		}

		private static int FindPath(Dictionary<string, (string left, string right)> map, string instructions)
		{
			string pos = "AAA";
			int index = 0;
			int steps = 0;
			while (pos != "ZZZ")
			{
				(string left, string right) = map[pos];

				if (instructions[index] == 'R')
				{
					pos = right;
				}
				else
				{
					pos = left;
				}

				steps++;
				index = (index+1)% instructions.Length;
			}

			return steps;
		}

		private static long FindPathGhost(Dictionary<string, (string left, string right)> map, string instructions)
		{
			List<(string start, string cur)> positions = new List<(string, string)>();
			List<(string, string)> nextPos = new List<(string, string)>();
			Dictionary<string, long> stepMath = new Dictionary<string, long>();

			foreach (string k in map.Keys)
			{
				if (k.EndsWith('A'))
				{
					positions.Add((k, k));
				}
			}
			int index = 0;
			long steps = 0;
			
			while (true)
			{
				foreach ((string start, string pos) in positions)
				{
					string npos;
					(string left, string right) = map[pos];

					if (instructions[index] == 'R')
					{
						npos = right;
					}
					else
					{
						npos = left;
					}
					nextPos.Add((start,npos));
				}

				if (positions.Any(p => p.cur.EndsWith('Z')))
				{
					foreach ((string start, string cur) q in positions.Where(p => p.cur.EndsWith('Z')))
					{
						stepMath.Add(q.start, steps);
					}
					
					if (stepMath.Count == positions.Count)
					{
						return stepMath.Select(kvp => kvp.Value).Aggregate(1L, LCM);
					}
				}
				positions.Clear();
				positions.AddRange(nextPos);
				nextPos.Clear();
				steps++;
				index = (index + 1) % instructions.Length;
			}
		}

		private static long GDC(long a, long b) => (b == 0 || b == 1) ? a : GDC(b, a % b);
		private static long LCM(long b, long a) => (b == 0 || b == 1) ? a : a / GDC(a, b) * b;
		
		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<string, (string left, string right)> map = new Dictionary<string, (string left, string right)>();
			string instructions = null;
			foreach (string line in lines)
			{
				if (instructions == null)
				{
					instructions = line;
					continue;
				}
				if (string.IsNullOrEmpty(line)) continue;
				string[] parts = line.Split('=');
				string h = parts[0].Replace(" ", "");
				string[] dir = parts[1].Split(',');
				string l = dir[0].Replace("(", "").Replace(" ", "");
				string r = dir[1].Replace(")", "").Replace(" ", "");
				map.Add(h, (l, r));
			}
			result = FindPathGhost(map, instructions);
			return result;
		}
	}
}