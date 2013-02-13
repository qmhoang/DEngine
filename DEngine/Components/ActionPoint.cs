//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Diagnostics.Contracts;
//using DEngine.Components.Actions;
//using DEngine.Core;
//using DEngine.Entities;
//
//namespace DEngine.Components {
//	public interface IUpdateable : IComponentEvent {
//		void Update();
//	}
//
//	public class ActionPoint : Component {
//		/// <summary>
//		/// How much action points an entity has
//		/// </summary>
//		public int ActionPoints { get; set; }
//
//		/// <summary>
//		/// The rate in which an entity's AP is changed.
//		/// </summary>
//		/// <returns></returns>
//		public int ActionPointPerTurn { get; set; }
//
//		public const int DEFAULT_SPEED = 100;
//
//		/// <summary>
//		/// Can entity call update right now?
//		/// </summary>
//		public bool Updateable { get { return ActionPoints > 0; } }
//
//		public ActionPoint(int actionPoints = 0, int speed = DEFAULT_SPEED) {
//			Contract.Requires<ArgumentException>(speed > 0);
//			ActionPoints = actionPoints;
//			ActionPointPerTurn = speed;			
//		}
//
//		public override Component Copy() {
//			return new ActionPoint(ActionPoints, ActionPointPerTurn);
//		}
//
//		[ContractInvariantMethod]
//		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
//		private void ObjectInvariant() {
//			Contract.Invariant(ActionPointPerTurn > 0);			
//		}
//	}
//
//}