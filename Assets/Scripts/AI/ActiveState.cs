
public class ActiveState {

	protected CatAI cat;

	public ActiveState(CatAI cat) {
		this.cat = cat;
	}

	/**
	 * General behavior
	 */
	public void Update() {
		// doing nothing at the moment
	}

	protected void changeStates(ActiveState newState) {
		cat.getBrain().setActiveState(newState);
	}
}