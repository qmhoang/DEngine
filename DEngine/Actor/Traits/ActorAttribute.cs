namespace DEngine.Actor.Traits {
    public class ActorAttribute {
        public ActorAttribute(int value) {
            Base = Current = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ActorAttribute(int @base, int current) {
            Base = @base;
            Current = current;
        }

        public ActorAttribute(ActorAttribute that) : this(that.Base, that.Current) {}
        public int Base { get; set; }
        public int Current { get; set; }
    }
}