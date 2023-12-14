using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day13
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long resultR = 0l;
			long resultC = 0l;
			string partial = "";
			List<string> patterns = new List<string>();
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					partial = partial.Substring(1);
					patterns.Add(partial);
					partial = new string("");
				}
				else
				{
					partial += "\n"+line;
				}
			}
			partial = partial.Substring(1);
			patterns.Add(partial);
			foreach (string pattern in patterns)
			{
				Grid g = new Grid(pattern,true);
				int R = GetRowReflection(g);
				int C = GetColReflection(g);
				resultC += C;
				resultR += R;
			}
			return 100*resultR + resultC;
		}

		private static int GetColReflection(Grid pattern, bool p2 = false)
		{
			for (int x = pattern.MinX; x+1 < pattern.MaxX; x++)
			{
				bool p2_b = p2;
				if (CheckCol(pattern, x, ref p2_b))
				{
					if (p2 && !p2_b)
						return x + 1;
					if (!p2)
						return x + 1;
				}
			}

			return 0;
		}
		private static bool CheckCol(Grid pattern, int x, ref bool p2)
		{
			for (int o = 0; x - o >= pattern.MinX && x + o + 1 < pattern.MaxX; o++)
			{
				for (int y = pattern.MinY; y < pattern.MaxY; y++)
				{
					if (pattern[x - o, y] != pattern[x + o + 1, y])
					{
						if (!p2)
							return false;
						p2 = false;
					}
				}
			}

			return true;
		}

		private static int GetRowReflection(Grid pattern, bool p2=false)
		{
			for (int y = pattern.MinY; y+1 < pattern.MaxY; y++)
			{
				bool p2_b = p2;
				if (CheckRow(pattern, y, ref p2_b))
				{
					if(p2 && !p2_b)
						return y + 1;
					if (!p2)
						return y + 1;
				}
			}

			return 0;
		}

		private static bool CheckRow(Grid pattern, int y, ref bool p2)
		{
			for (int o = 0; y - o >= pattern.MinY && y + o + 1 < pattern.MaxY; o++)
			{
				if(pattern[0, y - o] == 0 || pattern[0, y + o + 1] == 0)
					continue;
				for (int x = pattern.MinX; x < pattern.MaxX; x++)
				{
					if (pattern[x, y - o] != pattern[x, y + o + 1])
					{
						if(!p2)
							return false;
						p2 = false;
					}
				}
			}

			return true;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long resultR = 0l;
			long resultC = 0l;
			string partial = "";
			List<string> patterns = new List<string>();
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					partial = partial.Substring(1);
					patterns.Add(partial);
					partial = new string("");
				}
				else
				{
					partial += "\n" + line;
				}
			}
			partial = partial.Substring(1);
			patterns.Add(partial);
			foreach (string pattern in patterns)
			{
				Grid g = new Grid(pattern, true);
				int R = GetRowReflection(g);
				int C = GetColReflection(g);

				int R2 = GetRowReflection(g,true);
				int C2 = GetColReflection(g,true);
				
				if (R == R2) R2 = R = 0;
				if (C == C2) C2 = C = 0;
				
				resultC += C2;
				resultR += R2;
			}
			return 100 * resultR + resultC;
		}
	}
}
