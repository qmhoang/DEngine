using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Actor;

namespace DEngine.Entity {
	/// <summary>
	///  If you need to group your entities together, e.g. tanks going into "units" group or explosions into "effects",
	/// then use this manager. You must retrieve it using world instance.
	/// 
	/// A entity can only belong to one group at a time.
	/// </summary>
	public class GroupManager {
		private Dictionary<UniqueId, string> entityIDToEntityLUT;
		private Dictionary<string, HashSet<Entity>> entitiesByGroup;

		public GroupManager() {
			entityIDToEntityLUT = new Dictionary<UniqueId, string>();
			entitiesByGroup = new Dictionary<string, HashSet<Entity>>();
		}

		/// <summary>
		/// Set the entity's group, resets any previous entity's group
		/// </summary>
		/// <param name="group"></param>
		/// <param name="e"></param>
		public void Set(string group, Entity e) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(group));			
			Contract.Requires<ArgumentNullException>(e != null);
		
			Remove(e);

			if (!entitiesByGroup.ContainsKey(group)) {
				entitiesByGroup.Add(group, new HashSet<Entity>());
			}
			entitiesByGroup[group].Add(e);
			entityIDToEntityLUT.Add(e.Id, group);
		}

		/// <summary>
		/// Get all entities belonging to a group
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
		public IEnumerable<Entity> GetEntities(string group) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(group));
			
			if (entitiesByGroup.ContainsKey(group))
				return entitiesByGroup[group];
			else
				return new List<Entity>();
		}

		/// <summary>
		/// Removes the provided entity from the group it is assigned to, if any.
		/// </summary>
		/// <param name="e"></param>
		public void Remove(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			if (entityIDToEntityLUT.ContainsKey(e.Id)) {
				string group = entityIDToEntityLUT[e.Id];

				entitiesByGroup.Remove(group);				
				entityIDToEntityLUT.Remove(e.Id);				
			}
		}

		/// <summary>
		/// the name of the group that this entity belongs to, null if none.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public string GetGroupOf(Entity e) {
			if (entityIDToEntityLUT.ContainsKey(e.Id))
				return entityIDToEntityLUT[e.Id];
			return null;
		}

		/// <summary>
		/// Checks if the entity belongs to any group.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>true if it is in any group, false if none.</returns>
		public bool IsGrouped(Entity e) {
			return GetGroupOf(e) != null;
		}
	}
}
