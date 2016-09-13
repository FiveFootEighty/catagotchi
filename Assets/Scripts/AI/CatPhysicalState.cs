using System;

public class CatPhysicalState {

	public float hunger;
	public float pain;

	public CatPhysicalState() {
		this.hunger = 0.0f;
		this.pain = 0.0f;
	}

	public void increaseHunger(float increment) {
		if(this.hunger < 1.0f) {
			this.hunger += increment;
		}
	}

	public void decreaseHunger(float decrement) {
		if(this.hunger > 0.0f) {
			this.hunger -= decrement;
		}
	}

	public void increasePain(float increment) {
		if(this.pain < 1.0f) {
			this.pain += increment;
		}
	}

	public void decreasePain(float decrement) {
		if(this.pain > 0.0f) {
			this.pain -= decrement;
		}
	}
}