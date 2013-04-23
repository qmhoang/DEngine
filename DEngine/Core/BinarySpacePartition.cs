using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public class BinarySpacePartition {
		public Rectangle Rectangle { get; private set; }
		public BinarySpacePartition Parent { get; private set; }
		public IEnumerable<BinarySpacePartition> Childrens { get { return childrens; } }
		public bool IsLeaf { get { return childrens.Count == 0; } }

		public BinarySpacePartition(Rectangle rectangle) : this(rectangle, null, new LinkedList<BinarySpacePartition>()) { }

		private BinarySpacePartition(Rectangle rectangle, BinarySpacePartition parent, LinkedList<BinarySpacePartition> childrens) {
			Rectangle = rectangle;
			Parent = parent;
			this.childrens = childrens;

		}

		public void RemoveChildren() {
			childrens.Clear();
		}

		private LinkedList<BinarySpacePartition> childrens;
	}
}
