using System;

namespace DEngine.Actor {
	/// <summary>
	/// Global unique id for every game element in the universe
	/// </summary>
	public class UniqueId : IEquatable<UniqueId>, IComparable<UniqueId> {
		private static ulong _nextId = 0;

		private readonly ulong _id;

		public UniqueId() {
			_id = _nextId++;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (UniqueId))
				return false;
			return Equals((UniqueId) obj);
		}

		public override int GetHashCode() {
			return _id.GetHashCode();
		}

		public int CompareTo(UniqueId other) {
			return _id.CompareTo(other._id);
		}

		public override string ToString() {
			return string.Format("Id: {0}", _id);
		}

		public bool Equals(UniqueId other) {
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return other._id == _id;
		}
	}
}