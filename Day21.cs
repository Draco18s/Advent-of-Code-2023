using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Draco18s.AoCLib;
using Newtonsoft.Json.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using static Draco18s.AoCLib.Grid;

namespace AdventofCode2023
{
	internal static class Day21
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid plots = new Grid(input, true);
			Vector2 p = GetStartPos(plots);

			int maxStep = 64;

			return ResultFor(plots, maxStep, p);
		}

		private static long ResultFor(Grid plots, int maxStep, Vector2 start)
		{
			plots[GetStartPos(plots)] = '.';

			Vector2 p = start;
			long result = 0l;

			plots[p] = maxStep + 1;

			FloodFill(plots, p, (cur, fill) =>
			{
				return plots[cur] - 1;
			}, (value, fill) =>
			{
				return (value == '.' && fill > 0);
			}, () => int.MinValue);

			for (int x = 0; x < plots.Width; x++)
			{
				for (int y = 0; y < plots.Height; y++)
				{
					if (plots[x, y] != int.MaxValue && plots[x, y] % 2 == 1)
					{
						result++;
						plots[x, y] = 'O';
					}
					else if (plots[x, y] != int.MaxValue)
					{
						plots[x, y] = '.';
					}
					else if (plots[x, y] == int.MaxValue)
					{
						plots[x, y] = '#';
					}
				}
			}

			//Console.WriteLine(plots);
			return result;
		}

		public static long FloodFill(Grid grid, Vector2 pos, Func<Vector2, Vector2, int> fillValue, ShouldFill shouldFill, EdgeHandler edgeHandler, bool allowDiagonals = false)
		{
			long size = 1;
			
			List<Vector2> open = new List<Vector2>();
			open.Add((pos));

			while (open.Count > 0)
			{
				Vector2 p = open[0];
				open.RemoveAt(0);

				int L = grid[pos];
				
				if (p.x >= grid.MaxX || p.y >= grid.MaxY || p.x < grid.MinX || p.y < grid.MinY) continue;
				
				int N = (p.y == grid.MinY) ? edgeHandler() : grid[p.x, p.y - 1];
				int W = (p.x == grid.MinX) ? edgeHandler() : grid[p.x - 1, p.y];

				int S = (p.y == grid.MaxY - 1) ? edgeHandler() : grid[p.x, p.y + 1];
				int E = (p.x == grid.MaxX - 1) ? edgeHandler() : grid[p.x + 1, p.y];

				if (shouldFill(N, L))
				{
					if (p.y-1 < grid.MinY) continue;
					grid[p.x, p.y-1] = fillValue(new Vector2(p.x, p.y), new Vector2(p.x, p.y-1));
					open.Add(new Vector2(p.x, p.y - 1));
					size++;
				}
				if (shouldFill(S, L))
				{
					if (p.y + 1 >= grid.MaxY) continue;
					grid[p.x, p.y + 1] = fillValue(new Vector2(p.x, p.y), new Vector2(p.x, p.y + 1));
					open.Add(new Vector2(p.x, p.y + 1));
					size++;
				}
				if (shouldFill(W, L))
				{
					if (p.x - 1 < grid.MinX) continue;
					grid[p.x - 1, p.y] = fillValue(new Vector2(p.x, p.y), new Vector2(p.x - 1, p.y));
					open.Add(new Vector2(p.x - 1, p.y));
					size++;
				}
				if (shouldFill(E, L))
				{
					if (p.x + 1 >= grid.MaxX) continue;
					grid[p.x + 1, p.y] = fillValue(new Vector2(p.x, p.y), new Vector2(p.x + 1, p.y));
					open.Add(new Vector2(p.x + 1, p.y));
					size++;
				}
			}

			return size;

		}

		private static Vector2 GetStartPos(Grid plots)
		{
			for (int x = 0; x < plots.Width; x++)
			{
				for (int y = 0; y < plots.Height; y++)
				{
					if (plots[x, y] == '#')
					{
						plots[x, y] = int.MaxValue;
					}
				}
			}
			for (int x = 0; x < plots.Width; x++)
			{
				for (int y = 0; y < plots.Height; y++)
				{
					if (plots[x, y] == 'S')
					{
						return new Vector2(x, y);
					}
				}
			}
			return new Vector2(0, 0);
		}

		internal static long Part2(string input)
		{
			Grid plots = new Grid(input, true);

			Vector2 p = GetStartPos(plots);

			int maxSteps = 26501365;

			long bigNum = (maxSteps / plots.Width);

			int remainingStepsOrtho = (maxSteps % plots.Width) + (plots.Width / 2);
			int remainingStepsDiagShort = maxSteps % plots.Width - 1;
			int remainingStepsDiagLongs = maxSteps % plots.Width - 1 + plots.Width;

			long reachableMapFullA = ResultFor(new Grid(input, true), plots.Width * 4 + (maxSteps % 2) + 0, p);
			long reachableMapFullB = ResultFor(new Grid(input, true), plots.Width * 4 + (maxSteps % 2) + 1, p);
			long numFullCopiesReachable = g(bigNum-1, reachableMapFullA, reachableMapFullB);

			long everyReachableN = ResultFor(new Grid(input, true), remainingStepsOrtho, new Vector2(p.x, 0));
			long everyReachableW = ResultFor(new Grid(input, true), remainingStepsOrtho, new Vector2(0, p.y));
			long everyReachableE = ResultFor(new Grid(input, true), remainingStepsOrtho, new Vector2(plots.Width-1, p.y));
			long everyReachableS = ResultFor(new Grid(input, true), remainingStepsOrtho, new Vector2(p.x, plots.Height - 1));

			long everyReachableNWs = ResultFor(new Grid(input, true), remainingStepsDiagShort, new Vector2(0, 0));
			long everyReachableNEs = ResultFor(new Grid(input, true), remainingStepsDiagShort, new Vector2(plots.Width - 1, 0));
			long everyReachableSEs = ResultFor(new Grid(input, true), remainingStepsDiagShort, new Vector2(plots.Width - 1, plots.Height - 1));
			long everyReachableSWs = ResultFor(new Grid(input, true), remainingStepsDiagShort, new Vector2(0, plots.Height - 1));
			
			long everyReachableNWl = ResultFor(new Grid(input, true), remainingStepsDiagLongs, new Vector2(0, 0));
			long everyReachableNEl = ResultFor(new Grid(input, true), remainingStepsDiagLongs, new Vector2(plots.Width - 1, 0));
			long everyReachableSEl = ResultFor(new Grid(input, true), remainingStepsDiagLongs, new Vector2(plots.Width - 1, plots.Height - 1));
			long everyReachableSWl = ResultFor(new Grid(input, true), remainingStepsDiagLongs, new Vector2(0, plots.Height - 1));
			
			long ortho = everyReachableN + everyReachableW + everyReachableE + everyReachableS;
			long diagonalShort = (everyReachableNWs + everyReachableNEs + everyReachableSEs + everyReachableSWs);
			long diagonalLongs = (everyReachableNWl + everyReachableNEl + everyReachableSEl + everyReachableSWl);
			long diagonals = g2(bigNum, diagonalShort, diagonalLongs);

			return ortho + numFullCopiesReachable + diagonals;
		}

		private static long f(long n)
		{
			if (n < 0) return 0;
			if(n == 0) return 1;
			if(n == 1) return 4;

			long r = f(n % 2);
			while (n > 1)
			{
				r += 4 * n;
				n -= 2;
			}

			return r;
		}

		private static long g(long n, long A1, long A2)
		{
			if (n < 0) return 0;
			if (n == 0) return 1 * A1;
			if (n == 1) return 4 * A2 + A1;
			
			long resA1 = 0;
			long resA2 = 0;

			while (n > 0)
			{
				if (n % 2 == 0)
				{
					resA1 += (n * 4) * A1;
				}
				else
				{
					resA2 += (n * 4) * A2;
				}
				n--;
			}

			return resA1 + resA2 + A1;
		}

		private static long g2(long n, long A1, long A2)
		{
			return n * A1 + (n - 1) * A2;
		}
	}
}

