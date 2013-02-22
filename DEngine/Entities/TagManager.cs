using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Entities {
	public sealed class TagManager<T> {
		private readonly Dictionary<T, Entity> entityLUT;
		private readonly Dictionary<Entity, List<T>> tags;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(entityLUT != null);
			Contract.Invariant(tags != null);
		}

		public TagManager() {
			entityLUT = new Dictionary<T, Entity>();
			tags = new Dictionary<Entity, List<T>>();
		}

		public void Register(Entity e, T tag) {
			Contract.Requires<ArgumentNullException>(e != null);

			if (entityLUT.ContainsKey(tag))
				entityLUT[tag] = e;
			else
				entityLUT.Add(tag, e);

			if (!tags.ContainsKey(e)) {
				tags.Add(e, new List<T>());
			}
			Contract.Assume(tags.ContainsKey(e));
			tags[e].Add(tag);
		}

		public void Unregister(T tag) {
			if (entityLUT.ContainsKey(tag)) {
				tags[entityLUT[tag]].Remove(tag);
				entityLUT.Remove(tag);
			}
		}

		public bool IsRegistered(T tag) {
			return entityLUT.ContainsKey(tag);
		}

		public Entity GetEntity(T tag) {
			return entityLUT[tag];
		}

		public void Remove(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");

			if (tags.ContainsKey(e)) {
				foreach (var tag in tags[e]) {
					entityLUT.Remove(tag);
				}
				tags.Remove(e);
			}
		}

		public IEnumerable<T> GetTags(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return tags[e];
		}

		public bool HasTags(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return tags.ContainsKey(e);
		}

		public Entity this[T tag] {
			get {
				return GetEntity(tag);
			}
			set {
				Contract.Requires<ArgumentNullException>(value != null, "value");
				Register(value, tag);
			}
		}
	}

}
