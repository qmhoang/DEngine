using System;
using System.Diagnostics.Contracts;
using DEngine.Entities;

namespace DEngine.Components {
	public class Identifier : Component {
		public string Name { get; set; }
		public string Description { get; set; }

		public Identifier(string name) {
			Name = name;
			Description = name;
		}

		public Identifier(string name, string description) {
			Name = name;
			Description = description;
		}
		
		public override Component Copy() {
			return new Identifier(Name, Description);
		}

		public static string GetNameOrId(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return e.Has<Identifier>() ? e.Get<Identifier>().Name : e.Id.ToString();
		}

		public override string ToString() {
			return String.Format("Name: {0}, Description: {1}", Name, Description);
		}
	}
}