using System;
using UnityEngine;

public class AIEvent {

	public int precedence;
	public int lifeSpan = 1; // in seconds

	public Transform location;

	public Enum eventType;

	public AIEvent(Enum eventType) {
		this.eventType = eventType;
		this.precedence = 0;
		this.location = null;
	}

	public AIEvent(Enum eventType, int precedence) {
		this.eventType = eventType;
		this.precedence = precedence;
		this.location = null;
	}

	public AIEvent(Enum eventType, int precedence, Transform location) {
		this.eventType = eventType;
		this.precedence = precedence;
		this.location = location;
	}
}