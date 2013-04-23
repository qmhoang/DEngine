namespace DEngine.Core {
	public class MessageEntry<T> {
		public string Text { get; set; }
		public T Type { get; set; }
		public int Count { get; set; }

		public MessageEntry(string text, T type) {
			Text = text;
			Type = type;
			Count = 1;
		}
	}
}
