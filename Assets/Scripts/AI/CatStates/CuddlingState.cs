using UnityEngine;
using System.Collections;


public class CuddlingState : ActiveState {

	public CuddlingState() {
		Debug.Log("Cuddling!");
	}

	public void Update() {
		Debug.Log("Cuddle tick");
	}
}

