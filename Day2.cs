using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventofCode2023 {
	internal static class Day2 {
		internal static long Part1(string input)
		{
			Regex red = new Regex("(\\d+) red");
			Regex green = new Regex("(\\d+) green");
			Regex blue = new Regex("(\\d+) blue");
			string[] lines = input.Split('\n');
			long result = 0l;
			int gameId = 0;

			int maxR = 12;
			int maxG = 13;
			int maxB = 14;

			foreach (string line in lines)
			{
				gameId++;
				string[] games = line.Split(':')[1].Split(';');
				bool possible = true;
				foreach (string game in games)
				{
					int r = 0, g = 0, b = 0;
					var cr = red.Matches(game);
					if(cr.Count > 0)
						r = int.Parse(cr[0].Groups[1].Value);
					var cg = green.Matches(game);
					if (cg.Count > 0)
						g = int.Parse(cg[0].Groups[1].Value);
					var cb = blue.Matches(game);
					if (cb.Count > 0)
						b = int.Parse(cb[0].Groups[1].Value);
					if (r > maxR || g > maxG || b > maxB)
					{
						possible = false;
						break;
					}
				}
				if(possible)
					result += gameId;
			}
			
			return result;
		}

		internal static long Part2(string input) {
			Regex red = new Regex("(\\d+) red");
			Regex green = new Regex("(\\d+) green");
			Regex blue = new Regex("(\\d+) blue");
			string[] lines = input.Split('\n');
			long result = 0l;
			int gameId = 0;

			foreach (string line in lines)
			{
				gameId++;
				string[] games = line.Split(':')[1].Split(';');

				int maxR = 0;
				int maxG = 0;
				int maxB = 0;

				foreach (string game in games)
				{
					int r = 0, g = 0, b = 0;
					var cr = red.Matches(game);
					if (cr.Count > 0)
						r = int.Parse(cr[0].Groups[1].Value);
					var cg = green.Matches(game);
					if (cg.Count > 0)
						g = int.Parse(cg[0].Groups[1].Value);
					var cb = blue.Matches(game);
					if (cb.Count > 0)
						b = int.Parse(cb[0].Groups[1].Value);
					
					if(r > maxR) maxR = r;
					if(g > maxG) maxG = g;
					if(b > maxB) maxB = b;
				}

				long power = maxR * maxG * maxB;
				Console.WriteLine(gameId + ": " + power);
				result += power;
			}

			return result;
		}
	}
}