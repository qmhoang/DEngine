using System;

namespace DEngine.Actor {
    /// <summary>
    /// Global unique id for every game element in the universe
    /// </summary>
    public class UniqueId : IComparable<UniqueId>, IEquatable<UniqueId> {
        public long Id { get; set; }

        public int CompareTo(UniqueId other) {
            return Id.CompareTo(other.Id);
        }

        public bool Equals(UniqueId other) {
            return Id.Equals(other.Id);
        }
    }
}
