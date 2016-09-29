using UnityEngine;
using System.Collections;


public class RestingState : ActiveState {

	public RestingState(CatAI cat) : base(cat) {
		Debug.Log("Resting and licking");
	}

	public override void Update() {

	}
}