/***
Scratch pad
generate a simple map with no walls
	this is easily verifiable with f(n)
walls will be taken into account with the directionally based quadrants

.O.O.
O.O.O
.O.O.
O.O.O
.O.O.
     ===12
O.O.O
.O.O.
O.O.O
.O.O.
O.O.O
     ===13
..O..
.O.O.
O.O.O
.O.O.
O.O.O
     ===11	<^v>
.....
.....
.....
....O
...O.
     ===2	`'
..O.O
.O.O.
O.O.O
.O.O.
     ===9	\/


   ^ 
  '2`
 '/1\`
<21012>
 `\1/'
  `2'
   v
  
short edges, just barely reaching in
long edges, just barely short of getting a full tile

     |     |.....|  O
     |     |.....| O O
     |     |.....|O   O
     |     |....O|
     |     |...O.|
------------------------
     |.....|..O.O|.O.O.|
     |.....|.O.O.|O.O.O|
     |.....|O.O.O| 12  | alternating full tiles
     |....O|.O.O.|O.O.O|
     |...O.|O.O.O|.O.O.|
------------------------
.....|..O.O|     |
.....|.O.O.|     |
.....|O.O.O| 12  |  13
....O|.O.O.|     |
...O.|O.O.O|     |
-----------------------
..O.O|.O.O.|O.O.O|.O.O.
.O.O.|O.O.O|.O.O.|O.O.O
O.O.O| 12  | 13  |  S   
.O.O.|O.O.O|.O.O.|O.O.O
..O.O|.O.O.|O.O.O|.O.O.

center tile is 12 or 13 based on parity of numSteps
*/
