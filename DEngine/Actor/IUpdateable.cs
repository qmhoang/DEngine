namespace DEngine.Actor {

    /// <summary>
    /// Interface for all entities can that do something (ie update)
    /// </summary>
    public interface IUpdateable : IDead {
        /// <summary>
        /// How much action points a person has
        /// </summary>
        int ActionPoints { get; set; }        

        /// <summary>
        /// The rate in which an object's AP is changed.
        /// </summary>
        /// <returns></returns>
        int Speed { get; }

        /// <summary>
        /// We have enough AP, execute the action and substract the AP cost for the action
        /// </summary>
        void Update();
    }
}
