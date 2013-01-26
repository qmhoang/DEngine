using DEngine.Entity;

namespace DEngine.Components {
	public class Actionable : EntityComponent {
		/// <summary>
		/// How much action points an entity has
		/// </summary>
		public int ActionPoints { get; set; }

		/// <summary>
		/// The rate in which an entity's AP is changed.
		/// </summary>
		/// <returns></returns>
		public int Speed { get; set; }

		/// <summary>
		/// Can entity call update right now?
		/// </summary>
		public bool Updateable { get; set; }

		public const int DEFAULT_SPEED = 100;

		public Actionable(int actionPoints = 0, int speed = DEFAULT_SPEED, bool updateable = false) {
			ActionPoints = actionPoints;
			Speed = speed;
			Updateable = updateable;
		}
	}
}