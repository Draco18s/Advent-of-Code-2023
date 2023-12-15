using Draco18s.AoCLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows.Markup;

namespace AdventofCode2023
{
	internal static class Day12
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string[] data = line.Split(' ');
				List<int> sizes = data[1].Split(',').Select(int.Parse).ToList();
				result += GetPermutations(data[0], sizes);
			}
			return result;
		}

		private static long GetPermutations(string s, List<int> sizes)
		{
			if(s.Length == 0 || sizes.Count == 0) return 1; //default
			s = s.Trim('.');

			int minLength = sizes.Sum() + sizes.Count - 1;
			if (s.Length < minLength) return 0;
			if (s.Length == minLength) return s.Contains('.') ? 0 : 1; //no possible arrangements are possible except 1
			
			long count = 0;
			int m = s.Count(c => c == '?');
			for (long i = 0; i < (1 << m); i++)
			{
				string binary = Convert.ToString(i, 2).PadLeft(m,'0');
				char[] rep = s.ToCharArray();
				int j = 0;
				for (int i2 = 0; i2 < rep.Length; i2++)
				{
					if (rep[i2] == '?')
					{
						rep[i2] = j < binary.Length ? (binary[j] == '1' ? '#' : '.') : '.';
						j++;
					}
				}
				
				if (Compare(rep, s, sizes, out int O))
				{
					count++;
					//string str = string.Join("", rep);
					//Console.WriteLine($"{i}: {str}");
				}
			}
			return count;
		}

		private static bool Compare(char[] a, string b, List<int> sizes, out int index)
		{
			int l = b.Length;
			if (a.Length < l)
			{
				char[] d = new char[l];
				var startAt = d.Length - a.Length;
				Array.Copy(a, 0, d, startAt, a.Length);
				a = d;
			}
			int s = 0;
			int c = 0;
			for (int i = 0; i < l; i++)
			{
				index = i;
				if (a[i] == '#')
				{
					c++;
					if (s >= sizes.Count || c > sizes[s])
						return false;
				}

				if (a[i] == '.')
				{
					if (c > 0)
					{
						if (c < sizes[s])
							return false;
						c = 0;
						s++;
					}
				}
				if (!Compare(a[i], b[i]))
					return false;
			}
			index = l;
			if (c > 0)
			{
				if (c < sizes[s])
					return false;
				s++;
			}
			return s == sizes.Count;
		}

		private static bool Compare(char a, char b)
		{
			return a == b || b == '?';
		}


		private static Dictionary<SpringBreakdown, long> Lookup = new Dictionary<SpringBreakdown, long>();

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string[] data = line.Split(' ');

				data[0] = data[0] + '?' + data[0] + '?' + data[0] + '?' + data[0] + '?' + data[0];
				data[1] = data[1] + ',' + data[1] + ',' + data[1] + ',' + data[1] + ',' + data[1];

				int[] sizes = data[1].Split(',').Select(int.Parse).ToArray();
				
				SpringBreakdownContainer cont = new SpringBreakdownContainer(data[0], sizes);
				result += cont.GetPermutations();
			}
			
			return result;
		}

		private static void Inc(int[] s, int max, int[] resetVal)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (s.Sum()+1 < max)
				{
					s[i]++;
					return;
				}
				else if(i < s.Length-1)
				{
					s[i] = resetVal[i];
				}
			}
			s[^1] = max;
		}

		private class SpringBreakdownContainer
		{
			public string chunk;
			public int[] values;
			public List<SpringBreakdown[]> subsections;
			public long permute = -1;

			public SpringBreakdownContainer(string chunk, int[] values)
			{
				this.chunk = chunk;
				this.values = values;
				subsections = new List<SpringBreakdown[]>();

				while (chunk.Contains(".."))
					chunk = chunk.Replace("..", ".");

				int[] s = new int[values.Length];
				//int minVal = s.Min();
				for (int i = 0; i < s.Length; i++)
				{
					s[i] = values[i];
				}
				int m = chunk.Length;
				long loops = 0;
				for (; Cont(s, m); Inc(s, m, values))
				{
					loops++;
					//this takes too long
					if (Invalid(s, values)) continue;
					SpringBreakdown[] frags = new SpringBreakdown[s.Length];
					int skip = 0;
					for (int i = 0; i < s.Length; i++)
					{
						frags[i] = new SpringBreakdown(string.Join("",chunk.Skip(skip).Take(s[i])), values[i]);
						skip += s[i];
						if (frags[i].GetPermutations() == 0)
							break;
					}

					if(frags.Any(f => f.GetPermutations() == 0)) continue;

					loops = 0;
					if (!subsections.Any(list => Day12.Matches(list, frags)))
					{
						subsections.Add(frags);
					}
				}
			}

			private bool Invalid(int[] s, int[] j)
			{
				for (int i = 0; i < s.Length/2; i++)
				{
					if (s[i] < j[i]) return true;
					if (s[^(i+1)] < j[^(i+1)]) return true;
				}

				return false;
			}

			private static bool Cont(int[] ints, int target)
			{
				for (int i = 0; i < ints.Length; i++)
				{
					target -= ints[i];
					if (target <= 0) return false;
				}
				return true;
			}

			public long GetPermutations()
			{
				if (permute < 0)
					permute = subsections.Sum(sec => sec.Aggregate(1L, (r, s) => r*s.GetPermutations()));

				return permute;
			}

			public override string ToString()
			{
				return $"{chunk} {string.Join(',', values)}";
			}
		}

		private static bool Matches(SpringBreakdown[] a, SpringBreakdown[] b)
		{
			return !a.Where((t, i) => !t.Equals(b[i])).Any();
		}

		private class SpringBreakdown
		{
			private readonly string chunk;
			private readonly int value;
			private long permute = -1;

			public SpringBreakdown(string chunk, int value)
			{
				this.chunk = chunk;
				this.value = value;
			}

			public long GetPermutations()
			{
				if (permute >= 0) return permute;

				if (Lookup.TryGetValue(this, out long v))
				{
					permute = v;
				}
				else
				{
					permute = Day12.GetPermutations(chunk, new List<int> { value });
					Lookup.Add(this, permute);
				}

				return permute;
			}

			public override bool Equals(object obj)
			{
				if (obj is SpringBreakdown other)
				{
					return other.chunk.Trim('.') == chunk.Trim('.') && other.value == value;
				}
				return false;	
			}

			public override int GetHashCode()
			{
				return 13 * chunk.GetHashCode() + value;
			}

			public override string ToString()
			{
				return $"{chunk} {value}";
			}
		}
	}
}
