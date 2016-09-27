using UnityEngine;
using System.Collections;

/**
 * CatController is the Cat Prefabs interface to Unity. I'm going to try to keep all 'AI'
 * decisions abstract from Unity in the CatAI class.
 * 
 * Flow: 1. Things are happening (i.e. user approaches cat, pets cat, cat sees a toy, hears a sound, etc)
 *       2. CatEvent is generated for any thing that the Cat is aware of happening and
 * 			passed to CatAI
 * 		 3. Each tick, CatAI is deciding which CatEvent is at the top of the list (most prominent) 
 * 			and manages the State the Cat is in
 * 		 4. on Update() CatController is asking CatAI what Active State the Cat is in
 * 	     5. CatController updates what the Cat is "doing" based on Active State
 * 		 
 * 
 */
public class CatController : MonoBehaviour {

	private CatAI brain;
	private ActiveState activeState;
	private float lastTick = 0.0f;

	// Use this for initialization
	void Start () {
		this.brain = new CatAI(new RestingState());
		this.activeState = this.brain.getActiveState();
	}

	void Update() {
		this.brain.getActiveState().Update();

		// should be the last thing to happen
		this.brain.update(Time.time);
	}

	/*
	 * Trigger and Collider functions to mimic Cat senses. These functions
	 * will generate CatEvents based on environmental cues like being pet or hearing a sound
	 */

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			brain.addEvent(new AIEvent(CatEvent.PET, 8));
		}
	}
}