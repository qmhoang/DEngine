using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public delegate void EventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;

	public class EventArgs<T> : EventArgs {
		private readonly T data1;

		public EventArgs(T data) {
			data1 = data;
		}

		public T Data {
			get { return data1; }
		}
	}

	public class EventArgs<T1, T2> : EventArgs {
		private readonly T1 data1;
		private readonly T2 data2;

		public T1 Data1 {
			get { return data1; }
		}

		public T2 Data2 {
			get { return data2; }
		}

		public EventArgs(T1 data1, T2 data2) {
			this.data1 = data1;
			this.data2 = data2;
		}
	}
}