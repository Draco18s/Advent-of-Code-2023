using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023 {
	internal static class Day4 {
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string[] halves = line.Split('|');
				string[] numbers = halves[0].Split(':')[1].Split(' ');
				string[] winning = halves[1].Split(' ');
				
				List<int> nums = numbers.Where(s => s.Length > 0).Select(int.Parse).ToList();
				List<int> wins = winning.Where(s => s.Length > 0).Select(int.Parse).ToList();

				int pts = nums.Aggregate(0, (r, c) =>
				{
					int v = wins.Contains(c) ? (r == 0 ? 1 : r * 2) : r;

					return v;
				});

				result += pts;
			}
			return result;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;

			Dictionary<int, int> winnings = new Dictionary<int, int>();
			Dictionary<int, int> copies = new Dictionary<int, int>();
			

			int cardId = 0;
			
			foreach (string line in lines)
			{
				cardId++;
				copies.Add(cardId, 1);
				string[] halves = line.Split('|');
				string[] numbers = halves[0].Split(':')[1].Split(' ');
				string[] winning = halves[1].Split(' ');

				List<int> nums = numbers.Where(s => s.Length > 0).Select(int.Parse).ToList();
				List<int> wins = winning.Where(s => s.Length > 0).Select(int.Parse).ToList();
				
				int v = 0;
				foreach (int i in nums)
				{
					if (wins.Contains(i))
					{
						v++;
					}
				}

				winnings.Add(cardId, v);
			}
			cardId = 0;
			foreach (string _ in lines)
			{
				cardId++;
				int c = winnings[cardId];
				for (int n = 0; n < c; n++)
				{
					copies[cardId+n+1] += copies[cardId];
				}
			}

			foreach (var kvp in copies)
			{
				result += kvp.Value;
			}
			return result;
		}
	}
}