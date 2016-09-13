using System.Collections;
using System.Collections.Generic;

/**
 * TODO: Make an interface from this with more general Events and terms for use with other AI in the environment 
 */
public class CatAI {
	// fields
	private MentalState mentalState;
	private CatActiveState currentActiveState;
	private CatPhysicalState physicalState;

	private CatEvent lastTickEvent = CatEvent.NONE;
	private HashSet<CatEvent> currentTickEvents;

	private float lastTick = 0.0f;

	public CatAI(CatActiveState initialActiveState) {
		this.currentActiveState = initialActiveState;
		this.physicalState = new CatPhysicalState();
		this.mentalState = new MentalState();
		this.currentTickEvents = new HashSet<CatEvent>();
	}

	/**
	 * Update the ActiveState of the Cat, this is essentially the top level state machine 
	 * that determines what a Cat is physically doing (what animation is running and
	 * where they are moving too if that is relevant)
	 */
	private void updateActiveState() {

		CatEvent topEvent = this.determineMostPressingEvent();

		if(!lastTickEvent.Equals(topEvent)) {
			// react to the new more pressing event
			switch(topEvent) {

			case CatEvent.DISTRACTED_BY_OBJECT:
				this.currentActiveState = CatActiveState.HUNTING;
				break;
			case CatEvent.PET:
				this.currentActiveState = CatActiveState.CUDDLING;
				break;
			case CatEvent.BORED:
				this.currentActiveState = CatActiveState.RESTING;
				break;
			}

			this.lastTickEvent = topEvent;
		}
	}

	/** 
	 * Determine the most pressing event based on the cat's physical and mental states
	 */
	private CatEvent determineMostPressingEvent() {
		CatEvent topEvent = this.lastTickEvent;
		foreach(CatEvent e in currentTickEvents) {
			topEvent = e;
		}

		return topEvent;
	}

	/**
	 * 
	 * Update time dependent state variables
	 */
	private void updateTimeDependentVariables(float duration) {

		// update hunger
		if(duration >= this.lastTick + 60.0f) {
			this.physicalState.increaseHunger(0.01f);

			if(this.physicalState.hunger > HUNGER_THRESHOLD) {
				this.mentalState.increaseAnger(0.01f);
				this.currentTickEvents.Add(CatEvent.HUNGRY);
			}
		}

		// update pain
		if(duration >= this.lastTick + 5.0f) {
			this.physicalState.decreasePain(0.1f);
		}

		this.lastTick = duration;
	}

	/**
	 * Update function that can be called at whatever time frequency makes sense for 
	 * the game/experience this library is being called from (i.e. could be every time
	 * Update() is called in Unity)
	 */
	public void update(float gameDuration) {
		this.updateTimeDependentVariables(gameDuration);
		this.updateActiveState();

		// empty the events set to prepare for the next round of events
		this.currentTickEvents.Clear();
	}

	public void addEvent(CatEvent e) {
		this.currentTickEvents.Add(e);
	}

	public CatActiveState getCurrentActiveState() {
		return this.currentActiveState;
	}

	private const float HUNGER_THRESHOLD = 7.0f;
}
