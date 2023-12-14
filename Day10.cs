using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Channels;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day10
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid pipes = new Grid(input, true);

			Vector2 s = GetStart(pipes);

			char c = DetermineStartPipe(pipes, s);
			pipes[s] = c;
			int step = 0;

			result = FlowFillPipes(pipes, s);

			return result;
		}

		private static Vector2 GetStart(Grid pipes)
		{
			for (int x = 0; x < pipes.Width; x++)
			{
				for (int y = 0; y < pipes.Height; y++)
				{
					if (pipes[x, y] == 'S')
					{
						return new Vector2(x, y);
					}
				}
			}
			throw new Exception("Oh god");
		}

		private static long FlowFillPipes(Grid pipes, Vector2 p)
		{
			List<(Vector2 p, long step)> open = new List<(Vector2, long)>();
			List<Vector2> closed = new List<Vector2>();
			open.Add((p, 0));
			
			long max = 0;

			while (open.Count > 0)
			{
				(Vector2 pos, long step) = open[0];
				open.RemoveAt(0);
				if (closed.Contains(pos)) continue;
				closed.Add(pos);
				max = Math.Max(max, step);
				char pipe = (char)pipes[pos];

				if (DoesPipeConnect(pipe, Direction.Right) && !open.Exists(v => v.p == new Vector2(pos.x + 1, pos.y)))
					open.Add((new Vector2(pos.x + 1, pos.y), step + 1));
				if (DoesPipeConnect(pipe, Direction.Left) && !open.Exists(v => v.p == new Vector2(pos.x - 1, pos.y)))
					open.Add((new Vector2(pos.x - 1, pos.y), step + 1));
				if (DoesPipeConnect(pipe, Direction.Down) && !open.Exists(v => v.p == new Vector2(pos.x, pos.y + 1)))
					open.Add((new Vector2(pos.x, pos.y + 1), step + 1));
				if (DoesPipeConnect(pipe, Direction.Up) && !open.Exists(v => v.p == new Vector2(pos.x, pos.y - 1)))
					open.Add((new Vector2(pos.x, pos.y - 1), step + 1));

				pipes[pos] = '.';
			}
			
			return max;
		}

		private static Grid FloodFillPipes(Grid pipes, Vector2 p)
		{
			Grid loop = new Grid(pipes.Width*3, pipes.Height*3);
			for (int x = 0; x < loop.Width; x++)
			{
				for (int y = 0; y < loop.Height; y++)
				{
					loop[x, y] = ' ';
				}
			}
			List<(Vector2 p, long step)> open = new List<(Vector2, long)>();
			List<Vector2> closed = new List<Vector2>();
			open.Add((p, 0));
			
			while (open.Count > 0)
			{
				(Vector2 pos, long step) = open[0];
				open.RemoveAt(0);
				if (closed.Contains(pos)) continue;
				closed.Add(pos);
				char pipe = (char)pipes[pos];
				ZoomFill(loop, new Vector2(pos.x *3, pos.y * 3), pipe);

				if (DoesPipeConnect(pipe, Direction.Right) && !open.Exists(v => v.p == new Vector2(pos.x + 1, pos.y)))
					open.Add((new Vector2(pos.x + 1, pos.y), step + 1));
				if (DoesPipeConnect(pipe, Direction.Left) && !open.Exists(v => v.p == new Vector2(pos.x - 1, pos.y)))
					open.Add((new Vector2(pos.x - 1, pos.y), step + 1));
				if (DoesPipeConnect(pipe, Direction.Down) && !open.Exists(v => v.p == new Vector2(pos.x, pos.y + 1)))
					open.Add((new Vector2(pos.x, pos.y + 1), step + 1));
				if (DoesPipeConnect(pipe, Direction.Up) && !open.Exists(v => v.p == new Vector2(pos.x, pos.y - 1)))
					open.Add((new Vector2(pos.x, pos.y - 1), step + 1));

				pipes[pos] = '.';
			}
			
			return loop;
		}

		private static void ZoomFill(Grid loop, Vector2 pos, char pipe)
		{
			switch (pipe)
			{
				case '|':
					loop[new Vector2(pos.x + 1, pos.y + 0)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 2)] = '.';
					break;
				case '-':
					loop[new Vector2(pos.x + 0, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 2, pos.y + 1)] = '.';
					break;
				case 'L':
					loop[new Vector2(pos.x + 1, pos.y + 0)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 2, pos.y + 1)] = '.';
					break;
				case 'J':
					loop[new Vector2(pos.x + 1, pos.y + 0)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 0, pos.y + 1)] = '.';
					break;
				case '7':
					loop[new Vector2(pos.x + 0, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 2)] = '.';
					break;
				case 'F':
					loop[new Vector2(pos.x + 2, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 1)] = '.';
					loop[new Vector2(pos.x + 1, pos.y + 2)] = '.';
					break;
			}
		}

		private static char DetermineStartPipe(Grid pipes, Vector2 start)
		{
			bool R = pipes.MaxX > start.x && DoesPipeConnect((char)pipes[start.x + 1, start.y], Direction.Left);
			bool L = pipes.MinX < start.x && DoesPipeConnect((char)pipes[start.x - 1, start.y], Direction.Right);
			bool U = pipes.MaxY > start.y && DoesPipeConnect((char)pipes[start.x, start.y - 1], Direction.Down);
			bool D = pipes.MinY < start.y && DoesPipeConnect((char)pipes[start.x, start.y + 1], Direction.Up);

			if (R && L) return '-';
			if (U && D) return '|';
			if (U && R) return 'L';
			if (U && L) return 'J';
			if (D && L) return '7';
			if (D && R) return 'F';

			throw new Exception("Oh god");
			return '.';
		}

		enum Direction { Left, Right, Up, Down }

		private static bool DoesPipeConnect(char c, Direction dir)
		{
			switch (c)
			{
				case '|':
					return dir == Direction.Down || dir == Direction.Up;
				case '-':
					return dir == Direction.Left || dir == Direction.Right;
				case 'L':
					return dir == Direction.Up || dir == Direction.Right;
				case 'J':
					return dir == Direction.Up || dir == Direction.Left;
				case '7':
					return dir == Direction.Left || dir == Direction.Down;
				case 'F':
					return dir == Direction.Down || dir == Direction.Right;
			}

			Console.WriteLine(c);
			return false;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid pipes = new Grid(input, true);

			Vector2 s = GetStart(pipes);

			char c = DetermineStartPipe(pipes, s);
			pipes[s] = c;

			Grid zoom = FloodFillPipes(pipes, s);

			zoom.FloodFill(0, 0, '.', (value, fill) => true, false);

			for (int x = 1; x < zoom.Width; x+=3)
			{
				for (int y = 1; y < zoom.Height; y += 3)
				{
					if (zoom[x, y] == ' ')
						result++;
				}
			}

			return result;
		}
	}
}
