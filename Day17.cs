using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Draco18s.AoCLib;
using Microsoft.VisualBasic;

namespace AdventofCode2023
{
	internal static class Day17
	{
		internal static long Part1(string input)
		{
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid city = new Grid(input, false);
			result = Pathfind(city, new Vector2(city.MaxX - 1, city.MaxY - 1), 3);
			return result;
		}

		private static long Pathfind(Grid city, Vector2 dest, int max, int min=0)
		{
			List<(Vector2 pos,Orientation dir, int stepsInDir, int heatLoss)> open = new List<(Vector2, Orientation, int, int)>();
			open.Add((new Vector2(0, 0), Orientation.EAST, 0, 0));
			open.Add((new Vector2(0, 0), Orientation.SOUTH, 0, 0));
			List<(Vector2 pos, Orientation dir, int stepsInDir)> visited = new List<(Vector2,Orientation,int)>();
			long low = 9999;
			while (open.Count > 0)
			{
				(Vector2 pos, Orientation dir, int stepsInDir, int heatLoss) node = open[0];
				open.RemoveAt(0);
				
				if (visited.Any(n => n.pos==node.pos && n.dir==node.dir && n.stepsInDir == node.stepsInDir))
					continue;

				visited.Add((node.pos,node.dir,node.stepsInDir));
				
				Vector2 pos = node.pos + Offset(node.dir);

				if (!city.IsInside(pos))
					continue;

				if (node.pos == dest)
				{
					if (node.stepsInDir < min)
					{
						Console.WriteLine(node.heatLoss);
						low = Math.Min(low, node.heatLoss);
					}
					else
					{
						Console.WriteLine($"Reached end with {node.heatLoss} but can't stop");
					}
				}
				
				int heat = node.heatLoss + city[pos];
				if (node.stepsInDir < min-1)
				{
					open.Add((pos, node.dir, node.stepsInDir + 1, heat));
				}
				else
				{
					if (node.stepsInDir < max - 1)
					{
						open.Add((pos, node.dir, node.stepsInDir + 1, heat));
					}

					open.Add((pos, TurnLeft(node.dir), 0, heat));
					open.Add((pos, TurnRight(node.dir), 0, heat));
				}

				open = open.OrderBy(n => n.heatLoss + (dest.x - n.pos.x + dest.y - n.pos.y)).ToList();
			}
			Console.WriteLine("===");
			return low; //870 too high
		}

		private static string FacingChar(Orientation nodeDir)
		{
			switch (nodeDir)
			{
				case Orientation.NORTH:
					return "^";
				case Orientation.SOUTH:
					return "v";
				case Orientation.WEST:
					return "<";
				case Orientation.EAST:
					return ">";
			}
			return ".";
		}

		private static Vector2 Offset(Orientation dir)
		{
			switch (dir)
			{
				case Orientation.NORTH:
					return new Vector2(0, -1);
				case Orientation.SOUTH:
					return new Vector2(0, 1);
				case Orientation.WEST:
					return new Vector2(-1, 0);
				case Orientation.EAST:
					return new Vector2(1, 0);
			}
			return new Vector2(0, 0);
		}

		private static Orientation TurnRight(Orientation nodeDir)
		{
			switch (nodeDir)
			{
				case Orientation.NORTH:
					return Orientation.EAST;
				case Orientation.SOUTH:
					return Orientation.WEST;
				case Orientation.WEST:
					return Orientation.NORTH;
				case Orientation.EAST:
					return Orientation.SOUTH;
			}
			return Orientation.NORTH;
		}

		private static Orientation TurnLeft(Orientation nodeDir)
		{
			switch (nodeDir)
			{
				case Orientation.NORTH:
					return Orientation.WEST;
				case Orientation.SOUTH:
					return Orientation.EAST;
				case Orientation.WEST:
					return Orientation.SOUTH;
				case Orientation.EAST:
					return Orientation.NORTH;
			}
			return Orientation.NORTH;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid city = new Grid(input, false);
			result = Pathfind(city, new Vector2(city.MaxX - 1, city.MaxY - 1), 10, 4);
			return result;
		}
	}
}
