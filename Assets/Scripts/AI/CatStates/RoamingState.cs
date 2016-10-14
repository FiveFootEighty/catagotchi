using UnityEngine;
using System.Collections;


public class RoamingState : ActiveState {

	public RoamingState(CatAI cat) : base(cat) {
		Debug.Log("Just roaming");

	}

}