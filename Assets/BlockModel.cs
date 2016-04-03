using UnityEngine;
using System;

namespace VoxelEngine
{
	public class BlockModel
	{
		static public readonly Vector3 size = new Vector3 (1, 1, 1);

		public readonly Bounds bounds;
		public readonly bool density;
		public bool visible;
		public readonly Point3D<int> localIndex;

		internal BlockModel (Vector3 centerPoint, bool density, Point3D<int> localIndex)
		{
			this.bounds = new Bounds (centerPoint, size);
			this.density = density;
			this.visible = false;
			this.localIndex = localIndex;
		}
	}
}

