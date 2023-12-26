using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2023
{
	internal static class Day25
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			List<string> components = new List<string>();
			foreach (string line in lines)
			{
				string[] parts = line.Split(": ");
				string[] conns = parts[1].Split(' ');
				components.Add(parts[0]);
				components.AddRange(conns);
			}
			components = components.Distinct().ToList();

			Graph<string> graph = new Graph<string>();
			foreach (string comp in components)
			{
				graph.AddVertex(comp);
			}
			foreach (string line in lines)
			{
				string[] parts = line.Split(": ");
				string[] conns = parts[1].Split(' ');
				foreach (string comp in conns)
				{
					graph.AddEdge(parts[0], comp);
					graph.AddEdge(comp, parts[0]);
				}
			}

			List<GraphEdge> alledges = new List<GraphEdge>();
			foreach (Vertex<string> vert in graph.Vertices)
			{
				foreach ((Vertex<string> node, _) in vert.Edges)
				{
					alledges.Add(new GraphEdge()
					{
						left = vert,
						right = node,
					});
				}
			}
			
			alledges = alledges.Distinct().Where(e => !CheckCommonNeighbors(e)).OrderBy(e => e.ToString()).ToList();

			Dictionary<GraphEdge, int> edgeUseCount = new Dictionary<GraphEdge, int>();

			List<Vertex<string>> verts = graph.Vertices.ToList();
			
			for (int i = 0; i < 1000; i++)
			{
				verts.Shuffle();
				(List<string> path, bool valid) = graph.FindPath(verts[0], verts[1]);
				for (var j = 1; j < path.Count; j++)
				{
					Vertex<string> vert1 = graph.GetVertex(path[j-1]);
					Vertex<string> vert2 = graph.GetVertex(path[j]);
					GraphEdge edge = new GraphEdge()
					{
						left = vert1,
						right = vert2,
					};
					edgeUseCount.TryAdd(edge, 0);
					edgeUseCount[edge]++;
				}
			}

			KeyValuePair<GraphEdge, int>[] top3 = edgeUseCount.OrderByDescending(kvp => kvp.Value).Take(3).ToArray();

			TryDisconnect(graph, top3[0].Key, top3[1].Key, top3[2].Key, out result);

			return result;
		}

		private static bool TryPathDisconnect(Graph<string> graph, GraphEdge e1, GraphEdge e2, out long res)
		{
			graph.RemoveEdge(e1.left, e1.right);
			graph.RemoveEdge(e2.left, e2.right);

			graph.RemoveEdge(e1.right, e1.left);
			graph.RemoveEdge(e2.right, e2.left);

			(List<string> path, bool valid) p1 = graph.FindPath(e1.left, e2.right);
			(List<string> path, bool valid) p2 = graph.FindPath(e1.left, e2.right);
			foreach (string p1n in p1.path)
			{
				if (!p2.path.Contains(p1n)) continue;

				Vertex<string> common = graph.GetVertex(p1n);
				(Vertex<string> node, int weight)[] edgesToCheck = common.Edges.ToArray();
				foreach ((Vertex<string> node, int weight) e in edgesToCheck)
				{
					if(!p1.path.Contains(e.node.Value)) continue;
					if(!p2.path.Contains(e.node.Value)) continue;

					GraphEdge e3 = new GraphEdge()
					{
						left = common,
						right = e.node
					};

					if (TryDisconnect(graph, e1, e2, e3, out res))
					{
						return true;
					}
				}
			}

			graph.AddEdge(e1.left, e1.right);
			graph.AddEdge(e2.left, e2.right);

			graph.AddEdge(e1.right, e1.left);
			graph.AddEdge(e2.right, e2.left);
			res = -1;
			return false;
		}

		private static bool TryDisconnect(Graph<string> graph, GraphEdge e1, GraphEdge e2, GraphEdge e3, out long l)
		{
			l = -1;

			graph.RemoveEdge(e1.left, e1.right);
			graph.RemoveEdge(e2.left, e2.right);
			graph.RemoveEdge(e3.left, e3.right);

			graph.RemoveEdge(e1.right, e1.left);
			graph.RemoveEdge(e2.right, e2.left);
			graph.RemoveEdge(e3.right, e3.left);

			int reachableFromE1L = graph.GetConnectivity(e1.left);
			int totalReach = reachableFromE1L;
			if (reachableFromE1L < graph.Vertices.Count)
			{

				int reachableFromE1R = graph.GetConnectivity(e1.right);
				totalReach += reachableFromE1R;

				l = (reachableFromE1L) * (reachableFromE1R);
			}

			graph.AddEdge(e1.left, e1.right);
			graph.AddEdge(e2.left, e2.right);
			graph.AddEdge(e3.left, e3.right);

			graph.AddEdge(e1.right, e1.left);
			graph.AddEdge(e2.right, e2.left);
			graph.AddEdge(e3.right, e3.left);

			return totalReach == graph.Vertices.Count && l > 0;
		}

		private static bool CheckCommonNeighbors(GraphEdge edge)
		{
			foreach ((Vertex<string> node, int weight) e1 in edge.left.Edges)
			{
				if(e1.node == edge.right) continue;
				// find and invalidate triangles
				foreach ((Vertex<string> node, int weight) e2 in edge.right.Edges)
				{
					if (e2.node == edge.left) continue;
					if (e1 == e2) return true;

					/*// find and invalidate all quadrilaterals 
					foreach ((Vertex<string> node, int weight) e3 in e2.node.Edges)
					{
						if (e3.node == e2.node) continue;

						if (e3 == e1) return true;

						// find and invalidate all pentangles 
						foreach ((Vertex<string> node, int weight) e4 in e1.node.Edges)
						{
							if (e4.node == e1.node) continue;

							if (e4 == e3) return true;
						}
					}*/
				}
			}

			return false;
		}

		private class GraphEdge
		{
			public Vertex<string> left;
			public Vertex<string> right;

			public override bool Equals(object obj)
			{
				if (obj is GraphEdge e)
				{
					return (e.left.Equals(left) && e.right.Equals(right)) || (e.left.Equals(right) && e.right.Equals(left));
				}
				return false;
			}

			public override int GetHashCode()
			{
				return left.Value.GetHashCode() * right.Value.GetHashCode();
			}

			public override string ToString()
			{
				return $"{left.Value}—{right.Value}";
			}
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{

			}
			return result;
		}
	}
}
