using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using DEngine.Entities;

namespace DEngine.Components {
	public class ActionPoint : Component {
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
		public bool Updateable { get { return ActionPoints > 0; } }

		public const int DEFAULT_SPEED = 100;

		public ActionPoint(int actionPoints = 0, int speed = DEFAULT_SPEED) {
			ActionPoints = actionPoints;
			Speed = speed;			
		}

		public override Component Copy() {
			return new ActionPoint(ActionPoints, Speed);
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(Speed > 0);			
		}
	}

}