using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {

	private CatAI brain;
	private CatActiveState activeState;
	private float lastTick = 0.0f;

	// Use this for initialization
	void Start () {
		brain = new CatAI(CatActiveState.RESTING);
		this.activeState = brain.getCurrentActiveState();
	}

	void Update() {
		print("activeState: " + activeState);
		// need to ask brain what Active state we are in and react if it's changed
		if(!this.activeState.Equals(this.brain.getCurrentActiveState())) {
			// activeState has changed, do something

			if(this.activeState.Equals(CatActiveState.CUDDLING)) {
				// cuddling animation (rotate square)
				print("I'm cuddling!");
			}
		}

		// should be the last thing to happen
		if(Time.time > lastTick + 2.0f) {
			print("updating brain");
			brain.update(Time.time);
			lastTick = Time.time;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			brain.addEvent(CatEvent.PET);
		}
	}
}
