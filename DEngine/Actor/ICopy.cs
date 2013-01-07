namespace DEngine.Actor {
	/// <summary>
	/// Make a hard copy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICopy<T> {
		T Copy();
	}
}