using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DEngine.Core {
	public class Log {
		public event EventHandler<Log, EventArgs> Logged;

		private void OnLogged(EventArgs e) {
			var handler = Logged;
			if (handler != null)
				handler(this, e);
		}

		public ReadOnlyCollection<MessageEntry> Entries { get { return entries.AsReadOnly(); } }		

		private List<MessageEntry> entries;

		public Log() {
			entries = new List<MessageEntry>();
		}

		public void Write(MessageType type, string text) {
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(text), "string \"text\" cannot be null or empty");

			if (entries.Count > 0 && entries[entries.Count - 1].Text == text) {
				entries[entries.Count - 1].Count++;
			} else {
				entries.Add(new MessageEntry(text, type));
			}

			OnLogged(EventArgs.Empty);
		}

		public void Aborted(string text) {
			Write(MessageType.Aborted, text);
		}

		public void AbortedFormat(string text, params object[] args) {
			Write(MessageType.Aborted, String.Format(text, args));
		}

		public void Fail(string text) {
			Write(MessageType.Fail, text);
		}

		public void FailFormat(string text, params object[] args) {
			Write(MessageType.Fail, String.Format(text, args));
		}

		public void Bad(string text) {
			Write(MessageType.Bad, text);
		}

		public void BadFormat(string text, params object[] args) {
			Write(MessageType.Bad, String.Format(text, args));
		}

		public void Normal(string text) {			
			Write(MessageType.Normal, text);			
		}

		public void NormalFormat(string text, params object[] args) {
			Write(MessageType.Normal, String.Format(text, args));
		}

		public void Good(string text) {
			Write(MessageType.Good, text);
		}

		public void GoodFormat(string text, params object[] args) {
			Write(MessageType.Good, String.Format(text, args));
		}

		public void Special(string text) {
			Write(MessageType.Special, text);
		}

		public void SpecialFormat(string text, params object[] args) {
			Write(MessageType.Special, String.Format(text, args));
		}
	}
}