using System;

namespace Draco18s.AoCLib {
	public struct Vector2
	{
		//public static Vector2 ONE = new Vector2(1, 1);

		public readonly int x;
		public readonly int y;
		public double magnitude => Math.Sqrt(x * x + y * y);
		public Vector2(int _x, int _y) {
			x = _x;
			y = _y;
		}

		public static Vector2 Parse(string val)
		{
			return Parse(val, ',');
		}

		public static Vector2 Parse(string val, char split)
		{
			string[] vals = val.Split(split);
			return new Vector2(int.Parse(vals[0]), int.Parse(vals[1]));
		}

		public static Vector2 operator *(Vector2 a, int b)
		{
			return new Vector2(a.x * b, a.y * b);
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