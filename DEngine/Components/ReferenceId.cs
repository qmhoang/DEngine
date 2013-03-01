using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using DEngine.Entities;

namespace DEngine.Components {
	public sealed class ReferenceId : Component, IEquatable<ReferenceId> {
		public string RefId { get; private set; }

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant() {
			Contract.Invariant(!String.IsNullOrEmpty(RefId));			
		}

		public ReferenceId(string refId) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(refId));
			RefId = refId;
		}

		public override Component Copy() {
			return new ReferenceId(RefId);
		}

		public bool Equals(ReferenceId other) {
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Equals(other.RefId, RefId);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(ReferenceId))
				return false;
			return Equals((ReferenceId) obj);
		}

		public override int GetHashCode() {
			return (RefId != null ? RefId.GetHashCode() : 0);
		}

		public static bool operator ==(ReferenceId left, ReferenceId right) {
			return Equals(left, right);
		}

		public static bool operator !=(ReferenceId left, ReferenceId right) {
			return !Equals(left, right);
		}
	}
}