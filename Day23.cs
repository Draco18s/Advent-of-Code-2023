using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;
using static System.Net.Mime.MediaTypeNames;

namespace AdventofCode2023
{
	internal static class Day23
	{
		internal static long Part1(string input)
		{
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid trail = new Grid(input, true);

			result = FindLongestPath(trail);

			return result;
		}

		private static long FindLongestPath(Grid trail)
		{
			Vector2 start = new Vector2(0, 0);
			Vector2 end = new Vector2(0, 0);

			for (int x = 0; x < trail.Width; x++)
			{
				if (trail[x, 0] == '.')
				{
					start = new Vector2(x, 0);
				}
				if (trail[x, trail.MaxY-1] == '.')
				{
					end = new Vector2(x, trail.MaxY - 1);
				}
			}
			Grid pathCost = new Grid(trail.Width, trail.Height);
			pathCost[start] = 0;
			
			pathCost.FloodFill(start, (cur, next) =>
			{
				if (pathCost[next] != 0)
				{
					int n = pathCost[next];
					int c = pathCost[cur] - 1;
					if (n == c)
						return pathCost[next];
					return Math.Max(pathCost[next], pathCost[cur] + 1);
				}

				return pathCost[cur] + 1;
			}, (nPos, cPos, curr, next) =>
			{
				if (!trail.IsInside(nPos))
					return false;
				//if (next > 0)
				//	return next < curr+1;
				if (trail[nPos] == '#') return false;
				if (trail[nPos] == '.' && trail[cPos] == '.') return true;

				char charAtPosHere = (char)trail[cPos];
				Vector2 allowedOffsetHere = GetOffsetFor(charAtPosHere);
				char charAtPosNext = (char)trail[nPos];
				Vector2 allowedOffsetNext = GetOffsetFor(charAtPosNext);

				if (charAtPosNext == '.')
				{
					return nPos == cPos + allowedOffsetHere;
				}
				else if(charAtPosHere == '.')
				{
					return nPos == cPos + allowedOffsetNext;
				}

				return false;
			}, Grid.returnNegInf);
			
			return pathCost[end];
		}

		private static Vector2 GetOffsetFor(char charAtPos)
		{
			switch (charAtPos)
			{
				case '<':
					return new Vector2(-1, 0);
				case '>':
					return new Vector2(1, 0);
				case '^':
					return new Vector2(0, -1);
				case 'v':
					return new Vector2(0, 1);
			}

			return new Vector2(0, 0);
		}

		private class SplitPoint
		{
			public readonly Vector2 pos;

			public SplitPoint(Vector2 p)
			{
				pos = p;
			}

			public override int GetHashCode()
			{
				return 10079 * pos.x + pos.y;
			}

			public static bool operator ==(SplitPoint a, SplitPoint b)
			{
				return a.Equals(b);
			}

			public static bool operator !=(SplitPoint a, SplitPoint b)
			{
				return !a.Equals(b);
			}

			public override bool Equals(object obj)
			{
				if (obj is SplitPoint sp)
					return sp.pos == pos;
				return false;
			}

			public override string ToString()
			{
				return pos.ToString();
			}
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			Grid trail = new Grid(input, true);

			List<Vector2> splitPoints = FindPathSplits(trail);

			Vector2 start = new Vector2(0, 0);
			Vector2 end = new Vector2(0, 0);

			for (int x = 0; x < trail.Width; x++)
			{
				if (trail[x, 0] == '.')
				{
					start = new Vector2(x, 0);
				}
				if (trail[x, trail.MaxY - 1] == '.')
				{
					end = new Vector2(x, trail.MaxY - 1);
				}
			}

			splitPoints.Add(start);
			splitPoints.Add(end);

			Graph<SplitPoint> graph = new Graph<SplitPoint>();

			foreach (Vector2 point in splitPoints)
			{
				graph.AddVertex(new SplitPoint(point));
			}
			
			foreach (Vector2 point in splitPoints)
			{
				SplitPoint A = new SplitPoint(point);
				SplitPoint B = new SplitPoint(point);
				(Vector2 pos, int distance) r = FindNeighbor(trail, point, new Vector2(-1,0), splitPoints);
				B = new SplitPoint(r.pos);
				if (r.distance > 0 && !graph.HasEdge(A, B))
				{
					graph.AddEdge(A, B, r.distance);
				}

				r = FindNeighbor(trail, point, new Vector2(1, 0), splitPoints);
				B = new SplitPoint(r.pos);
				if (r.distance > 0 && !graph.HasEdge(A, B))
				{
					graph.AddEdge(A, B, r.distance);
				}

				r = FindNeighbor(trail, point, new Vector2(0, -1), splitPoints);
				B = new SplitPoint(r.pos);
				if (r.distance > 0 && !graph.HasEdge(A, B))
				{
					graph.AddEdge(A, B, r.distance);
				}

				r = FindNeighbor(trail, point, new Vector2(0, 1), splitPoints);
				B = new SplitPoint(r.pos);
				if (r.distance > 0 && !graph.HasEdge(A, B))
				{
					graph.AddEdge(A, B, r.distance);
				}
			}
			
