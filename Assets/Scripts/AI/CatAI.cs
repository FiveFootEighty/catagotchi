using UnityEngine;
using System.Collections;

/**
 * CatAI is the Cat Prefabs interface to Unity. It will mimic cat senses through trigger behavior
 * and onUpdate ray casting in order to generate the Cat's perception of the environment. All events
 * are passed to the CatStateMachine which will determine what Active State the Cat should be in.
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
public class CatAI : MonoBehaviour {

	private CatStateMachine stateMachine;

	// Use this for initialization
	void Start () {
		this.stateMachine = new CatStateMachine(this, new RoamingState(this));
	}

	void Update() {
		this.stateMachine.getActiveState().Update();

		// should be the last thing to happen
		this.stateMachine.update(Time.time);
	}

	/*
	 * Trigger and Collider functions to mimic Cat senses. These functions
	 * will generate CatEvents based on environmental cues like being pet or hearing a sound
	 */

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			this.stateMachine.addEvent(new AIEvent(CatEvent.PET, 8));
		}
	}
}