using UnityEngine;
using System.Collections;


public class CuddlingState : ActiveState {

	public CuddlingState(CatAI cat) : base(cat) {
		Debug.Log("Cuddling!");
	}

	public override void Update() {
		//Debug.Log("Cuddle tick");
	}
}

