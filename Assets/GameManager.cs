using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject terrain = new GameObject ("Terrain");
		terrain.AddComponent<Terrain>();
		terrain.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
