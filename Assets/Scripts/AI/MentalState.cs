using System;

public class MentalState {

	public float joy;
	public float sadness;
	public float anger;
	public float fear;
	public float disgust;

	enum emotions {
		HAPPY,
		CONTENT,
		HUNGRY,
		PLAYFUL,
		UPSET,
		FEARFUL,
		ANGRY,
		SAD,
		EXCITED
	}

	public MentalState() {
		this.joy = 0.5f;
		this.sadness = 0.5f;
		this.anger = 0.5f;
		this.fear = 0.5f;
		this.disgust = 0.5f;
	}

	public void increaseAnger(float increment) {
		if(this.anger < 1.0f) {
			this.anger += increment;
		}
	}

	public void decreaseAnger(float decrement) {
		if(this.anger > 0.0f) {
			this.anger -= decrement;
		}
	}

}