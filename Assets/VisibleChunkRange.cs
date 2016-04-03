using UnityEngine;
using System;

namespace VoxelEngine
{
	/// <summary>
	/// Chunkを表示する領域。
	/// </summary>
	public class VisibleChunkRange
	{
		public event Action UpdatedVisibleChunkRange;

		public Bounds Bounds { get; private set; }

		Vector3 basePoint;

		/// <summary>
		/// prepareBoundsを超えるとVisibleChunkRangeが更新される。
		/// </summary>
		Bounds prepareBounds;

		readonly float prepareBoundsRate;

		public VisibleChunkRange (Bounds bounds, float prepareBoundsRate)
		{
			Bounds = bounds;
			prepareBoundsRate = prepareBoundsRate;
			basePoint = Bounds.center;

			prepareBounds = CreatePrepareBounds ();
		}

		public void UpdateBasePoint (Vector3 basePoint)
		{
			this.basePoint = basePoint;

			if (!prepareBounds.Contains(this.basePoint)) {
				Bounds = new Bounds (this.basePoint, Bounds.size);
				prepareBounds = CreatePrepareBounds ();

				if (UpdatedVisibleChunkRange != null) {
					UpdatedVisibleChunkRange ();
				}
			}
		}

		Bounds CreatePrepareBounds ()
		{
			return new Bounds (Bounds.center, new Vector3(Bounds.size.x * prepareBoundsRate, Bounds.size.y * prepareBoundsRate, Bounds.size.z * prepareBoundsRate));
		}
	}
}

