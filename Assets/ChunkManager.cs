using UnityEngine;
using System.Collections;

namespace VoxelEngine
{
	public class ChunkManager : MonoBehaviour {

		// Use this for initialization
		void Start () {
			ChunkModelStore store = new ChunkModelStore ();
			store.PrepareForBasePoint (new Vector3 (0, 0, 0));
		}
	
		// Update is called once per frame
		void Update () {
	
		}
	}
}
