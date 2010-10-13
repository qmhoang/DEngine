namespace DEngine.Core.Interfaces {

    /// <summary>
    /// Interface for all entities can that do something (ie update)
    /// </summary>
    public interface IUpdateable : IDead {
        int ActionPoints { get; set; }

        /// <summary>
        /// Perform any action and returns the update rate
        /// </summary>
        /// <returns></returns>
        int Speed { get; }

        /// <summary>
        /// We have enough AP, execute the action and substract the AP cost for the action
        /// </summary>
        void Update();
    }
}
