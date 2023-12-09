using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023
{
	internal static class Day9
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				long[] values = line.Split(' ').Select(long.Parse).ToArray();
				long prediction = Compute(values);
				result += prediction;
			}
			return result;
		}

		private static long Compute(long[] values)
		{
			long[] diff = new long[values.Length-1];
			bool wasNonZero = false;
			for (int i = 0; i < diff.Length; i++)
			{
				diff[i] = values[i+1] - values[i];
				wasNonZero |= diff[i] != 0;
			}

			if (!wasNonZero)
			{
				return values[0];
			}

			return values[^1] + Compute(diff);
		}

		private static long Compute2(long[] values)
		{
			long[] diff = new long[values.Length - 1];
			bool wasNonZero = false;
			for (int i = 0; i < diff.Length; i++)
			{
				diff[i] = values[i + 1] - values[i];
				wasNonZero |= diff[i] != 0;
			}

			if (!wasNonZero)
			{
				return values[0];
			}

			return values[0] - Compute2(diff);
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				long[] values = line.Split(' ').Select(long.Parse).ToArray();
				long prediction = Compute2(values);
				result += prediction;
			}
			return result;
		}
	}
}
