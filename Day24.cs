using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Draco18s.AoCLib;
using static AdventofCode2023.Day22;

namespace AdventofCode2023
{
	//started 5:14pm on 12/24
	internal static class Day24
	{
		public class Hailstone
		{
			public Vector3L pos;
			public Vector3L velocity;
		}
		// solved 5:38pm
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			List<Hailstone> allStones = new List<Hailstone>();
			foreach (string line in lines)
			{
				string[] parts = line.Split(" @ ");
				string[] vals = parts[0].Split(',');
				Vector3L pos = new Vector3L(long.Parse(vals[0]), long.Parse(vals[1]), long.Parse(vals[2]));
				vals = parts[1].Split(',');
				Vector3L vel = new Vector3L(long.Parse(vals[0]), long.Parse(vals[1]), long.Parse(vals[2]));
				Hailstone h = new Hailstone()
				{
					pos = pos,
					velocity = vel,
				};
				allStones.Add(h);
			}

			result = PossibleIntersections_Part1(allStones) >> 1;

			return result;
		}

		private static long PossibleIntersections_Part1(List<Hailstone> allStones)
		{
			long ret = 0L;
			long minTest = 200000000000000l;
			long maxTest = 400000000000000l;
			foreach (Hailstone h1 in allStones)
			{
				foreach (Hailstone h2 in allStones)
				{
					if(h1 == h2) continue;

					if (IntersectsXY(h1.pos.x, h1.pos.y,
						    (h1.pos.x + h1.velocity.x * 600000000000000l), (h1.pos.y + h1.velocity.y * 600000000000000l),
								h2.pos.x, h2.pos.y,
								(h2.pos.x + h2.velocity.x * 600000000000000l), (h2.pos.y + h2.velocity.y * 600000000000000l), out long cX, out long cY))
					{
						if (cX >= minTest && cX <= maxTest && cY >= minTest && cY <= maxTest)
						{
							ret++;
						}
					}
				}
			}

			return ret;
		}

		private static bool IntersectsXY(long p0_x, long p0_y, long p1_x, long p1_y, long p2_x, long p2_y, long p3_x, long p3_y, out long i_x, out long i_y)
		{
			i_x = 0;
			i_y = 0;

			float s1_x, s1_y, s2_x, s2_y;
			s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
			s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

			float s, t;
			s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
			t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

			if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
			{
				i_x = (long)(p0_x + (t * s1_x));
				i_y = (long)(p0_y + (t * s1_y));
				return true;
			}
			return false;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			List<Hailstone> allStones = new List<Hailstone>();
			foreach (string line in lines)
			{
				string[] parts = line.Split(" @ ");
				string[] vals = parts[0].Split(',');
				Vector3L pos = new Vector3L(long.Parse(vals[0]), long.Parse(vals[1]), long.Parse(vals[2]));
				vals = parts[1].Split(',');
				Vector3L vel = new Vector3L(long.Parse(vals[0]), long.Parse(vals[1]), long.Parse(vals[2]));
				Hailstone h = new Hailstone()
				{
					pos = pos,
					velocity = vel,
				};
				allStones.Add(h);
			}

			List<Hailstone> allStonesBut = allStones.Skip(2).ToList();

			long offset1 = 0;
			long offset2 = 0;
			foreach (Vector3L s in GetPositions(allStones[0]))
			{
				foreach (Vector3L e in GetPositions(allStones[1]))
				{
					offset2++;
					Vector3L v = e - s;

					long lcm = AoCMath.GDC(Math.Abs(v.x), AoCMath.GDC(Math.Abs(v.y), Math.Abs(v.z)));

					Vector3L minMax = v / lcm;
					long min = Math.Min(minMax.x, Math.Min(minMax.y, minMax.z));
					long max = Math.Max(minMax.x, Math.Max(minMax.y, minMax.z));

					Vector3L vf = v;// / lcm;
					Vector3L f = s - vf * offset1;

					if (IntersectAllStones(f, vf, allStones))
					{
						//return f.x + f.y + f.z;
						;
					}

					/*for (long o = min; IsBetween(o, min, max); o += lcm)
					{
						if(o == 0)
							continue;
						if (v.x % o != 0)
							continue;
						if (v.y % o != 0)
							continue;
						if (v.z % o != 0)
							continue;

						Vector3L vf = v / o;
						Vector3L f = s - vf * (offset1);

						//3D intersection test against all hailstones
						if (IntersectAllStones(f, vf, allStones))
						{
							return f.x + f.y + f.z;
						}
						else
						{
							;
						}
					}*/
					if (offset2 > 100) break;
				}

				offset1++;
				offset2 = offset1;
				if (offset1 > 100) break;
			}

			return result;
		}

