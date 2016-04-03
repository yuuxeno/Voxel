using UnityEngine;
using System;

namespace VoxelEngine
{
	/// <summary>
	/// ChunkModelを分割したModel。
	/// Chunkを分割して表示するために使う。
	/// </summary>
	public class MiniChunkModel
	{
		readonly BlockModels blockModels;

		public Bounds Bounds { get; private set; }

		public MiniChunkModel (BlockModels blockModels)
		{
			this.blockModels = blockModels;
			this.Bounds = this.blockModels.Bounds;
		}
	}
}

