using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draco18s.AoCLib
{
	public class Graph<T> where T : class
	{
		public IReadOnlyList<Vertex<T>> Vertices => _vertices;

		private readonly List<Vertex<T>> _vertices;

		private const int INVALID_VALUE = 10000;

		public Graph()
		{
			_vertices = new List<Vertex<T>>();
		}

		public void AddVertex(T vertex)
		{
			if (vertex == null) throw new ArgumentNullException();
			_vertices.Add(new Vertex<T>(vertex));
		}

		public void RemoveVertex(Vertex<T> vertex)
		{
			foreach (Vertex<T> vert in _vertices)
			{
				if (vert.Value == vertex) continue;
				if (vert.Edges.Any(v => v.node == vertex))
				{
					throw new ArgumentException("Can only remove orphaned vertices!");
				}
			}
			_vertices.Remove(vertex);
		}

		public void AddEdge(T start, T end) => AddEdge(start, end, 1);

		public void AddEdge(Vertex<T> start, Vertex<T> end, int weight=1)
		{
			start.AddEdge(end, weight);
		}

		public void AddEdge(T start, T end, int weight)
		{
			GetVertex(start).AddEdge(GetVertex(end), weight);
		}

		public Vertex<T> GetVertex(T value)
		{
			return _vertices.Find(x => x.Value.Equals(value));
		}

		public bool HasNeighbor(T start, T end)
		{
			return _vertices.Find(x => x.Value == start && x.HasNeighbor(end)) != null;
		}

		public void RemoveEdge(Vertex<T> start, Vertex<T> end)
		{
			(Vertex<T> node, int weight) edgeToRemove = start.Edges.Find(edge => edge.node == end);
			if (edgeToRemove.node != null)
			{
				start.Edges.Remove(edgeToRemove);
			}
		}

		public void RemoveEdge(T start, T end)
		{
			Vertex<T> startVertex = GetVertex(start);
			Vertex<T> endVertex = GetVertex(end);

			if (startVertex == null || endVertex == null) return;

			(Vertex<T> node, int weight) edgeToRemove = startVertex.Edges.Find(edge => edge.node == endVertex);
			if (edgeToRemove.node != null)
			{
				startVertex.Edges.Remove(edgeToRemove);
			}
		}

		public bool HasEdge(T start, T end)
		{
			Vertex<T> startVertex = GetVertex(start);
			Vertex<T> endVertex = GetVertex(end);

			if (startVertex != null && endVertex != null)
			{
				return startVertex.Edges.Exists(edge => edge.node == endVertex);
			}

			return false;
		}

		public (List<T> path, bool valid) FindPath(T start, T end) => FindPath(start, end, null);
		public (List<T> path, bool valid) FindPath(T start, T end, Predicate<T> conditions)
		{
			if (start == end)
			{
				Console.Error.WriteLine("Cannot pass in same start and end node.");
				return (new List<T>(), false);
			}

			// Search all nodes
			List<Vertex<T>> searchList = new List<Vertex<T>>(); // We will grow this unvisited set as we go along until it's empty
			Vertex<T> startVertex = GetVertex(start);
			InitializePathFinding(startVertex);
			searchList.Add(startVertex);

			// Keep searching and looking at neighbors until we've exhausted all neighbors and everything reachable has been visited
			//int iterations = 0;
			while (searchList.Count > 0)
			{
				int ct = searchList.Count;
				/*if (iterations > 1000)
				{
					Console.Error.WriteLine("Infinite loop possibly detected, breaking out.");
					break;
				}*/
				var node = searchList.OrderByDescending(x => x.Score).ToList().Last();
				node.Visited = true;
				searchList.Remove(node);
				foreach (Vertex<T> newNode in CalculateNeighboringNodes(node, conditions))
				{
					// Add all the new nodes discovered to the queue
					if(!searchList.Contains(newNode))
						searchList.Add(newNode);
				}
				//iterations++;
				if (searchList.Count == ct)
					break;
			}

			// Once all possible nodes visited backtrack from the end node back to the start
			List<Vertex<T>> path = ReconstructPath(startVertex, GetVertex(end));
			if (path.Count == 0)
			{
				return (new List<T>(), false); // No path found
			}
			else
			{
				// Convert to the list of direct value references and also check if the path is valid or invalid in terms of the given conditions
				List<T> convertedPath = new List<T>();
				var valid = true;
				foreach (Vertex<T> vertex in path)
				{
					convertedPath.Add(vertex.Value);
					if (conditions != null && !conditions(vertex.Value)) valid = false;
				}
				return (convertedPath, valid);
			}
		}

		private void InitializePathFinding(Vertex<T> start)
		{
			foreach (Vertex<T> vertex in _vertices)
			{
				vertex.Visited = false;
				vertex.Score = int.MaxValue;
			}
			start.Score = 0;
			start.Visited = true;
		}

		// Returns the next set of nodes to be visited
		private List<Vertex<T>> CalculateNeighboringNodes(Vertex<T> node, Predicate<T> conditions)
		{
			List<Vertex<T>> nextSet = new List<Vertex<T>>();
			//Debug.Log((node.Value as World.Territory).Name + " | " + node.Score);
			foreach ((Vertex<T> node, int weight) neighbor in node.Edges)
			{
				int neighborScoreFromThisNode = node.Score + neighbor.weight;
				if (conditions != null)
				{
					// If we fail the conditions then add a large value to the score to make it heavily unfavorable
					neighborScoreFromThisNode += conditions(neighbor.node.Value) ? 0 : INVALID_VALUE;
				}
				if (neighborScoreFromThisNode < neighbor.node.Score)
				{
					neighbor.node.Score = neighborScoreFromThisNode;
				}

				if (!neighbor.node.Visited) nextSet.Add(neighbor.node);
			}
			return nextSet;
		}

		private List<Vertex<T>> ReconstructPath(Vertex<T> start, Vertex<T> end)
		{
			List<Vertex<T>> path = new List<Vertex<T>>();
			if (!end.Visited) return path;
			path.Add(end);
			// First check if end has any neighbors or is surrounded unvisited nodes
			Vertex<T> curNode = end;
			int iterations = 0;
			while (curNode != start)
			{
				if (iterations > 1000)
				{
					Console.Error.WriteLine("Infinite loop possibly detected, breaking out.");
					return new List<Vertex<T>>();
				}
				var lowestValue = int.MaxValue;
				Vertex<T> lowestNode = null;
				foreach ((Vertex<T> node, int weight) neighbor in curNode.Edges)
				{
					if (neighbor.node.Visited && neighbor.node.Score < lowestValue)
					{
						lowestValue = neighbor.node.Score;
						lowestNode = neighbor.node;
					}
				}
				if (lowestNode == null)
				{
					// End node was never visited therefore no path exists
					path.Clear();
					return path;
				}
				// don't backtrack
				if (!path.Contains(lowestNode))
				{
					path.Add(lowestNode);
					curNode = lowestNode;
				}
				iterations++;
			}
			path.Reverse(); // Make this path go forwards from the start node
			return path;
		}
	}
}
