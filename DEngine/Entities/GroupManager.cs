using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DEngine.Actor;

namespace DEngine.Entities {
	/// <summary>
	///  If you need to group your entities together, e.g. tanks going into "units" group or explosions into "effects",
	/// then use this manager. You must retrieve it using world instance.
	/// 
	/// A entity can only belong to one group at a time.
	/// </summary>
	public sealed class GroupManager<T> where T : IEquatable<T> {
		private Dictionary<UniqueId, T> IdToGroupLUT;
		private Dictionary<T, HashSet<Entity>> entitiesByGroup;

		public GroupManager() {
			IdToGroupLUT = new Dictionary<UniqueId, T>();
			entitiesByGroup = new Dictionary<T, HashSet<Entity>>();
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(entitiesByGroup != null);
			Contract.Invariant(IdToGroupLUT != null);			
		}

		/// <summary>
		/// Set the entity's group, resets any previous entity's group
		/// </summary>
		/// <param name="e"></param>
		/// <param name="group"></param>
		public void Set(Entity e, T @group) {
			Contract.Requires<ArgumentNullException>(e != null);
		
			Remove(e);

			if (!entitiesByGroup.ContainsKey(group)) {
				entitiesByGroup.Add(group, new HashSet<Entity>());
			}
			Contract.Assume(entitiesByGroup.ContainsKey(group));
			entitiesByGroup[group].Add(e);
			IdToGroupLUT.Add(e.Id, group);
		}

		/// <summary>
		/// Get all entities belonging to a group
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
		public IEnumerable<Entity> GetEntities(T group) {
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

			if (IdToGroupLUT.ContainsKey(e.Id)) {
				T group = IdToGroupLUT[e.Id];

				entitiesByGroup.Remove(group);				
				IdToGroupLUT.Remove(e.Id);				
			}
		}

		/// <summary>
		/// the name of the group that this entity belongs to.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public T GetGroupOf(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			
//			if (IdToGroupLUT.ContainsKey(e.Id))
				return IdToGroupLUT[e.Id];			
		}

		/// <summary>
		/// Checks if the entity belongs to any group.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>true if it is in any group, false if none.</returns>
		public bool IsGrouped(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");

			return IdToGroupLUT.ContainsKey(e.Id);			
		}

		public bool IsValidGroup(T group) {
			return entitiesByGroup.ContainsKey(group);
		}
	}
}
