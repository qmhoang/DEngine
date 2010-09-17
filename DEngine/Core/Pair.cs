namespace Roguelike.Utility
{
    public class Pair<G, H>
    {
        public Pair()
        { }

        public Pair(G first, H second)
        {
            this.First = first;
            this.Second = second;
        }

        public G First { get; set; }
        public H Second { get; set; }
    };

}
