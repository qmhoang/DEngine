using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DEngine.Core {
	/// <summary>
	/// A logger stores game messages divided by generic message type
	/// </summary>
	/// <typeparam name="T">The type of messages</typeparam>
	public class Log<T> {
		public event ComplexEventHandler<Log<T>, EventArgs> Logged;

		private void OnLogged(EventArgs e) {
			var handler = Logged;
			if (handler != null)
				handler(this, e);
		}

		public ReadOnlyCollection<MessageEntry<T>> Entries { get { return _entries.AsReadOnly(); } }

		private readonly List<MessageEntry<T>> _entries;

		public Log() {
			_entries = new List<MessageEntry<T>>();
		}

		public void Write(T type, string text) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(text), "string \"text\" cannot be null or empty");

			if (_entries.Count > 0 && _entries[_entries.Count - 1].Text == text) {
				_entries[_entries.Count - 1].Count++;
			} else {
				_entries.Add(new MessageEntry<T>(text, type));
			}

			OnLogged(EventArgs.Empty);
		}
	}
}