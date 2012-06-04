using System;

namespace DEngine.Actor {
    public class RefId : IComparable<RefId>, IEquatable<RefId> {
        /// <summary>
        /// Reference Id for object creation
        /// </summary>
        public string Id { get; set; }

        public int CompareTo(RefId other) {            
            return Id.CompareTo(other.Id);
        }

        public bool Equals(RefId other) {
            return Id.Equals(other.Id);
        }
    }
}
