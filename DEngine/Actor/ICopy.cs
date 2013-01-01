namespace DEngine.Actor {
	/// <summary>
	/// Make a hard copy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICopy<out T> {
		T Copy();
	}
}