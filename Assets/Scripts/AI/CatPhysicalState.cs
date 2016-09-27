using System;

public class CatPhysicalState {

	public uint hunger;
	public uint pain;
	public uint stamina;
	public uint energy;

	public CatPhysicalState() {
		this.hunger = 0;
		this.pain = 0;
		this.stamina = 100;
		this.energy = 0;
	}

	public void increaseHunger(uint increment) {
		if(this.hunger < 100) {
			this.hunger += increment;
		}
	}

	public void decreaseHunger(uint decrement) {
		if(this.hunger > 0) {
			this.hunger -= decrement;
		}
	}

	public void increasePain(uint increment) {
		if(this.pain < 100) {
			this.pain += increment;
		}
	}

	public void decreasePain(uint decrement) {
		if(this.pain > 0) {
			this.pain -= decrement;
		}
	}
}