		private static bool IntersectAllStones(Vector3L pos, Vector3L vel, List<Hailstone> allStones)
		{
			foreach (Hailstone stone in allStones)
			{
				if (LineLineIntersect(stone.pos, stone.pos + stone.velocity, pos, pos + vel, out Vector3L pa, out Vector3L pb, out _, out _))
				{
					if(pa != pb)
						return false;
				}
			}
			return true;
		}

		private static bool IsBetween(long l, long min, long max)
		{
			return l >= min && l <= max;
		}

		private static bool AllIntersections(Vector3L startPos, Vector3L vel, List<Hailstone> allStonesBut)
		{
			long minTest = 7;
			long maxTest = 27;
			
			foreach (Hailstone h2 in allStonesBut)
			{
				if (IntersectsXY(startPos.x, startPos.y,
					    (startPos.x + vel.x * 600000000000000l), (startPos.y + vel.y * 600000000000000l),
					    h2.pos.x, h2.pos.y,
					    (h2.pos.x + h2.velocity.x * 600000000000000l), (h2.pos.y + h2.velocity.y * 600000000000000l), out long cX, out long cY))
				{
					if (cX >= minTest && cX <= maxTest && cY >= minTest && cY <= maxTest)
					{
						continue;
					}
					else return false;
				}
				else return false;
			}
			
			return true;
		}

		private static IEnumerable<Vector3L> GetPositions(Hailstone stone)
		{
			long step = 0;
			while(step < 100)
				yield return stone.pos + stone.velocity * (step++);
		}

		private static bool LineLineIntersect(
			Vector3L p1, Vector3L p2, Vector3L p3, Vector3L p4, out Vector3L pa, out Vector3L pb,
			out double mua, out double mub)
		{
			pa = new Vector3L(0, 0, 0);
			pb = new Vector3L(0, 0, 0);
			mua = 0;
			mub = 0;
			Vector3L p13, p43, p21;
			double d1343, d4321, d1321, d4343, d2121;
			double numer, denom;
			
			p13 = new Vector3L(p1.x - p3.x, p1.y - p3.y, p1.z - p3.z);
			p43 = new Vector3L(p4.x - p3.x, p4.y - p3.y, p4.z - p3.z);

			if (Math.Abs(p43.x) < 0.1 && Math.Abs(p43.y) < 0.1 && Math.Abs(p43.z) < 0.1)
				return false;

			p21 = new Vector3L(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);
			if (Math.Abs(p21.x) < 0.1 && Math.Abs(p21.y) < 0.1 && Math.Abs(p21.z) < 0.1)
				return false;

			d1343 = p13.x * p43.x + p13.y * p43.y + p13.z * p43.z;
			d4321 = p43.x * p21.x + p43.y * p21.y + p43.z * p21.z;
			d1321 = p13.x * p21.x + p13.y * p21.y + p13.z * p21.z;
			d4343 = p43.x * p43.x + p43.y * p43.y + p43.z * p43.z;
			d2121 = p21.x * p21.x + p21.y * p21.y + p21.z * p21.z;

			denom = d2121 * d4343 - d4321 * d4321;
			if (Math.Abs(denom) < 0.1)
				return false;
			numer = d1343 * d4321 - d1321 * d4343;

			mua = numer / denom;
			mub = (d1343 + d4321 * mua) / d4343;
			
			pa = new Vector3L((long)(p1.x + mua * p21.x), (long)(p1.y + mua * p21.y), (long)(p1.z + mua * p21.z));
			pb = new Vector3L((long)(p3.x + mub * p43.x), (long)(p3.y + mub * p43.y), (long)(p3.z + mub * p43.z));

			return true;
		}

	}
}
