using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draco18s.AoCLib
{
	public static class AoCMath
	{
		public static long GDC(long a, long b) => (b == 0 || b == 1) ? a : GDC(b, a % b);
		public static long LCM(long b, long a) => (b == 0 || b == 1) ? a : a / GDC(a, b) * b;
	}
}
