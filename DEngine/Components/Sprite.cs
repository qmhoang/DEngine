using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
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
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(asset));
			Asset = asset;			
			ZOrder = zorder;
		}

		public override Component Copy() {
			return new Sprite(Asset, ZOrder);
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(!string.IsNullOrEmpty(Asset));			
		}
	}
}