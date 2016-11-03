using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called once per frame
	// guaranteed to run after all items have been processed in Update
	// so when we set the camera's position we know absolutely that the player has moved for that frame
	//
	// better to use for:
	// 1. Follow cameras
	// 2. Procedural animation
	// 3. gathering last known states
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
