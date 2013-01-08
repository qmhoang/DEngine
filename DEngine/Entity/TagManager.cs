using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Entity {
	public class TagManager {
		private Dictionary<string, Entity> entityLUT;
		private Dictionary<Entity, List<string>> tags;

		public TagManager() {
			entityLUT = new Dictionary<string, Entity>();
			tags = new Dictionary<Entity, List<string>>();
		}

		public void Register(String tag, Entity e) {
			entityLUT.Add(tag, e);
			if (!tags.ContainsKey(e)) {
				tags.Add(e, new List<string>());
			}
			tags[e].Add(tag);
		}

		public void Unregister(String tag) {
			if (!entityLUT.ContainsKey(tag))
				return;

			tags[entityLUT[tag]].Remove(tag);
			entityLUT.Remove(tag);
		}

		public bool IsRegistered(String tag) {
			return entityLUT.ContainsKey(tag);
		}

		public Entity GetEntity(String tag) {
			return entityLUT[tag];
		}

		public void Remove(Entity e) {
			foreach (var tag in tags[e]) {
				entityLUT.Remove(tag);
			}
			tags.Remove(e);			
		}

		public IEnumerable<string> GetTags(Entity e) {
			return tags[e];
		}

		public Entity this[string tag] {
			get { return GetEntity(tag); }
			set {
				Register(tag, value);
			}
		}
	}
}
