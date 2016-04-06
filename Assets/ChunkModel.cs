using UnityEngine;
using System;
using System.Collections.Generic;

namespace VoxelEngine
{
	class ChunkModel
	{
		const int maxHeight = 256;
		const int xLength = 16;
		const int yLength = 64;
		const int zLength = 16;
		const int numberOfMiniChunk = 8;
		const int miniChunkYLength = yLength / numberOfMiniChunk;

		readonly Point3D<int> chunkPoint;
		readonly Vector3 basePoint;

		public Bounds Bounds { get; private set; }
		public BlockModels blockModels = new BlockModels (xLength, yLength, zLength);

		/// <summary>
		/// ChunkModelをnumberOfMiniChunk個に分割したもの。
		/// </summary>
		readonly List<MiniChunkModel> miniChunkModels;

		public string ID { get; private set; }

		public ChunkModel (int indexX, int indexY, int indexZ)
		{
			chunkPoint = new Point3D<int> (indexX, indexY, indexZ);

			float centerPointX = xLength * indexX;
			float centerPointY = yLength * indexY;
			float centerPointZ = zLength * indexZ;

			Vector3 centerPoint = new Vector3 (centerPointX, centerPointY, centerPointZ);

			Bounds = new Bounds (centerPoint, new Vector3(BlockModel.size.x * xLength, BlockModel.size.y * yLength, BlockModel.size.z * zLength));
			miniChunkModels = new List<MiniChunkModel> ();

			float basePointX = Bounds.center.x - (xLength / 2 * BlockModel.size.x);
			float basePointY = Bounds.center.y - (yLength / 2 * BlockModel.size.y);
			float basePointZ = Bounds.center.z - (zLength / 2 * BlockModel.size.z);
			basePoint = new Vector3 (basePointX, basePointY, basePointZ);

			Prepare ();
		}

		/// <summary>
		/// boundsに交差しているMiniChunkを取得する。
		/// </summary>
		public List<MiniChunkModel> MiniChunkFromBounds (Bounds bounds)
		{
			List<MiniChunkModel> miniChunkModels = new List<MiniChunkModel> ();

			foreach (MiniChunkModel miniChunkModel in this.miniChunkModels) {
				if (bounds.Intersects(miniChunkModel.Bounds)) {
					miniChunkModels.Add (miniChunkModel);
				}
			}

			return miniChunkModels;
		}

		void Prepare ()
		{
			PrepareBlockModels ();

			SetVisibleBlockModels ();

			DivideChunk ();
		}
			
		void PrepareBlockModels ()
		{
			BlockModels terrainSurfaceBlockModels = CreateTerrainSurfaceBlockModels ();

			for (int x = 0; x < blockModels.xLength; x++) {
				for (int z = 0; z < blockModels.zLength; z++) {
					for (int y = 0; y < blockModels.yLength; y++) {

						Point3D<int> localPoint = new Point3D<int> (x, y, z);

						Vector3 blockModelCenterPoint = BlockModelCenterPointFromBlockModelLocalPoint (localPoint);

						bool density = false;

						if (terrainSurfaceBlockModels[x, 0, z].bounds.center.y >= blockModelCenterPoint.y) {
							density = true;
						}

						blockModels [x, y, z] = new BlockModel (blockModelCenterPoint, density, localPoint);
					}
				}
			}
		}

		/// <summary>
		/// 表示するブロックを設定する。
		/// </summary>
		void SetVisibleBlockModels ()
		{
			foreach (BlockModel blockModel in blockModels) {
				if (InBorder (blockModel)) {
					blockModel.visible = true;
				}
			}
		}

		/// <summary>
		/// ChunkをMiniChunkに分割する。
		/// </summary>
		void DivideChunk () 
		{
			for (int y = 0; y < this.blockModels.yLength; y += miniChunkYLength) {
				BlockModels blockModels = this.blockModels.blockModelsFromIndexYRange (y, y + miniChunkYLength - 1);
				miniChunkModels.Add (new MiniChunkModel (blockModels));
			}
		}

		/// <summary>
		/// 地形の表面部分にあるブロックを生成する。
		/// </summary>
		BlockModels CreateTerrainSurfaceBlockModels ()
		{
			BlockModels blockModels = new BlockModels (this.blockModels.xLength, 1, this.blockModels.zLength);

			for (int x = 0; x < blockModels.xLength; x++)
			{
				for (int z = 0; z < blockModels.zLength; z++)
				{
					Vector3 blockModelCenterPoint = BlockModelCenterPointFromBlockModelLocalPoint (new Point3D<int> (x, 0, z));
					float worldY = maxHeight * Mathf.PerlinNoise (blockModelCenterPoint.x, blockModelCenterPoint.z);

					blockModelCenterPoint = new Vector3 (blockModelCenterPoint.x, NormalizeWorldY (worldY), blockModelCenterPoint.z);
					bool boxDensity = true;

					blockModels [x, 0, z] = new BlockModel (blockModelCenterPoint, boxDensity, new Point3D<int> (x, 0, z));
				}
			}

			return blockModels;
		}

		Vector3 BlockModelCenterPointFromBlockModelLocalPoint (Point3D<int> point)
		{
			float worldX = basePoint.x + BlockModel.size.x * point.x + BlockModel.size.x / 2;
			float worldZ = basePoint.z + BlockModel.size.z * point.z + BlockModel.size.z / 2;
			float worldY = basePoint.y + BlockModel.size.y * point.y + BlockModel.size.y / 2;

			return new Vector3 (worldX, worldY, worldZ);
		}

		float NormalizeWorldY (float y)
		{
			return Mathf.Floor (y);
		}
			
		/// <summary>
		/// blockModelが何もないブロックと隣り合っているかを判定する。
		/// </summary>
		bool InBorder (BlockModel blockModel)
		{
			bool inBorder = false;

			for (int moveX = -1; moveX <= 1; moveX++) {
				for (int moveY = -1; moveY <= 1; moveY++) {
					for (int moveZ = -1; moveZ <= 1; moveZ++) {

						int movingDistance = Mathf.Abs (moveX) + Mathf.Abs (moveY) + Mathf.Abs (moveZ);

						if (1 != movingDistance)
						{
							continue;
						}

						int moveIndexX = blockModel.localIndex.x + moveX;
						int moveIndexY = blockModel.localIndex.y + moveY;
						int moveIndexZ = blockModel.localIndex.z + moveZ;

						if (!blockModels.existsIndex(moveIndexX, moveIndexY, moveIndexZ)) {
							inBorder = true;
						} else if (!blockModels [moveIndexX, moveIndexY, moveIndexZ].density) {
							inBorder = true;
						}
				
					}
				}
			}

			return inBorder;
		}
	}
}

