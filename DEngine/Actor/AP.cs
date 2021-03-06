﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace DEngine.Actor {

	public class AP {

		public AP(int apPerTurn, int actionPoints = 0) {
			Contract.Requires<ArgumentException>(apPerTurn > 0);
			ActionPoints = actionPoints;
			ActionPointPerTurn = apPerTurn;
		}

		/// <summary>
		///     How much action points an entity has
		/// </summary>
		public int ActionPoints { get; set; }

		/// <summary>
		///     The rate in which an entity's AP is changed.
		/// </summary>
		/// <returns></returns>
		public int ActionPointPerTurn { get; set; }

		public bool Updateable {
			get { return ActionPoints > 0; }
		}

		public void Gain() {
			ActionPoints += ActionPointPerTurn;
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts."
			)]
		private void ObjectInvariant() {
			Contract.Invariant(ActionPointPerTurn > 0);
		}
	}
}