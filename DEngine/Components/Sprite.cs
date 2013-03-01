using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using DEngine.Entities;

namespace DEngine.Components {
	public sealed class Sprite : Component {
		public const int TerrainLayer = 0;
		public const int FeaturesLayer = 10;
		public const int ItemsLayer = 20;
		public const int ActorLayer = 30;
		public const int PlayerLayer = 40;

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