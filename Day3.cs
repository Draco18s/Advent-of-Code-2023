using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023 {
	internal static class Day3 {
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			for (var l = 0; l < lines.Length; l++)
			{
				var line = lines[l];
				for (var k = 0; k < line.Length; k++)
				{
					var c = line[k];
					if (char.IsDigit(c))
					{
						int num = ExtractValue(lines, l, ref k);
						if (num > 0)
						{
							result += num;
						}
					}
				}
			}

			return result;
		}

		private static int ExtractValue(string[] lines, int y, ref int x)
		{
			char symbol = '\0';
			for (int ox = -1; ox <= 1; ox++)
			{
				for (int oy = -1; oy <= 1; oy++)
				{
					if(ox ==0  && oy == 0) continue;
					if(y+oy < 0) continue;
					if(x+ox < 0) continue;
					if(x+ox >= lines.Length) continue;
					if(y+oy  >= lines.Length) continue;
					if (char.IsDigit(lines[y+oy][x+ox]) || lines[y+oy][x+ox] == '.') continue;
					symbol = lines[y+oy][x+ox];
				}
			}

			if (symbol == '\0') return 0;

			int ex = x;
			while (x >= 0 && char.IsDigit(lines[y][x]))
			{
				x--;
			}
			while (ex < lines[y].Length && char.IsDigit(lines[y][ex]))
			{
				ex++;
			}

			string v = lines[y].Substring(x+1, ex-x-1);

			x = ex;

			return int.Parse(v);
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			for (var l = 0; l < lines.Length; l++)
			{
				var line = lines[l];
				for (var k = 0; k < line.Length; k++)
				{
					var c = line[k];
					if (c == '*')
					{
						List<int> values = new List<int>();
						for (int oy = -1; oy <= 1; oy++)
						{
							for (int ox = -1; ox <= 1; ox++)
							{
								int X = k + ox;

								if(!char.IsDigit(lines[l + oy][X])) continue;

								int num = ExtractValue(lines, l + oy, ref X);

								if (num > 0)
									values.Add(num);
								ox = X - k;
							}
						}

						//values = values.Distinct().ToList();
						if(values.Count != 2)
							continue;
						int v = values.Aggregate(1, (v, r) => r * v);
						result += v;
					}
				}
			}
			return result;
		}
	}
}