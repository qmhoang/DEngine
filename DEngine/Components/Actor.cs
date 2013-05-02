using System;
using System.Linq;
using System.Text;
using DEngine.Actions;
using DEngine.Actor;
using DEngine.Entities;

namespace DEngine.Components {
	/// <summary>
	/// Component that controls the emission of actions.  It is basically a wrapper around the Controller (since Components cannot be inherited from) and AP.
	/// </summary>
	public sealed class ControllerComponent : Component {
		public IAction NextAction() {
			return Controller.NextAction();
		}

		public void Disturb() {
			Controller.Disturb();
		}

		public void Cancel() {
			Controller.Cancel();
		}

		public Controller Controller { get; private set; }
		public AP AP { get; private set; }

		public ControllerComponent(Controller controller, AP ap) {
			Controller = controller;
			AP = ap;
			Controller.Holder = this;
		}

		public void Enqueue(IAction action) {
			Controller.Enqueue(action);
		}

		public override Component Copy() {
			return new ControllerComponent(Controller.Copy(), new AP(AP.ActionPointPerTurn, AP.ActionPoints));
		}
	}
}
