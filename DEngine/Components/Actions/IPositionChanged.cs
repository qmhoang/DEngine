using DEngine.Core;

namespace DEngine.Components.Actions {
	public interface IComponentEvent {
		
	}

	public interface IPositionChanged : IComponentEvent {
		void Move(Point prev, Point curr);
	}
}
