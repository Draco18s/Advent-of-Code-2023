using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day11
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid space = new Grid(input, true);
			for (int y = space.MinY; y < space.MaxY; y++)
			{
				bool hasGalaxy = false;
				for (int x = space.MinX; x < space.MaxX; x++)
				{
					if (space[x,y] == '#')
					{
						hasGalaxy = true;
						break;
					}
				}

				if (!hasGalaxy)
				{
					space.IncreaseGridBy(0, -1, () => '.');
					for (int y2 = space.MinY; y2 < y; y2++)
					{
						for (int x2 = space.MinX; x2 < space.MaxX; x2++)
						{
							space[x2, y2] = space[x2, y2+1];
						}
					}
				}
			}

			
			for (int x = space.MinX; x < space.MaxX; x++)
			{
				bool hasGalaxy = false;
				for (int y = space.MinY; y < space.MaxY; y++)
				{
					if (space[x, y] == '#')
					{
						hasGalaxy = true;
						break;
					}
				}

				if (!hasGalaxy)
				{
					space.IncreaseGridBy(-1, 0, () => '.');
					for (int x2 = space.MinX; x2 < x; x2++)
					{
						for (int y2 = space.MinY; y2 < space.MaxY; y2++)
						{
							space[x2, y2] = space[x2 + 1, y2];
						}
					}
				}
			}

			List<Vector2> galaxies = new List<Vector2>();

			for (int y = space.MinY; y < space.MaxY; y++)
			{
				for (int x = space.MinX; x < space.MaxX; x++)
				{
					if (space[x, y] == '#')
					{
						galaxies.Add(new Vector2(x,y));
					}
				}
			}

			for (var i = 0; i < galaxies.Count; i++)
			{
				Vector2 p1 = galaxies[i];
				long minDist = long.MaxValue;
				for (var j = i; j < galaxies.Count; j++)
				{
					if (i == j) continue;

					Vector2 p2 = galaxies[j];
					Vector2 d = (p2 - p1);
					minDist = Math.Abs(d.x) + Math.Abs(d.y);
					result += minDist;
				}
			}


			return result;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid space = new Grid(input, true);

			List<Vector2> galaxies = new List<Vector2>();
			List<int> vertSpace = new List<int>();
			List<int> horzSpace = new List<int>();

			for (int y = space.MinY; y < space.MaxY; y++)
			{
				bool hasGalaxy = false;
				for (int x = space.MinX; x < space.MaxX; x++)
				{
					if (space[x, y] == '#')
					{
						hasGalaxy = true;
						break;
					}
				}
				if (!hasGalaxy)
				{
					horzSpace.Add(y);
				}
			}
			for (int x = space.MinX; x < space.MaxX; x++)
			{
				bool hasGalaxy = false;
				for (int y = space.MinY; y < space.MaxY; y++)
				{
					if (space[x, y] == '#')
					{
						hasGalaxy = true;
						break;
					}
				}
				if (!hasGalaxy)
				{
					vertSpace.Add(x);
				}
			}

			for (int y = space.MinY; y < space.MaxY; y++)
			{
				for (int x = space.MinX; x < space.MaxX; x++)
				{
					if (space[x, y] == '#')
					{
						galaxies.Add(new Vector2(x, y));
					}
				}
			}

			for (var i = 0; i < galaxies.Count; i++)
			{
				Vector2 p1 = galaxies[i];
				long minDist = long.MaxValue;
				for (var j = i; j < galaxies.Count; j++)
				{
					if (i == j) continue;
					Vector2 p2 = galaxies[j];

					result += GetDistance(p1, p2, horzSpace, vertSpace, 1000000);
				}
			}

			return result;
		}

		private static long GetDistance(Vector2 p1, Vector2 p2, List<int> horzSpace, List<int> vertSpace, int expCoef)
		{
			/*Vector2 d = (p2 - p1);
					minDist = Math.Abs(d.x) + Math.Abs(d.y);
					result += minDist;*/

			int minx = Math.Min(p1.x, p2.x);
			int miny = Math.Min(p1.y, p2.y);
			int maxx = Math.Max(p1.x, p2.x);
			int maxy = Math.Max(p1.y, p2.y);

			int hExp = vertSpace.Count(h => h >= minx && h < maxx) * (expCoef - 1);
			int vExp = horzSpace.Count(h => h >= miny && h < maxy) * (expCoef - 1);

			return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y) + hExp + vExp;
		}
	}
}
