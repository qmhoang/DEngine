using DEngine.Entities;

namespace DEngine.Components {
	public class Sprite : Component {
		public const int TERRAN_LAYER = 0;
		public const int FEATURES_LAYER = 10;
		public const int ITEMS_LAYER = 20;
		public const int ACTOR_LAYER = 30;
		public const int PLAYER_LAYER = 40;

		public string Asset { get; set; }		
		public int ZOrder { get; set; }

		public Sprite(string asset, int zorder) {
			Asset = asset;			
			ZOrder = zorder;
		}

		public override Component Copy() {
			return new Sprite(Asset, ZOrder);
		}
	}
}