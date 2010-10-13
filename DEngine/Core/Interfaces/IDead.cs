namespace DEngine.Core {
    public interface IDead {
        /// <summary>
        /// Is entity dead?
        /// </summary>
        bool Dead { get; }

        /// <summary>
        /// Do something when entity dies
        /// </summary>
        void OnDeath();
    }
}