using System;

namespace VoxelEngine
{
	public struct Point3D<T>
	{
		public readonly T x;
		public readonly T y;
		public readonly T z;

		public Point3D (T x, T y, T z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}

