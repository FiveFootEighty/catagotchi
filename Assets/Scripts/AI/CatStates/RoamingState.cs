using UnityEngine;
using System.Collections;


public class RoamingState : ActiveState {

	public RoamingState(CatAI cat) : base(cat) {
		Debug.Log("Just roaming");
	}

	public void Update() {
		base.Update();

		if(this.cat.getBrain().getPriority().eventType.Equals(CatEvent.PET)) {
			this.changeStates(new CuddlingState(this.cat));
		}
	}

}