using UnityEngine;
using System.Collections;
using VoxelEngine;

public class Terrain : MonoBehaviour
{
	ChunkManager chunkManager;

	// Use this for initialization
	void Start ()
	{
		gameObject.AddComponent<ChunkManager> ();
		chunkManager = gameObject.GetComponent<ChunkManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

