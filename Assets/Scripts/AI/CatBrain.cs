using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * TODO: Make an interface from this with more general Events and terms for use with other AI in the environment
 * 
 * TODO: Note: Hive brain?
 */
public class CatBrain {
	// fields
	private CatAI cat;
	private MentalState mentalState;
	private ActiveState currentActiveState;
	private CatPhysicalState physicalState;

	private EventQueue eventQueue;
	private const float EVENT_UPDATE_INTERVAL = 1.0f;

	private float lastTick = 0.0f;
	private float lastHungerTick = 0.0f;
	private float lastPainTick = 0.0f;
	private float lastEventTick = 0.0f;

	public CatBrain(CatAI cat, ActiveState initialActiveState) {
		this.cat = cat;
		this.currentActiveState = initialActiveState;
		this.physicalState = new CatPhysicalState();
		this.mentalState = new MentalState();
		this.eventQueue = new EventQueue(EVENT_UPDATE_INTERVAL);
	}

	public CatBrain(CatAI cat)
	{
		this.cat = cat;
		this.currentActiveState = new RestingState(this.cat);
		this.physicalState = new CatPhysicalState();
		this.mentalState = new MentalState();
		this.eventQueue = new EventQueue(EVENT_UPDATE_INTERVAL);
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
				this.eventQueue.enqueue(new AIEvent(CatEvent.HUNGRY, 4));
			}

			this.lastHungerTick = duration;
		}

		// update pain
		if(duration >= this.lastPainTick + PAIN_DOWNTICK_FREQUENCY) {
			this.physicalState.decreasePain(PAIN_DOWNTICK_AMPLITUDE);

			this.lastPainTick = duration;
		}

		// update Event Queue
		if (duration >= this.lastEventTick + EVENT_TICK_FREQUENCY) {
			this.eventQueue.update();

			this.lastEventTick = duration;
		}

	}

	/**
	 * Update function that can be called at whatever time frequency makes sense for 
	 * the game/experience this library is being called from (i.e. could be every time
	 * Update() is called in Unity)
	 */
	public void update(float gameDuration) {
		this.updateTimeDependentVariables(gameDuration);
	}

	public void addEvent(AIEvent e) {
		this.eventQueue.enqueue(e);
	}

	public ActiveState getActiveState() {
		return this.currentActiveState;
	}
	public void setActiveState(ActiveState newState) {
		this.currentActiveState = newState;
	}

	public AIEvent getPriority() {
		return this.eventQueue.priority();
	}

	private const float SECOND_TICK = 1.0f;

	private const float HUNGER_TICK_FREQUENCY = 1.0f;
	private const uint HUNGER_TICK_AMPLITUDE = 10;
	private const uint HUNGER_THRESHOLD = 70;

	private const uint HANGRY_TICK_AMPLITUDE = 1;

	private const float PAIN_DOWNTICK_FREQUENCY = 5.0f;
	private const uint PAIN_DOWNTICK_AMPLITUDE = 10;

	private const float EVENT_TICK_FREQUENCY = 1.0f;
}
