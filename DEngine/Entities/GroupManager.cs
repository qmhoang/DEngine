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
		private readonly Dictionary<UniqueId, T> _tagById;
		private readonly Dictionary<T, HashSet<Entity>> _entitiesByGroup;

		public GroupManager() {
			_tagById = new Dictionary<UniqueId, T>();
			_entitiesByGroup = new Dictionary<T, HashSet<Entity>>();
		}

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(_entitiesByGroup != null);
			Contract.Invariant(_tagById != null);			
		}

		/// <summary>
		/// Set the entity's group, resets any previous entity's group
		/// </summary>
		/// <param name="group"></param>
		/// <param name="e"></param>
		public void Set(T @group, Entity e) {
			Contract.Requires<ArgumentNullException>(e != null);
		
			Remove(e);

			if (!_entitiesByGroup.ContainsKey(group)) {
				_entitiesByGroup.Add(group, new HashSet<Entity>());
			}
			Contract.Assume(_entitiesByGroup.ContainsKey(group));
			_entitiesByGroup[group].Add(e);
			_tagById.Add(e.Id, group);
		}

		/// <summary>
		/// Get all entities belonging to a group
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
		public IEnumerable<Entity> GetEntities(T group) {
			if (_entitiesByGroup.ContainsKey(group))
				return _entitiesByGroup[group];
			else
				return new List<Entity>();
		}

		/// <summary>
		/// Removes the provided entity from the group it is assigned to, if any.
		/// </summary>
		/// <param name="e"></param>
		public void Remove(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");

			if (_tagById.ContainsKey(e.Id)) {
				T group = _tagById[e.Id];

				_entitiesByGroup.Remove(group);				
				_tagById.Remove(e.Id);				
			}
		}

		/// <summary>
		/// Name of the group that this entity belongs to.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public T GetGroupOf(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");
			
//			if (IdToGroupLUT.ContainsKey(e.Id))
				return _tagById[e.Id];			
		}

		/// <summary>
		/// Checks if the entity belongs to any group.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>true if it is in any group, false if none.</returns>
		public bool IsGrouped(Entity e) {
			Contract.Requires<ArgumentNullException>(e != null, "e");

			return _tagById.ContainsKey(e.Id);			
		}

		/// <summary>
		/// Check if any entities belongs to the group
		/// </summary>
		/// <param name="group"></param>
		/// <returns>True if there are entities that is grouped into that group, false if no entities exist in group</returns>
		public bool IsValidGroup(T group) {
			return _entitiesByGroup.ContainsKey(group);
		}
	}
}
