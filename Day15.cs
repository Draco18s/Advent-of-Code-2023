using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023
{
	internal static class Day15
	{
		internal static long Part1(string input)
		{
			//input = "HASH";
			long result = 0l;
			string[] lines = input.Split(',');
			foreach (string line in lines)
			{
				int r = 0;
				foreach (char c in line)
				{
					if(c == '\n') continue;
					r += (int)c;
					r *= 17;
					r = r % 256;
				}
				result += r;
			}

			return result;
		}

		public static int Hash(string op)
		{
			int r = 0;
			foreach (char c in op)
			{
				if (c == '\n') continue;
				r += (int)c;
				r *= 17;
				r = r % 256;
			}
			return r;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split(',');
			long result = 0l;
			LensBox[] boxes = new LensBox[256];
			for (var index = 0; index < boxes.Length; index++)
			{
				boxes[index] = new LensBox();
			}

			foreach (string line in lines)
			{
				int i = line.IndexOf('-') >= 0 ? line.IndexOf('-') : line.IndexOf('=');
				string label = line.Substring(0, i);
				string num = line.Substring(i+1);
				char op = line[i];
				int boxId = Hash(label);
				int ll = string.IsNullOrEmpty(num) ? 0 : int.Parse(num);

				LabeledLens lens = new LabeledLens()
				{
					lable = label,
					focal = ll
				};
				switch (op)
				{
					case '-':
						boxes[boxId].lenses.Remove(lens);
						break;
					case '=':
						if (boxes[boxId].lenses.Contains(lens))
						{
							int j = boxes[boxId].lenses.IndexOf(lens);
							boxes[boxId].lenses[j] = lens;
						}
						else
						{
							boxes[boxId].lenses.Add(lens);
						}
						break;
				}
			}
			
			for (var b = 0; b < boxes.Length; b++)
			{
				LensBox box = boxes[b];
				for (int l = 0; l < box.lenses.Count; l++)
				{
					result += (b + 1) * (l + 1) * box.lenses[l].focal;
				}
			}

			return result;
		}

		public struct LabeledLens
		{
			public string lable;
			public int focal;

			public override bool Equals(object obj)
			{
				if(obj is LabeledLens other)
					return other.lable.Equals(lable);
				return base.Equals(obj);
			}

			public override int GetHashCode()
			{
				return lable.GetHashCode();
			}
		}

		public class LensBox
		{
			public List<LabeledLens> lenses = new List<LabeledLens>();
		}
	}
}
