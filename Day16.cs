using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day16
	{
		internal static long Part1(string input)
		{
			return RunAt(input, new Vector2(-1, 0), Orientation.EAST);
		}

		private static long RunAt(string input, Vector2 start, Orientation dir)
		{
			long result = 0L;
			Grid grid = new Grid(input, true);
			Grid energ = new Grid(grid.Width, grid.Height);
			List<(Vector2 pos, Orientation dir)> beams = new List<(Vector2 pos, Orientation dir)>();
			beams.Add((start, dir));
			List<Vector2> splittersUsed = new List<Vector2>();
			while (beams.Count > 0)
			{
				for (var i = beams.Count - 1; i >= 0; i--)
				{
					(Vector2 pos, Orientation dir) b = beams[i];
					if (grid.IsInside(b.pos))
						energ[b.pos] = '#';
					//Console.WriteLine(energ.ToString("char"));
					switch (b.dir)
					{
						case Orientation.EAST:
							b.pos = new Vector2(b.pos.x + 1, b.pos.y);
							break;
						case Orientation.WEST:
							b.pos = new Vector2(b.pos.x - 1, b.pos.y);
							break;
						case Orientation.SOUTH:
							b.pos = new Vector2(b.pos.x, b.pos.y + 1);
							break;
						case Orientation.NORTH:
							b.pos = new Vector2(b.pos.x, b.pos.y - 1);
							break;
					}

					if (!grid.IsInside(b.pos))
					{
						beams.RemoveAt(i);
						continue;
					}

					if (grid[b.pos] == '/')
					{
						switch (b.dir)
						{
							case Orientation.EAST:
								b.dir = Orientation.NORTH;
								break;
							case Orientation.WEST:
								b.dir = Orientation.SOUTH;
								break;
							case Orientation.SOUTH:
								b.dir = Orientation.WEST;
								break;
							case Orientation.NORTH:
								b.dir = Orientation.EAST;
								break;
						}
					}

					if (grid[b.pos] == '\\')
					{
						switch (b.dir)
						{
							case Orientation.EAST:
								b.dir = Orientation.SOUTH;
								break;
							case Orientation.WEST:
								b.dir = Orientation.NORTH;
								break;
							case Orientation.SOUTH:
								b.dir = Orientation.EAST;
								break;
							case Orientation.NORTH:
								b.dir = Orientation.WEST;
								break;
						}
					}

					if (grid[b.pos] == '-')
					{
						switch (b.dir)
						{
							case Orientation.EAST:
							case Orientation.WEST:
								break;
							case Orientation.SOUTH:
							case Orientation.NORTH:
								if (!splittersUsed.Contains(b.pos))
								{
									beams.Add((b.pos, Orientation.WEST));
									beams.Add((b.pos, Orientation.EAST));
									splittersUsed.Add(b.pos);
								}
								b.pos = new Vector2(-10, -10);
								beams[i] = b;
								continue;
						}
					}
					if (grid[b.pos] == '|')
					{

						switch (b.dir)
						{
							case Orientation.EAST:
							case Orientation.WEST:
								if (!splittersUsed.Contains(b.pos))
								{
									splittersUsed.Add(b.pos);
									beams.Add((b.pos, Orientation.NORTH));
									beams.Add((b.pos, Orientation.SOUTH));
								}
								b.pos = new Vector2(-10, -10);
								beams[i] = b;
								continue;
							case Orientation.SOUTH:
							case Orientation.NORTH:
								break;
						}
					}

					beams[i] = b;
				}
			}

			for (int x = energ.MinX; x < energ.MaxX; x++)
			{
				for (int y = energ.MinY; y < energ.MaxY; y++)
				{
					if (energ[x, y] == '#')
						result++;
				}
			}
			return result;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			foreach (var e in Enum.GetValues(typeof(Orientation)))
			{
				Orientation o = (Orientation)e;
				for (int i = 0; i < lines.Length; i++)
				{
					Vector2 s = new Vector2(-1, 0);
					switch (o)
					{
						case Orientation.EAST:
							s = new Vector2(-1, i);
							break;
						case Orientation.WEST:
							s = new Vector2(lines.Length, i);
							break;
						case Orientation.NORTH:
							s = new Vector2(i, -1);
							break;
						case Orientation.SOUTH:
							s = new Vector2(i, lines.Length);
							break;
					}

					result = Math.Max(RunAt(input, s, o), result);
				}
			}

			return result;
		}
	}
}
