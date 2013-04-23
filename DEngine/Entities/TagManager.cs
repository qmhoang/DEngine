using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DEngine.Entities {
	public sealed class TagManager<T> {
		private readonly Dictionary<T, Entity> _entityLUT;
		private readonly Dictionary<Entity, List<T>> _tags;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(_entityLUT != null);
			Contract.Invariant(_tags != null);
		}

		public TagManager() {
			_entityLUT = new Dictionary<T, Entity>();
			_tags = new Dictionary<Entity, List<T>>();
		}

		public void Register(Entity e, T tag) {
			Contract.Requires<ArgumentNullException>(e != null);

			if (_entityLUT.ContainsKey(tag))
				_entityLUT[tag] = e;
			else
				_entityLUT.Add(tag, e);

			if (!_tags.ContainsKey(e)) {
				_tags.Add(e, new List<T>());
			}
			Contract.Assume(_tags.ContainsKey(e));
			_tags[e].Add(tag);
		}

		public void Unregister(T tag) {
			if (_entityLUT.ContainsKey(tag)) {
				_tags[_entityLUT[tag]].Remove(tag);
				_entityLUT.Remove(tag);
			}
		}

		public bool IsRegistered(T tag) {
			return _entityLUT.ContainsKey(tag);
		}

		public Entity GetEntity(T tag) {
			return _entityLUT[tag];
		}

		public void Remove(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");

			if (_tags.ContainsKey(e)) {
				foreach (var tag in _tags[e]) {
					_entityLUT.Remove(tag);
				}
				_tags.Remove(e);
			}
		}

		public IEnumerable<T> GetTags(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return _tags[e];
		}

		public bool HasTags(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			return _tags.ContainsKey(e);
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
