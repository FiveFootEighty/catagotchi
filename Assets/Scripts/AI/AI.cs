using System;
using System.Collections.Generic;

public abstract class AI {

	// internal methods
	protected abstract void updateActiveState();
	protected abstract void determineMostPressingEvent();
	protected abstract void updateTimeDependentVariables(float duration);


	// public interface
	public abstract void update(float duration);
	//public void addEvent(AIEvent e);
}


