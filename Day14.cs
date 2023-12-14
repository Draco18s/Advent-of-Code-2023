using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day14
	{
		internal static long Part1(string input)
		{
			Grid grid = new Grid(input, true);
			long result = 0l;

			while (RollRocks(grid)) ;

			grid.Rotate(Orientation.SOUTH);

			for (int y = grid.MinY; y < grid.MaxY; y++)
			{
				for (int x = grid.MinX; x < grid.MaxX; x++)
				{
					if (grid[x, y] == 'O')
					{
						result += y + 1;
					}
				}
			}

			return result;
		}

		private static bool RollRocks(Grid grid)
		{
			bool didShift = false;
			for (int y = grid.MinY; y+1 < grid.MaxY; y++)
			{
				for (int x = grid.MinX; x < grid.MaxX; x++)
				{
					if (grid[x, y] == '.' && grid[x, y + 1] == 'O')
					{
						grid[x, y+1] = '.';
						grid[x, y] = 'O';
						didShift = true;
					}
				}
			}

			return didShift;
		}

		internal static long Part2(string input)
		{
			Grid grid = new Grid(input, true);
			long result = 0l;

			// I GOT STUPID LUCKY
			/*for (int i = 0; i < 1000; i++)
			{
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
			}
			
			return  CalcLoad(grid);
			*/

			//This will work for any input

			for (int i = 0; i < 200; i++)
			{
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
			}
			List<long> cycleVals = new List<long>();
			for (int i = 0; i < 200; i++)
			{
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);
				while (RollRocks(grid)) ;
				grid.Rotate(Orientation.EAST);

				long v = CalcLoad(grid);

				if (cycleVals.Contains(v))
				{
					int j = cycleVals.LastIndexOf(v);
					int cycleLen = (i - j);

					int q = (1000000000 - 200 - i -1) % cycleLen;

					int n = (q + j) % cycleLen;
					result = cycleVals[n];

					break;
				}

				cycleVals.Add(v);
			}

			return result;
		}

		private static long CalcLoad(Grid grid)
		{
			long result = 0L;
			grid.Rotate(Orientation.SOUTH);

			for (int y = grid.MinY; y < grid.MaxY; y++)
			{
				for (int x = grid.MinX; x < grid.MaxX; x++)
				{
					if (grid[x, y] == 'O')
					{
						result += y + 1;
					}
				}
			}

			grid.Rotate(Orientation.SOUTH);
			return result;
		}
	}
}
