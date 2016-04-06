using UnityEngine;
using System;
using System.Collections.Generic;

namespace VoxelEngine
{
	class ChunkModelStore
	{
		const float storeChunkModelsRangeXLength = 300;
		const float storeChunkModelsRangeYLength = 300;
		const float storeChunkModelsRangeZLength = 300;

		const float storeChunkModelsUpdateRangeXLength = storeChunkModelsRangeXLength/2;
		const float storeChunkModelsUpdateRangeYLength = storeChunkModelsRangeYLength/2;
		const float storeChunkModelsUpdateRangeZLength = storeChunkModelsRangeZLength/2;

		public readonly List<ChunkModel> chunkModels;

		public ChunkModelStore ()
		{
			chunkModels = new List<ChunkModel> ();
		}

		public void PrepareForBasePoint (Vector3 basePoint)
		{
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					for (int z = -1; z <= 1; z++) {
						chunkModels.Add (new ChunkModel (x, y, z));
					}
				}
			}
		}

		/// <summary>
		/// boundsに交差しているChunkModelを取得する。
		/// </summary>
		public List<ChunkModel> ChunkModelsFromBounds (Bounds bounds)
		{
			List<ChunkModel> chunkModels = new List<ChunkModel> ();

			foreach (ChunkModel chunkModel in this.chunkModels) {
				if (bounds.Intersects(chunkModel.Bounds)) {
					chunkModels.Add (chunkModel);
				}
			}

			return chunkModels;
		}
			
	}
}

