using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed; // can be changed in the editor because it's public

	private Rigidbody rb;

	// called on first frame that the script is active (often the first frame of the game)
	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	// called before rendering a frame
	void Update() {}

	// called before performing any physics calculations
	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.AddForce(movement * speed);
	}
}