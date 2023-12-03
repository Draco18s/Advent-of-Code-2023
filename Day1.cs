using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023 {
	internal static class Day1 {
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string val = "";
				foreach (var c in line)
				{
					if (char.IsDigit(c))
						val += c;
				}

				result += int.Parse(val[0].ToString() + val[^1].ToString());
			}
			return result;
		}

		private static bool CheckFor(string value, string line, int index)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (i+ index >= line.Length) return false;
				if (value[i] != line[i+ index]) return false;
			}

			return true;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string val = "";
				for (var index = 0; index < line.Length; index++)
				{
					var c = line[index];
					if (char.IsDigit(c))
						val += c;
					if (CheckFor("one", line, index))
						val += '1';
					if (CheckFor("two", line, index))
						val += '2';
					if (CheckFor("three", line, index))
						val += '3';
					if (CheckFor("four", line, index))
						val += '4';
					if (CheckFor("five", line, index))
						val += '5';
					if (CheckFor("six", line, index))
						val += '6';
					if (CheckFor("seven", line, index))
						val += '7';
					if (CheckFor("eight", line, index))
						val += '8';
					if (CheckFor("nine", line, index))
						val += '9';
				}
				result += int.Parse(val[0].ToString() + val[^1].ToString());
			}
			return result;
		}
	}
}