using UnityEngine;
using System.Collections;


public class RestingState : ActiveState {

	private Animator animator;

	public RestingState(CatAI cat) : base(cat) {
		Debug.Log("Resting and licking");
		animator = cat.GetComponent<Animator>();
	}
}
