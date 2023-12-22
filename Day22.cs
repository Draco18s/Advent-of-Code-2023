using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Draco18s.AoCLib;
using static AdventofCode2023.Day22;

namespace AdventofCode2023
{
	internal static class Day22
	{
		public class Brick
		{
			public int id;
			public Vector3 pos1;
			public Vector3 pos2;

			public int[] GetBlocksProvidingSupport(Grid3D grid)
			{
				List<int> list = new List<int>();

				for (int x = pos1.x; x <= pos2.x; x++)
				{
					for (int y = pos1.y; y <= pos2.y; y++)
					{
						list.Add(grid[x, y, pos1.z - 1]);
					}
				}

				return list.Distinct().OrderBy(x => x).Where(x => x > 0 && x != int.MaxValue).ToArray();
			}
		}

		private static Grid3D bricks = new Grid3D(0, 0, 0);
		private static List<Brick> allBricks = new List<Brick>();

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			int brickID = 1;

			foreach (string line in lines)
			{
				string[] parts = line.Split('~');
				string[] vals = parts[0].Split(',');
				Vector3 pos1 = new Vector3(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
				vals = parts[1].Split(',');
				Vector3 pos2 = new Vector3(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
				bricks.IncreaseGridToInclude(pos1, () => 0);
				bricks.IncreaseGridToInclude(pos2, () => 0);
				for (int x = pos1.x; x <= pos2.x; x++)
				{
					for (int y = pos1.y; y <= pos2.y; y++)
					{
						for (int z = pos1.z; z <= pos2.z; z++)
						{
							bricks[x, y, z] = brickID;
						}
					}
				}
				allBricks.Add(new Brick()
				{
					id = brickID,
					pos1 = pos1,
					pos2 = pos2,
				});
				brickID++;
			}
			for (int x = bricks.MinX; x < bricks.MaxX; x++)
			{
				for (int y = bricks.MinY; y < bricks.MaxY; y++)
				{
					bricks[x, y, 0] = int.MaxValue;
				}
			}

			SettleBricks(allBricks, bricks);

			result = GetFreeBricks(allBricks, bricks).Count;

			return result;
		}

		private static List<Brick> GetFreeBricks(List<Brick> allBricks, Grid3D grid)
		{
			List<Brick> freeBricks = new List<Brick>();
			freeBricks.AddRange(allBricks);
			foreach (Brick b in allBricks)
			{
				int[] sups = b.GetBlocksProvidingSupport(grid);
				if (sups.Length == 1)
				{
					freeBricks.RemoveAll(r => r.id == sups[0]);
				}
			}

			return freeBricks;
		}

		private static void SettleBricks(List<Brick> allBricks, Grid3D bricks)
		{
			while (true)
			{
				Brick b = FindBrickThatCanFall(allBricks, bricks);
				if (b == null) return;
				MoveBrickDown(bricks, b);
			}
		}

		private static void MoveBrickDown(Grid3D bricks, Brick b)
		{
			for (int x = b.pos1.x; x <= b.pos2.x; x++)
			{
				for (int y = b.pos1.y; y <= b.pos2.y; y++)
				{
					bricks[x, y, b.pos1.z - 1] = b.id;
					bricks[x, y, b.pos2.z + 0] = 0;
				}
			}

			b.pos1 = new Vector3(b.pos1.x, b.pos1.y, b.pos1.z - 1);
			b.pos2 = new Vector3(b.pos2.x, b.pos2.y, b.pos2.z - 1);
		}

		private static Brick FindBrickThatCanFall(List<Brick> allBricks, Grid3D bricks)
		{
			foreach (Brick b in allBricks)
			{
				if(b.pos1.z == 1) continue;
				if(BrickIsStable(b, bricks)) continue;
				return b;
			}
			return null;
		}

		private static bool BrickIsStable(Brick b, Grid3D bricks)
		{
			for (int x = b.pos1.x; x <= b.pos2.x; x++)
			{
				for (int y = b.pos1.y; y <= b.pos2.y; y++)
				{
					if (bricks[x, y, b.pos1.z - 1] > 0)
					{
						int supid = bricks[x, y, b.pos1.z - 1];
						return true;
					}
				}
			}
			return false;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			Dictionary<int, int[]> supportLookup = new Dictionary<int, int[]>();

			foreach (Brick b in allBricks)
			{
				supportLookup.Add(b.id, GetBricksBeingSupported(b, bricks, allBricks));
			}

			//Brick bb = allBricks.First(br => br.id == 1);

			foreach (Brick b in allBricks.OrderByDescending(br => br.pos1.z))
			{
				result += ComputeUnstable(b, supportLookup, bricks, allBricks) - 1;
			}

			return result;
		}

		private static long ComputeUnstable(Brick b, Dictionary<int, int[]> supportLookup, Grid3D grid, List<Brick> allBricks)
		{
			List<int> allUnstableIds = new List<int>();
			allUnstableIds.Add(b.id);

			Queue<Brick> toCheck = new Queue<Brick>();
			toCheck.Enqueue(b);

			while (toCheck.Count > 0)
			{
				Brick chBrick = toCheck.Dequeue();

				int[] ids = supportLookup[chBrick.id];
				foreach (int bID in ids)
				{
					Brick b2 = allBricks.First(r => r.id == bID);
					int[] otherSups = b2.GetBlocksProvidingSupport(grid);
					if (otherSups.All(d => allUnstableIds.Contains(d)))
					{
						allUnstableIds.Add(bID);
						toCheck.Enqueue(b2);
					}
				}
			}

			return allUnstableIds.Distinct().Count();
		}

		private static int[] GetBricksBeingSupported(Brick b, Grid3D grid, List<Brick> list)
		{
			List<int> supportedBy = new List<int>();
			for (int x = b.pos1.x; x <= b.pos2.x; x++)
			{
				for (int y = b.pos1.y; y <= b.pos2.y; y++)
				{
					if (grid[x, y, b.pos2.z + 1] > 0 && !supportedBy.Contains(grid[x, y, b.pos2.z + 1]))
					{
						supportedBy.Add(grid[x, y, b.pos2.z + 1]);
					}
				}
			}

			return supportedBy.ToArray();
		}
	}
}
