using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023
{
	internal static class Day19
	{
		private enum BoolOP
		{
			any,less, greater, lessOrEqual, greaterOrEqual, equal
		}
		private class SortInstruction
		{
			public readonly string id;
			public readonly List<SortRule> rules;
			public SortInstruction(string entry)
			{
				id = entry.Substring(0,entry.IndexOf('{'));
				string rls = entry[(1 + entry.IndexOf('{'))..^1];
				rules = new List<SortRule>();
				string[] allRules = rls.Split(',');
				foreach (string rl in allRules)
				{
					rules.Add(new SortRule(rl));
				}
			}

			public string Process(Dictionary<char, int> part)
			{
				foreach (var rule in rules)
				{
					string res = rule.GetResult(part);
					if (!string.IsNullOrEmpty(res)) return res;
				}

				return "R";
			}
		}

		private class SortRule
		{
			public char check;
			public int value;
			public BoolOP operation;
			public string acceptDest;

			public SortRule(string entry)
			{
				if (entry.Contains(':'))
				{
					check = entry[0];
					operation = GetOP(entry[1]);
					string[] parts = entry[2..].Split(':');
					value = int.Parse(parts[0]);
					acceptDest = parts[1];
				}
				else
				{
					operation = BoolOP.any;
					acceptDest = entry;
					check = 'x';
				}
			}

			public string GetResult(Dictionary<char, int> obj)
			{
				int valueToCheck = obj[check];
				switch (operation)
				{
					case BoolOP.any:
						return acceptDest;
					case BoolOP.less:
						return valueToCheck < value ? acceptDest : "";
					case BoolOP.greater:
						return valueToCheck > value ? acceptDest : "";
				}

				return "R";
			}

			private static BoolOP GetOP(char c)
			{
				switch (c)
				{
					case '>':
						return BoolOP.greater;
					case '<':
						return BoolOP.less;
				}
				return BoolOP.equal;
			}
		}

		private static Dictionary<string, SortInstruction> allInstructions = new Dictionary<string, SortInstruction>();

		internal static long Part1(string input)
		{
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			bool readingInstructions = true;
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					readingInstructions = false;
					continue;
				}
				if (readingInstructions)
				{
					SortInstruction inst = new SortInstruction(line);
					allInstructions.Add(inst.id, inst);
				}
				else
				{
					result += ProcessPart(allInstructions["in"], line[1..^1]);
				}
			}

			return result;
		}

		private static long ProcessPart(SortInstruction workflowStart, string line)
		{
			string[] values = line.Split(',');
			Dictionary<char, int> partAttr = new Dictionary<char, int>();
			foreach (string value in values)
			{
				string[] p = value.Split('=');
				partAttr.Add(p[0][0], int.Parse(p[1]));
			}
			SortInstruction currInstruction = workflowStart;
			while (true)
			{
				string res = currInstruction.Process(partAttr);
				;
				if (res == "R")
				{
					return 0;
				}

				if (res == "A")
				{
					;
					long result = 0;
					foreach (int v in partAttr.Values)
					{
						result += v;
					}

					return result;
				}
				currInstruction = allInstructions[res];

			}
			return 0;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			allInstructions.Clear();
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					break;
				}
				SortInstruction inst = new SortInstruction(line);
				allInstructions.Add(inst.id, inst);
			}

			long result = 0l;
			Dictionary<char, int> eee = new Dictionary<char, int>();

			List<((long, long) x, (long, long) m, (long, long) a, (long, long) s, string loc)> openList = new List<((long, long) x, (long, long) m, (long, long) a, (long, long) s, string loc)>();
			openList.Add(((1, 4000), (1, 4000), (1, 4000), (1, 4000), "in"));
			return ProcessAllParts(openList);
			//return result;
		}

		private static long ProcessAllParts(List<((long, long) x, (long, long) m, (long, long) a, (long, long) s, string loc)> openList)
		{
			long result = 0L;
			while (openList.Count > 0)
			{
				((long, long) x, (long, long) m, (long, long) a, (long, long) s, string loc) pop = openList[0];
				openList.RemoveAt(0);

				// degenerate case, range has no items
				if (pop.x.Item1 > pop.x.Item2 || pop.m.Item1 > pop.m.Item2 || pop.a.Item1 > pop.a.Item2 || pop.s.Item1 > pop.s.Item2) continue;
				if (pop.loc == "R") continue;
				if (pop.loc == "A")
				{
					result += (pop.x.Item2 - pop.x.Item1 + 1) * (pop.m.Item2 - pop.m.Item1 + 1) * (pop.a.Item2 - pop.a.Item1 + 1) * (pop.s.Item2 - pop.s.Item1 + 1);
					continue;
				}

				SortInstruction currInstruction = allInstructions[pop.loc];
				foreach (SortRule inst in currInstruction.rules)
				{
					// degenerate case, range has no items
					if (pop.x.Item1 > pop.x.Item2 || pop.m.Item1 > pop.m.Item2 || pop.a.Item1 > pop.a.Item2 || pop.s.Item1 > pop.s.Item2)
					{
						break;
					}

					switch (inst.operation)
					{
						case BoolOP.any:
							openList.Add((pop.x, pop.m, pop.a, pop.s, inst.acceptDest));
							break;
						case BoolOP.less:
							switch (inst.check)
							{
								// guaranteed to never fully encompass the range, either inclusively or exclusively
								// I did a break point check on the if-else-if blocks and they were never hit so I removed them
								case 'x':
									(long, long) newX = (pop.x.Item1, inst.value - 1);
									openList.Add((newX, pop.m, pop.a, pop.s, inst.acceptDest));
									pop.x.Item1 = inst.value;
									break;
								case 'm':
									(long, long) newM = (pop.m.Item1, inst.value - 1);
									openList.Add((pop.x, newM, pop.a, pop.s, inst.acceptDest));
									pop.m.Item1 = inst.value;
									break;
								case 'a':
									(long, long) newA = (pop.a.Item1, inst.value - 1);
									openList.Add((pop.x, pop.m, newA, pop.s, inst.acceptDest));
									pop.a.Item1 = inst.value;
									break;
								case 's':
									(long, long) newS = (pop.s.Item1, inst.value - 1);
									openList.Add((pop.x, pop.m, pop.a, newS, inst.acceptDest));
									pop.s.Item1 = inst.value;
									break;
							}
							break;
						case BoolOP.greater:
							switch (inst.check)
							{
								case 'x':
									(long, long) newX = (inst.value + 1, pop.x.Item2);
									openList.Add((newX, pop.m, pop.a, pop.s, inst.acceptDest));
									pop.x.Item2 = inst.value;
									break;
								case 'm':
									(long, long) newM = (inst.value + 1, pop.m.Item2);
									openList.Add((pop.x, newM, pop.a, pop.s, inst.acceptDest));
									pop.m.Item2 = inst.value;
									break;
								case 'a':
									(long, long) newA = (inst.value + 1, pop.a.Item2);
									openList.Add((pop.x, pop.m, newA, pop.s, inst.acceptDest));
									pop.a.Item2 = inst.value;
									break;
								case 's':
									(long, long) newS = (inst.value + 1, pop.s.Item2);
									openList.Add((pop.x, pop.m, pop.a, newS, inst.acceptDest));
									pop.s.Item2 = inst.value;
									break;
							}
							break;
					}
				}
			}
			return result;
		}
	}
}