			return DoLongPathfind(graph, start, end);
		}

		private static (Vector2 pos, int distance) FindNeighbor(Grid trail, Vector2 start, Vector2 firstStep, List<Vector2> splitPoints)
		{
			Grid pathCost = new Grid(trail.Width, trail.Height);
			pathCost[start] = 0;

			if (!pathCost.IsInside(start + firstStep))
			{
				return (new Vector2(0, 0), -1);
			}

			pathCost[start + firstStep] = 1;
			
			pathCost.FloodFill(start+firstStep, (cur, next) =>
			{
				if (pathCost[next] != 0)
				{
					int n = pathCost[next];
					int c = pathCost[cur] - 1;
					if (n == c)
						return pathCost[next];
					return Math.Min(pathCost[next], pathCost[cur] + 1);
				}

				return pathCost[cur] + 1;
			}, (nPos, cPos, curr, next) =>
			{
				if (!trail.IsInside(nPos))
					return false;

				if (nPos == start) return false;

				if(splitPoints.Contains(cPos)) return false;

				if (trail[nPos] == '#' || trail[cPos] == '#') return false;
				
				return true;
			}, Grid.returnNegInf);

			foreach (Vector2 p in splitPoints)
			{
				if (pathCost[p] > 0)
				{
					return (p, pathCost[p]);
				}
			}

			return (new Vector2(0,0), -1);
		}

		private static List<Vector2> FindPathSplits(Grid trail)
		{
			List<Vector2> result = new List<Vector2>();
			for (int x = 1; x < trail.Width-1; x++)
			{
				for (int y = 1; y < trail.Height-1; y++)
				{
					if (trail[x, y] != '.') continue;

					bool N = (trail[x, y - 1] != '#');
					bool S = (trail[x, y + 1] != '#');
					bool E = (trail[x + 1, y] != '#');
					bool W = (trail[x - 1, y] != '#');
					int dirCount = N ? 1 : 0;
					dirCount += S ? 1 : 0;
					dirCount += E ? 1 : 0;
					dirCount += W ? 1 : 0;
					if (dirCount > 2)
					{
						result.Add(new Vector2(x,y));
					}
				}
			}
			return result;
		}

		private class SearchNode
		{
			public Vertex<SplitPoint> pos;
			public readonly List<SearchNode> previousNodes = new List<SearchNode>();
			public long distance;

			public bool HasVisited(Vector2 loc)
			{
				if(pos.Value.pos == loc) return true;
				return previousNodes.Any(n => n.pos.Value.pos == loc);
			}

			public bool HasVisited(Vertex<SplitPoint> loc)
			{
				if (pos.Value.pos == loc.Value.pos) return true;
				return previousNodes.Any(n => n.pos.Value.pos == loc.Value.pos);
			}
		}

		private static long DoLongPathfind(Graph<SplitPoint> trail, Vector2 s, Vector2 e)
		{
			Vertex<SplitPoint> start = trail.GetVertex(new SplitPoint(s)).Edges[0].node;
			Vertex<SplitPoint> end = trail.GetVertex(new SplitPoint(e)).Edges[0].node;

			long baseDistance = trail.GetVertex(new SplitPoint(s)).Edges[0].weight + trail.GetVertex(new SplitPoint(e)).Edges[0].weight;

			trail.RemoveEdge(new SplitPoint(s), start.Value);
			trail.RemoveEdge(new SplitPoint(e), end.Value);

			Queue<SearchNode> open = new Queue<SearchNode>();
			open.Enqueue(new SearchNode()
			{
				pos = start,
				distance = 0
			});

			long maxDist = 0;

			while (open.Count > 0)
			{
				SearchNode cur = open.Dequeue();

				if (cur.pos.Value.pos == end.Value.pos)
				{
					long distTraveled = cur.distance;
					if(distTraveled > maxDist) Console.WriteLine(distTraveled);
					maxDist = Math.Max(maxDist, distTraveled);
					
					continue;
				}

				foreach ((Vertex<SplitPoint> node, int weight) in cur.pos.Edges)
				{
					if(cur.HasVisited(node)) continue;
					SearchNode newNode = new SearchNode(){ pos = node, distance = cur.distance + weight };
					newNode.previousNodes.AddRange(cur.previousNodes);
					newNode.previousNodes.Add(cur);
					open.Add(newNode);
				}
			}

			return baseDistance + maxDist;
		}
	}
}
