using System;

namespace Draco18s.AoCLib {
	public struct Vector2 {
		public readonly int x;
		public readonly int y;
		public double magnitude => Math.Sqrt(x * x + y * y);
		public Vector2(int _x, int _y) {
			x = _x;
			y = _y;
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}
		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}
		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Vector2 a, Vector2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public override string ToString() {
			return $"({x},{y})";
		}
	}
}