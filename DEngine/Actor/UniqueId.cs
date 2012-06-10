﻿using System;

namespace DEngine.Actor {
    public class RefId : IEquatable<RefId> {
        private readonly string id;

        public RefId(string refId) {
            id = refId;
        }

        public RefId(RefId copy) {
            id = copy.id;
        }

        public bool Equals(RefId other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.id == id;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(RefId))
                return false;
            return Equals((RefId)obj);
        }

        public override int GetHashCode() {
            return id.GetHashCode();
        }
    }

    /// <summary>
    /// Global unique id for every game element in the universe
    /// </summary>
    public class UniqueId : IEquatable<UniqueId> {
        private static int nextId = 0;

        private readonly int id;

        public UniqueId() {
            id = nextId++;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(UniqueId))
                return false;
            return Equals((UniqueId)obj);
        }

        public override int GetHashCode() {
            return id;
        }

        public override string ToString() {
            return string.Format("Id: {0}", id);
        }

        public bool Equals(UniqueId other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.id == id;
        }
    }
}