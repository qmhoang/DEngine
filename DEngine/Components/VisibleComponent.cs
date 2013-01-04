using System;
using System.Collections.Generic;
using DEngine.Actor;
using DEngine.Core;
using DEngine.Entity;

namespace DEngine.Components {
	public class VisibleComponent : EntityComponent {
		//todo visibility difficulty

		/// <summary>
		/// How difficulty is it to see the item, -1 means its impossible to see
		/// </summary>
		public int VisibilityIndex { get; set; }
		public int DefaultIndex { get; set; }

		public void Reset() {
			VisibilityIndex = DefaultIndex;
		}

		public VisibleComponent(int defaultIndex) {
			VisibilityIndex = DefaultIndex = defaultIndex;
		}

		public VisibleComponent() {}
	}
}