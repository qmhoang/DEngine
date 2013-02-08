using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Actor {
	public class AP {
		/// <summary>
		/// How much action points an entity has
		/// </summary>
		public int ActionPoints { get; set; }

		/// <summary>
		/// The rate in which an entity's AP is changed.
		/// </summary>
		/// <returns></returns>
		public int ActionPointPerTurn { get; set; }

		public bool Updateable { get { return ActionPoints > 0; } }

		public const int DEFAULT_SPEED = 100;

		public void Gain() {
			ActionPoints += ActionPointPerTurn;
		}

		public AP(int speed = DEFAULT_SPEED, int actionPoints = 0) {
			Contract.Requires<ArgumentException>(speed > 0);
			ActionPoints = actionPoints;
			ActionPointPerTurn = speed;			
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(ActionPointPerTurn > 0);
		}
	}
}
