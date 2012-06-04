namespace DEngine.Actor {

    /// <summary>
    /// Interface for all entities can that do something (ie update)
    /// </summary>
    public interface IEntity {
        /// <summary>
        /// How much action points an entity has
        /// </summary>
        int ActionPoints { get; set; }

        /// <summary>
        /// The rate in which an entity's AP is changed.
        /// </summary>
        /// <returns></returns>
        int Speed { get; }

        /// <summary>
        /// We have enough AP, execute the action and substract the AP cost for the action
        /// </summary>
        void Update();

        /// <summary>
        /// Is entity dead?
        /// </summary>
        bool Dead { get; }

        /// <summary>
        /// Do something when entity dies
        /// </summary>
        void OnDeath();

        /// <summary>
        /// Can entity call update right now?
        /// </summary>
        bool Updateable { get; }
    }
}
