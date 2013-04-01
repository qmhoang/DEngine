using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public class BinarySpacePartition {
		public Point Position { get; private set; }
		public BinarySpacePartition Parent { get; private set; }
		public IEnumerable<BinarySpacePartition> Children { get; private set; }
	}
}
