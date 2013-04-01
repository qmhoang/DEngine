using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
	public delegate void EventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;

	/// <summary>
	/// Generic event args that contain a single generic data
	/// </summary>
	/// <typeparam name="T">The type of data in the event arg</typeparam>
	public class EventArgs<T> : EventArgs {
		private readonly T data;

		public EventArgs(T data) {
			this.data = data;
		}

		public T Data {
			get { return data; }
		}
	}

	/// <summary>
	/// Generic event args that contain a two generic data
	/// </summary>
	/// <typeparam name="T1">the type of data for the first data point in the event arg</typeparam>
	/// <typeparam name="T2">the type of data for the second data point in the event arg</typeparam>
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