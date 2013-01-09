using DEngine.Entity;

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
	}
}