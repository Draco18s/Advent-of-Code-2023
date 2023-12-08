using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023 {
	internal static class Day6 {
		public class RaceData
		{
			public long duration;
			public long record;
		}

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 1l;
			string[] times = lines[0].Split(' ');
			string[] distances = lines[1].Split(' ');

			int[] timeInts = times.Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
			int[] distInts = distances.Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();

			List<RaceData> races = new List<RaceData>();

			for (int i = 0; i < timeInts.Length; i++)
			{
				races.Add(new RaceData()
				{
					duration = timeInts[i],
					record = distInts[i]
				});
			}

			foreach (RaceData race in races)
			{
				long j = GetNumberOfWins(race);
				if (j > 0) result *= j;
			}

			return result;
		}

		private static long GetNumberOfWins(RaceData race)
		{
			long wins = 0;
			for (long hold = 1; hold < race.duration - 1; hold++)
			{
				if (ComputeScore(hold, race.duration - hold) > race.record)
				{
					wins++;
				}
			}
			return wins;
		}

		private static long ComputeScore(long hold, long time)
		{
			return hold * time;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 1l;
			string[] times = lines[0].Replace(" ", "").Split(':');
			string[] distances = lines[1].Replace(" ", "").Split(':');

			long[] timeInts = times.Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToArray();
			long[] distInts = distances.Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToArray();

			RaceData race = new RaceData()
			{
				duration = timeInts[0],
				record = distInts[0]
			};

			result = GetNumberOfWins(race);

			return result;
		}
	}
}