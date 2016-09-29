
public abstract class ActiveState {

	protected CatAI cat;

	public ActiveState(CatAI cat) {
		this.cat = cat;
	}

	public abstract void Update();
}