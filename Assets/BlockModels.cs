using UnityEngine;
using System;
using System.Collections;

namespace VoxelEngine
{
	public class BlockModels:IEnumerable
	{
		public readonly int xLength;
		public readonly int yLength;
		public readonly int zLength;
		BlockModel[,,] blockModels;

		public BlockModels (int xLength, int yLength, int zLength)
		{
			this.xLength = xLength;
			this.yLength = yLength;
			this.zLength = zLength;
			blockModels = new BlockModel[this.xLength, this.yLength, this.zLength];
		}

		public BlockModels blockModelsFromIndexYRange (int startIndex, int endIndex)
		{
			BlockModels blockModels = new BlockModels (this.xLength, endIndex - startIndex + 1, this.zLength);

			for (int y = startIndex; y <= endIndex; y++) {
				for (int x = 0; x < xLength; x++) {
					for (int z = 0; z < zLength; z++) {
						blockModels [x, y - startIndex, z] = this.blockModels [x, y, z];
					}
				}
			}

			return blockModels;
		}

		public BlockModel this[int x, int y, int z]
		{
			set { this.blockModels [x, y, z] = value; }
			get { return this.blockModels [x, y, z]; }
		}

		public Bounds Bounds
		{
			get 
			{
				float centerX = (blockModels [0, 0, 0].bounds.center.x + blockModels [xLength - 1, 0, 0].bounds.center.x) / 2;
				float centerY = (blockModels [0, 0, 0].bounds.center.y + blockModels [yLength - 1, 0, 0].bounds.center.y) / 2;
				float centerZ = (blockModels [0, 0, 0].bounds.center.z + blockModels [zLength - 1, 0, 0].bounds.center.z) / 2;
				Vector3 center = new Vector3 (centerX, centerY, centerZ);

				Vector3 blockSize = BlockModel.size;
				float sizeX = Mathf.Abs (blockModels [0, 0, 0].bounds.center.x - blockModels [xLength - 1, 0, 0].bounds.center.x) + blockSize.x;
				float sizeY = Mathf.Abs (blockModels [0, 0, 0].bounds.center.y - blockModels [yLength - 1, 0, 0].bounds.center.y) + blockSize.y;
				float sizeZ = Mathf.Abs (blockModels [0, 0, 0].bounds.center.z - blockModels [zLength - 1, 0, 0].bounds.center.z) + blockSize.z;
				Vector3 size = new Vector3 (sizeX, sizeY, sizeZ);

				return new Bounds (center, size);
			}
		}

		public bool existsIndex (int x, int y, int z)
		{
			if (0 > x || 0 > y || 0 > z) {
				return false;
			} else if (xLength - 1 < x || yLength - 1 < y || zLength - 1 < z) {
				return false;
			} 

			return true;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator ();
		}

		public IEnumerator GetEnumerator()
		{
			for (int x = 0; x < xLength; x++) {
				for (int y = 0; y < yLength; y++) {
					for (int z = 0; z < zLength; z++) {
						yield return blockModels[x, y , z];
					}
				}
			}
		}
	}
}

