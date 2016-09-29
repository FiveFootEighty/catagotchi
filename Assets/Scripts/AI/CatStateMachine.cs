using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * TODO: Make an interface from this with more general Events and terms for use with other AI in the environment 
 */
public class CatStateMachine {
	// fields
	private CatAI cat;
	private MentalState mentalState;
	private ActiveState currentActiveState;
	private CatPhysicalState physicalState;

	private AIEvent lastTickEvent;

	//private Dictionary<CatEvent, class> eventState;

	/*
	 * Explore the idea of making currentEvents a combination set / array of Linked Lists 
	 * (or some other weird data structure) where each index indicates duration (in seconds) 
	 * so that I can give events a lifespan (that can be renewed)
	 */
	private Dictionary<AIEvent, int> currentEvents;
	private HashSet<AIEvent> currentTickEvents; 

	private float lastTick = 0.0f;
	private float lastHungerTick = 0.0f;
	private float lastPainTick = 0.0f;

	public CatStateMachine(CatAI cat, ActiveState initialActiveState) {
		this.cat = cat;
		this.currentActiveState = initialActiveState;
		this.physicalState = new CatPhysicalState();
		this.mentalState = new MentalState();
		this.currentTickEvents = new HashSet<AIEvent>();
		this.lastTickEvent = new AIEvent(CatEvent.NONE, 0);
	}

	public CatStateMachine(CatAI cat) {
		this.cat = cat;
		this.currentActiveState = new RestingState(this.cat);
		this.physicalState = new CatPhysicalState();
		this.mentalState = new MentalState();
		this.currentTickEvents = new HashSet<AIEvent>();
		this.lastTickEvent = new AIEvent(CatEvent.NONE, 0);
	}

	/**
	 * Update the ActiveState of the Cat, this is essentially the top level state machine 
	 * that determines what a Cat is physically doing (what animation is running and
	 * where they are moving too if that is relevant)
	 */
	private void updateActiveState() {

		AIEvent topEvent = this.determineMostPressingEvent();

		if(!topEvent.Equals(lastTickEvent)) {
			// react to the new more pressing event
			switch((CatEvent)topEvent.eventType) {

			case CatEvent.HUNGRY:
				this.currentActiveState = new HuntingState(this.cat);
				break;
			case CatEvent.PET:
				this.currentActiveState = new CuddlingState(this.cat);
				break;
			}

			this.lastTickEvent = topEvent;
		}
	}

	/** 
	 * Determine the most pressing event based on the cat's physical and mental states
	 */
	private AIEvent determineMostPressingEvent() {
		AIEvent topEvent = this.lastTickEvent;

		foreach(AIEvent e in currentTickEvents) {
			if(e.precedence > topEvent.precedence) {
				topEvent = e;
			}
		}

		return topEvent;
	}

	/**
	 * Update time dependent state variables
	 */
	private void updateTimeDependentVariables(float duration) {

		// update hunger
		if(duration >= this.lastHungerTick + HUNGER_TICK_FREQUENCY) {
			this.physicalState.increaseHunger(HUNGER_TICK_AMPLITUDE);

			if(this.physicalState.hunger > HUNGER_THRESHOLD) {
				this.mentalState.increaseAnger(HANGRY_TICK_AMPLITUDE);
				this.currentTickEvents.Add(new AIEvent(CatEvent.HUNGRY));
			}

			this.lastHungerTick = duration;
		}

		// update pain
		if(duration >= this.lastPainTick + PAIN_DOWNTICK_FREQUENCY) {
			this.physicalState.decreasePain(PAIN_DOWNTICK_AMPLITUDE);

			this.lastPainTick = duration;
		}

		// update events hash
		/*if(duration >= this.lastTick + SECOND_TICK) {

			foreach(AIEvent currentEvent in currentEvents.Keys) {
				currentEvents[currentEvent]--;

				if(currentEvents[currentEvent] == 0) {
					currentEvents.Remove(currentEvent);
				}
			}

			this.lastTick = duration;
		}*/
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

	public void addEvent(AIEvent e) {
		this.currentTickEvents.Add(e);
	}

	public ActiveState getActiveState() {
		return this.currentActiveState;
	}

	private const float SECOND_TICK = 1.0f;

	private const float HUNGER_TICK_FREQUENCY = 1.0f;
	private const uint HUNGER_TICK_AMPLITUDE = 10;
	private const uint HUNGER_THRESHOLD = 70;

	private const uint HANGRY_TICK_AMPLITUDE = 1;

	private const float PAIN_DOWNTICK_FREQUENCY = 5.0f;
	private const uint PAIN_DOWNTICK_AMPLITUDE = 10;
}
