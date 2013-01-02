using DEngine.Entity;

namespace DEngine.Components {
	public class Sprite : EntityComponent {
		public string Asset { get; set; }		
		public int Order { get; set; }

		public Sprite(string asset, int order = 0) {
			Asset = asset;			
			Order = order;
		}
	}
}