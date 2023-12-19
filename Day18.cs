using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day18
	{
		internal static long Part1(string input)
		{
			// these take forever on real data
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid lagoon = new Grid(1,1);
			Vector2 pos = new Vector2(0,0);
			lagoon[pos] = '#';
			foreach (string line in lines)
			{
				string[] inst = line.Split(' ');
				Dig(lagoon, ref pos, inst[0][0], inst[1]);
			}

			//lagoon.FloodFill(new Vector2(2, -2), '#', delegate(int value, int fill) { return value != '#'; }, false);
			
			for (int x = lagoon.MinX; x < lagoon.MaxX; x++)
			{
				for (int y = lagoon.MinY; y < lagoon.MaxY; y++)
				{
					if (lagoon[x, y] == '#')
					{
						result++;
					}
				}
			}
			Console.WriteLine(lagoon);
			return result;
		}

		private static void Dig(Grid lagoon, ref Vector2 pos, char dir, string d)
		{
			Vector2 move = new Vector2(0,0);
			switch (dir)
			{
				case 'U':
					move = new Vector2(0, -1);
					break;
				case 'D':
					move = new Vector2(0, 1);
					break;
				case 'L':
					move = new Vector2(-1, 0);
					break;
				case 'R':
					move = new Vector2(1, 0);
					break;
			}

			int dist = int.Parse(d);
			for (int i = 0; i < dist; i++)
			{
				pos += move;
				lagoon.IncreaseGridToInclude(pos, () => '.');
				lagoon[pos] = '#';
			}
		}

		internal static long Part2(string input)
		{
			// these take forever on real data
			// on the order of 3 hours
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<long, List<long>> lagoonH = new Dictionary<long, List<long>>();
			//Dictionary<long, List<long>> lagoonV = new Dictionary<long, List<long>>();
			List<(char dir,long dist)> digInstructions = new List<(char, long)>();
			foreach (string line in lines)
			{
				string[] inst = line.Split(' ');
				string len = inst[2].Substring(2, 5);

				string p =inst[2].Substring(7, 1);
				switch (p[0])
				{
					case '0':
						inst[0] = "R";
						break;
					case '1':
						inst[0] = "D";
						break;
					case '2':
						inst[0] = "L";
						break;
					case '3':
						inst[0] = "U";
						break;
				}

				digInstructions.Add((inst[0][0], long.Parse(len, NumberStyles.HexNumber)));
			}
			Vector2 pos = new Vector2(0, 0);

			List<long> ls = new List<long> { pos.x };
			lagoonH.Add(pos.y, ls);
			ls = new List<long> { pos.y };
			Console.WriteLine($"x1=\"{pos.x/1000}\" y1=\"{pos.y / 1000}\"");

			foreach ((char dir, long dist) in digInstructions)
			{
				Vector2 move = new Vector2(0, 0);
				Console.WriteLine($" x2=\"{pos.x / 1000f}\" y2=\"{pos.y / 1000f}\"");
				Console.WriteLine($"x1=\"{pos.x / 1000f}\" y1=\"{pos.y / 1000f}\"");
				switch (dir)
				{
					case 'U':
						move = new Vector2(0, -1);
						break;
					case 'D':
						move = new Vector2(0, 1);
						break;
					case 'L':
						move = new Vector2(-1, 0);
						break;
					case 'R':
						move = new Vector2(1, 0);
						break;
				}
				for (int i = 0; i < dist; i++)
				{
					pos += move;
					if (lagoonH.TryGetValue(pos.y, out List<long> list))
					{
						if(!list.Contains(pos.x))
							list.Add(pos.x);
					}
					else
					{
						List<long> l = new List<long> { pos.x };
						lagoonH.Add(pos.y,l);
					}
				}
			}
			Console.WriteLine($"x2=\"{pos.x / 1000}\" y2=\"{pos.y / 1000}\"");

			bool wasDrawing = false;
			bool drawingNow = true;
			bool onEdge = false;
			Orientation enterDir = Orientation.WEST;
			long v = result;
			
			foreach (KeyValuePair<long, List<long>> kvp in lagoonH)
			{
				List<long> spans = kvp.Value;
				spans.Sort();
				
				for (int i = 0; i+1 < spans.Count; i++)
				{
					if (i == 0)
					{
						wasDrawing = false;
						drawingNow = true;
					}
					long span = spans[i + 1] - spans[i];
					if (span > 1)
					{
						onEdge = false;
					}

					if (span == 1 && !onEdge)
					{
						if (spans.Contains(spans[i + 1] + 1))
						{
							onEdge = true;
							drawingNow = true;

							if (lagoonH.TryGetValue(kvp.Key - 1, out List<long> priorRow))
							{
								if (priorRow.Contains(spans[i]))
								{
									enterDir = Orientation.NORTH;

									if (lagoonH.TryGetValue(kvp.Key + 1, out List<long> nextRow))
									{
										if (nextRow.Contains(spans[i]))
											Console.WriteLine("CROSSING POINT ENTER!?");
									}
								}
								else
								{
									enterDir = Orientation.SOUTH;
								}
							}
							else
							{
								enterDir = Orientation.SOUTH;
							}
						}
						else
						{
							Console.WriteLine("Huh, a 2-wide");
						}
					}

					if (drawingNow)
						result += span + (onEdge ? 0 : 1);

					if (!onEdge)
					{
						wasDrawing = drawingNow;
						drawingNow = !drawingNow;
					}
					else if(!spans.Contains(spans[i + 1] + 1))
					{
						Orientation exitDir = Orientation.EAST;
						if (lagoonH.TryGetValue(kvp.Key - 1, out List<long> priorRow))
						{
							if (priorRow.Contains(spans[i + 1]))
							{
								exitDir = Orientation.NORTH;

								if (lagoonH.TryGetValue(kvp.Key + 1, out List<long> nextRow))
								{
									if (nextRow.Contains(spans[i + 1]))
										Console.WriteLine("CROSSING POINT EXIT!?");
								}
							}
							else
							{
								exitDir = Orientation.SOUTH;
							}
						}

						if (!wasDrawing)
							result++;

						drawingNow = exitDir == enterDir ? wasDrawing : !wasDrawing;
						wasDrawing = true;
						if (drawingNow)
							result--;
					}
				}
			}

			return result+1;
		}
	}
}
