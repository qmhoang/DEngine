namespace DEngine.Actor.Traits {
    public class ActorAttribute {
        public ActorAttribute(string name, int value) : this(name, value, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ActorAttribute(string name, int @base, int current) {
            Name = name;
            Base = @base;
            Current = current;
        }

        public ActorAttribute(ActorAttribute that) : this(that.Name, that.Base, that.Current) {}
        public int Base { get; set; }
        public int Current { get; set; }
        public string Name { get; private set; }
    }
}