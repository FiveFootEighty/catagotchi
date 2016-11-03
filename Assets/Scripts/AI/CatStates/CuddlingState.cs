using UnityEngine;
using System.Collections;


public class CuddlingState : ActiveState {

	private Animator animator;
	private float lastPet;

	public CuddlingState(CatAI cat) : base(cat) {
		Debug.Log("Cuddling!");
		animator = cat.GetComponent<Animator>();

		animator.SetBool("PET", true);
		lastPet = Time.time;
	}
		
	public void Update() {
		base.Update();

		if((Time.time - lastPet) > 5.0f) {
			animator.SetBool("PET", false);
			cat.getBrain().addEvent(new AIEvent(CatEvent.FINISHED, 1));
		}
	}
}

