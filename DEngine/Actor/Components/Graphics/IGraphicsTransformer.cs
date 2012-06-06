namespace DEngine.Actor.Components.Graphics {
    public interface IGraphicsTransformer<out T> where T:Image {
        T Transform(Actor actor);
    }
}