using UnityEngine;
using System.Collections;


public class HuntingState : ActiveState {

	public HuntingState(CatAI cat) : base(cat) {
		Debug.Log("Hunting for food!");

		// setup initial animations/navigation for hunting
	}

	public override void Update() {
		
	}
}

