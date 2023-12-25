using System;
using System.Collections.Generic;
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

			int totalVerticies = graph.Vertices.Count;
			alledges = alledges.Distinct().ToList();

			for (int a = 0; a < alledges.Count; a++)
			{
				for (int b = a+1; b < alledges.Count; b++)
				{
					for (int c = b+1; c < alledges.Count; c++)
					{
						if (TryDisconnect(graph, alledges[a], alledges[b], alledges[c], out long res, totalVerticies))
						{
							if(res > 2640)
								Console.WriteLine(res);
						}
					}
				}
			}

			return -1;
		}

		private static bool TryDisconnect(Graph<string> graph, GraphEdge e1, GraphEdge e2, GraphEdge e3, out long l, int numVerts)
		{
			graph.RemoveEdge(e1.left, e1.right);
			graph.RemoveEdge(e2.left, e2.right);
			graph.RemoveEdge(e3.left, e3.right);

			graph.RemoveEdge(e1.right, e1.left);
			graph.RemoveEdge(e2.right, e2.left);
			graph.RemoveEdge(e3.right, e3.left);

			(List<string> _, bool valid) = graph.FindPath(e1.left.Value, e1.right.Value);
			if (valid)
			{
				graph.AddEdge(e1.left, e1.right);
				graph.AddEdge(e2.left, e2.right);
				graph.AddEdge(e3.left, e3.right);

				graph.AddEdge(e1.right, e1.left);
				graph.AddEdge(e2.right, e2.left);
				graph.AddEdge(e3.right, e3.left);

				l = -1;
				return false;
			}
			else
			{
				List<string> reachableFromE1L = new List<string>();
				foreach (Vertex<string> vert in graph.Vertices)
				{
					if(vert == e1.left) continue;

					(_, valid) = graph.FindPath(e1.left.Value, vert.Value);
					if (valid)
					{
						reachableFromE1L.Add(vert.Value);
					}
				}
				List<string> reachableFromE1R = new List<string>();
				foreach (Vertex<string> vert in graph.Vertices)
				{
					if (vert == e1.right) continue;

					(_, valid) = graph.FindPath(e1.right.Value, vert.Value);
					if (valid)
					{
						reachableFromE1R.Add(vert.Value);
					}
				}

				l = (reachableFromE1L.Count+1) * (reachableFromE1R.Count+1);
				
				graph.AddEdge(e1.left, e1.right);
				graph.AddEdge(e2.left, e2.right);
				graph.AddEdge(e3.left, e3.right);

				graph.AddEdge(e1.right, e1.left);
				graph.AddEdge(e2.right, e2.left);
				graph.AddEdge(e3.right, e3.left);

				return reachableFromE1L.Count + reachableFromE1R.Count + 2 == numVerts;
			}
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
